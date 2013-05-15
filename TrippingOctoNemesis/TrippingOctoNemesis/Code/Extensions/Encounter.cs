using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.Extensions
{
    public class Encounter
    {
        int eventIndex = 0;
        Event currentEvent
        {
            get
            {
                if (eventIndex >= Events.Count)
                    return null;
                return Events[eventIndex];
            }
        }
        StatusReport report = null;

        public List<Event> Events = new List<Event>();

        public bool DeleteFlag { get; private set; }


        public Encounter(Game game, Map owner)
        {

        }

        public void Update(GameTime gameTime)
        {
            if(Events.Count==0)
            {
                Delete();
                return;
            }

            if (currentEvent.DeleteFlag)
            {
                report = currentEvent.Finish();
                eventIndex++;
                if (currentEvent == null) { Delete(); return; }

                currentEvent.Begin(report);
            }

            currentEvent.Update(gameTime);
        }

        public StatusReport Finish()
        {
            return report;
        }

        protected void Delete()
        {
            DeleteFlag = true;
        }


        /// <summary>
        /// Spawns a group of enemies
        /// </summary>
        /// <param name="group"></param>
        public void Spawn(Group group) { }
        public void Clear() { }
        public void Clear(TimeSpan timeout) { }
        public void Distance(int amount) { }
        public void Particle(Particle item) { }

        public void Point() { }
        public void Hop() { }
        public void Return() { }

        public void JumpPoint(string message, Encounter dest) { }
    }
}
