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
        public readonly string Name;
        public readonly int Priority;
        public readonly int Importance;
        public readonly ExtendedString StatusReport;

        public float AvailableEnergy;
        public float MinEnergyDemand;

        public float EnergyOverload { get { return AvailableEnergy / MinEnergyDemand; } }


        protected readonly Main Parant;

        public Subsystem(int priority, int importance, string name)
        {
            Priority = priority;
            Importance = importance;
            Name = name;
            StatusReport = new ExtendedString();
            StatusReport.Write(name + " subsystem status report\n");
            StatusReport.Write("Energy: ");
            StatusReport.Write(() => "" + (int)AvailableEnergy, EnergyDemandColor);
            StatusReport.Write(() => "/" + MinEnergyDemand + " (" + (int)(EnergyOverload * 100) + "% overload");
        }

        Color EnergyDemandColor()
        {
            if (AvailableEnergy<MinEnergyDemand) return Color.Red;
            if (AvailableEnergy < MinEnergyDemand*1.2f) return Color.Yellow;
            if (AvailableEnergy > MinEnergyDemand * 2) return Color.Green;
            return Color.White;
        }
    }

    public interface IDamage
    {
        void Damage(float amountOfDamage);
    }
}
