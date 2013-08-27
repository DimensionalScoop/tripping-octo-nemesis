using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewSpaceShipSystem
{
    public class Subsystem
    {
        public readonly string Name;
        public readonly int Priority;
        public float AvailableEnergy;
        public float EnergyDemand;

        protected readonly Main Parant;

        public Subsystem(int priority, string name) { Priority = priority; Name = name; }
    }

    public interface IDamage
    {
        void Damage(float amountOfDamage);
    }
}
