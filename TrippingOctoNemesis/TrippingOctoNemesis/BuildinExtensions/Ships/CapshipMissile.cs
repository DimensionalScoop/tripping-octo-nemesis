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
    public class CapshipMissile:Predator
    {
        public CapshipMissile(Fraction fraction, Func<Missile, SpaceShip> target):base(fraction,target)
        {
            //Sprite=new spr
            //ExplosionDamage = 50;

            //ExplosionColor = Color.Blue;
        }
    }
}
