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
            protected SpaceShip Owner;

            public KI(SpaceShip owner)
            {
                Owner = owner;
            }

            public virtual void Update(GameTime gameTime) { }
            public virtual void LongUpdate(TimeSpan elapsedTime) { }
        }

        public class Dumb:KI
        {
            public Dumb(SpaceShip owner) : base(owner) { }
        }

        public class NearestEnemy : KI
        {
            public NearestEnemy(SpaceShip owner) : base(owner) { }

            public override void Update(GameTime gameTime)
            {
                if (Owner.TargetShip == null && Owner.TargetShip.Hitpoints>0)
                    if (Owner.Carrier != null)
                        Owner.TargetPosition = Owner.Carrier.Position;
                    else
                        Owner.TargetPosition = Owner.Position;
                else Owner.TargetPosition = Owner.TargetShip.Position;
            }
        }

        public class FixedEnemy : NearestEnemy
        {
            public FixedEnemy(SpaceShip owner, SpaceShip target) : base(owner) 
            {
                Owner.TargetShip = target;
            }

            public override void Update(GameTime gameTime)
            {
                if (Owner.TargetShip == null && Owner.TargetShip.Hitpoints > 0)
                    Owner.Delete(DeleteReasons.SelfDestruction);
                else
                    Owner.TargetPosition = Owner.TargetShip.Position;
            }
        }



        public SpaceShip TargetShip;
        
        public int TargetShipDistanceSquared;

        //public bool AutoTargetShip = true;
        //public bool HasTarget = true;
        //public bool KeepScreenPosition = true;

        public KI Ki;
        public Weapon Weapon;
        public event Action<SpaceShip> ReachedTarget;
    }
}