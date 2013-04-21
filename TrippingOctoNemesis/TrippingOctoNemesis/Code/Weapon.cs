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
    public class Weapon
    {
        public int Damage = 2;
        public int RangeSquared = 100 ^ 2;
        public TimeSpan WeaponCooldown = TimeSpan.FromMilliseconds(1500);

        protected TimeSpan LastShoot;
        protected TimeSpan ShootDuration = TimeSpan.FromMilliseconds(500);


        public void Update(GameTime gameTime,List<SpaceShip> enem)
        {
            if (gameTime.TotalGameTime > LastShoot + WeaponCooldown)
            {
                
                
                LastShoot = gameTime.TotalGameTime;

            }
        }

        public void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {

        }
    }
}
