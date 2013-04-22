﻿using System;
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

        List<SpaceShip> Ships = new List<SpaceShip>();
        List<Fraction> Fractions = new List<Fraction>();
        Player[] Player = new Player[2];
        Fraction[] Enemys = new Fraction[1];
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
            _graphics.PreferMultiSampling = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(30);
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            GameTime gameTime = new GameTime();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.Initialize(Content, GraphicsDevice);
            Font.Initialize(Content);

            CreatePlayer(0, ControlKeySettings.DefaultPlayerOne(), gameTime);
            CreatePlayer(1, ControlKeySettings.DefaultPlayerTwo(), gameTime);
            Player[0].Allys |= Player[1].Id;
            Player[1].Allys |= Player[0].Id;
            CreateEnemy(0, gameTime);

            Fractions.AddRange(Player);
            Fractions.AddRange(Enemys);

            Ships.Add(new OctoJelly(Player[1], gameTime) { Position = new Vector2(200, 200), HasTarget = false,Status= SpaceShip.Condition.Airborne, Angle = -MathHelper.PiOver2 });
        }

        private void CreateEnemy(int p, GameTime gameTime)
        {
            Enemys[0] = new Fraction();
            Ships.Add(new D1Enemy(hud, Enemys[0], gameTime) { Position = new Vector2(300, -200), TargetPosition = new Vector2(500, 200) });
        }

        private void CreatePlayer(int p,ControlKeySettings keys,GameTime gameTime)
        {
            Player[p] = new Player() { Keys = keys };

            var motherShip = new MotherShip(hud, Player[p],gameTime) { Position = new Vector2(300 + 300 * p, 500) };
            Player[p].AssignMotherShip(motherShip);
            Ships.Add(motherShip);

            for (int i = 0; i < 4; i++)
            {
                var ship = new SpaceShip(Player[p], gameTime) { Carrier = motherShip };
                Player[p].AddShip(ship);
                Ships.Add(ship);
                motherShip.Slots[i] = new DeploySlots(ship);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (_input.Key.KeysStroked.Contains(Keys.Escape))
                Exit();

            stars.MoveCamera(new Vector2(0,1));

            LongUpdate(gameTime);
            Ships.ForEach(p=>p.Update(gameTime, hud, Ships));
            Fractions.ForEach(p=>p.Update(gameTime,hud));

            base.Update(gameTime);
        }



        const int worldUpdatesPerSecond = 10;
        TimeSpan lastLongUpdateDuration;
        TimeSpan lastFirstLongUpdate;
        int updaterPosition = 0;
        private void LongUpdate(GameTime gameTime)
        {
            int possibleUpdates = (int)(gameTime.ElapsedGameTime.TotalSeconds * worldUpdatesPerSecond * Ships.Count);

            while (possibleUpdates > 0)
            {
                possibleUpdates--;
                updaterPosition++;
                if (updaterPosition >= Ships.Count)
                {
                    updaterPosition = 0;
                    lastLongUpdateDuration = gameTime.TotalGameTime - lastFirstLongUpdate;
                    lastFirstLongUpdate = gameTime.TotalGameTime;
                }

                Ships[updaterPosition].LongUpdate(lastLongUpdateDuration,hud,Ships);
            }
        }


        Font font = new Font("font");
        long frames;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(30,30,30));

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.FrontToBack,BlendState.NonPremultiplied);

            Fractions.ForEach(p=>p.Draw(spriteBatch,hud,gameTime));
            Ships.ForEach(p => p.Draw(spriteBatch, hud, gameTime));

            frames++;
            spriteBatch.DrawString(font, "Speed: " + (int)gameTime.ElapsedGameTime.TotalMilliseconds + "/" + (int)TargetElapsedTime.TotalMilliseconds, Vector2.Zero, gameTime.IsRunningSlowly ? Color.Red : Color.Green);
            spriteBatch.DrawString(font, "\nPerformance: " + (int)(frames / gameTime.TotalGameTime.TotalSeconds * 100), Vector2.Zero, Color.WhiteSmoke);

            spriteBatch.End();
        }
    }
}
