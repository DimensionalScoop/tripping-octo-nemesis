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

namespace TrippingOctoNemesis.Particles
{
    public class DestroyedShip:Particle
    {
        Vector2 momentum;
        Vector2 leftDrift;
        Vector2 rightDrift;

        public DestroyedShip(Vector2 origin,Vector2 momentum, SpriteSheet sprite)
        {
            Origin = origin;
            Sprite = sprite;
            this.momentum = momentum/10f;
            MaxAge = TimeSpan.FromSeconds(15);
            var angle = MathHelper.PiOver2 * 1.5f + Random.NextFloat() * (MathHelper.Pi - MathHelper.PiOver2 * 0.5f);
            leftDrift = Vector2.Zero.Transform(angle, 5);
            rightDrift = Vector2.Zero.Transform(Random.NextFloat() * MathHelper.PiOver4 - angle - MathHelper.PiOver4 / 2, 1);
        }

        protected override Color CalcColor(GameTime gameTime)
        {
            return new Color(Color.R, Color.G, Color.B, 255);
        }

        protected override void PreDrawUpdate(GameTime gameTime)
        {
            if (Random.NextFloat() > 0.99f)
            {
                var side = Vector2.Zero;
                if (Random.Coin())
                    side = Origin + leftDrift * (float)Age(gameTime).TotalSeconds + momentum * (float)Age(gameTime).TotalSeconds - Sprite.TextureOrigin;
                else
                    side = Origin + rightDrift * (float)Age(gameTime).TotalSeconds + new Vector2(Sprite.TextureOrigin.X, 0) + momentum * (float)Age(gameTime).TotalSeconds - Sprite.TextureOrigin;

                    Particle.Add(new Particles.MultiBlast(side, (int)Sprite.TextureOrigin.Length() * 2));
            }

            base.PreDrawUpdate(gameTime);
        }
        
        
        //FIX: Spaceships firing at already dead ships
        //FIX: Display of sliced/dead SpriteSheet ships showing (random) parts SpriteSheet instead of single ships
        //FIX: Correct sliced/dead rotation
        //FIX: Multiple sprites showing after destruction

        public override void Draw(SpriteBatch spriteBatch, Vector2 camera, GameTime gameTime)
        {
            if (RelativeAge(gameTime) > 1) { Delete(); return; }
            if (RelativeAge(gameTime) < 0) return;

            PreDrawUpdate(gameTime);

            if (Sprite != null)
            {
                var left = leftDrift * (float)Age(gameTime).TotalSeconds+Vector2.Zero;
                var right = rightDrift * (float)Age(gameTime).TotalSeconds;

                spriteBatch.Draw(
                    Sprite,
                    CalcPosition(gameTime, camera) + left + momentum * (float)Age(gameTime).TotalSeconds - new Vector2(Sprite.TextureOrigin.X / 2,0),
                    new Rectangle(0, 0, (int)Sprite.TextureOrigin.X, (int)Sprite.Texture.Height),
                    CalcColor(gameTime),
                    CalcRotation(gameTime),
                    new Vector2(Sprite.TextureOrigin.X/2, Sprite.TextureOrigin.Y),
                    CalcScale(gameTime),
                    SpriteEffects.None,
                    0);

                spriteBatch.Draw(
                    Sprite,
                    CalcPosition(gameTime, camera) + right + new Vector2(Sprite.TextureOrigin.X, 0) + momentum * (float)Age(gameTime).TotalSeconds - new Vector2(Sprite.TextureOrigin.X / 2,0),
                    new Rectangle((int)Sprite.TextureOrigin.X, 0, (int)Sprite.TextureOrigin.X + 1, (int)Sprite.Texture.Height),
                    CalcColor(gameTime),
                    CalcRotation(gameTime),
                    new Vector2(Sprite.TextureOrigin.X/2,Sprite.TextureOrigin.Y),
                    CalcScale(gameTime),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
