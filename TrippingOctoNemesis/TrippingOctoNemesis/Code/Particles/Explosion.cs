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

namespace TrippingOctoNemesis.Particles
{
    public class Explosion:Particle
    {
        int Size;

        static readonly Varriation lenght = new Varriation(100, 30);
        static readonly VarriationInt blasts = new VarriationInt(5, 2);

        public Explosion(Vector2 origin,int size=10)
        {
            Origin = origin;
            Size = size;
        }

        public override void Delete()
        {
            var angle=MathHelper.TwoPi * Random.NextFloat();

            for (int i = 0; i < blasts.Random; i++)
            {
                Add(new PartialExplosion(Origin,angle , lenght.Random) {Color=Color, DelayFlag=TimeSpan.FromSeconds(0.3f*Random.NextFloat()) });
                angle += MathHelper.TwoPi * Random.NextFloat() * 0.25f;
            }

            Add(new Burst(Origin, Size*2));

            base.Delete();
        }
    }
}
