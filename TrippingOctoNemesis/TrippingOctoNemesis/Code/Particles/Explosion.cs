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
            for (int i = 0; i < blasts.Random; i++)
            {
                Add(new PartialExplosion(Origin, MathHelper.TwoPi * Random.NextFloat(), lenght.Random) {DelayFlag=TimeSpan.FromSeconds(0.3f*Random.NextFloat()) });
            }

            Add(new MultiBlast(Origin, Size));

            base.Delete();
        }
    }
}
