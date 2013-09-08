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
using Nuclex.Support.Collections;

namespace TrippingOctoNemesis.SPS
{
    public struct TrailElement
    {
        public Vector2 Position;
        public TimeSpan GenerationTime; 
    }

    public class Trail
    {
        public Deque<TrailElement> Trail = new Deque<TrailElement>();
        public Vector2 Position = Vector2.Zero;
        public Color Color = Color.Orange;
        public TimeSpan TrailDuration = TimeSpan.FromSeconds(3);

        public float PercentualLifetime(GameTime gameTime, int element)
        {
            return (float)((gameTime.TotalGameTime - Trail[element].GenerationTime).TotalMilliseconds / TrailDuration.TotalMilliseconds);
        }
    }

    public class EngineDrawer:Subsystem,IDrawable
    {
        bool stopTrackGeneration = false;

        public Deque<Trail> Trails = new Deque<Trail>();


        public EngineDrawer()
            : base(0, -1, "Engine trail drawing system")
        {

        }

        public void Show()
        {
            stopTrackGeneration = false;
        }

        public void Hide()
        {
            stopTrackGeneration = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!stopTrackGeneration)
                for (int i = 0; i < Trails.Count; i++)
                {
                    Trails[i].Trail.AddLast(new TrailElement() { GenerationTime = gameTime.TotalGameTime, Position = Trails[i].Position.Rotate(Parant.Heading) });

                    //while (Trails[i].Count > TrailLengh) Trails[i].RemoveFirst();
                }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var item in Trails)
            {
                for (int i = 0; i < item.Trail.Count; i++)
                {
                    if (i == 0) GameControl.BackPlotter.DrawLine(Parant.Position + GameControl.Hud.Camera, item.Trail[0].Position + GameControl.Hud.Camera, item.Color, Color.Lerp(item.Color, Color.Transparent, item.PercentualLifetime(gameTime, 0)));
                    else GameControl.BackPlotter.DrawLine(item.Trail[i - 1].Position + GameControl.Hud.Camera, item.Trail[i].Position + GameControl.Hud.Camera, Color.Lerp(item.Color, Color.Transparent, item.PercentualLifetime(gameTime, i-1)), Color.Lerp(item.Color, Color.Transparent, item.PercentualLifetime(gameTime, i)));

                    if (item.Trail[i].GenerationTime + item.TrailDuration < gameTime.TotalGameTime) item.Trail.RemoveAt(i--);
                }
            }
        }
    }
}
