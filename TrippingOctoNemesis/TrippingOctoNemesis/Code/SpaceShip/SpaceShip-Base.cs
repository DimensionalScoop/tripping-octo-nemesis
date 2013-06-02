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
            SetEngines(Vector2.Zero);
            Fraction = fraction;
            Color = Color.Lerp(Color.White,Fraction.Color,fractionColorBrightness);
            Weapon = new Laser(this,Vector2.Zero);
            Class = ShipClasses.Fighter;
            Ki = new NearestEnemy();
        }


        public enum DeleteReasons { Destroyed, Debug, SelfDestruction }
        public void Delete(DeleteReasons reason= DeleteReasons.Destroyed)
        {
            if (DeleteFlag) return;

            if (reason == DeleteReasons.Destroyed||reason==DeleteReasons.SelfDestruction)
            {
                Particle.Add(new Particles.Explosion(Position, (int)this.Sprite.TextureOrigin.Length()));
                Particle.Add(new Particles.DestroyedShip(Position,Vector2.Zero.Transform(Angle,Speed), Sprite) { Color = Color, Rotation = MathHelper.PiOver2 + Angle });
            }
            DeleteableShips = true;
            DeleteFlag = true;
            if (DeleteFlagSet != null) DeleteFlagSet(this);
        }
    }
}