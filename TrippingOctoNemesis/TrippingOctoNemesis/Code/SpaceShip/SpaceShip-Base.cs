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

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        protected static Random Random = new Random();

        public event Action<SpaceShip> HitpointsChanged;
        public event Action<SpaceShip> ReachedTarget;
        public event Action<SpaceShip> StatusChanged;


        public SpaceShip(Fraction fraction)
        {
            EnginePositions[0] = Vector2.Zero;
            Fraction = fraction;
            Color = Color.Lerp(Color.White,Fraction.Color,fractionColorBrightness);
        }
    }
}