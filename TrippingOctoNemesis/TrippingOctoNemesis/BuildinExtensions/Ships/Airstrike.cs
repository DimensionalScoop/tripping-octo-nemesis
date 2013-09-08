using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrippingOctoNemesis.SPS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;

namespace TrippingOctoNemesis.BuildinExtensions.Ships
{
    public class Airstrike:SpaceShip
    {
        public Airstrike(Fraction fraction)
            : base(fraction)
        {
            AddSubsystem(new Hull(10));
            AddSubsystem(new Shields(0.5f));
            AddSubsystem(new EnergyDistributor());
            AddSubsystem(new HullDrawer());
            AddSubsystem(new EngineDrawer());
            AddSubsystem(new InfoModule(ShipClasses.Fighter));
            AddSubsystem(new Storage(10));
            AddSubsystem(new TargetingAI());
            AddSubsystem(new Autopilot());
            AddSubsystem(new Gear());
            AddSubsystem(new Engines());
            AddSubsystem(new Weapon("Laser"));

            var engineDrawer = FindSubsystem<EngineDrawer>();
            engineDrawer.Trails.AddLast(new Trail());
        }
    }
}
