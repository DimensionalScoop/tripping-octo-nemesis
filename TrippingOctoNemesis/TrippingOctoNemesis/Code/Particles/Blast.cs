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
    public class Blast:Particle
    {
        public Blast(Vector2 position):base()
        {
            Sprite = new SpriteSheet("p\\explosion",25);
            Origin = position;
            MaxAge = TimeSpan.FromSeconds(0.35f);
        }

        //protected override Vector2 CalcPosition(GameTime gameTime, Vector2 camera)
        //{
        //    return Origin;
        //}
    }
}
