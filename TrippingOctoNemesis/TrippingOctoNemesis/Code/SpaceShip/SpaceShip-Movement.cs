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
    //public class Navigatior
    //{
    //    public float Speed { get; protected set; }
    //    public float Angle { get; protected set; }
    //    public readonly float DefaultSpeed;
    //    public readonly float DeploySpeed;
    //    public readonly float DefaltAngleSpeed;
    //    public readonly float DeployAngleSpeed;

    //    public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }

    //    public Navigatior(ü
    //}

    public partial class SpaceShip
    {
        public Vector2 Position = new Vector2(100, 100);
        public Vector2 TargetPosition = new Vector2(500, 500);
        public float Speed = 120;
        public float AngleSpeed = 5;
        public float NormaleAngleSpeed = 5;
        public float DeploySpeed = 250;
        public float NormalSpeed = 120;
        public float Angle;
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }

        public bool ActivateEvasion;

        static readonly int targetMarginSquared = 20 ^ 2;


        protected void CalcMovement(GameTime gameTime)
        {
            if (ActivateEvasion&&Status!= Conditions.ReturningPhase1&&Status!=Conditions.ReturningPhase2)
            {
                Angle += 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed;
                ActivateEvasion = false;
            }
            
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


        readonly static float minDistanceForAngleSpeedReduction = (float)Math.Pow(200, 2);
        private void CalcNewAngle(GameTime gameTime, float difference)
        {
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

                //var actualDifference = difference * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed;
                //actualDifference *= MathHelper.SmoothStep(0, 1, MathHelper.Clamp((TargetPosition - Position).LengthSquared() / minDistanceForAngleSpeedReduction, 0, 1));
                if (
                    (actualDifference < 0 && actualDifference < difference) ||
                    (actualDifference > 0 && actualDifference > difference))
                    actualDifference = difference;

                Angle = MathHelper.WrapAngle(Angle + actualDifference);
            }
        }
    }
}