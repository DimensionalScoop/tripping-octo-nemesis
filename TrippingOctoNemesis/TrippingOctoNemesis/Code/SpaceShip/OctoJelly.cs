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
    public class OctoJelly:SpaceShip
    {
        public OctoJelly(Fraction fraction, GameTime gameTime)
            : base(fraction, gameTime)
        {
            Sprite = new SpriteSheet("e\\Tripping-Octo-Jelly_spritesheet",4);
            Color = Color.LightBlue;
            DamageParticleBaseColor = Color.Green;//TODO: weapon particle color modifier
            Scale = 1;//TODO: bigger OctoJellies
        }
    }
}
