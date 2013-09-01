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
            StatusReport.Write(() => "Status: ");
            StatusReport.Write(() => (Online ? "Online (" + (int)MinSpeed + "/" + (int)MaxSpeed + "px/s, " + (int)MathHelper.ToDegrees(maxTurnSpeed) + "°/s, " + (int)engineAcceleration + "px/s²)" : "Offline"), () => Online ? new Color(200, 255, 200) : Color.Gray);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }


        private float minSpeed=10;
        public float MinSpeed
        {
            get { return minSpeed; }
            set { minSpeed = value; }
        }

        private float maxSpeed=30;
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        private float maxTurnSpeed=MathHelper.TwoPi/3f;
        public float MaxTurnSpeed
        {
            get { return maxTurnSpeed; }
            set { maxTurnSpeed = value; }
        }


        private float engineAcceleration=20/1f;
        public float EngineAcceleration
        {
            get { return engineAcceleration; }
            set { engineAcceleration = value; }
        }
        
    }
}
