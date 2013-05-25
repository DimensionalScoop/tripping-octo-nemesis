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
using System.Diagnostics;

namespace TrippingOctoNemesis.Extensions
{
    [Serializable]
    public class Map
    {
        public Color Color = Color.Gray;
        public string Name = "Unnamed Map";
        public string NextMap;
        public bool DeleteFlag;

        List<MapObject> entities= new List<MapObject>();
        List<BackgroundObject> bg = new List<BackgroundObject>();
        MapObject current { get { return index>=entities.Count?null:entities[index]; } }
        int index;

        float size;
        float shipPosition;

        string shipName = "You";

        Font font = new Font("f\\sfont");
        const int screenYOffset = 40;


        /// <summary>
        /// Call when all entities are added.
        /// </summary>
        public void CreateMap()
        {
            Debug.Assert(entities.Count>0);

            float pos = 0;
            foreach (var item in entities)
            {
                item.Position = pos;
                pos += item.Size;
            }
            size = entities.Sum(p => p.Size);

            entities.ForEach(p => p.MapCreated(this));
            bg.ForEach(p => p.MapCreated(this));
        }

        public void Update(GameTime gameTime)
        {
            if (current == null) { Delete(); return; }

            bg.ForEach(p => p.Update(gameTime));
            current.Update(gameTime);

            if (current.DeleteFlag)
                index++;
        }

        public void Delete() { DeleteFlag = true; }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            bg.ForEach(p => p.Draw(spriteBatch, gameTime));
            if(current!=null)current.Draw(spriteBatch, gameTime);
        }

        public void DrawMapUI(SpriteBatch spriteBatch, Vector2 screenSize, bool text)
        {
            screenSize.Y -= screenYOffset * 2;
            var offset=new Vector2(0,screenYOffset);

            spriteBatch.DrawRectangle(offset, 3, screenSize.Y, Color.LightGray,0.8f);

            foreach (var item in entities)
            {
                if (!item.VisibleOnMap) continue;
                spriteBatch.DrawRectangle(new Vector2(1,0)+offset+screenSize*new Vector2(0, item.Position / size), 1, item.Size / size, item.Color, 0.81f);
                if (text) spriteBatch.DrawText(item.Name, new Vector2(3, -font.SpriteFont.LineSpacing / 2) + offset + screenSize * new Vector2(0, item.Position / size + item.Size / size / 2), false, font, new Color(230,230,230));
            }

            spriteBatch.DrawRectangle(new Vector2(1, 0) + offset + screenSize * new Vector2(0, shipPosition / size), 1, 1, Color.Red, 0.82f);
            if (text) spriteBatch.DrawText(shipName, new Vector2(3, -font.SpriteFont.LineSpacing / 2) + offset + screenSize * new Vector2(0, shipPosition / size), false, font, Color.White);
        }
    }
}
