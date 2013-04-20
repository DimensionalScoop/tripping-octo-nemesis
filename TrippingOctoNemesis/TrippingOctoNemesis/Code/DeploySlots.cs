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
    public class DeploySlots
    {
        public int TotalFlyers { get { return Flyers.Count; } }
        public List<SpaceShip> Flyers = new List<SpaceShip>();

        public DeploySlots(params SpaceShip[] flyers)
        {
            Flyers = flyers.ToList();
        }
    }
}
