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
    public class D1Enemy:MotherShip
    {
        TimeSpan lastDeploy;
        TimeSpan deployCooldown;
        TimeSpan deployDuration;
        int availableFlyer;
        int packSize;
        int currentPackSize;

        public D1Enemy(Hud hud,Fraction fraction,GameTime gameTime)
            : base(hud, fraction, gameTime)
        {
            Sprite = new Sprite("e\\Tripping-Octo-Nemesis-Base");
            Status = Condition.Airborne;
            HasTarget = true;
            Speed = 1000/16+10;
            AngleSpeed = 1;
            Angle = 0;// -MathHelper.PiOver2;
            availableFlyer = 9;
            packSize = 3;
            currentPackSize = 3;
            deployCooldown = TimeSpan.FromSeconds(10);
            deployDuration = TimeSpan.FromMilliseconds(500);

            EnginePositions = new Vector2[0];
            //EnginePositions[0] = new Vector2(-7, -Sprite.TextureOrigin.Y+5);
            //EnginePositions[1] = new Vector2(7, -Sprite.TextureOrigin.Y+5);
            //EnginePositions[2] = new Vector2(Sprite.TextureOrigin.X - 2, -Sprite.TextureOrigin.Y);
            //EnginePositions[3] = new Vector2(-Sprite.TextureOrigin.X+2, -Sprite.TextureOrigin.Y);
        }

        public override void Update(GameTime gameTime, Hud hud, List<SpaceShip> otherSpaceShips)
        {
            base.Update(gameTime, hud, otherSpaceShips);

            if (availableFlyer > 0 && gameTime.TotalGameTime > lastDeploy + deployCooldown + TimeSpan.FromMilliseconds((packSize - currentPackSize) * deployDuration.TotalMilliseconds))
            {
                var ship = new SpaceShip(Fraction,gameTime) { KI = KIs.NearestEnemy, Carrier = this };
                otherSpaceShips.Add(ship);
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
