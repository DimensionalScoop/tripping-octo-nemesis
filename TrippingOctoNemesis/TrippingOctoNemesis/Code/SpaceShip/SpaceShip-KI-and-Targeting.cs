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
        public SpaceShip TargetShip;
        public int TargetShipDistanceSquared;
        public bool AutoTargetShip = true;
        public bool HasTarget = true;
        public bool KeepScreenPosition = true;

        public enum KIs { FixedTargetPosition, NearestEnemy }
        public KIs KI = KIs.FixedTargetPosition;

        public Weapon Weapon;


        private void CalcKI(GameTime gameTime)
        {
            switch (KI)
            {
                case KIs.FixedTargetPosition: return;

                case KIs.NearestEnemy:
                    if (TargetShip==null) TargetPosition = Position;
                    else TargetPosition = TargetShip.Position;
                    return;

                default: throw new NotImplementedException();
            }
        }

        private void TargetNearesEnemy(List<SpaceShip> otherSpaceShips)
        {
            if (AutoTargetShip)
            {
                TargetShip = null;
                TargetShipDistanceSquared = -1;
                int minRange = int.MaxValue;
                int range;
                for (int i = 0; i < otherSpaceShips.Count; i++)
                    if (Fraction.IsEnemy(otherSpaceShips[i].Fraction))
                    {
                        range = (int)Vector2.DistanceSquared(otherSpaceShips[i].Position, Position);
                        if (range < minRange)
                        {
                            minRange = range;
                            TargetShip = otherSpaceShips[i];
                            TargetShipDistanceSquared = range;
                        }
                    }
            }
        }
    }
}
