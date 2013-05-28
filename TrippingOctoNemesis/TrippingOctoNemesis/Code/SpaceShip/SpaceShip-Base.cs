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
    public partial class SpaceShip
    {
        public static bool DeleteableShips { get; private set; }

        protected static Random Random = new Random();

        public bool DeleteFlag { get; private set; }
        public event Action<SpaceShip> DeleteFlagSet;

        public SpaceShip(Fraction fraction)
        {
            EnginePositions[0] = Vector2.Zero;
            Fraction = fraction;
            Color = Color.Lerp(Color.White,Fraction.Color,fractionColorBrightness);
            Weapon = new Weapon(this);
            Class = ShipClasses.Fighter;
        }


        public enum DeleteReasons { Destroyed, Debug }
        public void Delete(DeleteReasons reason= DeleteReasons.Destroyed)
        {
            if (reason == DeleteReasons.Destroyed)
            {
                Particle.Add(new Particles.Explosion(Position, (int)this.Sprite.TextureOrigin.Length()));
                Particle.Add(new Particles.DestroiedShip(Position,Vector2.Zero.Transform(Angle,Speed), Sprite) { Color = Color, Rotation = MathHelper.PiOver2 + Angle });
            }
            DeleteableShips = true;
            DeleteFlag = true;
            if (DeleteFlagSet != null) DeleteFlagSet(this);
        }
    }
}