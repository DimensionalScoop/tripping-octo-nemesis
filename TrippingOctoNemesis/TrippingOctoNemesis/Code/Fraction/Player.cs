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
    public class Player : Fraction
    {
        public MotherShip MotherShip;

        public ControlKeySettings Keys;
        
        #region Assets and pixle positions
        static readonly Sprite hull = new Sprite("i\\hull");
        static readonly Sprite incLeft = new Sprite("i\\inc-left");
        static readonly Sprite incRight = new Sprite("i\\inc-right");
        static readonly Sprite engine = new Sprite("i\\engine");
        static readonly Sprite hullPoint = new Sprite("i\\hull-point");
        static readonly Sprite incPoint = new Sprite("i\\inc-point");
        static readonly Sprite engineBar = new Sprite("i\\engine-bar");
        static readonly Sprite pointer = new Sprite("i\\pointer");

        static readonly Vector2 hullPosition = new Vector2(36, 40);
        static readonly Vector2 hullPointPosition = new Vector2(54, 30);
        static readonly Vector2[] incPosition = new Vector2[]{
            new Vector2(-43, 22),new Vector2(-27, -4),new Vector2(48,-5),
            new Vector2(63,22)
        };
        static readonly Vector2[] incPointPosition = new Vector2[]{
        new Vector2(13,0),new Vector2(13,0),new Vector2(26,0),new Vector2(26,0)
        };
        static readonly Vector2 enginePosition = new Vector2(21, 99);
        static readonly Vector2 engineBarPosition = new Vector2(63, 5);

        static readonly Vector2[] deploySlotPositions = new Vector2[]{
            new Vector2(10,42),new Vector2(23,16), new Vector2(46,16),new Vector2(59,42)};
        #endregion

        TimeSpan showDamageTimer;
        TimeSpan[] showDeploySlotTimer = new TimeSpan[4];
        TimeSpan showEngineTimer;
        static readonly TimeSpan showTimeout = TimeSpan.FromSeconds(3);
        static readonly TimeSpan fadeDuration = TimeSpan.FromSeconds(0.4f);
        static readonly float CursorSpeed = 800;
        

        public void AssignMotherShip(MotherShip motherShip)
        {
            MotherShip = motherShip;
            MotherShip.HitpointsChanged += MotherShip_HitpointsChanged;
        }

        public void AddShipToCarrier(SpaceShip ship)//TODO: make carrier-spaceship interface more clear
        {
            ship.Carrier = MotherShip;
            ship.StatusChanged += ship_StatusChanged;
            ship.HitpointsChanged += ship_HitpointsChanged;
        }

        void ship_HitpointsChanged(SpaceShip obj)
        {
            for (int i = 0; i < MotherShip.Slots.Length; i++)
                if (MotherShip.Slots[i].Flyers.Contains(obj))
                    showDeploySlotTimer[i] = lastUpdate;
        }

        void MotherShip_HitpointsChanged(SpaceShip none)
        {
            showDamageTimer = lastUpdate;
        }

        public override void Update(GameTime gameTime, Hud hud)
        {
            base.Update(gameTime, hud);

            HandleInput(gameTime, hud);
        }

        private void HandleInput(GameTime gameTime, Hud hud)
        {
            if (!hud.Key.KeysPressed.Contains(Keys.Control))
            {
                var newPos = MotherShip.Position + GetDPadPosition(gameTime, hud) * MotherShip.FuelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((newPos + hud.Camera).X > 0 && (newPos + hud.Camera).Y > 0 && (newPos + hud.Camera).X < hud.ScreenSize.X && (newPos + hud.Camera).Y < hud.ScreenSize.Y)
                {
                    if (newPos != MotherShip.Position)
                    {
                        MotherShip.Fuel = MathHelper.Clamp(MotherShip.Fuel - MotherShip.FuelConsumptionPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds
                            , 0, MotherShip.MaxFuel);

                        showEngineTimer = gameTime.TotalGameTime;
                        MotherShip.Position = newPos;
                    }
                }
            }
            else
            {
                var newPos = MotherShip.CursorPosition + new Vector2(0, 1) * GetDPadPosition(gameTime, hud) * CursorSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((newPos + hud.Camera).Y > 0 && (newPos + hud.Camera).Y < hud.ScreenSize.Y)
                    MotherShip.CursorPosition = newPos;
            }

            MotherShip.CursorPosition -= hud.CameraDelta;
            MotherShip.CursorPosition.X = MotherShip.Position.X;



            if (!hud.Key.KeysPressed.Contains(Keys.Control))
            {
                for (int i = 0; i < 4; i++)
                    if (hud.Key.KeysStroked.Contains(Keys.NumberKeys[i]) && MotherShip.Slots[i].Flyers.Any(p => p.Status == SpaceShip.Condition.InHangar))
                    {
                        var ship = MotherShip.Slots[i].Flyers.First(p => p.Status == SpaceShip.Condition.InHangar);

                        ship.Deploy(MotherShip.Position + deploySlotPositions[i] - MotherShip.Sprite.TextureOrigin, MotherShip.CursorPosition, gameTime);
                        showDeploySlotTimer[i] = gameTime.TotalGameTime;
                    }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    if (hud.Key.KeysStroked.Contains(Keys.NumberKeys[i]) && MotherShip.Slots[i].Flyers.Any(p => p.Status == SpaceShip.Condition.Airborne))
                    {
                        MotherShip.Slots[i].Flyers.First(p => p.Status == SpaceShip.Condition.Airborne)
                            .Return();
                        showDeploySlotTimer[i] = gameTime.TotalGameTime;
                    }
            }
        }

        private Vector2 GetDPadPosition(GameTime gameTime, Hud hud)
        {
            var delta = Vector2.Zero;
            if (hud.Key.KeysPressed.Contains(Keys.Up)) delta.Y--;
            if (hud.Key.KeysPressed.Contains(Keys.Down)) delta.Y++;
            if (hud.Key.KeysPressed.Contains(Keys.Left)) delta.X--;
            if (hud.Key.KeysPressed.Contains(Keys.Right)) delta.X++;
            if (delta != Vector2.Zero)
            {
                delta.Normalize();
                return delta;
            }
            return delta;
        }

        void ship_StatusChanged(SpaceShip ship)
        {
            if (ship.Status != SpaceShip.Condition.ReturningPhase2)
                for (int i = 0; i < 4; i++)
                    if (MotherShip.Slots[i].Flyers.Contains(ship)) showDeploySlotTimer[i] = lastUpdate;
        }



        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            base.Draw(spriteBatch, hud, gameTime);

            DrawUI(spriteBatch, hud, gameTime);

        }

        private void DrawUI(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            spriteBatch.Draw(pointer, MotherShip.CursorPosition + hud.Camera, null, new Color(Color.R, Color.G, Color.B, 0.3f), (float)gameTime.TotalGameTime.TotalSeconds * MathHelper.TwoPi / 10, pointer.TextureOrigin, 1, SpriteEffects.None, DrawOrder.Flyer - 0.04f);

            if (gameTime.TotalGameTime < showDamageTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showDamageTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = MotherShip.Position + hud.Camera + hullPosition - MotherShip.Sprite.TextureOrigin;

                spriteBatch.Draw(hull, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                for (int i = 0; i < (int)(8 * (MotherShip.Hitpoints / (float)MotherShip.MaxHitpoints)); i++)
                    spriteBatch.Draw(hullPoint, (new Vector2(i * (hullPoint.Texture.Width + 1), 0) + pos + hullPointPosition).Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
            }
            //TODO: abstract hull damage display for enemy carrier

            for (int i = 0; i < 4; i++)
                if (gameTime.TotalGameTime < showDeploySlotTimer[i] + showTimeout)
                {
                    var t = gameTime.TotalGameTime - showDeploySlotTimer[i];
                    var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                    var pos = MotherShip.Position + hud.Camera + incPosition[i] - MotherShip.Sprite.TextureOrigin;
                    spriteBatch.Draw(i < 2 ? incLeft : incRight, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                    for (int f = 0; f < MotherShip.Slots[i].TotalFlyers; f++)
                        spriteBatch.Draw(incPoint, (pos + incPointPosition[i] + new Vector2(f * (1 + incPoint.Texture.Width), 0)).Round(), null,
                            new Color(MotherShip.Slots[i].Flyers[f].StatusColor.R, MotherShip.Slots[i].Flyers[f].StatusColor.G, MotherShip.Slots[i].Flyers[f].StatusColor.B, col.A),
                            0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                }

            if (gameTime.TotalGameTime < showEngineTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showEngineTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = MotherShip.Position + hud.Camera + enginePosition - MotherShip.Sprite.TextureOrigin;

                spriteBatch.Draw(engine, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                spriteBatch.Draw(engineBar, (pos + engineBarPosition).Round(),
                    new Rectangle(0, 0, (int)(engineBar.Texture.Width * MotherShip.Fuel / MotherShip.MaxFuel), engineBar.Texture.Height),
                    col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
            }
        }
    }
}