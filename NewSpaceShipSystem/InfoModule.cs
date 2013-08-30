using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public enum ShipClasses { None, Fighter, Carrier, Supporter, Transporter, Special, Missile }

    public class InfoModule:Subsystem,IInfo
    {
        public ShipClasses ShipClass;
        /// <summary>
        /// Set to appropriated value if ship has special powers which make her stronger/weaker.
        /// </summary>
        protected float DCModifier;
        /// <summary>
        /// Measures how difficult it is to fight this ship.
        /// </summary>
        public float DC
        {
            get
            {
                float returnValue = Parant.FindAllSubsystems<IDamage>().Sum(p => p.DamageSuppression());
                returnValue *= Parant.FindAllSubsystems<IWeapon>().Sum(p => p.WeightedWeaponPower());
                returnValue += DCModifier;

                if (ShipClass == ShipClasses.Carrier) returnValue *= 5;
                if (ShipClass == ShipClasses.Transporter) returnValue /= 3;
                return returnValue;
            }
        }


        public InfoModule(ShipClasses shipClass,float dcModifier=0)
            : base(-10, 0, "Info module")
        {
            ShipClass = shipClass;
            DCModifier = dcModifier;
        }

    }

    interface IInfo
    {

    }
}
