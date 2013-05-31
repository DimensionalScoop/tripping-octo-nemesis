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
    /// Does nothing and sets delete flag.
    /// </summary>
    public class Null : MapObject
    {
        public Null()
        {
            Name = "Null";
            VisibleOnMap = false;
            //TODO: Size=amountOfEnemieFighters
        }

        public override void Activated(GameTime gameTime)
        {
            Delete();
            base.Activated(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
