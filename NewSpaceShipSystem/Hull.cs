using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Hull:Subsystem,IDamage
    {
        public float MaxHullPoints;
        public float CurrentHullPoints;

        public Hull(float maxHullPoints):base(0,20,"Hull")
        {
            MaxHullPoints = maxHullPoints;
            CurrentHullPoints = MaxHullPoints;


            StatusReport.Write("Hull Integrity: ");
            StatusReport.Write(() => "" + (int)CurrentHullPoints, HullStatusColor);
            StatusReport.Write(() => "/" + (int)MaxHullPoints + " hp ");
            StatusReport.AddBarGraph(10, () => CurrentHullPoints / MaxHullPoints, HullStatusColor);
            StatusReport.Write("\n");
        }

        Color HullStatusColor()
        {
            if (CurrentHullPoints == MaxHullPoints) return Color.Green;
            if (CurrentHullPoints > MaxHullPoints * 0.5f) return Color.Lerp(Color.GreenYellow, Color.Yellow, (CurrentHullPoints / MaxHullPoints - 0.5f) * 2);
            if (CurrentHullPoints > MaxHullPoints * 0.2f) return Color.Orange;
            if (CurrentHullPoints <= 0) return Color.Gray;
            return Color.Red;
        }

        public void Damage(float amountOfDamage)
        {
            CurrentHullPoints -= amountOfDamage;
            if (CurrentHullPoints >= 0) Parant.Destroy("due to massive hull damage");
        }

        public float DamageSuppression() { return MaxHullPoints; }
    }
}