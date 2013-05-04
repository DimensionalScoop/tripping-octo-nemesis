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
using System.Diagnostics;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public virtual void Update(GameTime gameTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            if (DeleteFlag) return;

            if (Status != Condition.InHangar && Status != Condition.Repairing)
            {
                CalcTrack();
                CalcKI(gameTime);
                CalcTargetAngle(gameTime, hud);
                CalcMovement(gameTime);
                if (Weapon != null) Weapon.Update(gameTime);
            }

            CalcCarrierBehaviour(gameTime);
        }

        public virtual void LongUpdate(TimeSpan elapsedTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            Debug.Assert(!DeleteFlag);
            TargetNearesEnemy(otherSpaceShips);
        }
    }
}