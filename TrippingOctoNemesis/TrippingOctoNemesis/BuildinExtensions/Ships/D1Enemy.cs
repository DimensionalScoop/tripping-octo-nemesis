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
    public class D1Enemy:Carrier
    {
        TimeSpan lastDeploy;
        TimeSpan deployCooldown;
        TimeSpan deployDuration;
        int availableFlyer;
        int packSize;
        int currentPackSize;

        public D1Enemy(Hud hud,Fraction fraction)
            : base(hud, fraction)
        {
            Sprite = new SpriteSheet("s\\Tripping-Octo-Nemesis-Base");
            Status = Conditions.Airborne;
            Speed = 1000/16+10;
            AngleSpeed = 1;
            Angle = 0;
            availableFlyer = 9;
            packSize = 3;
            currentPackSize = 3;
            deployCooldown = TimeSpan.FromSeconds(10);
            deployDuration = TimeSpan.FromMilliseconds(500);
            Class = ShipClasses.Carrier;

            SetEngines(0,null);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (availableFlyer > 0 && gameTime.TotalGameTime > lastDeploy + deployCooldown + TimeSpan.FromMilliseconds((packSize - currentPackSize) * deployDuration.TotalMilliseconds))
            {
                var ship = new OctoJelly(Fraction) { Carrier = this, Ki=new NearestEnemy() };
                GameControl.Ships.Add(ship);
                ship.Deploy(Position, Vector2.Zero, gameTime);
                availableFlyer--;
                currentPackSize--;

                if (currentPackSize == 0)
                {
                    currentPackSize = packSize;
                    lastDeploy = gameTime.TotalGameTime;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Hud hud, GameTime gameTime)
        {
            base.Draw(spriteBatch, hud, gameTime);
        }
    }
}
