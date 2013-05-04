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
        public static bool DeleteableShips { get; private set; }

        protected static Random Random = new Random();

        public bool DeleteFlag { get; private set; }
        public event Action<SpaceShip> DeleteFlagSet;

        public SpaceShip(Fraction fraction, GameTime gameTime)
        {
            EnginePositions[0] = Vector2.Zero;
            Fraction = fraction;
            Color = Color.Lerp(Color.White,Fraction.Color,fractionColorBrightness);
            Weapon = new Weapon(this,gameTime);
        }


        public enum DeleteReasons { Destroyed, Debug }
        public void Delete(DeleteReasons reason= DeleteReasons.Destroyed)
        {
            if (reason == DeleteReasons.Destroyed)
                ;//add explosions
            DeleteableShips = true;
            DeleteFlag = true;
            if (DeleteFlagSet != null) DeleteFlagSet(this);
        }
    }
}