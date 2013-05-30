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
using System.Diagnostics;

namespace TrippingOctoNemesis
{
    public partial class SpaceShip
    {
        public enum ShipClasses { None, Fighter, Carrier, Supporter, Transporter, Special }
        public enum Conditions { InHangar, Airborne, Deployed, ReturningPhase1, ReturningPhase2, Repairing }

        public Conditions Status = Conditions.InHangar;
        public bool IsAirborne
        {
            get
            {
                switch (Status)
                {
                    case Conditions.Airborne: return true;
                    case Conditions.Deployed: return true;
                    case Conditions.InHangar: return false;
                    case Conditions.Repairing: return false;
                    case Conditions.ReturningPhase1: return true;
                    case Conditions.ReturningPhase2: return true;
                    default: throw new NotImplementedException();
                }
            }
        }
        /// <summary>
        /// Color of the ship icon displayed in the carrier ui.
        /// </summary>
        public Color StatusColor
        {
            get
            {
                switch (Status)
                {
                    case Conditions.InHangar: return Color.AntiqueWhite;
                    case Conditions.Deployed: return Color.DarkOrange;
                    case Conditions.Airborne: return Color.Lerp(Color.Red, Color.LightBlue, Hitpoints / (float)MaxHitpoints);
                    case Conditions.Repairing: return Color.Lerp(Color.MediumVioletRed, Color.GreenYellow, Hitpoints / (float)MaxHitpoints);
                    case Conditions.ReturningPhase1: return Color.Gray;
                    case Conditions.ReturningPhase2: return Color.Gray;
                    default: throw new NotImplementedException();
                }
            }
        }
        /// <summary>
        /// The ship icon displayed in the carrier ui.
        /// </summary>
        public Sprite Icon = new Sprite("i\\inc-point");

        public Fraction Fraction;

        public int Hitpoints = 10;
        public int MaxHitpoints = 10;
        public ShipClasses Class = ShipClasses.Fighter;

        /// <summary>
        /// Set to appropriated value if ship has special powers which make her stronger/weaker.
        /// </summary>
        protected float DCModifier;
        /// <summary>
        /// Measures how difficult it is to fight this ship.
        /// </summary>
        public float DC
        {
            get
            {
                float returnValue = MaxHitpoints;
                if (Weapon != null) returnValue *= (float)(Weapon.Damage / Weapon.WeaponCooldown.TotalSeconds);
                returnValue += DCModifier;

                if (Class == ShipClasses.Carrier) returnValue *= 5;
                if (Class == ShipClasses.Transporter) returnValue /= 3;
                return returnValue;
            }
        }

        public event Action<SpaceShip> HitpointsChanged;
        public event Action<SpaceShip> StatusChanged;

        /// <summary>
        /// Deals the given amount of damage to this ship.
        /// </summary>
        /// <param name="damage"></param>
        public void DealDamage(int damage)
        {
            Debug.Assert(damage<=0);

            Hitpoints = (int)MathHelper.Clamp(Hitpoints + damage, 0, MaxHitpoints);
            if (HitpointsChanged != null) HitpointsChanged(this);

            if (Hitpoints == 0) Delete();
        }
    }
}