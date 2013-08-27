using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Hull:Subsystem,IDamage
    {
        public float MaxHullPoints;
        public float CurrentHullPoints;

        public Hull(float maxHullPoints):base(0,"Hull")
        {
            MaxHullPoints = maxHullPoints;
        }

        public void Damage(float amountOfDamage)
        {
            CurrentHullPoints -= amountOfDamage;
            if (CurrentHullPoints >= 0) Parant.Destroy("due to massive hull damage");
        }
    }
}
