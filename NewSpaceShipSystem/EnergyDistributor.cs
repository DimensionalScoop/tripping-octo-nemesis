using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class EnergyDistributor:Subsystem
    {
        public enum Techniques { Normal, Aggressive, Defensive, Speed }
        public Techniques Technique;

        public EnergyDistributor()
            : base(0, 15, "Energy distribution")
        {//TODO: add auto-overload overload-capable subsystems with unused energy
            StatusReport.Write(() => "Distribution technique: " + Enum.GetName(typeof(Techniques), Technique) + "\n");
            StatusReport.Write(() => "Unused energy: " + Parant.CurrentAvailableEnergy + "/" + Parant.MaxAvailableEnergy + "e/s");
            StatusReport.Write(() => " (" + (int)(Parant.AssignedEnergy / Parant.MaxAvailableEnergy * 100) + " % efficiency) ");
            StatusReport.AddBarGraph(15, () => Parant.CurrentAvailableEnergy / Parant.MaxAvailableEnergy, () => Color.Yellow, ()=>Color.CornflowerBlue);
            StatusReport.Write("\n");
        }

        public override void Update(GameTime gameTime)
        {
            CalculateEnergy();//XXX: possible performance issue

            base.Update(gameTime);
        }

        void CalculateEnergy()
        {
            var all = Parant.Subsystems.FindAll(p => p.MinEnergyDemand > 0);
            all.Reverse();//subsystems with the lowest priority are activated first.
            var others = all;

            var overwriteOn = all.FindAll(p => p.HasCharacteristic(Properties.OverwriteOn));
            others.RemoveAll(p => p.HasCharacteristic(Properties.OverwriteOn));

            var engines = others.FindAll(p => p is IEngine);
            var weapons = others.FindAll(p => p is IWeapon);
            var shields = others.FindAll(p => p is IDamage);

            var nonPrimary = others.FindAll(p => p.HasCharacteristic(Properties.NonPrimary));

            PrepareEnergyList(true, engines, weapons, shields);

            others.RemoveAll(p => engines.Contains(p) || weapons.Contains(p) || shields.Contains(p));
            others.RemoveAll(p => p.HasCharacteristic(Properties.NonPrimary));
            others.AddRange(nonPrimary);



            for (int i = 0; Parant.CurrentAvailableEnergy > 0 && i < overwriteOn.Count; i++)
            {
                overwriteOn[i].PowerSubsystem();
            }

            switch (Technique)
            {
                case Techniques.Normal:
                    {
                        int fails = 0;
                        for (int i = 0; Parant.CurrentAvailableEnergy > 0 || fails < 2; i++)
                        {
                            fails = 0;
                            if (i < engines.Count) engines[i].PowerSubsystem(); else fails++;

                            if (i < weapons.Count && i < shields.Count)
                            {
                                if (weapons[i].MinEnergyDemand > shields[i].MinEnergyDemand)//tries to power the subsystem with the lower energy demand first
                                {
                                    shields[i].PowerSubsystem();
                                    weapons[i].PowerSubsystem();
                                }
                                else
                                {
                                    weapons[i].PowerSubsystem();
                                    shields[i].PowerSubsystem();
                                }
                            }
                            else
                            {
                                if (i < weapons.Count) weapons[i].PowerSubsystem(); else fails++;
                                if (i < shields.Count) shields[i].PowerSubsystem(); else fails++;
                            }
                        }
                    } break;

                case Techniques.Aggressive:
                    {
                        if (engines.Count > 0) engines[0].PowerSubsystem();
                        weapons.ForEach(p => p.PowerSubsystem());
                        shields.ForEach(p => p.PowerSubsystem());
                        engines.ForEach(p => p.PowerSubsystem());
                    } break;

                case Techniques.Defensive:
                    {
                        if (engines.Count > 0) engines[0].PowerSubsystem();
                        shields.ForEach(p => p.PowerSubsystem());
                        engines.ForEach(p => p.PowerSubsystem());
                        shields.ForEach(p => p.OverloadSubsystem(2));
                        weapons.ForEach(p => p.PowerSubsystem());
                    } break;

                case Techniques.Speed:
                    {
                        engines.ForEach(p => p.PowerSubsystem());
                        shields.ForEach(p => p.PowerSubsystem());
                        weapons.ForEach(p => p.PowerSubsystem());
                    } break;
            }

            for (int i = 0; Parant.CurrentAvailableEnergy > 0 || i < others.Count; i++)
            {
                others[i].PowerSubsystem();
            }
        }

        void PrepareEnergyList(bool deleteNonPrimaryparams,params List<Subsystem>[] items)
        {
            foreach (var elem in items)
            {
                elem.RemoveAll(p =>
                    p.HasCharacteristic(Properties.OverwriteOff) ||
                    p.HasCharacteristic(Properties.OverwriteOn) ||
                    p.HasCharacteristic(Properties.UserActivated) ||
                    p.HasCharacteristic(Properties.Fallback));

                if (deleteNonPrimaryparams)
                    elem.RemoveAll(p => p.HasCharacteristic(Properties.NonPrimary));

                var primary=elem.FindAll(p => p.HasCharacteristic(Properties.Primary));
                elem.RemoveAll(p => p.HasCharacteristic(Properties.Primary));
                elem.InsertRange(0, primary);
            }
        }
    }
}
