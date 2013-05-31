using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using X45Game.Extensions;
using System.Diagnostics;

namespace TrippingOctoNemesis.Extensions
{
    public abstract class Map
    {
        /// <summary>
        /// The ui color the map is displayed in.
        /// </summary>
        public abstract Color Color { get; }
        /// <summary>
        /// The name of the map.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// The unique identifier of this map.
        /// </summary>
        public abstract Guid Id { get; }
        /// <summary>
        /// Determines the next map when this map reaches its last map object. Set to Guid.Empty to go back to menu.
        /// </summary>
        public Guid NextMap;
        /// <summary>
        /// Is set via Delete() when this map has reached its end.
        /// </summary>
        public bool DeleteFlag;

        /// <summary>
        /// Contains all map objects (e.g. ships, asteroids, items, etc.) in chronological order. Only one object is active at a any time.
        /// </summary>
        protected List<MapObject> Entities= new List<MapObject>();
        /// <summary>
        /// Contains all map objects that are always active (e.g. background galaxies, additional stars, etc).
        /// </summary>
        protected List<BackgroundObject> Bg = new List<BackgroundObject>();
        /// <summary>
        /// Gets the map object that is active at the moment.
        /// </summary>
        protected MapObject current { get { return index >= Entities.Count ? null : Entities[index]; } }
        /// <summary>
        /// The index of the active map object.
        /// </summary>
        protected int index;

        /// <summary>
        /// The size of the map.
        /// </summary>
        protected float size;
        /// <summary>
        /// The player's ship's position within the map.
        /// </summary>
        protected float shipPosition;//SCOTT

        /// <summary>
        /// The player's ship's name that is displayed on the minimap.
        /// </summary>
        protected virtual string shipName { get { return "You"; } }//SCOTT

        /// <summary>
        /// The ui font of the minimap.
        /// </summary>
        static readonly Font font = new Font("font");
        /// <summary>
        /// Vertical offset of the minimap.
        /// </summary>
        protected const int screenYOffset = 40;


        /// <summary>
        /// Every map should have an empty constructor in order to function.
        /// </summary>
        public Map() { }

        /// <summary>
        /// Is called when this map was finished or just constructed, in order to prepare this map for a (new) run.
        /// </summary>
        public virtual void Reset()
        {
            Entities.Clear();
            Bg.Clear();
            NextMap = Guid.Empty;
            DeleteFlag = false;
            index = 0;

            CreateMapObjects();
            CreateMap();
        }

        /// <summary>
        /// The method in which all map objects should be created.
        /// </summary>
        public abstract void CreateMapObjects();

        /// <summary>
        /// Is call when all entities are added.
        /// </summary>
        public void CreateMap()
        {
            Debug.Assert(Entities.Count>0);

            float pos = 0;
            foreach (var item in Entities)
            {
                item.Position = pos;
                pos += item.Size;
            }
            size = Entities.Sum(p => p.Size);

            Entities.ForEach(p => p.MapCreated(this));
            Bg.ForEach(p => p.MapCreated(this));
        }

        /// <summary>
        /// Is called each frame to update map objects.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (current == null) { Delete(); return; }

            Bg.ForEach(p => p.Update(gameTime));
            current.Update(gameTime);

            if (current.DeleteFlag)
            {
                index++;
                current.Activated(gameTime);
                if (current != null) shipPosition = current.Position;
            }
        }

        /// <summary>
        /// Marks the this map as finished.
        /// </summary>
        public void Delete() { DeleteFlag = true; }

        /// <summary>
        /// Is called as often as possible per frame to draw map objects.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Bg.ForEach(p => p.Draw(spriteBatch, gameTime));
            if(current!=null)current.Draw(spriteBatch, gameTime);
        }


        /// <summary>
        /// Is called when the minimap should be drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="screenSize"></param>
        /// <param name="text"></param>
        public void DrawMapUI(SpriteBatch spriteBatch, Vector2 screenSize, bool text,GameTime gameTime)
        {
            screenSize.Y -= screenYOffset * 2;
            var offset = new Vector2(0, font.SpriteFont.LineSpacing*1.1f);

            var pos = offset*3 + new Vector2(10, 0);
            foreach (var item in Entities)
            {

                if (!item.VisibleOnMap)
#if DEBUG
                    ;
#else
                    continue;
#endif
                var sym = Symbols.RightArrows1;
                if (gameTime.TotalGameTime.Milliseconds > 500)
                    sym = Symbols.RightArrows2;

                if (text) spriteBatch.DrawText((current==item?sym:' ')+ item.Name, pos, false, font, new Color(230, 230, 230));
                pos += offset;
            }
            //Old(spriteBatch, screenSize, text);
        }
        private void Old(SpriteBatch spriteBatch, Vector2 screenSize, bool text)
        {
            screenSize.Y -= screenYOffset * 2;
            var offset = new Vector2(0, screenYOffset);

            spriteBatch.DrawRectangle(offset, 3, screenSize.Y, Color.LightGray, 0.8f);

            foreach (var item in Entities)
            {
                if (!item.VisibleOnMap) continue;
                spriteBatch.DrawRectangle(new Vector2(1, 0) + offset + screenSize * new Vector2(0, item.Position / size), 1, item.Size / size, item.Color, 0.81f);
                if (text) spriteBatch.DrawText(item.Name, new Vector2(3, -font.SpriteFont.LineSpacing / 2) + offset + screenSize * new Vector2(0, item.Position / size + item.Size / size / 2), false, font, new Color(230, 230, 230));
            }

            spriteBatch.DrawRectangle(new Vector2(1, 0) + offset + screenSize * new Vector2(0, shipPosition / size), 1, 1, Color.Blue, 0.82f);
            if (text) spriteBatch.DrawText(shipName, new Vector2(3, -font.SpriteFont.LineSpacing / 2) + offset + screenSize * new Vector2(0, shipPosition / size), false, font, Color.White);
        }

        public void Start(GameTime gameTime)
        {
            current.Activated(gameTime);
        }
    }
}
