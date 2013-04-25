using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public MotherShip Carrier;
        public TimeSpan LaunchTime;
        public readonly TimeSpan LaunchDuration = TimeSpan.FromSeconds(1);


        private void CalcCarrierBehaviour(GameTime gameTime)
        {
            if (Status == Condition.Deployed)
            {
                var t = gameTime.TotalGameTime - LaunchTime;
                Speed = MathHelper.SmoothStep(DeploySpeed, NormalSpeed, (float)(t.TotalSeconds / LaunchDuration.TotalSeconds));
                AngleSpeed = MathHelper.SmoothStep(0, NormaleAngleSpeed, (float)(t.TotalSeconds / LaunchDuration.TotalSeconds));

                if (gameTime.TotalGameTime > LaunchTime + LaunchDuration)
                {
                    Status = Condition.Airborne;
                    if (StatusChanged != null) StatusChanged(this);
                    Speed = NormalSpeed;
                    AngleSpeed = NormaleAngleSpeed;
                }

                if (Status == Condition.ReturningPhase1 || Status == Condition.ReturningPhase2)
                    AngleSpeed = 12;
            }

            if (Status == Condition.ReturningPhase1)
                TargetPosition = Carrier.Position + new Vector2(0, 100);
            else if (Status == Condition.ReturningPhase2)
                TargetPosition = Carrier.Position + new Vector2(0, Carrier.Sprite.TextureOrigin.Y / 2);
        }

        public void Deploy(Vector2 origin, Vector2 target, GameTime gameTime)
        {
            Status = Condition.Deployed;
            if(StatusChanged!=null) StatusChanged(this);
            Position = origin;
            TargetPosition = target;
            Angle=-MathHelper.PiOver2;
            LaunchTime = gameTime.TotalGameTime;
            Speed = DeploySpeed;
            track.Clear();
        }

        public void Return()
        {
            Status = Condition.ReturningPhase1;
            if (StatusChanged != null) StatusChanged(this);
            TargetPosition = Carrier.Position + new Vector2(0, 100);
            ReachedTarget += ReturningPhase1End;
        }

        void ReturningPhase1End(SpaceShip none)
        {
            Status = Condition.ReturningPhase2;
            if (StatusChanged != null) StatusChanged(this);
            TargetPosition = Carrier.Position + new Vector2(0, Carrier.Sprite.TextureOrigin.Y/2);
            ReachedTarget += ReturningPhase2End;
        }

        void ReturningPhase2End(SpaceShip none)
        {
            Status = Condition.InHangar;
            if (StatusChanged != null) StatusChanged(this);
            track.Clear();
        }
    }
}
