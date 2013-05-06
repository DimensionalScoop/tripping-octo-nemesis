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

namespace TrippingOctoNemesis.Particles
{
    public class SmallBlast:Blast
    {
        public SmallBlast(Vector2 position):base(position)
        {
            MaxAge = TimeSpan.FromSeconds(0.40f);
        }

        protected override float CalcScale(GameTime gameTime)
        {
            return 0.25f;
        }
    }
}
