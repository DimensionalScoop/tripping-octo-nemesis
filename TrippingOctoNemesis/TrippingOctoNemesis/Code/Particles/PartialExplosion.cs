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
    class PartialExplosion : Particle
    {
        protected float Angle;
        protected float Length;

        const float particlesPerLength = 1 / 2f;
        const float smallBlastVarriation = 5;
        const int blastRange = 30;
        const float blastDelaymsVarriation = 500;
        static readonly Varriation delaymsPerLength = new Varriation(1400 / 300f, 1000 / 300f);
        static readonly VarriationInt countBlasts = new VarriationInt(6, 3);


        public PartialExplosion(Vector2 origin, float angle = MathHelper.PiOver4, float lenght = 100)
        {
            Angle = angle;
            Length = lenght;
            Origin = origin;
        }

        public override void Delete()
        {
            for (int i = 0; i < Length * particlesPerLength; i++)
            {
                Add(new Particles.SmallBlast(
                    Origin.Transform(MathHelper.PiOver2,i / particlesPerLength/20).Transform(Angle, i / particlesPerLength).Transform(MathHelper.TwoPi * Random.NextFloat(), smallBlastVarriation * Random.NextFloat())
                    ) {Color=Color, DelayFlag = TimeSpan.FromMilliseconds(delaymsPerLength.Random * i / particlesPerLength) }
                    );
            }

            int blasts = countBlasts.Random;
            Vector2 blastOrigin = Origin.Transform(Angle, Length);
            for (int i = 0; i < blasts; i++)
            {
                Add(new Particles.Blast(
                    blastOrigin.Transform(MathHelper.PiOver2, Length/20).Transform(MathHelper.TwoPi * Random.NextFloat(), Random.NextFloat() * blastRange)) { Color=Color,DelayFlag = TimeSpan.FromMilliseconds(blastDelaymsVarriation * Random.NextFloat() - blastDelaymsVarriation + delaymsPerLength.Base * Length) });
            }

            base.Delete();
        }
    }
}