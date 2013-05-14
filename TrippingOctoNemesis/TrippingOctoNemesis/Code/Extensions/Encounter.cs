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

namespace TrippingOctoNemesis.Extensions
{
    public class Encounter
    {
        protected List<Group> Groups = new List<Group>();

        /// <summary>
        /// Spawns a group of enemies
        /// </summary>
        /// <param name="group"></param>
        public void Spawn(Group group) { }
        public void Clear() { }
        public void Clear(TimeSpan timeout) { }
        public void Distance(int amount) { }
        public void Particle(Particle item) { }

        public void Point() { }
        public void Hop() { }
        public void Return() { }

        public void JumpPoint(string message, Encounter dest) { }
    }
}
