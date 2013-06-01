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
    public class Weapon              
    {
        protected static Random Random = new Random();
        protected static Sprite TargetIcon = new Sprite("i\\target");

        public SpaceShip Owner;

        public SpaceShip Target;
        public int TargetDistanceSquared;

        public int Damage = 2;
        public int RangeSquared = (int)Math.Pow(200,2);
        /// <summary>
        /// The position from which the lasers, missiles, etc. are emitted from. Only influences the visible effect, not the (e.g. range) calculations.
        /// </summary>
        public Vector2 WeaponPosition;
        public TimeSpan WeaponCooldown = TimeSpan.FromMilliseconds(2500);
        /// <summary>
        /// If false, the weapon waits when it has reloaded if there is no legal target and fires immediately if a target becomes available.
        /// If true, the weapon does not wait after it has reloaded but cools down as if it had shoot.
        /// Use the former for matter based weapons (bullets, missiles) or weapons with a large cooldown and the latter for energy based weapons (laser, ion).
        /// </summary>
        public bool Cycling;

        protected TimeSpan LastShoot;
        protected TimeSpan ShootDuration = TimeSpan.FromMilliseconds(150);
        protected TimeSpan ExplosionDuration = TimeSpan.FromMilliseconds(250);
        protected Vector2 TargetVariation;
        protected bool TargetInRange;
        protected SpaceShip SelectedTarget;
        protected bool DisplayShoot;
        protected bool DisplayMiniDerbis;

        protected const int ExplosionPixels = 10;


        public enum Conditions { CoolingDown, Shelling }
        public Conditions Status;


        public Weapon(SpaceShip owner)
        {
            Owner = owner;
            Status=
            LastShoot =  GameControl.LastUpdate.TotalGameTime + TimeSpan.FromMilliseconds(WeaponCooldown.TotalMilliseconds * Random.NextFloat());
        }

        bool firstUpdateAfterShootHitTarget;
        public void Update(GameTime gameTime)
        {


#if Old
            if (Owner.TargetShip != null  && Owner.TargetShip.Hitpoints>0 && RangeSquared>=Owner.TargetShipDistanceSquared&&gameTime.TotalGameTime > LastShoot + WeaponCooldown)
            {
                DisplayShoot = true;
                DisplayMiniDerbis = false;
                firstUpdateAfterShootHitTarget = false;
                TargetInRange = true;
                SelectedTarget=Owner.TargetShip;
                TargetVariation = SelectedTarget.Sprite.TextureOrigin.Rotate(Random.NextFloat() * MathHelper.TwoPi) / 2 * Random.NextFloat();
            }
            else if (Owner.TargetShip == null && gameTime.TotalGameTime > LastShoot + WeaponCooldown)
            {
                TargetInRange = false;
                SelectedTarget = null;
            }//  

            if (SelectedTarget!=null && !firstUpdateAfterShootHitTarget && RangeSquared>=Owner.TargetShipDistanceSquared && gameTime.TotalGameTime > LastShoot && gameTime.TotalGameTime < LastShoot + ShootDuration)
            {
                DisplayShoot = false;
                DisplayMiniDerbis = true;
                SelectedTarget.DealDamage(-Damage, SelectedTarget.Position + TargetVariation);
                firstUpdateAfterShootHitTarget = true;
            }

            if(gameTime.TotalGameTime > LastShoot + WeaponCooldown)
                LastShoot = gameTime.TotalGameTime;
#endif
        }


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
                return true;
            return false;
        }


        public void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            if (SelectedTarget == null) return;

            if (DisplayMiniDerbis && gameTime.TotalGameTime > LastShoot + ShootDuration && gameTime.TotalGameTime < LastShoot + ShootDuration + ExplosionDuration)
            {
                float t = (float)((gameTime.TotalGameTime - LastShoot - ShootDuration).TotalSeconds / ExplosionDuration.TotalSeconds);
                Debug.Assert(t >= 0 && t <= 1);

                var source = hud.Camera + Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = hud.Camera + SelectedTarget.Position + TargetVariation;

                var spos = Vector2.Lerp(source, target, 1+(float)(((gameTime.TotalGameTime - LastShoot).TotalSeconds / ShootDuration.TotalSeconds)-1)/10);

                for (int i = 0; i < ExplosionPixels; i++)
                    spriteBatch.DrawRectangle(spos + TargetVariation + new Vector2(10, 10) * new Vector2(Random.NextFloat() - 0.5f, Random.NextFloat() - 0.5f), 1, 1,
                        new Color(Random.NextFloat()*0.5f+t/2f, Random.NextFloat()*0.5f+t/2f, t, 0.5f)
                        , DrawOrder.Bullet);
            }

            if(DisplayShoot && gameTime.TotalGameTime > LastShoot && gameTime.TotalGameTime < LastShoot + ShootDuration)
            {
                var t = (float)((gameTime.TotalGameTime - LastShoot).TotalSeconds / ShootDuration.TotalSeconds);
                Debug.Assert(t >= 0 && t <= 1);

                var source=hud.Camera + Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = hud.Camera + SelectedTarget.Position + TargetVariation;

                var spos=Vector2.Lerp(source,target,t);
                var tpso = spos.Transform(target.Angle(source), 10);//(target - source)/10+target;//spos.Transform(FireAngle , 15);

                spriteBatch.DrawLine(spos,tpso,
                    new Color(Owner.Fraction.Color.R,Owner.Fraction.Color.G,Owner.Fraction.Color.B,
                        1f//(float)Math.Sin(t*MathHelper.TwoPi*3)
                        ), DrawOrder.Bullet);
            }
        }
    }
}