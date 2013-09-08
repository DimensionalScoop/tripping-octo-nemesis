using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.SPS
{
    public class LandingSlot:Subsystem,ILandingSlot
    {
        public List<SpaceShip> LandedShips = new List<SpaceShip>();

        public LandingSlot(string name)
            : base(3, 8, name)
        {

        }

        
        int capacity;
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public bool HasCapacityLeft
        {
            get { return LandedShips.Count <capacity; }
        }

        public Vector2 ReceiveLandingSlot(SpaceShip ship)
        {
            throw new NotImplementedException();
        }

        public Vector2 LeaveLandingSlot(SpaceShip ship)
        {
            throw new NotImplementedException();
        }
    }
}
