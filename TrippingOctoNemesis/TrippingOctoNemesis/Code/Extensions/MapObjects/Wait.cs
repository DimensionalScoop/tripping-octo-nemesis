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

namespace TrippingOctoNemesis.Extensions.MapObjects
{
    /// <summary>
    /// Waits a certain amount of time before the next map object is spawned
    /// </summary>
    public class Wait:MapObject
    {
        TimeSpan start;
        TimeSpan duration;

        public Wait(float seconds)
        {
            Name = "Wait "+(int)seconds+" s";
            VisibleOnMap = false;
            duration = TimeSpan.FromSeconds(seconds);
            Size = (float)duration.TotalSeconds*10;
            //TODO: Size=Speed*duration
        }

        public override void Activated(GameTime gameTime)
        {
            start = gameTime.TotalGameTime;

            base.Activated(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - start >= duration)
                Delete();
            
            base.Update(gameTime);
        }
    }
}
