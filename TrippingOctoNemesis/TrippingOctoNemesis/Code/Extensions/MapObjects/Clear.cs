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

namespace TrippingOctoNemesis.Extensions.MapObjects
{
    /// <summary>
    /// Waits until all non-player ships vanished before the next map object is spawned.
    /// </summary>
    public class Clear:MapObject
    {
        public Clear()
        {
            Name = "Clear";
            VisibleOnMap = false;
            //TODO: Size=amountOfEnemieFighters
        }

        public override void Update(GameTime gameTime)
        {
            if (GameControl.Ships.All(p =>
                p.Fraction == GameControl.Player[0] ||
                p.Fraction == GameControl.Player[1]))
                Delete();

            base.Update(gameTime);
        }
    }
}
