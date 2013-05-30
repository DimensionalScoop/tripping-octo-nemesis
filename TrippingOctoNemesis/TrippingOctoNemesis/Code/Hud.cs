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

namespace TrippingOctoNemesis
{
    public class Hud : DrawableGameComponent
    {
        public Vector2 Camera;
        public Vector2 CameraDelta;
        public Vector2 CameraSpeed = new Vector2(0, 1000 / 16);
        public Vector2 ScreenSize;

        public KeyProvider Key;
        SpriteBatch spriteBatch;

        static readonly Font font = new Font("f\\sfont");


        public Hud(Game game)
            : base(game)
        {
            Game.Components.Add(this);
            Game.Services.AddService(typeof(Hud), this);
        }

        protected override void LoadContent()
        {
            Key = Game.Services.GetService(typeof(KeyProvider)) as KeyProvider;
            ScreenSize = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            CameraDelta = CameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Camera += CameraDelta;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

#if DEBUG
            string info = "";

            info += "Warp Factor: " + (int)(GameControl.Stars.WarpFactor*100)+"%";
            info += "\nCamera: " + Camera;
            info += "\nPlayer: " + GameControl.Ships.Find(p => p.Fraction == GameControl.Player[0]).Position;
            if (GameControl.Ships.Find(p => p.Fraction.IsEnemy(GameControl.Player[0])) != null)
            {
                var enepos = GameControl.Ships.Find(p => p.Fraction.IsEnemy(GameControl.Player[0])).Position;
                info += "\nFirst Enemy Wing: " + enepos;
                if (new Rectangle((int)Camera.X, (int)-Camera.Y, (int)ScreenSize.X, (int)ScreenSize.Y).Contains((int)enepos.X, (int)enepos.Y))
                    info += " (visible)";
            }


            spriteBatch.DrawText(info, new Vector2(ScreenSize.X/2-100, 50), false, font, Color.White);

#endif

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}