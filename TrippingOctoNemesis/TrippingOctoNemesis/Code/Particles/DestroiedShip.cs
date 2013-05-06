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

namespace TrippingOctoNemesis.Particles
{
    public class DestroiedShip:Particle
    {
        float firstAngle;
        float secondAngle;

        public DestroiedShip(Vector2 origin,SpriteSheet sprite)
        {
            Origin = origin;
            Sprite = sprite;
            MaxAge = TimeSpan.FromSeconds(2);
        }

        protected override Color CalcColor(GameTime gameTime)
        {
            return new Color(Color.R, Color.G, Color.B, 1 - Color.A * RelativeAge(gameTime));
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 camera, GameTime gameTime)
        {
            base.Draw(spriteBatch, camera, gameTime);
        }
    }
}
