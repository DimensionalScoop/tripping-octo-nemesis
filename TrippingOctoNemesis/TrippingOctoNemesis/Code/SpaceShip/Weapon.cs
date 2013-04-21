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
    public class Weapon
    {
        protected static Random Random = new Random();
        protected static Sprite Target = new Sprite("i\\target");

        public SpaceShip Owner;

        public int Damage = 2;
        public int RangeSquared = 1000 ^ 2;
        public Vector2 WeaponPosition;
        public TimeSpan WeaponCooldown = TimeSpan.FromMilliseconds(2500);

        protected TimeSpan LastShoot;
        protected TimeSpan ShootDuration = TimeSpan.FromMilliseconds(150);
        protected TimeSpan ExplosionDuration = TimeSpan.FromMilliseconds(250);
        protected Vector2 LaserVector;
        protected bool TargetInRange;
        protected Color LaserColor = Color.PaleVioletRed;
        protected float FireAngle;
        protected SpaceShip SelectedTarget;

        protected const int ExplosionPixels = 10;


        public Weapon(SpaceShip owner,GameTime gameTime)
        {
            Owner = owner;
            LastShoot = gameTime.ElapsedGameTime + TimeSpan.FromMilliseconds(WeaponCooldown.TotalMilliseconds * Random.NextFloat());
        }

        public void Update(GameTime gameTime)
        {
            if (Owner.TargetShip != null  && gameTime.TotalGameTime > LastShoot + WeaponCooldown)
            {
                TargetInRange = true;
                SelectedTarget=Owner.TargetShip;
                LastShoot = gameTime.TotalGameTime;
                LaserVector = SelectedTarget.Sprite.TextureOrigin.Rotate(Random.NextFloat() * MathHelper.TwoPi)/2*Random.NextFloat();
                FireAngle = SelectedTarget.Position.Angle(Owner.Position);// +MathHelper.PiOver2;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            if (SelectedTarget == null) return;

            //if (Owner.TargetShip != null && TargetInRange)
            //    if (gameTime.TotalGameTime > LastShoot && gameTime.TotalGameTime < LastShoot + ShootDuration)

            if (gameTime.TotalGameTime > LastShoot + ShootDuration && gameTime.TotalGameTime < LastShoot + ShootDuration + ExplosionDuration)
            {
                float t = (float)((gameTime.TotalGameTime - LastShoot - ShootDuration).TotalSeconds / ExplosionDuration.TotalSeconds);
                Debug.Assert(t >= 0 && t <= 1);

                var source = hud.Camera + Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = hud.Camera + SelectedTarget.Position + LaserVector;

                var spos = Vector2.Lerp(source, target, 1+(float)(((gameTime.TotalGameTime - LastShoot).TotalSeconds / ShootDuration.TotalSeconds)-1)/10);

                for (int i = 0; i < ExplosionPixels; i++)
                    Basic.DrawRectangle(spriteBatch, spos + LaserVector + new Vector2(10, 10) * new Vector2(Random.NextFloat() - 0.5f, Random.NextFloat() - 0.5f), 1, 1,
                        new Color(Random.NextFloat()*0.5f+t/2f, Random.NextFloat()*0.5f+t/2f, t, 0.5f)
                        , DrawOrder.Bullet);
            }

            //HA: Ph: Nr.9 195

            if(gameTime.TotalGameTime > LastShoot && gameTime.TotalGameTime < LastShoot + ShootDuration)
            {
                var t = (float)((gameTime.TotalGameTime - LastShoot).TotalSeconds / ShootDuration.TotalSeconds);
                Debug.Assert(t >= 0 && t <= 1);

                var source=hud.Camera + Owner.Position + WeaponPosition.Rotate(MathHelper.PiOver2 + Owner.Angle);
                var target = hud.Camera + SelectedTarget.Position + LaserVector;

                var spos=Vector2.Lerp(source,target,t);
                var tpso = spos.Transform(target.Angle(source), 10);//(target - source)/10+target;//spos.Transform(FireAngle , 15);

                Basic.DrawLine(spriteBatch,
                    spos,
                    tpso,
                    new Color(Owner.Fraction.Color.R,Owner.Fraction.Color.G,Owner.Fraction.Color.B,
                        1f//(float)Math.Sin(t*MathHelper.TwoPi*3)
                        ), DrawOrder.Bullet);

                //Basic.DrawLine(spriteBatch,
                //    source,
                //    target,
                //    new Color(Owner.Fraction.Color.R, Owner.Fraction.Color.G, Owner.Fraction.Color.B,
                //        0.2f//(float)Math.Sin(t*MathHelper.TwoPi*3)
                //        ), DrawOrder.Bullet);
            }

            //spriteBatch.Draw(Target, hud.Camera+Owner.TargetShip.Position, null, Owner.Fraction.Color, 0, Target.TextureOrigin, 1, SpriteEffects.None, DrawOrder.Bullet);
        }
    }
}