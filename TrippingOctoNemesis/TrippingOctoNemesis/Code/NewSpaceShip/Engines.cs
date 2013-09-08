using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.SPS
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

        private float minSpeed = 10;
        public float MinSpeed
        {
            get { return minSpeed; }
            set { minSpeed = value; }
        }
        public float MinFuelConsumption = 1;

        private float maxSpeed = 30;
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        public float MaxFuelConsumption = 3.5f;

        private float maxTurnSpeed = MathHelper.TwoPi / 3f;
        public float MaxTurnSpeed
        {
            get { return maxTurnSpeed; }
            set { maxTurnSpeed = value; }
        }

        public float CurrentSpeed;
        public float CurrentFuelConsumption { get { return CurrentSpeed==0?0:MathHelper.SmoothStep(MinFuelConsumption, MaxFuelConsumption, (CurrentSpeed - MinSpeed) / (MaxSpeed - MinSpeed)); } }


        private float engineAcceleration = 20 / 1f;
        public float EngineAcceleration
        {
            get { return engineAcceleration; }
            set { engineAcceleration = value; }
        }


        public Engines()
            : base(5, 5, "Engines")
        {
            StatusReport.Write(() => "Status: ");
            StatusReport.Write(() => (Online ? "Online (" + (int)MinSpeed + "/" + (int)MaxSpeed + "px/s, " + (int)MathHelper.ToDegrees(maxTurnSpeed) + "°/s, " + (int)engineAcceleration + "px/s²)" : "Offline"), () => Online ? new Color(200, 255, 200) : Color.Gray);
            StatusReport.Write(() => "Fuel consumption: " + CurrentFuelConsumption+" u/s");
        }

        public override void Update(GameTime gameTime)
        {
            if (Online)
            {
                var storage=Parant.FindAllSubsystems<IStorage>();
                float consumption = CurrentFuelConsumption * (float)gameTime.ElapsedGameTime.TotalSeconds;
                for (int i = 0; consumption > 0; i++)
                {
                    if(storage.Count<=i)
                    {
                        Online=false;
                        CurrentSpeed=0;
                    }
                    consumption-=storage[i].GetItem(Item.GetFuel(consumption)).Amount;
                }

                Parant.Position=Parant.Position.Transform(Parant.Heading, CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        public void SetSpeed(float percentage)
        {
            CurrentSpeed = MathHelper.Lerp(MinSpeed, MaxSpeed, percentage);
        }
    }
}
