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
    public class Missile:SpaceShip
    {
        public Missile(Fraction fraction, Vector2 postion)
            : base(fraction)
        {

        }

        public override void Update(GameTime gameTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            base.Update(gameTime, hud, otherSpaceShips);
        }
    }
}
