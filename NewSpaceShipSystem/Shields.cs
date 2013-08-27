using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    class Shields:Subsystem,IDamage
    {
        public float MaxShieldPower;
        public float CurrentShieldPower;

        public Shields(float maxShieldPower):base(5,5,"Shield")
        {
            MaxShieldPower = maxShieldPower;
            
            StatusReport.Write(
        }

        public void Damage(float amountOfDamage)
        {
            float newShields = CurrentShieldPower - amountOfDamage;
            if (newShields >= 0)
            {
                CurrentShieldPower = newShields;
                return;
            }
            else
            {
                float relayedDamage = -newShields - CurrentShieldPower;
                CurrentShieldPower = 0;
                Parant.FindSubsystem<IDamage>(Priority - 1).Damage(relayedDamage);
            }
        }
    }
}
