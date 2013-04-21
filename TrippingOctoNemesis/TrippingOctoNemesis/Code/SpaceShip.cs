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
    public class SpaceShip
    {
        public enum Condition { InHangar, Airborne, Deployed, ReturningPhase1, ReturningPhase2, Repairing }
        public Condition Status = Condition.InHangar;
        public Color StatusColor
        {
            get
            {
                switch (Status)
                {
                    case SpaceShip.Condition.InHangar: return Color.AntiqueWhite;
                    case SpaceShip.Condition.Deployed: return Color.DarkOrange;
                    case SpaceShip.Condition.Airborne: return Color.Lerp(Color.Red, Color.LightBlue, Hitpoints / (float)MaxHitpoints);
                    case SpaceShip.Condition.Repairing: return Color.Lerp(Color.MediumVioletRed, Color.GreenYellow, Hitpoints / (float)MaxHitpoints);
                    case SpaceShip.Condition.ReturningPhase1: return Color.Gray;
                    case SpaceShip.Condition.ReturningPhase2: return Color.Gray;
                    default: throw new NotImplementedException();
                }
            }
        }
        public bool IntPosition;
        public Vector2 Position=new Vector2(100,100);
        public Vector2 TargetPosition = new Vector2(500, 500);
        public SpaceShip TargetShip;
        public int TargetShipDistanceSquared;
        public bool AutoTargetShip = true;
        public bool HasTarget = true;
        public bool KeepScreenPosition = true;
        public Fraction Fraction;
        public float Angle;
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }
        public float Speed = 120;
        public float AngleSpeed=12;
        public float DeploySpeed=250;
        public float NormalSpeed=120;
        public MotherShip Carrier;
        public enum KIs { FixedTargetPosition,NearestEnemy }
        public KIs KI= KIs.FixedTargetPosition;

        public Weapon Weapon;
        public int Hitpoints = 10;
        public int MaxHitpoints = 10;
        public TimeSpan ActionTimer;
        public TimeSpan LaunchTime;
        public readonly TimeSpan LaunchDuration = TimeSpan.FromSeconds(1);

        public Sprite Sprite = new Sprite("s\\ship");
        public Color Color = Color.White;
        public float Scale = 1f;

        const float fractionColorBrightness = 0.5f;

        List<Vector2> track = new List<Vector2>();
        protected int TrackLenght = 50;
        protected Vector2[] EnginePositions = new Vector2[1];
        protected static Random random = new Random();

        public event Action<SpaceShip> HitpointsChanged;
        public event Action<SpaceShip> ReachedTarget;
        public event Action<SpaceShip> StatusChanged;

        static readonly int targetMarginSquared=20^2;



        public SpaceShip(Fraction fraction)
        {
            EnginePositions[0] = Vector2.Zero;
            Fraction = fraction;
            Color = Color.Lerp(Color.White,Fraction.Color,fractionColorBrightness);
        }

        public virtual void Update(GameTime gameTime,Hud hud,List<SpaceShip> otherSpaceShips)
        {
            if (Status != Condition.InHangar && Status != Condition.Repairing)
            {
                CalcTrack();
                CalcKI(gameTime);
                CalcTargetAngle(gameTime, hud);
                CalcMovement(gameTime);
            }

            CalcDeploy(gameTime);

            if (Status == Condition.ReturningPhase1)
                TargetPosition = Carrier.Position + new Vector2(0, 100);
            else if (Status == Condition.ReturningPhase2)
                TargetPosition = Carrier.Position + new Vector2(0, Carrier.Sprite.TextureOrigin.Y / 2);
        }

        public virtual void LongUpdate(TimeSpan elapsedTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            if (AutoTargetShip)
            {
                TargetShip = null;
                TargetShipDistanceSquared = -1;
                int minRange = int.MaxValue;
                int range;
                for (int i = 0; i < otherSpaceShips.Count; i++)
                    if (Fraction.IsEnemy(otherSpaceShips[i].Fraction))
                    {
                        range = (int)Vector2.DistanceSquared(otherSpaceShips[i].Position, Position);
                        if (range < minRange)
                        {
                            minRange = range;
                            TargetShip = otherSpaceShips[i];
                            TargetShipDistanceSquared = range;
                        }
                    }
            }
        }

        private void CalcKI(GameTime gameTime)
        {
            switch (KI)
            {
                case KIs.FixedTargetPosition: return;

                case KIs.NearestEnemy:
                    if (TargetShip==null) TargetPosition = Position;
                    else TargetPosition = TargetShip.Position;
                    return;

                default: throw new NotImplementedException();
            }
        }

        private void CalcDeploy(GameTime gameTime)
        {
            if (Status == Condition.Deployed)
            {
                var t = gameTime.TotalGameTime - LaunchTime;
                Speed = MathHelper.SmoothStep(DeploySpeed, NormalSpeed, (float)(t.TotalSeconds / LaunchDuration.TotalSeconds));

                if (gameTime.TotalGameTime > LaunchTime + LaunchDuration)
                {
                    Status = Condition.Airborne;
                    if (StatusChanged != null) StatusChanged(this);
                    Speed = NormalSpeed;
                }
            }
        }

        protected void CalcMovement(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void CalcTrack()
        {
            foreach (var engine in EnginePositions)
            {
                var pos = Position + Vector2.Zero.Transform(engine.Angle(Vector2.Zero) - Angle, engine.Length());
                track.Add(pos);
                track.Add(pos - Direction * 2);
            }

            if (track.Count > TrackLenght) track.RemoveRange(0, EnginePositions.Length*2);
        }

        protected void CalcTargetAngle(GameTime gameTime, Hud hud)
        {
            if (HasTarget)
            {
                float targetAngle = (float)Math.Atan2((TargetPosition - Position).Y, (TargetPosition - Position).X);
                float difference = MathHelper.WrapAngle(targetAngle - Angle);
                CalcNewAngle(gameTime, difference);

                if (KeepScreenPosition) TargetPosition -= hud.CameraDelta;


                if(ReachedTarget!=null&&Vector2.DistanceSquared(Position,TargetPosition)<targetMarginSquared)
                {
                    var methods = ReachedTarget.GetInvocationList();
                    ReachedTarget=null;
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

        protected float additionalLayerDepth;
        public virtual void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            if (Status == Condition.InHangar || Status == Condition.Repairing) return;

            spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, null, Color, MathHelper.PiOver2 + Angle, Sprite.TextureOrigin, Scale, SpriteEffects.None, DrawOrder.Flyer+additionalLayerDepth);
            for (int i = 0; i < track.Count; i++)
            {
                float f = i / (float)TrackLenght;
                Basic.DrawRectangle(spriteBatch, track[i] + hud.Camera, 1, 1, new Color(f, 0.5f * f, 0, f), DrawOrder.Flyer-0.01f + additionalLayerDepth);
            }
        }
    }
}