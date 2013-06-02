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
using System.Diagnostics;

namespace TrippingOctoNemesis
{
    public abstract class Weapon              
    {
        protected static Random Random = new Random();
        protected static Sprite TargetIcon = new Sprite("i\\target");

        public SpaceShip Owner;

        public string Name;

        public SpaceShip Target;
        public int TargetDistanceSquared;

        public int Damage;
        public int RangeSquared;
        public TimeSpan Cooldown;
        public TimeSpan ShellingDuration;
        /// <summary>
        /// If false, the weapon waits when it has reloaded if there is no legal target and fires immediately if a target becomes available.
        /// If true, the weapon does not wait after it has reloaded but cools down as if it had shoot.
        /// Use the former for matter based weapons (bullets, missiles) or weapons with a large cooldown and the latter for energy based weapons (laser, ion).
        /// </summary>
        public bool Cycling;
        /// <summary>
        /// The position from which the lasers, missiles, etc. are emitted from. Only influences the visible effect, not the (e.g. range) calculations.
        /// </summary>
        public Vector2 WeaponPosition;
        /// <summary>
        /// The position of the weapon impact. Only influences the visible effect, not the (e.g. range) calculations.
        /// </summary>
        protected Vector2 TargetVarriation;

        public TimeSpan LastCooldown;

        public enum Conditions { CoolingDown, StartingShelling, Shelling, EndingShelling }
        public Conditions Status;

        public delegate bool TargetMethod();
        public TargetMethod TargetSelector;


        public Weapon(SpaceShip owner)
        {
            Owner = owner;
            Status =  Conditions.CoolingDown;
        }

        /// <summary>
        /// Randomizes the initial cooldown (not the cooldown duration). Otherwise all ships with the same weapon created at the same time also shoot at the same time.
        /// </summary>
        protected void AssignRandomCooldownPosition()
        {
            LastCooldown = GameControl.LastUpdate.TotalGameTime + TimeSpan.FromMilliseconds(Cooldown.TotalMilliseconds * Random.NextFloat());
        }

        public void Update(GameTime gameTime)
        {
            switch (Status)
            {
                case Conditions.CoolingDown:
                    if (gameTime.TotalGameTime - LastCooldown > Cooldown)
                        if (Cycling || (Target != null&&TargetDistanceSquared<=RangeSquared))
                            Status = Conditions.StartingShelling;
                    break;

                case Conditions.StartingShelling:
                    StartedShelling(gameTime);
                    Status = Conditions.Shelling;
                    break;

                case Conditions.Shelling:
                    if (gameTime.TotalGameTime - LastCooldown - Cooldown > ShellingDuration)
                        Status = Conditions.EndingShelling;
                    break;

                case Conditions.EndingShelling:
                    EndedShelling(gameTime);
                    Status = Conditions.CoolingDown;
                    LastCooldown = gameTime.TotalGameTime;
                    break;

                default: throw new NotImplementedException();
            }
        }

        public void LongUpdate(TimeSpan elsapsedTime)
        {
            if (Status == Conditions.CoolingDown)
                TargetSelector();
        }

        protected virtual void EndedShelling(GameTime gameTime)
        {
            if (Target != null&&TargetDistanceSquared<=RangeSquared)
                Target.DealDamage(-Damage, Target.Position+TargetVarriation);
        }

        protected virtual void StartedShelling(GameTime gameTime) 
        {
            if(Target!=null && TargetDistanceSquared<=RangeSquared)
            TargetVarriation = Target.Sprite.TextureOrigin.Rotate(Random.NextFloat() * MathHelper.TwoPi) / 2 * Random.NextFloat();
        }

        #region Targeting
        public bool TargetNearestEnemy()
        {
            return TargetNearest(p => Owner.Fraction.IsEnemy(p.Fraction));
        }

        public bool TagetNearestEnemyCarrier()
        {
            return TargetNearest(p => Owner.Fraction.IsEnemy(p.Fraction) && p.Class == SpaceShip.ShipClasses.Carrier);
        }

        /// <summary>
        /// Targets the nearest ship that meets the condition defined in selector.
        /// </summary>
        /// <param name="selector"></param>
        public bool TargetNearest(Func<SpaceShip, bool> selector)
        {
            Target = null;
            TargetDistanceSquared = int.MaxValue;
            int range;

            foreach (var ship in GameControl.Ships)
                if (ship.IsAirborne && !ship.DeleteFlag && selector(ship))
                {
                    range = (int)Vector2.DistanceSquared(ship.Position, Owner.Position);
                    if (range < TargetDistanceSquared)
                    {
                        Target = ship;
                        TargetDistanceSquared = range;
                    }
                }

            if (Target != null) 
                return true; 
            return false;
        }

        /// <summary>
        /// Targets the ship with the fewest hitpoints that meets the condition defined in selector.
        /// </summary>
        /// <param name="selector"></param>
        public bool TargetLowestHitpoints(Func<SpaceShip, bool> selector)
        {
            Target = null;
            int hitpoints = int.MaxValue;

            foreach (var ship in GameControl.Ships)
                if (ship.IsAirborne && !ship.DeleteFlag && selector(ship))
                {
                    if (ship.Hitpoints < hitpoints)
                    {
                        Target = ship;
                        hitpoints = ship.Hitpoints;
                    }
                }

            if (Target != null)
            {
                RangeSquared = (int)Vector2.DistanceSquared(Target.Position, Owner.Position);
                return true;
            }
            return false;
        }

        public SpaceShip SingleTarget;
        public bool TargetSingle()
        {
            Debug.Assert(SingleTarget != null);

            Target = SingleTarget;
            if (Target != null)
            {
                RangeSquared = (int)Vector2.DistanceSquared(Target.Position, Owner.Position);
                return true;
            }
            return false;
        }
        #endregion

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}