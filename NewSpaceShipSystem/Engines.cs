using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Engines:Subsystem,IGenerator,IMovementProvider
    {
        public float EnergyGeneration;
        float IGenerator.EnergyGeneration
        {
            get { return EnergyGeneration; }
        }

        public bool Online;

        public Engines()
            : base(5, 5, "Engines")
        {

        }
    }

    interface IMovementProvider
    {

    }

    interface IGenerator
    {
        /// <summary>
        /// How much energy this systems makes available to the spaceship.
        /// </summary>
        /// <returns></returns>
        float EnergyGeneration { get; }
    }
}
