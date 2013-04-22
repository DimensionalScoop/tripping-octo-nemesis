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
        public enum Condition { InHangar, Airborne, Deployed, ReturningPhase1, ReturningPhase2, Repairing }
        public Condition Status = Condition.InHangar;
        public Color StatusColor
        {
            get
            {
                switch (Status)
                {
                    case SpaceShip.Condition.InHangar: return Color.AntiqueWhite;
                    case SpaceShip.Condition.Deployed: return Color.DarkOrange;
                    case SpaceShip.Condition.Airborne: return Color.Lerp(Color.Red, Color.LightBlue, Hitpoints / (float)MaxHitpoints);
                    case SpaceShip.Condition.Repairing: return Color.Lerp(Color.MediumVioletRed, Color.GreenYellow, Hitpoints / (float)MaxHitpoints);
                    case SpaceShip.Condition.ReturningPhase1: return Color.Gray;
                    case SpaceShip.Condition.ReturningPhase2: return Color.Gray;
                    default: throw new NotImplementedException();
                }
            }
        }

        public Fraction Fraction;

        public int Hitpoints = 10;
        public int MaxHitpoints = 10;

        public event Action<SpaceShip> HitpointsChanged;
        public event Action<SpaceShip> StatusChanged;

        public void DealDamage(int damage)
        {
            Debug.Assert(damage<=0);

            Hitpoints = (int)MathHelper.Clamp(Hitpoints + damage, 0, MaxHitpoints);
            if (HitpointsChanged != null) HitpointsChanged(this);

            if (Hitpoints == 0) Delete();
        }
    }
}