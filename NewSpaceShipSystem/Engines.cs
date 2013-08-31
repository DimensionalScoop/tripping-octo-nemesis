using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Engines:Subsystem,IGenerator,IEngine
    {
        #region IGenerator
        public float EnergyGeneration=100;
        float IGenerator.EnergyGeneration
        {
            get { return EnergyGeneration; }
        }
        #endregion


        public Engines()
            : base(5, 5, "Engines")
        {

        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }




        public float MinSpeed
        {
            get { throw new NotImplementedException(); }
        }

        public float MaxSpeed
        {
            get { throw new NotImplementedException(); }
        }
    }
}
