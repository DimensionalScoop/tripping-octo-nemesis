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

namespace TrippingOctoNemesis
{
    struct Tentacle{public Sprite Sprite;public float Angle;public float AngleSpeed;}

    public class OctoNemesisEnemy:SpaceShip
    {
        static readonly Sprite eye = new Sprite("s\\ton-eye");
        static readonly Sprite[] tentacleSprites = new Sprite[] { new Sprite("s\\ton-ten-1"), new Sprite("s\\ton-ten-2") };

        List<Tentacle> tentacle = new List<Tentacle>();

        public OctoNemesisEnemy(Fraction fraction)
            : base(fraction)
        {
            Sprite = new SpriteSheet("s\\ton-eyeball");
            Status = Conditions.Airborne;
            //HasTarget = false;
            Speed = 1000/16f;
            Angle = -MathHelper.PiOver2;
            Class = ShipClasses.Carrier;

            var countTentacles=Random.Next(6,9);
            for (int i = 0; i < countTentacles; i++)
                tentacle.Add(new Tentacle() {
                    Sprite=tentacleSprites[Random.Next(tentacleSprites.Length)],
                    Angle=(0.5f-Random.NextFloat())*MathHelper.TwoPi*2,
                    AngleSpeed=(Random.NextFloat()+0.1f)*MathHelper.TwoPi*5
                });
        }

        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            foreach (var elem in tentacle)
                spriteBatch.Draw(elem.Sprite, Position + hud.Camera, null, Color.White, elem.Angle + elem.Angle * (float)gameTime.TotalGameTime.TotalSeconds, new Vector2(0, elem.Sprite.TextureOrigin.Y), Scale, SpriteEffects.None, DrawOrder.Flyer-0.01f);

            base.Draw(spriteBatch, hud, gameTime);

            var eyeColor = new Color(
             (byte)(Math.Sin(gameTime.TotalGameTime.Milliseconds/1000f * MathHelper.TwoPi) * 255 / 2 + 255 / 2),
             (byte)(Math.Cos(gameTime.TotalGameTime.Milliseconds / 1000f * MathHelper.TwoPi) * 255 / 2 + 255 / 2),
             (byte)(Math.Tan(gameTime.TotalGameTime.Milliseconds / 1000f * MathHelper.TwoPi) * 255 / 2 + 255 / 2),
             255);

            spriteBatch.Draw(eye, IntPosition ? (Position + hud.Camera).Round() : Position+hud.Camera, null, eyeColor, MathHelper.PiOver2 + Angle, eye.TextureOrigin, Scale, SpriteEffects.None, DrawOrder.Flyer+0.01f);
        }
    }
}
