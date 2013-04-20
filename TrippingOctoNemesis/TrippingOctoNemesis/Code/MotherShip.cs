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

        public MotherShip(Hud hud,Fraction fraction):base(fraction)
        {
            Sprite = new Sprite("s\\mothership");
            Speed = hud.CameraSpeed.Y;
            Angle = -MathHelper.PiOver2;

            EnginePositions = new Vector2[4];
            EnginePositions[0] = new Vector2(-18, -Sprite.TextureOrigin.Y+2);
            EnginePositions[1] = new Vector2(18, -Sprite.TextureOrigin.Y+2);
            EnginePositions[2] = new Vector2(-16, -Sprite.TextureOrigin.Y+2);
            EnginePositions[3] = new Vector2(16, -Sprite.TextureOrigin.Y+2);
            TrackLenght *= 3;
            
            Hitpoints = 100;
            MaxHitpoints = 100;
            IntPosition = true;
            HasTarget = false;
            Status = Condition.Airborne;
            additionalLayerDepth = -0.02f;
        }
    }
}
