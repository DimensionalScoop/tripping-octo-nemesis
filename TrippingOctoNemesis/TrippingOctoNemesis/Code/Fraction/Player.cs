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

namespace TrippingOctoNemesis
{
    public class Player : Fraction
    {
        public Carrier Carrier;

        public ControlKeySettings Keys;
        
        #region Assets and pixle positions
        static readonly Sprite hull = new Sprite("i\\hull");
        static readonly Sprite incLeft = new Sprite("i\\inc-left");
        static readonly Sprite incRight = new Sprite("i\\inc-right");
        static readonly Sprite engine = new Sprite("i\\engine");
        static readonly Sprite hullPoint = new Sprite("i\\hull-point");
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

        
        //FEATURE: make player's ship modifiable
        public void AssignCarrier(Carrier motherShip)
        {
            Carrier = motherShip;
            Carrier.HitpointsChanged += Carrier_HitpointsChanged;
        }

        public void AddShipToCarrier(SpaceShip ship)//TODO: make carrier-spaceship interface more clear (maybe remove slots).
        {
            ship.Carrier = Carrier;
            ship.StatusChanged += ship_StatusChanged;
            ship.HitpointsChanged += ship_HitpointsChanged;
        }

        void ship_HitpointsChanged(SpaceShip obj)
        {
            for (int i = 0; i < Carrier.Slots.Length; i++)
                if (Carrier.Slots[i].Flyers.Contains(obj))
                    showDeploySlotTimer[i] = lastUpdate;
        }

        void Carrier_HitpointsChanged(SpaceShip none)
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
                var newPos = Carrier.Position + GetDPadPosition(gameTime, hud) * Carrier.FuelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((newPos + hud.Camera).X > 0 && (newPos + hud.Camera).Y > 0 && (newPos + hud.Camera).X < hud.ScreenSize.X && (newPos + hud.Camera).Y < hud.ScreenSize.Y)
                {
                    if (newPos != Carrier.Position)
                    {
                        Carrier.Fuel = MathHelper.Clamp(Carrier.Fuel - Carrier.FuelConsumptionPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds
                            , 0, Carrier.MaxFuel);

                        showEngineTimer = gameTime.TotalGameTime;
                        Carrier.Position = newPos;
                    }
                }
            }
            else
            {
                var newPos = Carrier.CursorPosition + new Vector2(0, 1) * GetDPadPosition(gameTime, hud) * CursorSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((newPos + hud.Camera).Y > 0 && (newPos + hud.Camera).Y < hud.ScreenSize.Y)
                    Carrier.CursorPosition = newPos;
            }

            Carrier.CursorPosition -= hud.CameraDelta;
            Carrier.CursorPosition.X = Carrier.Position.X;



            if (!hud.Key.KeysPressed.Contains(Keys.Control))
            {
                for (int i = 0; i < 4; i++)
                    if (hud.Key.KeysStroked.Contains(Keys.NumberKeys[i]) && Carrier.Slots[i].Flyers.Any(p => p.Status == SpaceShip.Conditions.InHangar))
                    {
                        var ship = Carrier.Slots[i].Flyers.First(p => p.Status == SpaceShip.Conditions.InHangar);

                        ship.Deploy(Carrier.Position + deploySlotPositions[i] - Carrier.Sprite.TextureOrigin, Carrier.CursorPosition, gameTime);
                        showDeploySlotTimer[i] = gameTime.TotalGameTime;
                    }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    if (hud.Key.KeysStroked.Contains(Keys.NumberKeys[i]) && Carrier.Slots[i].Flyers.Any(p => p.Status == SpaceShip.Conditions.Airborne))
                    {
                        Carrier.Slots[i].Flyers.First(p => p.Status == SpaceShip.Conditions.Airborne)
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
            if (ship.Status != SpaceShip.Conditions.ReturningPhase2)
                for (int i = 0; i < 4; i++)
                    if (Carrier.Slots[i].Flyers.Contains(ship)) showDeploySlotTimer[i] = lastUpdate;

            if (ship.Status == SpaceShip.Conditions.Airborne)//Ship have just finished deployment
                ship.Ki = new SpaceShip.KeepScreenPosition(ship.TargetPosition+GameControl.Hud.Camera);
        }



        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            base.Draw(spriteBatch, hud, gameTime);

            DrawUI(spriteBatch, hud, gameTime);

        }

        private void DrawUI(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            spriteBatch.Draw(pointer, Carrier.CursorPosition + hud.Camera, null, new Color(Color.R, Color.G, Color.B, 0.3f), (float)gameTime.TotalGameTime.TotalSeconds * MathHelper.TwoPi / 10, pointer.TextureOrigin, 1, SpriteEffects.None, DrawOrder.Flyer - 0.04f);

            if (gameTime.TotalGameTime < showDamageTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showDamageTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = Carrier.Position + hud.Camera + hullPosition - Carrier.Sprite.TextureOrigin;

                spriteBatch.Draw(hull, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                for (int i = 0; i < (int)(8 * (Carrier.Hitpoints / (float)Carrier.MaxHitpoints)); i++)
                    spriteBatch.Draw(hullPoint, (new Vector2(i * (hullPoint.Texture.Width + 1), 0) + pos + hullPointPosition).Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
            }

            for (int i = 0; i < 4; i++)
                if (gameTime.TotalGameTime < showDeploySlotTimer[i] + showTimeout)
                {
                    var t = gameTime.TotalGameTime - showDeploySlotTimer[i];
                    var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                    var pos = Carrier.Position + hud.Camera + incPosition[i] - Carrier.Sprite.TextureOrigin;
                    spriteBatch.Draw(i < 2 ? incLeft : incRight, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);

                    for (int f = 0; f < Carrier.Slots[i].TotalFlyers; f++)
                    {
                        var sprite = Carrier.Slots[i].Flyers[f].Icon;
                        var icoPos = (pos + incPointPosition[i] + new Vector2(f * (1 + sprite.Texture.Width), 0)).Round();
                        var color = new Color(Carrier.Slots[i].Flyers[f].StatusColor.R, Carrier.Slots[i].Flyers[f].StatusColor.G, Carrier.Slots[i].Flyers[f].StatusColor.B, col.A);

                        spriteBatch.Draw(sprite,icoPos, null,color,0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                    }
                }

            if (gameTime.TotalGameTime < showEngineTimer + showTimeout)
            {
                var t = gameTime.TotalGameTime - showEngineTimer;
                var col = new Color(1, 1, 1, t > showTimeout - fadeDuration ? 1 - (float)(t - showTimeout + fadeDuration).TotalSeconds / (float)fadeDuration.TotalSeconds : 1);
                var pos = Carrier.Position + hud.Camera + enginePosition - Carrier.Sprite.TextureOrigin;

                spriteBatch.Draw(engine, pos.Round(), null, col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
                spriteBatch.Draw(engineBar, (pos + engineBarPosition).Round(),
                    new Rectangle(0, 0, (int)(engineBar.Texture.Width * Carrier.Fuel / Carrier.MaxFuel), engineBar.Texture.Height),
                    col, 0, Vector2.Zero, 1, SpriteEffects.None, DrawOrder.UI);
            }
        }
    }
}