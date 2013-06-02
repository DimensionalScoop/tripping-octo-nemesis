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
using System.Diagnostics;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        /// <summary>
        /// Is called every frame.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="hud"></param>
        /// <param name="otherSpaceShips"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (DeleteFlag) return;

            if (IsAirborne)
            {
                CalcTrack();
                Ki.Owner = this;
                if (UpdateKi) Ki.Update(gameTime);
                CalcMovement(gameTime);
                if (Weapon != null) Weapon.Update(gameTime);
            }

            CalcCarrierBehaviour(gameTime);
        }

        /// <summary>
        /// Is called for some ships per frame (round-robin), but at least once per second for every ship. Use for expensive calculations.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="hud"></param>
        /// <param name="otherSpaceShips"></param>
        public virtual void LongUpdate(TimeSpan elapsedTime)
        {
            Ki.LongUpdate(elapsedTime);
            if (Weapon != null) Weapon.LongUpdate(elapsedTime);
        }
    }
}