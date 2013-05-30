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
        protected TimeSpan Start;
        protected TimeSpan Duration;

        public Wait(float seconds)
        {
            Name = "Wait "+(int)seconds+" s";
            VisibleOnMap = false;
            Duration = TimeSpan.FromSeconds(seconds);
            Size = (float)Duration.TotalSeconds*10;
            //TODO: Size=Speed*duration
        }

        public override void Activated(GameTime gameTime)
        {
            Start = gameTime.TotalGameTime;

            base.Activated(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - Start >= Duration)
                Delete();
            
            base.Update(gameTime);
        }
    }
}
