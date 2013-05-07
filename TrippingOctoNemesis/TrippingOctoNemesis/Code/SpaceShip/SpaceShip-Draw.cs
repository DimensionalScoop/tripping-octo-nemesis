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

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public SpriteSheet Sprite = new SpriteSheet("s\\ship");
        public Color Color = Color.White;
        public Color DamageParticleBaseColor = Color.White;
        public float Scale = 1f;

        const float fractionColorBrightness = 0.5f;
        protected float additionalLayerDepth;

        List<Vector2> track = new List<Vector2>();
        protected int TrackLength = 50;
        protected Vector2[] EnginePositions = new Vector2[1];


        protected void CalcTrack()
        {
            foreach (var engine in EnginePositions)
            {
                var pos = Position + engine.Rotate(Angle);//Transform(-engine.Angle(Vector2.Zero) + Angle, engine.Length());
                track.Add(pos);
                track.Add(pos - Direction * 2);
            }

            while (track.Count > TrackLength) track.RemoveAt(0);
        }


        /// <summary>
        /// Value to prevent flickering on overlapping sprites of same depth
        /// </summary>
        float depthVarriation = Random.NextFloat() / 10000;
        public virtual void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {

            if (DeleteFlag) return;
            if (Status == Condition.InHangar || Status == Condition.Repairing) return;

            spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, gameTime, Color, Sprite.TextureOrigin, MathHelper.PiOver2 + Angle, Scale, DrawOrder.Flyer + additionalLayerDepth + depthVarriation);
            //spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, null, Color, MathHelper.PiOver2 + Angle, Sprite.TextureOrigin, Scale, SpriteEffects.None, DrawOrder.Flyer+additionalLayerDepth);
            for (int i = 0; i < track.Count; i++)
            {
                float f = i / (float)TrackLength;
                Basic.DrawRectangle(spriteBatch, track[i] + hud.Camera, 1, 1, new Color(f, 0.5f * f, 0, f), DrawOrder.Flyer-0.01f + additionalLayerDepth);
            }
            if (Weapon != null) Weapon.Draw(spriteBatch, hud, gameTime);
        }
    }
}
