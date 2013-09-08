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
using System.Diagnostics;

namespace TrippingOctoNemesis.SPS
{
    public abstract class Missile : SpaceShip
    {
        public int ExplosionDamage;
        public float SpeedGainPerSecond;
        public float MaxSpeed;
        public int ExplosionRangeSquared;
        public TimeSpan MaxRange;

        protected List<TimeSpan> StackEvents = new List<TimeSpan>();
        protected List<Action> StackEventsReaction = new List<Action>();

        public Color ExplosionColor;
        public int ExplosionSize;

        protected Func<Missile, SpaceShip> TargetSelector;

        public Missile(Fraction fraction, Func<Missile,SpaceShip> targetSelector)
            : base(fraction)
        {
            Class = ShipClasses.Missile;
            Weapon = new DummyWeapon(this);
            TargetSelector = targetSelector;
            PreciseAngleSpeed = true;
            ReachedTarget += Missile_ReachedTarget;
            StatusChanged += Missile_StatusChanged;
        }

        void Missile_StatusChanged(SpaceShip obj)
        {
            if (!IsSpawned) return;

            switch (Status)
            {
                case Conditions.Airborne:
                    LaunchTime = GameControl.LastUpdate.TotalGameTime;
                    Ki = new FixedEnemy(TargetSelector(this));
                    break;

                case Conditions.ReturningPhase1:
                case Conditions.ReturningPhase2:
                    Delete(DeleteReasons.SelfDestruction);
                    Debugger.Break();
                    break;
            }
        }

        public override void Spawn()
        {
            if (IsAirborne)
            {
                LaunchTime = GameControl.LastUpdate.TotalGameTime;
                Ki = new FixedEnemy(TargetSelector(this));
            }
            base.Spawn();
        }

        void Missile_ReachedTarget(SpaceShip obj)
        {
            if (IsAirborne)
                Delete(DeleteReasons.SelfDestruction);
        }

        public override void Delete(DeleteReasons reason = DeleteReasons.Destroyed)
        {
            if (reason == DeleteReasons.SelfDestruction)
            {
                Particle.Add(new Particles.Explosion(Position, ExplosionSize) { Color = ExplosionColor });

                foreach (var ship in GameControl.Ships)
                    if (ship!=this && Vector2.DistanceSquared(ship.Position, Position) <= ExplosionRangeSquared)
                        ship.DealDamage(-ExplosionDamage, Vector2.Zero);
            }

            base.Delete(reason);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAirborne)
            {
                Speed = SpeedGainPerSecond * (float)(gameTime.TotalGameTime - LaunchTime).TotalSeconds;


                for (int i = 0; i < StackEvents.Count; i++)
                {
                    if (gameTime.TotalGameTime - LaunchTime > StackEvents[i])
                    {
                        StackEventsReaction[i]();
                        StackEvents.RemoveAt(i);
                        StackEventsReaction.RemoveAt(i);
                        break;
                    }
                }

                if (gameTime.TotalGameTime - LaunchTime > MaxRange) Delete(DeleteReasons.SelfDestruction);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            base.Draw(spriteBatch, hud, gameTime);


            var pos = Position+GameControl.Hud.Camera+ new Vector2(20, -8);
            var text="Incoming Missile\nETA " + (int)((Position - TargetPosition).Length() / Speed * 10) + " t";

            if (!IsVisible)
            {
                if (pos.X < 0) pos.X = 0;
                if (pos.Y < 0) pos.Y = 0;
                if (pos.Y > GameControl.Hud.ScreenSize.Y - font.SpriteFont.LineSpacing * 2) pos.Y = GameControl.Hud.ScreenSize.Y - font.SpriteFont.LineSpacing * 2;
                if (pos.X > GameControl.Hud.ScreenSize.X - font.SpriteFont.MeasureString(text).X) pos.X = GameControl.Hud.ScreenSize.X - font.SpriteFont.MeasureString(text).X;
            }

            spriteBatch.DrawText(text, pos, false, font, new Color(255, 150, 150), DrawOrder.UI);
        }
        Font font = new Font("font");
    }
}