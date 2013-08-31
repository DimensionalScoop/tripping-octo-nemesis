using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X45Game.Interface;

namespace NewSpaceShipSystem
{
    public class Subsystem
    {
        [Flags()]
        public enum Properties
        {
            None = 0,
            /// <summary>
            /// Subsystem is not online in normal situations, but can be activated in an emergency
            /// </summary>
            Fallback = 1 << 0,
            /// <summary>
            /// Subsystem is only online if activated by the user (opposed to activation by the energy management)
            /// </summary>
            UserActivated=1<<1,
            /// <summary>
            /// Subsystem will be preferred over others of the same kind
            /// </summary>
            Primary=1<<2,
            /// <summary>
            /// Subsystem will be activated only if all other (non-fallback) systems are powered.
            /// </summary>
            NonPrimary=1<<3,
            /// <summary>
            /// Force subsystem activation
            /// </summary>
            OverwriteOn=1<<4,
            /// <summary>
            /// Force subsystem deactivation
            /// </summary>
            OverwriteOff=1<<5,
        }

        public readonly string Name;
        public Properties Characteristics;
        public bool HasCharacteristic(Properties item) { return (Characteristics & item) == item; }
        public readonly int Priority;
        public readonly int Importance;
        public readonly ExtendedString StatusReport;
        public readonly float MaxOverload;

        public float AvailableEnergy;
        public float MinEnergyDemand;

        public float EnergyOverload { get { return AvailableEnergy / MinEnergyDemand; } }


        protected readonly Main Parant;

        public Subsystem(int priority, int importance, string name, int maxOverload=1)
        {
            Priority = priority;
            Importance = importance;
            Name = name;
            MaxOverload = maxOverload;
            StatusReport = new ExtendedString();

            InitStatusReport();
        }

        protected void InitStatusReport()
        {
            StatusReport.Clear();
            StatusReport.Write(() => Name + " status report\n", CharacteristicsColor);

            StatusReport.Write(() => MinEnergyDemand != 0 ? "Energy: " : "");
            StatusReport.Write(() => MinEnergyDemand != 0 ? "" + (int)AvailableEnergy : "", EnergyDemandColor);
            StatusReport.Write(() => MinEnergyDemand != 0 ? "/" + MinEnergyDemand + " (" + (int)(EnergyOverload * 100) + "/" + MaxOverload * 100 + "% overload)\n" : "");
        }

        Color EnergyDemandColor()
        {
            if (AvailableEnergy < MinEnergyDemand) return Color.Red;
            if (AvailableEnergy < MinEnergyDemand * 1.2f) return Color.Yellow;
            if (AvailableEnergy > MinEnergyDemand * 2) return Color.Green;
            return Color.White;
        }

        Color CharacteristicsColor()
        {
            if (HasCharacteristic(Properties.OverwriteOff)) return Color.DimGray;
            if (HasCharacteristic(Properties.OverwriteOn)) return Color.GreenYellow;
            if (HasCharacteristic(Properties.UserActivated)) return Color.Yellow;
            if (HasCharacteristic(Properties.Fallback)) return Color.Tomato;
            if (HasCharacteristic(Properties.Primary)) return Color.Orange;

            return Color.White;
        }

        public virtual void DepowerSubsystem()
        {
            Parant.AssignedEnergy -= AvailableEnergy;
            AvailableEnergy = 0;
        }
        public virtual void PowerSubsystem()
        {
            if (AvailableEnergy >= MinEnergyDemand) return;
            if (Parant.CurrentAvailableEnergy < MinEnergyDemand) return;
            Parant.AssignedEnergy += MinEnergyDemand;
            AvailableEnergy = MinEnergyDemand;
        }
        public virtual void OverloadSubsystem(float by)
        {
            float totalEnergy = MinEnergyDemand * by;
            float requestedEnergy = totalEnergy - AvailableEnergy;

            if (requestedEnergy > Parant.CurrentAvailableEnergy || requestedEnergy <= 0) return;
            Parant.AssignedEnergy += requestedEnergy;
            AvailableEnergy = totalEnergy;
        }

        public virtual void Update(GameTime gameTime) { }
    }
}
