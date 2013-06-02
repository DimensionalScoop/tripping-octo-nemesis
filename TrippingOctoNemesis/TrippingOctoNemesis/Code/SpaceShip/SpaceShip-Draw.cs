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

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public SpriteSheet Sprite = new SpriteSheet("s\\ship");
        public Color Color = Color.White;
        public Color DamageParticleBaseColor = Color.White;
        public float Scale = 1f;
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
        protected float additionalLayerDepth;

        List<Vector2> track = new List<Vector2>();
        int trackLength;
        /// <summary>
        /// Each engine creates a visible track.
        /// </summary>
        Vector2[] EnginePositions;


        protected void SetEngines(params Vector2[] positions)
        { SetEngines(50, positions); }

        protected void SetEngines(int trackLenght)
        { SetEngines(trackLength, EnginePositions); }

        protected void SetEngines(int trackLenght, params Vector2[] positions)
        {
            this.trackLength = trackLenght;
            EnginePositions = positions;
            if (EnginePositions == null)
                EnginePositions = new Vector2[0];
        }

        protected void CalcTrack()
        {
            foreach (var engine in EnginePositions)
            {
                var pos = Position + engine.Rotate(Angle);//Transform(-engine.Angle(Vector2.Zero) + Angle, engine.Length());
                track.Add(pos);
                track.Add(pos - Direction * 2);
            }

            while (track.Count > trackLength) track.RemoveAt(0);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            if (DeleteFlag) return;
            if (!IsAirborne) return;

            spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, gameTime, Color, Sprite.TextureOrigin, MathHelper.PiOver2 + Angle, Scale, DrawOrder.Flyer + additionalLayerDepth + depthVarriation);
            //spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, null, Color, MathHelper.PiOver2 + Angle, Sprite.TextureOrigin, Scale, SpriteEffects.None, DrawOrder.Flyer+additionalLayerDepth);
            for (int i = 0; i < track.Count; i++)
            {
                float f = i / (float)trackLength;
                spriteBatch.DrawRectangle(track[i] + hud.Camera, 1, 1, new Color(f, 0.5f * f, 0, f), DrawOrder.Flyer-0.01f + additionalLayerDepth);
            }
            if (Weapon != null) Weapon.Draw(spriteBatch, gameTime); 
        }
    }
}
