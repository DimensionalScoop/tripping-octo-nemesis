using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;
using System.Diagnostics;


namespace TrippingOctoNemesis.Extensions.Test1
{
    public class DebugInterfaceOverlay:DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        long frames;
        Font font = new Font("font");

        public DebugInterfaceOverlay(Game game) : base(game) { }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

            frames++;
            spriteBatch.DrawString(font, "Speed: " + (int)gameTime.ElapsedGameTime.TotalMilliseconds + "/" + (int)Game.TargetElapsedTime.TotalMilliseconds, Vector2.Zero, gameTime.IsRunningSlowly ? Color.Red : Color.Green);
            spriteBatch.DrawString(font, "\nPerformance: " + (int)(frames / gameTime.TotalGameTime.TotalSeconds * 100), Vector2.Zero, Color.WhiteSmoke);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
