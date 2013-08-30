using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Weapon
    {
    }

    public interface IWeapon
    {
        /// <summary>
        /// Measures how powerful a weapon is. In the most simple case WeightedWeaponPower = DamagePerSecond.
        /// </summary>
        /// <returns></returns>
        float WeightedWeaponPower();
    }
}
