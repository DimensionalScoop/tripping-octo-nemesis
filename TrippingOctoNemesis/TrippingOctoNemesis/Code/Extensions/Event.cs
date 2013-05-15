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
    public abstract class Event
    {
        protected Game Game;
        protected Hud Hud;
        protected Encounter Parant;

        public bool DeleteFlag { get; private set; }
        public virtual string Name { get; protected set; }


        public void Init(Encounter parant, Game game)
        {
            Parant = parant;
            Game = game;
        }

        /// <summary>
        /// Is called when this event becomes the current event.
        /// </summary>
        /// <param name="owner"></param>
        public virtual void Begin(StatusReport report) { }
        /// <summary>
        /// Is called every frame.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }
        /// <summary>
        /// Is called when this event is no longer the current event.
        /// </summary>
        /// <param name="owner"></param>
        public virtual StatusReport Finish() 
        {
            return null;
        }

        /// <summary>
        /// Should be called when this event is finished.
        /// </summary>
        protected void Delete()
        {
            DeleteFlag = true;
        }
    }
}
