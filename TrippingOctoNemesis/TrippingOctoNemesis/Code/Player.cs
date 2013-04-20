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
    class Player:Fraction
    {
        public List<SpaceShip> SpaceShips = new List<SpaceShip>();
        public MotherShip MotherShip;

        public ControlKeySettings Keys;

        Sprite hull = new Sprite("i\\hull");
        Sprite incLeft = new Sprite("i\\inc-left");
        Sprite incRight = new Sprite("i\\inc-right");
        Sprite engine = new Sprite("i\\engine");
        Sprite hullPoint = new Sprite("i\\hull-point");
        Sprite incPoint = new Sprite("i\\inc-point");
        Sprite engineBar = new Sprite("i\\engine-bar");

        static readonly Vector2 hullPosition = new Vector2(36, 40);
        static readonly Vector2 hullPointPosition = new Vector2(54, 30);
        static readonly Vector2[] incPosition = new Vector2[]{
            new Vector2(-43, 22),new Vector2(-27, -4),new Vector2(48,-5),
            new Vector2(63,22)
        };
        static readonly Vector2[] incPointPosition=new Vector2[]{
        new Vector2(13,0),new Vector2(13,0),new Vector2(26,0),new Vector2(26,0)
        };
        static readonly Vector2 enginePosition=new Vector2(21,99);
        static readonly Vector2 engineBarPosition = new Vector2(63, 5);

        TimeSpan showDamageTimer;
        TimeSpan[] showDeploySlotTimer = new TimeSpan[4];
        TimeSpan showEngineTimer;
        static readonly TimeSpan showTimeout = TimeSpan.FromSeconds(3);
        static readonly TimeSpan fadeDuration = TimeSpan.FromSeconds(0.4f);

        TimeSpan lastUpdate;


        public Player(MotherShip motherShip)
        {
            MotherShip = motherShip;
            SpaceShips.Add(motherShip);

            MotherShip.WasDamaged += MotherShip_WasDamaged;
        }

        void MotherShip_WasDamaged()
        {
            showDamageTimer = lastUpdate;
        }

        public void Update(GameTime gameTime,Hud hud)
        {
            lastUpdate = gameTime.TotalGameTime;
            base.Update(gameTime);

            SpaceShips.ForEach(p => p.Update(gameTime,hud));

            HandleInput(gameTime, hud);
        }

        private void HandleInput(GameTime gameTime, Hud hud)
        {
            if (hud.Key.KeysPressed.Contains(Keys.Up)) MotherShip.Position.Y -= MotherShip.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (hud.Key.KeysPressed.Contains(Keys.Down)) MotherShip.Position.Y += MotherShip.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (hud.Key.KeysPressed.Contains(Keys.Left)) MotherShip.Position.X -= MotherShip.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (hud.Key.KeysPressed.Contains(Keys.Right)) MotherShip.Position.X += MotherShip.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            throw new Exception();
        }

        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            base.Draw(spriteBatch, hud, gameTime);

            SpaceShips.ForEach(p => p.Draw(spriteBatch, hud, gameTime));

            if (gameTime.TotalGameTime < showDamageTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showDamageTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = MotherShip.Position + hud.Camera + hullPosition - MotherShip.Sprite.TextureOrigin;

                spriteBatch.Draw(hull, pos.Round(), col);
                for (int i = 0; i < 8 * (MotherShip.Hitpoints / MotherShip.MaxHitpoints); i++)
                    spriteBatch.Draw(hullPoint, (new Vector2(i * (hullPoint.Texture.Width + 1), 0) + pos + hullPointPosition).Round(), col);
            }


            for (int i = 0; i < 4; i++)
                if (gameTime.TotalGameTime < showDeploySlotTimer[i] + showTimeout)
                {
                    var t = gameTime.TotalGameTime - showDeploySlotTimer[i];
                    var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                    var pos = MotherShip.Position + hud.Camera + incPosition[i] - MotherShip.Sprite.TextureOrigin;
                    spriteBatch.Draw(i < 2 ? incLeft : incRight, pos.Round(), col);
                    for (int f = 0; f < MotherShip.Slots[i].TotalFlyers; f++)
                        spriteBatch.Draw(incPoint, (pos + incPointPosition[i] + new Vector2(f * (1 + incPoint.Texture.Width), 0)).Round(), new Color(MotherShip.Slots[i].Color[f].R, MotherShip.Slots[i].Color[f].G, MotherShip.Slots[i].Color[f].B, col.A));
                }

            if (gameTime.TotalGameTime < showEngineTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showEngineTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = MotherShip.Position + hud.Camera + enginePosition - MotherShip.Sprite.TextureOrigin;

                spriteBatch.Draw(engine, pos.Round(), col);
                spriteBatch.Draw(engineBar, (pos + engineBarPosition).Round(), col);
            }
            
        }
    }
}