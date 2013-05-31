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

namespace TrippingOctoNemesis.Extensions
{
    public class Group
    {
        /// <summary>
        /// The overall difficulty of the group.
        /// </summary>
        public int DC;
        public string Name;
        /// <summary>
        /// The name displayed to the player.
        /// </summary>
        public string DisplayName;

        public Group() {}

        /// <summary>
        /// Begins a new block of the given size.
        /// </summary>
        /// <param name="size"></param>
        public void Block(int size) { }
        /// <summary>
        /// Spawns a new enemy inside the current block.
        /// </summary>
        /// <param name="position">The relative position of the enemy (x,y between 0f and 1f)</param>
        /// <param name="enemy"></param>
        public void Spawn(Vector2 position, Enemy enemy) { }
        /// <summary>
        /// Ends the current block and adds additional distance between this and the next block.
        /// </summary>
        /// <param name="distance"></param>
        public void EndBlock(int distance) { }
    }
}
