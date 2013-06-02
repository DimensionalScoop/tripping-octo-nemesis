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
    public class Predator : Missile
    {
        public Predator(Fraction fraction, Func<Missile, SpaceShip> target)
            : base(fraction, target)
        {
            Sprite = new SpriteSheet("s\\predator");
            ExplosionDamage = 15;
            SpeedGainPerSecond = 50;
            MaxSpeed = 1000;
            MaxHitpoints = 4; Hitpoints = 4;
            ExplosionRangeSquared = (int)Math.Pow(100, 2);
            ExplosionSize = 100;
            MaxRange = TimeSpan.FromSeconds(10);
            ExplosionColor = Color.LightYellow;
            //TODO: render ships in carrier 'invisible' to other ships.
            StackEvents.Add(TimeSpan.FromMilliseconds(0));
            StackEventsReaction.Add(FirstEngine);
            StackEvents.Add(TimeSpan.FromMilliseconds(1200));
            StackEventsReaction.Add(SecondEngine);
        }

        protected virtual void FirstEngine()
        {
            SetEngines(20, new Vector2(-2,-3), new Vector2(-2,4));
        }

        protected virtual void SecondEngine()
        {
            SetEngines(35, new Vector2(-3,-1), new Vector2(-3,0),new Vector2(-3,1));
        }
    }
}