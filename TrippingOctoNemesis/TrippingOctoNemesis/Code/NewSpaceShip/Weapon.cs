using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippingOctoNemesis.SPS
{
    public class Weapon:Subsystem,IWeapon
    {
        public Weapon(string name)
            : base(5, 6, name,2)
        {

        }

        public float WeightedWeaponPower()
        {
            throw new NotImplementedException();
        }
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
