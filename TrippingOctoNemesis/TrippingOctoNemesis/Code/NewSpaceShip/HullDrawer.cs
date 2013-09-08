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

namespace TrippingOctoNemesis.SPS
{
    public class HullDrawer:Subsystem,IDrawable
    {
        protected static Random Random = new Random();

        public SpriteSheet Sprite = new SpriteSheet("s\\ship");
        protected bool IsVisible = true;
        public Color Color = Color.White;

        /// <summary>
        /// If true the ship is drawn on Position.Round() to make it look sharper. Use for big ships which do not move all the time.
        /// </summary>
        public bool IntPosition = false;

        /// <summary>
        /// Value to prevent flickering on overlapping sprites of same depth
        /// </summary>
        float depthVarriation = Random.NextFloat() / 10000;

        /// <summary>
        /// Determines how visible the fraction color of this ship is.
        /// </summary>
        const float fractionColorBrightness = 0.5f;
        /// <summary>
        /// Use to display this ship appear above or behind other ships.
        /// </summary>
        public float AdditionalLayerDepth;



        public HullDrawer():base(0,-1,"Hull drawing display")
        {
        }
        

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            Vector2 pos=Parant.Position+GameControl.Hud.Camera;
            if(IntPosition)pos=pos.Round();

            spriteBatch.Draw(Sprite, pos, gameTime, Color, Sprite.TextureOrigin, MathHelper.PiOver2 + Parant.Heading, 1, 0.5f + AdditionalLayerDepth + depthVarriation);
        }

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }
    }
}
