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

namespace TrippingOctoNemesis.Extensions.Events
{
    public class Wait : Event
    {
        public override string Name
        {
            get
            {
                return "Wait";
            }
        }


        TimeSpan amount;

        public Wait(TimeSpan amount,Encounter parant, Game game)
            : base(parant, game)
        {
            this.amount = amount;
        }

        public override void Update(GameTime gameTime)
        {
            amount -= gameTime.ElapsedGameTime;
            if (amount <= TimeSpan.Zero) Delete();

            base.Update(gameTime);
        }
    }
    
    public class Distance:Wait
    {
        public override string Name
        {
            get
            {
                return "Distance";
            }
        }

        public Distance(int distance, Encounter parant, Game game)
            : base(TimeSpan.FromSeconds(distance / parant.Hud.CameraSpeed.Y), parant, game) { }
    }

    //TODO: Implement Spawn Enemy Groups
    public class Spawn : Event
    {
        public override string Name
        {
            get
            {
                return "Spawn";
            }
        }


        public Spawn(Group enemies,Encounter parant, Game game)
            : base(parant, game)
        {

        }

        public override void Begin(StatusReport report)
        {


            base.Begin(report);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
    }

    public class Clear : Wait
    {
        public override string Name
        {
            get
            {
                return "Clear";
            }
        }

        public Clear(Encounter parant, Game game, TimeSpan? timeout = null)
            : base(
            timeout == null ?
            TimeSpan.MaxValue :
            (TimeSpan)timeout,
            parant, game) { }

        public override void Update(GameTime gameTime)
        {
            if (!Parant.Player.Select(p => Parant.Ships.Any(q => q.Fraction.IsEnemy(p)))
                .Any(p => p == true))
                Delete();

            base.Update(gameTime);
        }
    }

    //TODO: Implement Jump Points
    public class JumpPoint : Event
    {
        string name;
        Encounter destination;

        public override string Name
        {
            get
            {
                return name+" Jump Point";
            }
        }

        public JumpPoint(string name, Encounter destination, Encounter parant, Game game)
            : base(parant, game)
        {
            this.name = name;
            this.destination = destination;
        }

        public override void Begin(StatusReport report)
        {


            base.Begin(report);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
    }
}
