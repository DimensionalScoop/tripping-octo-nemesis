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
    public class SpaceShip
    {
        public bool IntPosition;
        public Vector2 Position=new Vector2(100,100);
        public Vector2 TargetPosition = new Vector2(500, 500);
        public bool HasTarget = true;
        public bool KeepScreenPosition = true;
        public Fraction Fraction;
        public float Angle;
        public Vector2 Direction { get { return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)); } }
        public float Speed = 120;
        public float AngleSpeed=12;

        public Weapon Weapon;
        public int Hitpoints;
        public int MaxHitpoints;
        public TimeSpan ActionTimer;
        public TimeSpan LaunchTime;

        public Sprite Sprite = new Sprite("s\\ship");
        public Color Color = Color.White;
        public float Scale = 1f;

        List<Vector2> track = new List<Vector2>();
        protected int TrackLenght = 50;
        protected Vector2[] EnginePositions = new Vector2[2];
        Random random = new Random();


        public event Action WasDamaged;

        public SpaceShip()
        {
            EnginePositions[0] = Vector2.Zero;//new Vector2(-Sprite.TextureOrigin.X, Sprite.TextureOrigin.Y);
            EnginePositions[1] = Vector2.Zero;//new Vector2(Sprite.TextureOrigin.X, Sprite.TextureOrigin.Y);
        }

        public virtual void Update(GameTime gameTime,Hud hud)
        {
            CalcTrack();

            CalcTarget(gameTime, hud);

            CalcMovement(gameTime);
        }

        protected void CalcMovement(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void CalcTrack()
        {
            foreach (var engine in EnginePositions)
            {
                var pos = Position + Vector2.Zero.Transform(engine.Angle(Vector2.Zero) - Angle, engine.Length());
                track.Add(pos);
                track.Add(pos - Direction * 2);
            }

            if (track.Count > TrackLenght) track.RemoveRange(0, EnginePositions.Length*2);
        }

        protected void CalcTarget(GameTime gameTime, Hud hud)
        {
            if (HasTarget)
            {
                float targetAngle = (float)Math.Atan2((TargetPosition - Position).Y, (TargetPosition - Position).X);
                float difference = MathHelper.WrapAngle(targetAngle - Angle);
                Angle = MathHelper.WrapAngle(Angle + difference * (float)gameTime.ElapsedGameTime.TotalSeconds * AngleSpeed
                    * MathHelper.SmoothStep(0, 1, MathHelper.Clamp((TargetPosition - Position).LengthSquared() / 40000, 0, 1))
                    );//TODO: add better steering behavior

                if (KeepScreenPosition) TargetPosition -= hud.CameraDelta;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            spriteBatch.Draw(Sprite, IntPosition ? (Position + hud.Camera).Round() : Position + hud.Camera, null, Color, MathHelper.PiOver2 + Angle, Sprite.TextureOrigin, Scale, SpriteEffects.None, 0);
            for (int i = 0; i < track.Count; i++)
            {
                float f = i / (float)TrackLenght;
                Basic.DrawRectangle(spriteBatch, track[i] + hud.Camera, 1, 1, new Color(f, 0.5f * f, 0, f));
            }
        }
    }
}