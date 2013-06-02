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

namespace TrippingOctoNemesis
{
    public class Laser : Weapon
    {
        protected readonly TimeSpan DebrisDuration = TimeSpan.FromMilliseconds(250);
        protected readonly int CountDebris = 10;
        protected SpaceShip OldTarget;

        public Laser(SpaceShip owner, Vector2 weaponPosition)
            : base(owner)
        {
            WeaponPosition = weaponPosition;

            Name = "Laser";
            Cycling = true;
            Damage = 2;
            RangeSquared = (int)Math.Pow(200, 2);
            Cooldown = TimeSpan.FromMilliseconds(2750);
            ShellingDuration = TimeSpan.FromMilliseconds(150);
            TargetSelector = TargetNearestEnemy;
            AssignRandomCooldownPosition();
        }

        protected override void EndedShelling(GameTime gameTime)
        {
            OldTarget = Target;
            if (TargetDistanceSquared > RangeSquared) OldTarget = null;

            base.EndedShelling(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Status == Conditions.Shelling && Target != null && TargetDistanceSquared <= RangeSquared)
            {
                var t = (float)((gameTime.TotalGameTime - LastCooldown - Cooldown).TotalSeconds / ShellingDuration.TotalSeconds);
                if (t > 1) t = 1;

                var source = Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = Target.Position + TargetVarriation;

                var spos = Vector2.Lerp(source, target, t);
                var tpso = spos.Transform(target.Angle(source), 10);//(target - source)/10+target;//spos.Transform(FireAngle , 15);

                spriteBatch.DrawLine(spos + GameControl.Hud.Camera, tpso + GameControl.Hud.Camera,
                    new Color(Owner.Fraction.Color.R, Owner.Fraction.Color.G, Owner.Fraction.Color.B,
                        1f//(float)Math.Sin(t*MathHelper.TwoPi*3)
                        ), DrawOrder.Bullet);
            }

            if (Status == Conditions.CoolingDown && OldTarget != null && gameTime.TotalGameTime-LastCooldown<DebrisDuration)
            {
                float t = (float)((gameTime.TotalGameTime - LastCooldown).TotalSeconds / DebrisDuration.TotalSeconds);
                if (t > 1) t = 1;

                var source = Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = OldTarget.Position + TargetVarriation;

                var spos = Vector2.Lerp(source, target, 1+(t)/ 5);
                //var spos = Vector2.Lerp(source, target, 1+(float)(((gameTime.TotalGameTime - LastShoot).TotalSeconds / ShootDuration.TotalSeconds)-1)/10);

                for (int i = 0; i < CountDebris; i++)
                    spriteBatch.DrawRectangle(GameControl.Hud.Camera+ spos + TargetVarriation + new Vector2(10, 10) * new Vector2(Random.NextFloat() - 0.5f, Random.NextFloat() - 0.5f), 1, 1,
                        new Color(Random.NextFloat() * 0.5f + t / 2f, Random.NextFloat() * 0.5f + t / 2f, t, 0.5f)
                        , DrawOrder.Bullet);
            }
            
            base.Draw(spriteBatch, gameTime);
        }
    }
}