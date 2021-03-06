﻿using System;
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
    public class CarrierOld:SpaceShip
    {
        public DeploySlots[] Slots = new DeploySlots[4];
        public Vector2 CursorPosition;
        //LADY ESMERELDA THE NECROMANCER QUEEN OF THE POORLY LIT LANDS
        public float Fuel = 100;
        public float MaxFuel = 100;
        public float FuelConsumptionPerSecond = 1;
        public float FuelSpeed = 100;
        

        public Carrier(Hud hud, Fraction fraction)
            : base(fraction)
        {
            Sprite = new SpriteSheet("s\\mothership");
            Speed = hud.CameraSpeed.Y;
            Angle = -MathHelper.PiOver2;

            SetEngines(150,
            new Vector2(-Sprite.TextureOrigin.Y + 2, -18),
            new Vector2(-Sprite.TextureOrigin.Y + 2, 18),
            new Vector2(-Sprite.TextureOrigin.Y + 2, -16),
            new Vector2(-Sprite.TextureOrigin.Y + 2, 16));
            
            Hitpoints = 100;
            MaxHitpoints = 100;
            IntPosition = true;
            Status = Conditions.Airborne;
            additionalLayerDepth = -0.02f;
            Weapon = null;
            Carrier = this;
            Class = ShipClasses.Carrier;
            Ki = new SpaceShip.NoScreenMovement();
        }

        public override void LongUpdate(TimeSpan elapsedTime)
        {
            base.LongUpdate(elapsedTime);

            if (Fuel == 0)
            {
                FuelSpeed = 25;
                SetEngines(50);
            }
        }
    }
}
