using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using System.Diagnostics;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis
{
    //NEXT: Combine movement and targeting, extract KI

    public partial class SpaceShip
    {
        public Vector2 Position = new Vector2(100, 100);
        public Vector2 TargetPosition;
        public float Speed = 120;
        public float AngleSpeed = 5;
        public float NormaleAngleSpeed = 5;
        public float DeploySpeed = 250;
        public float NormalSpeed = 120;
        public float Angle;
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }

        public bool ActivateEvasion;

        public bool PreciseAngleSpeed;

        static readonly int targetReachedMarginSquared = (int)Math.Pow(10, 2);//XXX: corrected targetReachedMarginSquared. May cause major movement problems.


        protected void CalcMovement(GameTime gameTime)
        {
            if (Ki as NoScreenMovement != null) return;

            CalcTargetAngle(gameTime);//XXX: may cause performance issues

            if (!PreciseAngleSpeed && ActivateEvasion&&Status!= Conditions.ReturningPhase1&&Status!=Conditions.ReturningPhase2)
            {
                Angle += 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed;
                ActivateEvasion = false;
            }
            
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void CalcTargetAngle(GameTime gameTime)
        {
            float targetAngle = (float)Math.Atan2((TargetPosition - Position).Y, (TargetPosition - Position).X);
            float difference = MathHelper.WrapAngle(targetAngle - Angle);
            CalcNewAngle(gameTime, difference);

            
            if (ReachedTarget != null && Vector2.DistanceSquared(Position, TargetPosition) < targetReachedMarginSquared)
            {
                var methods = ReachedTarget.GetInvocationList();
                ReachedTarget = null;
                foreach (var elem in methods) elem.DynamicInvoke(this);
            }
        }


        readonly static float minDistanceForAngleSpeedReduction = (float)Math.Pow(200, 2);
        private void CalcNewAngle(GameTime gameTime, float difference)
        {
            if (PreciseAngleSpeed)
            {
                //float poss = (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed;

                //if (difference < 0) poss = -poss;
                //Angle = MathHelper.WrapAngle(Angle + poss);
                Angle += difference;
                return;
            }

            if (Status == Conditions.ReturningPhase1 || Status == Conditions.ReturningPhase2)
            {
                Angle = MathHelper.WrapAngle(Angle + difference * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed / 2);
            }
            else
            {
                var dist0 = 400;
                var dist = (TargetPosition - Position).LengthSquared() / dist0;
                if (dist > dist0) dist = dist0;

                var rangeAngleSpeed = MathHelper.SmoothStep(12, AngleSpeed, dist);


                var actualDifference = difference * (float)gameTime.ElapsedGameTime.TotalSeconds * rangeAngleSpeed;

                actualDifference *= MathHelper.SmoothStep(0, 1, MathHelper.Clamp((TargetPosition - Position).LengthSquared() / minDistanceForAngleSpeedReduction, 0, 1));

                if (
                    (actualDifference < 0 && actualDifference < difference) ||
                    (actualDifference > 0 && actualDifference > difference))
                    actualDifference = difference;

                Angle = MathHelper.WrapAngle(Angle + actualDifference);
            }
        }
    }
}