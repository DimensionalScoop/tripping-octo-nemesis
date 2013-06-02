using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrippingOctoNemesis.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using X45Game.Extensions;
using TrippingOctoNemesis.MapObjects;

namespace TrippingOctoNemesis
{
    public class TestMap : Map
    {
        public override string Name
        {
            get { return "Test Map"; }
        }
        public override Color Color
        {
            get { return Color.DarkRed; }
        }
        public override Guid Id
        {
            get { return Guid.Parse("a332e9f7-d4ad-4f79-b0e2-9d533e494f58"); }
        }

        public TestMap()
            : base()
        {
            NextMap = Id;
        }

        public override void CreateMapObjects()
        {
            AddEnemies();
        }

        private void AddEnemies()
        {
            Entities.AddRange(new MapObject[]{

                new Warp(1),
                new Wait(1),
                new Warp(0,1),
                new Wait(1),
                new Spawn(new Predator(GameControl.Enemys[1],p=>GameControl.Player[0].Carrier){Position=new Vector2(100,-10),Status= SpaceShip.Conditions.Airborne}),
                new Spawn(new Predator(GameControl.Enemys[1],p=>GameControl.Player[0].Carrier){Position=new Vector2(300,-10),Status= SpaceShip.Conditions.Airborne}),
                new Spawn(new Predator(GameControl.Enemys[1],p=>GameControl.Player[1].Carrier){Position=new Vector2(500,-10),Status= SpaceShip.Conditions.Airborne}),
                new Spawn(new Predator(GameControl.Enemys[1],p=>GameControl.Player[1].Carrier){Position=new Vector2(800,-10),Status= SpaceShip.Conditions.Airborne}),
                new Spawn(new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(100,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(200,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(300,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(400,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(500,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(600,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(700,-300),Status= SpaceShip.Conditions.Airborne},
                    new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(800,-300),Status= SpaceShip.Conditions.Airborne}),
                new Clear(),
                new Spawn(new D1Enemy(GameControl.Hud, GameControl.Enemys[0]) { Position = new Vector2(300, -200), Ki=new SpaceShip.KeepScreenPosition(new Vector2(500, 200)) }),
                new Clear(),
                new Spawn(new D1Enemy(GameControl.Hud, GameControl.Enemys[0]) { Position = new Vector2(300, -200), Ki=new SpaceShip.KeepScreenPosition(new Vector2(500, 200)) }),
                new Clear(),
                new Spawn(true,5,p=>new Vector2(p*200,-100-p*20),new SpaceShip(GameControl.Enemys[1]){Position=new Vector2(400,-300),Status= SpaceShip.Conditions.Airborne}),
                new Clear()

            });
        }
    }
}
