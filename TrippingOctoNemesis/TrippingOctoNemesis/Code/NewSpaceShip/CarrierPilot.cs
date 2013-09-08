using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippingOctoNemesis.SPS
{
    public class CarrierPilot:Subsystem,IPilot
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

        public bool TargetReached
        {
            get { return IsApproachingTarget && DistanceToTarget < RadiusOfInfluence; }
        }

        public int DistanceToTarget
        {
            get { return (int)Vector2.Distance(Parant.Position, Target); }
        }


        protected List<IEngine> AssignedEngines = new List<IEngine>();

        public float RadiusOfInfluence;

        public float MaxTurnSpeed;

        public float? OverwriteMinSpeed=null;//TODO: overwrite minSpeed if minSpeed<target.currentSpeed
        public float? OverwriteMaxSpeed=null;

        public float MinSpeed;
        public float MaxSpeed;
        public float EngineAcceleration;
        public float TargetSpeed;
        public float ActualSpeed;

        public float CurrentMinSpeed { get { if (OverwriteMinSpeed != null)return (float)OverwriteMinSpeed; return MinSpeed; } }
        public float CurrentMaxSpeed { get { if (OverwriteMaxSpeed != null)return (float)OverwriteMaxSpeed; return MaxSpeed; } }

        float turnAcceleration;
        float targetHeading;

        public float SmallestCircleRadius { get { return MathHelper.TwoPi / MaxTurnSpeed * MinSpeed / MathHelper.Pi / 2; } }

        protected float PreviousDistanceToTarget;
        protected float PreviousPosition;

        public bool IsApproachingTarget { get { return PreviousDistanceToTarget < DistanceToTarget; } }

        /// <summary>
        /// if distanceToTarget is smaller than SlowDownDelta+SlowdownDistance, the spaceship slows down to maintain the smallest circle surrounding the target possible.
        /// </summary>
        const float SlowDownDelta = 50;


        public CarrierPilot()
            : base(2, 0, "Semi-automatic carrier pilot")
        {
            StatusReport.Write(() => "Velocity: " + ActualSpeed.ToString("0.0") + "px/s (" + TargetSpeed.ToString("0.0") + "px/s ) " + MinSpeed.ToString("0.0") + "/" + MaxSpeed.ToString("0.0") + "px/s\n");

            TargetSpeed = MinSpeed;
        }


        public override void Update(GameTime gameTime)
        {
            GetEngineSpeed();
            CorrectSpeed(gameTime);
            CalculateEngineAcceleration(gameTime);

            base.Update(gameTime);
        }

        protected void GetEngineSpeed()//XXX: possible performance issue, easy to adjust
        {
            var engines = Parant.FindAllSubsystems<IEngine>();
            engines.RemoveAll(p => !(p as Subsystem).Online);
            AssignedEngines = engines;

            float weightedTurnSpeeds = 0;
            MaxSpeed = 0;
            MinSpeed = 0;
            EngineAcceleration = 0;

            foreach (var elem in engines)
            {
                weightedTurnSpeeds += elem.MaxTurnSpeed * elem.MaxSpeed;
                MaxSpeed += elem.MaxSpeed;
                MinSpeed += elem.MinSpeed;
                EngineAcceleration += elem.EngineAcceleration;
            }

            MaxTurnSpeed = weightedTurnSpeeds / MaxSpeed;
        }

        protected void CalculateEngineAcceleration(GameTime gameTime)
        {
            if (TargetSpeed != ActualSpeed)
            {
                float diff = TargetSpeed - ActualSpeed;
                float acc=EngineAcceleration*(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (diff <= acc) ActualSpeed = TargetSpeed;
                else if (TargetSpeed - ActualSpeed - acc > diff) ActualSpeed += acc;
                else ActualSpeed -= acc;

                float percentageSpeed = (ActualSpeed - MinSpeed) / (MaxSpeed - MinSpeed);
                AssignedEngines.ForEach(p => p.SetSpeed(percentageSpeed));
            }
        }

        protected void CorrectHeading(GameTime gameTime)
        {
            if (Target.X != Parant.Position.X)
            {
                float speed = (MaxSpeed - ActualSpeed)*(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Math.Abs(Target.X - Parant.Position.X) <= speed) Parant.Position.X = Target.X;
                else if (Target.X - Parant.Position.X - speed < Target.X - Parant.Position.X + speed)
                    Parant.Position.X += speed;
                else
                    Parant.Position.X -= speed;
            }
        }
    }
}
