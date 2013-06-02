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
using System.Diagnostics;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public abstract class KI
        {
            public SpaceShip Owner;

            public virtual void Update(GameTime gameTime) { }
            public virtual void LongUpdate(TimeSpan elapsedTime) { }
        }

        public class Dumb:KI
        {
        }

        public class NearestEnemy : KI
        {
            public override void Update(GameTime gameTime)
            {
                //FIX: strange order - ki updated before weapon is created? Debug.Assert(Owner.Weapon == null);
                if (Owner.Weapon == null) return;

                if (Owner.Weapon.Target==null)
                    if (Owner.Carrier != null)
                        Owner.TargetPosition = Owner.Carrier.Position;
                    else
                        Owner.TargetPosition = Owner.Position;
                else Owner.TargetPosition = Owner.Weapon.Target.Position;
            }
        }

        public class FixedEnemy : KI
        {
            SpaceShip target;

            public FixedEnemy(SpaceShip target)
            {
                this.target = target;
            }

            public override void Update(GameTime gameTime)
            {
                Owner.Weapon.SingleTarget = target;
                Owner.Weapon.TargetSelector = Owner.Weapon.TargetSingle;

                if (Owner.Weapon.SingleTarget.DeleteFlag)
                    Owner.Delete(DeleteReasons.SelfDestruction);
                else
                    Owner.TargetPosition = target.Position;
            }
        }

        public class KeepScreenPosition : KI
        {
            Vector2 ScreenPosition;

            public KeepScreenPosition(Vector2 screenPosition)
            {
                ScreenPosition = screenPosition;
            }

            public override void Update(GameTime gameTime)
            {
                Owner.TargetPosition = ScreenPosition - GameControl.Hud.Camera;//XXX
            }
        }

        public class Patrol : KI
        {
            int index;
            Vector2[] ScreenPositions;

            public Patrol(params Vector2[] screenPositions)
            {
                ScreenPositions = screenPositions;
                index = -1;
            }

            void Owner_ReachedTarget(SpaceShip obj)
            {
                index++;
                if (index == ScreenPositions.Length) index = 0;
            }

            public override void Update(GameTime gameTime)
            {
                if (index == -1) Owner.ReachedTarget += Owner_ReachedTarget;
                Owner.TargetPosition = ScreenPositions[index] - GameControl.Hud.Camera;//XXX
            }
        }

        public class NoScreenMovement : KI
        {
            public override void Update(GameTime gameTime)
            {
                Owner.Position -= GameControl.Hud.CameraDelta;
            }
        }

        public KI Ki;
        public Weapon Weapon;
        public event Action<SpaceShip> ReachedTarget;
    }
}