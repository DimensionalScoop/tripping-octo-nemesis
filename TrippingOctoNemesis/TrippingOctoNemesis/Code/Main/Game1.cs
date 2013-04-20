using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;

namespace TrippingOctoNemesis
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        InputProvider _input;
        EffectProvider _effect;
        StarField stars;

        Player[] Player = new Player[2];
        Hud hud;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _input = new InputProvider(this);
            _effect = EffectProvider.Initialize(this);
            stars = new StarField(this, 2, 1, 0.5f);
            hud = new Hud(this);

            _graphics.PreferredBackBufferWidth = 1360;
            _graphics.PreferredBackBufferHeight = 730;
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(30);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.Initialize(Content, GraphicsDevice);
            Font.Initialize(Content);

            CreatePlayer(0,ControlKeySettings.DefaultPlayerOne());
            CreatePlayer(1, ControlKeySettings.DefaultPlayerTwo());
        }

        private void CreatePlayer(int p,ControlKeySettings keys)
        {
            Player[p] = new Player() { Keys = keys };

            var motherShip = new MotherShip(hud, Player[p]) { Position = new Vector2(300 + 300 * p, 500) };
            Player[p].AssignMotherShip(motherShip);

            for (int i = 0; i < 4; i++)
            {
                var ship = new SpaceShip(Player[p]) { Carrier = motherShip };
                Player[p].AddShip(ship);
                motherShip.Slots[i] = new DeploySlots(ship);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (_input.Key.KeysStroked.Contains(Keys.Escape))
                Exit();

            stars.MoveCamera(new Vector2(0,1));

            foreach (var elem in Player) elem.Update(gameTime,hud);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(30,30,30));

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.NonPremultiplied);

            foreach (var elem in Player) elem.Draw(spriteBatch,hud,gameTime);

            spriteBatch.End();
        }
    }
}
