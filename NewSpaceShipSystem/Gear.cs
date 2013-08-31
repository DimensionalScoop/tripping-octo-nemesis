using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Gear:Subsystem,IGear
    {
        public Gear()
            : base(7, 1, "Landing system")
        {

        }

        public GearConditions Status
        {
            get { throw new NotImplementedException(); }
        }

        public void Land(ILandingSlot location)
        {
            throw new NotImplementedException();
        }

        public void Launch()
        {
            throw new NotImplementedException();
        }
    }
}