using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Autopilot:Subsystem,IPilot
    {
        public virtual void SetTarget(Func<Vector2> position)
        {
            targetFunction = position;
        }
        Func<Vector2> targetFunction;
        public Vector2 Target
        {
            get
            {
                return targetFunction();
            }
        }

        bool targetReached;
        public bool TargetReached
        {
            get { return targetReached; }
        }

        public int DistanceToTarget
        {
            get { return (int)Vector2.Distance(Parant.Position, Target); }
        }


        public float CurrentSpeed;
        public float CurrentTurnSpeed;

        public Autopilot()
            : base(2, 10, "Auto pilot")
        {
        }


        protected void CorrectHeading(GameTime gameTime)
        {
            float currentHeading = MathHelper.WrapAngle(Parant.Heading);
            float targetHeading = MathHelper.WrapAngle((float)Math.Atan2((Target - Parant.Position).Y, (Target - Parant.Position).X));
            float difference = MathHelper.WrapAngle(targetHeading - currentHeading);

            float turnSpeed=CurrentTurnSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(difference) < turnSpeed)
            {
                Parant.Heading = targetHeading;
                return;
            }

            if (MathHelper.WrapAngle(currentHeading - targetHeading + turnSpeed) < difference)
                Parant.Heading += turnSpeed;
            else
                Parant.Heading -= turnSpeed;
        }

        protected void CorrectSpeed(GameTime gameTime)
        {

        }
    }
}
