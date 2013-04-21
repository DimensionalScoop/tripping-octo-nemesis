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

namespace TrippingOctoNemesis
{
    public class Fraction:X45Game.Strategics.Player
    {
        protected List<SpaceShip> SpaceShips = new List<SpaceShip>();
        
        protected TimeSpan lastUpdate;


        public virtual void AddShip(SpaceShip ship)
        {
            SpaceShips.Add(ship);
        }

        public sealed override void Update(GameTime gameTime)
        {
            throw new Exception();
        }
        public virtual void Update(GameTime gameTime, Hud hud)
        {
            base.Update(gameTime);

            lastUpdate = gameTime.TotalGameTime;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime) 
        {
        }
    }
}