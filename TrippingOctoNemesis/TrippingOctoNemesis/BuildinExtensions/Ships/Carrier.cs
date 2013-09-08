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
    public class Carrier:SpaceShip
    {
        public Carrier(Fraction fraction)
            : base(fraction)
        {
            AddSubsystem(new Hull(80));
            AddSubsystem(new Shields(5f));
            AddSubsystem(new EnergyDistributor());
            AddSubsystem(new HullDrawer() { Sprite = new SpriteSheet("s\\mothership") });
            AddSubsystem(new EngineDrawer());
            AddSubsystem(new InfoModule(ShipClasses.Carrier));
            AddSubsystem(new Storage(10));
            AddSubsystem(new CarrierPilot());
            AddSubsystem(new Gear());
            AddSubsystem(new LandingSlot("Carrier Landing Bay"));
            AddSubsystem(new Engines());

            var hullDrawer=FindSubsystem<HullDrawer>();
            var engineDrawer = FindSubsystem<EngineDrawer>();
            engineDrawer.Trails.AddLast(new Trail() { Position = new Vector2(-18, hullDrawer.Sprite.TextureOrigin.Y - 2) });
            engineDrawer.Trails.AddLast(new Trail() { Position = new Vector2(-16, hullDrawer.Sprite.TextureOrigin.Y - 2) });
            engineDrawer.Trails.AddLast(new Trail() { Position = new Vector2(18, hullDrawer.Sprite.TextureOrigin.Y - 2) });
            engineDrawer.Trails.AddLast(new Trail() { Position = new Vector2(16, hullDrawer.Sprite.TextureOrigin.Y - 2) });

            hullDrawer.IntPosition = true;
            hullDrawer.AdditionalLayerDepth = -0.02f;
        }
    }
}
