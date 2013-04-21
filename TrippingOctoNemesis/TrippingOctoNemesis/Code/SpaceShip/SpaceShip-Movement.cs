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
        public bool IntPosition;
        public Vector2 Position = new Vector2(100, 100);
        public Vector2 TargetPosition = new Vector2(500, 500);
        public float Speed = 120;
        public float AngleSpeed = 12;
        public float DeploySpeed = 250;
        public float NormalSpeed = 120;
        public float Angle;
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }

        static readonly int targetMarginSquared = 20 ^ 2;


        protected void CalcMovement(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void CalcTargetAngle(GameTime gameTime, Hud hud)
        {
            if (HasTarget)
            {
                float targetAngle = (float)Math.Atan2((TargetPosition - Position).Y, (TargetPosition - Position).X);
                float difference = MathHelper.WrapAngle(targetAngle - Angle);
                CalcNewAngle(gameTime, difference);

                if (KeepScreenPosition) TargetPosition -= hud.CameraDelta;


                if (ReachedTarget != null && Vector2.DistanceSquared(Position, TargetPosition) < targetMarginSquared)
                {
                    var methods = ReachedTarget.GetInvocationList();
                    ReachedTarget = null;
                    foreach (var elem in methods) elem.DynamicInvoke(this);
                }
            }
        }

        private void CalcNewAngle(GameTime gameTime, float difference)
        {
            if (Status == Condition.ReturningPhase1 || Status == Condition.ReturningPhase2)
            {
                Angle = MathHelper.WrapAngle(Angle + difference * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed / 2);
            }
            else
            {
                Angle = MathHelper.WrapAngle(Angle + difference * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed
                    * MathHelper.SmoothStep(0, 1, MathHelper.Clamp((TargetPosition - Position).LengthSquared() / 40000, 0, 1))
                    );//TODO: add better steering behavior
            }
        }
    }
}