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
    public class MotherShip:SpaceShip
    {
        public DeploySlots[] Slots = new DeploySlots[4];
        public Vector2 CursorPosition;
        //LADY ESMERELDA THE NECROMANCER QUEEN OF THE POORLY LIT LANDSx
        public float Fuel = 100;
        public float MaxFuel = 100;
        public float FuelConsumptionPerSecond = 1;
        public float FuelSpeed = 100;
        

        public MotherShip(Hud hud, Fraction fraction, GameTime gameTime)
            : base(fraction, gameTime)
        {
            Sprite = new SpriteSheet("s\\mothership");
            Speed = hud.CameraSpeed.Y;
            Angle = -MathHelper.PiOver2;

            EnginePositions = new Vector2[4];
            EnginePositions[0] = new Vector2(-Sprite.TextureOrigin.Y+2,-18 );
            EnginePositions[1] = new Vector2(-Sprite.TextureOrigin.Y+2,18);
            EnginePositions[2] = new Vector2( -Sprite.TextureOrigin.Y+2,-16);
            EnginePositions[3] = new Vector2( -Sprite.TextureOrigin.Y+2,16);
            TrackLenght *= 3;
            
            Hitpoints = 100;
            MaxHitpoints = 100;
            IntPosition = true;
            HasTarget = false;
            Status = Condition.Airborne;
            additionalLayerDepth = -0.02f;
            Weapon = null;
            Carrier = this;
        }

        public override void LongUpdate(TimeSpan elapsedTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            base.LongUpdate(elapsedTime, hud, otherSpaceShips);

            if (Fuel == 0)
            {
                FuelSpeed = 25;
                TrackLenght = 50;
            }
        }
    }
}
