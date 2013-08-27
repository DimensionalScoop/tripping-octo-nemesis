using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using TrippingOctoNemesis.Communicator;
using TrippingOctoNemesis.Extensions;

namespace TrippingOctoNemesis
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            graphics.PreferredBackBufferWidth = 1360;
            graphics.PreferredBackBufferHeight = 730;
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            IsMouseVisible = true;
#if DEBUG
            TargetElapsedTime = TimeSpan.FromMilliseconds(30);
#else
            TargetElapsedTime = TimeSpan.FromMilliseconds(30);
#endif
            graphics.ApplyChanges();

            Components.Add(new GameControl(this));
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.Initialize(Content, GraphicsDevice);
            Font.Initialize(Content);

            #if DEBUG
            //var window = System.Windows.Forms.Form.FromHandle(Window.Handle);
            //window.Location = new System.Drawing.Point(-1366, 200);
            #endif

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)                                    
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(30,30,30));

            base.Draw(gameTime);
        }
    }
}