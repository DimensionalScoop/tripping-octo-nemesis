using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace NewSpaceShipSystem
{
    public class Main
    {
        protected List<Subsystem> Subsystems = new List<Subsystem>();
        
        public bool DeleteFlag;


        public void AddSubsystem(Subsystem item)
        {
            Subsystems.Add(item);
            Subsystems.Sort(new Comparison<Subsystem>((p, q) => q.Priority - p.Priority));//high priority first
        }

        public Interface FindSubsystem<Interface>(int maxPriority=int.MaxValue)
            where Interface: class
        {
            var returnValue=Subsystems.Find(p => p is Interface && p.Priority <= maxPriority);
            if (returnValue == null) throw new Exception("Ship interface data not handled!");
            return returnValue as Interface;
        }

        public void Destroy(string reason)
        {
            DeleteFlag = true;
        }
    }
}
