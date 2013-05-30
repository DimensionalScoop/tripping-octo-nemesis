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
    /// Manipulated the warp factor.
    /// </summary>
    public class Warp:Wait
    {
        protected float Value;
        protected float Source;

        /// <summary>
        /// Linear interpolates the warp factor from its current value to factor in duration.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="duration"></param>
        public Warp(float factor,float duration=0)
            : base(duration)
        {
            Name = "Warp "+(int)(factor*100)+"%";
            if (duration > 0)
                Name += " in " + (int)duration + "s";
            VisibleOnMap = true;
            Color = Color.CornflowerBlue;
            Value = factor;
        }

        public override void Activated(GameTime gameTime)
        {
            Source = GameControl.Stars.WarpFactor;
            base.Activated(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            GameControl.Stars.WarpFactor = MathHelper.Lerp(Source, Value, (float)((gameTime.TotalGameTime.TotalSeconds - Start.TotalSeconds) / Duration.TotalSeconds));
            base.Update(gameTime);
        }

        public override void Delete()
        {
            GameControl.Stars.WarpFactor = Value;
            base.Delete();
        }
    }
}
