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
        public Vector2 CameraSpeed = new Vector2(0, 1000/16);
        public Vector2 ScreenSize;

        public KeyProvider Key;


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

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            CameraDelta = CameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Camera += CameraDelta;
        }
    }
}