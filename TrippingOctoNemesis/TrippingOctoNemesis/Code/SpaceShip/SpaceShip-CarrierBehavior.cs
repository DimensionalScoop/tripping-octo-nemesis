using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public Carrier Carrier;
        public TimeSpan LaunchTime;
        public readonly TimeSpan LaunchDuration = TimeSpan.FromSeconds(1);


        private void CalcCarrierBehaviour(GameTime gameTime)
        {
            if (Status == Conditions.Deployed)
            {
                var t = gameTime.TotalGameTime - LaunchTime;
                Speed = MathHelper.SmoothStep(DeploySpeed, NormalSpeed, (float)(t.TotalSeconds / LaunchDuration.TotalSeconds));
                AngleSpeed = MathHelper.SmoothStep(0, NormaleAngleSpeed, (float)(t.TotalSeconds / LaunchDuration.TotalSeconds));

                if (gameTime.TotalGameTime > LaunchTime + LaunchDuration)
                {
                    Status = Conditions.Airborne;
                    Speed = NormalSpeed;
                    AngleSpeed = NormaleAngleSpeed;
                }

                if (Status == Conditions.ReturningPhase1 || Status == Conditions.ReturningPhase2)
                    AngleSpeed = 12;
            }

            if (Status == Conditions.ReturningPhase1)
                TargetPosition = Carrier.Position + new Vector2(0, 100);
            else if (Status == Conditions.ReturningPhase2)
                TargetPosition = Carrier.Position + new Vector2(0, Carrier.Sprite.TextureOrigin.Y / 2);
        }

        public void Deploy(Vector2 origin, Vector2 target, GameTime gameTime)
        {
            Status = Conditions.Deployed;
            Position = origin;
            TargetPosition = target;
            Angle=-MathHelper.PiOver2;
            LaunchTime = gameTime.TotalGameTime;
            Speed = DeploySpeed;
            track.Clear();
        }

        public void Return()
        {
            Status = Conditions.ReturningPhase1;
            TargetPosition = Carrier.Position + new Vector2(0, 100);
            ReachedTarget += ReturningPhase1End;
        }

        void ReturningPhase1End(SpaceShip none)
        {
            Status = Conditions.ReturningPhase2;
            TargetPosition = Carrier.Position + new Vector2(0, Carrier.Sprite.TextureOrigin.Y/2);
            ReachedTarget += ReturningPhase2End;
        }

        void ReturningPhase2End(SpaceShip none)
        {
            Status = Conditions.InHangar;
            track.Clear();
        }
    }
}
