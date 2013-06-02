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
using System.Diagnostics;

namespace TrippingOctoNemesis.MapObjects
{
    /// <summary>
    /// Spawns the given range of ships.
    /// </summary>
    public class Spawn:MapObject
    {
        bool spawnRelative;

        protected SpaceShip[] Ships;

        public Spawn(params SpaceShip[] ships) : this(true, ships) { }
        public Spawn(bool spawnRelative, params SpaceShip[] ships)
        {
            Debug.Assert(ships != null);

            VisibleOnMap = true;
            Color = Color.DarkRed;
            Size = ships.Sum(p => p.DC);
            Ships = ships;
            Name=GetName();
            this.spawnRelative = spawnRelative;
        }
        /// <summary>
        /// Spawns all ships times repetitions.
        /// </summary>
        /// <param name="spawnRelative"></param>
        /// <param name="repetitions"></param>
        /// <param name="ships"></param>
        public Spawn(bool spawnRelative,int repetitions,Func<int,Vector2> position, params SpaceShip[] ships)
        {
            Debug.Assert(ships != null);

            VisibleOnMap = true;
            Color = Color.DarkRed;
            List<SpaceShip> shipsToAdd = new List<SpaceShip>();
            for (int i = 1; i <= repetitions; i++)
            {//BUG: deep copy ships!!
                foreach (var elem in ships)
                    elem.Position = position(i);
                shipsToAdd.AddRange(ships);
            }
            Ships = shipsToAdd.ToArray();
            Size = Ships.Sum(p => p.DC);
            Name = GetName();
            this.spawnRelative = spawnRelative;
        }

        private string GetName()
        {
            string returnValue = "";

            if (Ships.All(p =>
                p.Fraction.IsEnemy(GameControl.Player[0]) &&
                p.Fraction.IsEnemy(GameControl.Player[1])))
                returnValue += "Enemy ";
            else if (Ships.All(p =>
                p.Fraction.IsAlley(GameControl.Player[0]) &&
                p.Fraction.IsAlley(GameControl.Player[1])))
            {
                if (Ships.All(p =>
                p.Fraction == GameControl.Player[0] ||
                p.Fraction == GameControl.Player[1]))
                    returnValue += "Friendly ";
                else
                    returnValue += "Neutral ";
            }
            else
                returnValue += "Unidentified ";

            if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Fighter))
                returnValue += "Fighter Wing";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Supporter))
                returnValue += "Support Wing";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Transporter))
                returnValue += "Unguarded Transporters";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Carrier))
                returnValue += "Carrier";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Transporter || p.Class == SpaceShip.ShipClasses.Fighter))
                returnValue += "Convoy";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Transporter || p.Class == SpaceShip.ShipClasses.Fighter || p.Class == SpaceShip.ShipClasses.Supporter))
                returnValue += "Supported Convoy";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Transporter || p.Class == SpaceShip.ShipClasses.Fighter || p.Class == SpaceShip.ShipClasses.Supporter || p.Class == SpaceShip.ShipClasses.Carrier))
                returnValue += "Heavy Convoy";
            else if (Ships.All(p => p.Class == SpaceShip.ShipClasses.Missile))
                returnValue += "Missile Strike";
            else
                returnValue += "Ships";

            returnValue += " " + Ships.GetHashCode().ToString("x");

            return returnValue;
        }

        public override void Activated(GameTime gameTime)
        {
            foreach (var ship in Ships)
            {
                if (spawnRelative)
                {//FIX: revise
                    ship.Position -= GameControl.Hud.Camera;//assert that ships are always spawned relative to the position of the player
                    ship.TargetPosition -= GameControl.Hud.Camera;
                    ship.Spawn();
                }
                GameControl.Ships.Add(ship);
            }

            Delete();
            
            base.Activated(gameTime);
        }
    }
}
