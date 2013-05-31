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

namespace TrippingOctoNemesis.Particles
{
    public class Text:Particle
    {
        protected string String;

        protected static readonly Font font = new Font("font");

        public Text(Vector2 position, string text, float durationInSeconds,Color color)
        {
            String = text;
            Color = color;
            Origin = position;
            MaxAge = TimeSpan.FromSeconds(durationInSeconds);
            LayerDepth = DrawOrder.Flyer + 0.1f;
        }
        //FIX: weapon cooldown randomization.
        protected override Color CalcColor(GameTime gameTime)
        {
            return new Color(Color.R, Color.G, Color.B, (byte)(Color.A * (1 - RelativeAge(gameTime))));
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 camera, GameTime gameTime)
        {
            //FIX: particle.draw() should not be called at all when RelativeAge(gameTime) > 1 (overwritten draw() does not do that).
            if (RelativeAge(gameTime) <= 1)
            spriteBatch.DrawText(String, CalcPosition(gameTime,camera), true, font, CalcColor(gameTime),LayerDepth);

            base.Draw(spriteBatch, camera, gameTime);
        }
    }
}
