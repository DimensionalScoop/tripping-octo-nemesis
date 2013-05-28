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
    public class MapObject
    {
        public float Position = -1;
        public float Size;
        public Color Color = Color.Blue;
        public string Name = "";
        public bool VisibleOnMap = true;
        public bool DeleteFlag;

        protected Map Parant;


        public virtual void MapCreated(Map map)
        {
            Parant = map;
        }

        public virtual void Activated(GameTime gameTime) { }
        
        public virtual void Delete() { DeleteFlag = true; }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
