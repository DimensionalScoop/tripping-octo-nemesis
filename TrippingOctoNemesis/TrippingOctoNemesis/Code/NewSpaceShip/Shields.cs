using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippingOctoNemesis.SPS
{
    class Shields:Subsystem,IDamage
    {
        public float MaxShieldPower;
        public float CurrentShieldPower;

        public float MaxShieldPerSecond = 2;
        public float CurrentShieldPerSecond;

        TimeSpan LastShieldHit;
        public TimeSpan DamageTimeout = TimeSpan.FromSeconds(2);

        public Shields(float maxShieldPower,int maxOverload=2)
            : base(5, 5, "Shield subsystem",maxOverload)
        {
            MaxShieldPower = maxShieldPower;
            MinEnergyDemand = 10;

            StatusReport.Write("Shield power: ");
            StatusReport.Write(()=>""+(int)CurrentShieldPower, ShieldColor);
            StatusReport.Write(() => "/" + (int)MaxShieldPower + " shl ");
            StatusReport.AddBarGraph(10, ()=>CurrentShieldPower / MaxShieldPower, ShieldColor);
            StatusReport.Write("\n");
            StatusReport.Write(() => "Regeneration rate: " + CurrentShieldPerSecond.ToString("0.00") + "/" + MaxShieldPerSecond + " shl/s\n");
        }

        Color ShieldColor()
        {
            if (CurrentShieldPower == MaxShieldPower) return Color.Green;
            if (CurrentShieldPower > MaxShieldPower * 0.5f) return Color.LightBlue;
            if (CurrentShieldPower > MaxShieldPower * 0.2f) return Color.Yellow;
            if (CurrentShieldPower <= 0) return Color.Gray;
            return Color.Red;
        }

        public void Damage(float amountOfDamage)
        {
            if (amountOfDamage >0)
            {
                //TODO: Set DamageTimer
                //DamageTimeout = GameControl;
            }

            float newShields = CurrentShieldPower - amountOfDamage;
            if (newShields >= 0)
            {
                CurrentShieldPower = newShields;
                return;
            }
            else
            {
                float relayedDamage = -newShields - CurrentShieldPower;
                CurrentShieldPower = 0;
                Parant.FindSubsystem<IDamage>(Priority - 1).Damage(relayedDamage);
            }
        }

        public float DamageSuppression()
        {
            return MaxShieldPower * MaxShieldPerSecond / (float)DamageTimeout.TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            if (EnergyOverload >= 1)
            {
                float maxShieldPerSecond = MaxShieldPerSecond * Math.Min(EnergyOverload, MaxOverload);
                if (gameTime.TotalGameTime - LastShieldHit < DamageTimeout)
                    CurrentShieldPerSecond = MathHelper.SmoothStep(0, maxShieldPerSecond, (float)((gameTime.TotalGameTime - LastShieldHit).TotalMilliseconds / DamageTimeout.TotalMilliseconds));
                else
                    CurrentShieldPerSecond = maxShieldPerSecond;
            }
            else
                CurrentShieldPerSecond = -Math.Min(5, 1 / EnergyOverload);

            if (CurrentShieldPower != MaxShieldPower)
            {
                CurrentShieldPower += CurrentShieldPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                CurrentShieldPower = MathHelper.Clamp(CurrentShieldPower, 0, MaxShieldPower);
            }
            
            base.Update(gameTime);
        }
    }
}
