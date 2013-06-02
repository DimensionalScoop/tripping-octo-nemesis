using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrippingOctoNemesis
{
    public class DummyWeapon:Weapon
    {
        public DummyWeapon(SpaceShip owner):base(owner)
        {
            Name = "None";
            Cycling = true;
            Damage = 0;
            RangeSquared = (int)Math.Pow(0, 2);
            Cooldown = TimeSpan.FromMilliseconds(1000);
            ShellingDuration = TimeSpan.FromMilliseconds(100);
            TargetSelector = TargetNearestEnemy;
            AssignRandomCooldownPosition();
        }
    }
}
