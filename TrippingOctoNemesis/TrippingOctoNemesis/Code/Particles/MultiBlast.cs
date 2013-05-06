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
    class MultiBlast : Particle
    {
        protected int Size;

        const float particlesPerLength = 1 / 2f;
        const float smallBlastVarriation = 5;
        const int blastRange = 30;
        const float blastDelaymsVarriation = 3000;
        static readonly Varriation countBlasts = new Varriation(6/100f, 3/100f);


        public MultiBlast(Vector2 origin, int size = 100)
        {
            Size=size;
            Origin = origin;
        }

        public override void Delete()
        {
            int blasts = (int)(countBlasts.Random*Math.Pow(Size,2));
            for (int i = 0; i < blasts; i++)
            {
                Add(new Particles.Blast(
                    Origin.Transform(MathHelper.TwoPi * Random.NextFloat(), Random.NextFloat() * Size)) { DelayFlag = TimeSpan.FromMilliseconds(blastDelaymsVarriation * Random.NextFloat()) });
            }

            base.Delete();
        }
    }
}