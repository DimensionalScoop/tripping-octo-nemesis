using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippingOctoNemesis.SPS
{
    public enum GearConditions { Landed, Landing, Launching, Airborne }
    
    /// <summary>
    /// Steers spaceship during flight.
    /// </summary>
    public interface IPilot
    {
        Vector2 Target { get;  }
        void SetTarget(Func<Vector2> position);
        bool TargetReached{get;}
        int DistanceToTarget { get; }


    }

    public interface IDrawable
    {
        void Show();
        void Hide();
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface IEngine
    {
        float MinSpeed { get; }
        float MaxSpeed{get;}
        float MaxTurnSpeed { get; }
        float EngineAcceleration { get; }
        void SetSpeed(float percentage);
    }

    public interface IStorage
    {
        Item GetItem(Item item);
        Item StoreItem(Item item);
    }

    /// <summary>
    /// Provides spaceship with energy.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// How much energy this systems makes available to the spaceship.
        /// </summary>
        /// <returns></returns>
        float EnergyGeneration { get; }
    }


    /// <summary>
    /// Allows other spaceships to land on this subsystem.
    /// </summary>
    public interface ILandingSlot
    {
        int Capacity { get; }
        bool HasCapacityLeft { get; }
        Vector2 ReceiveLandingSlot(SpaceShip ship);
        Vector2 LeaveLandingSlot(SpaceShip ship);
    }

    /// <summary>
    /// Steers spaceship during landing and takeoff. Also manages ship while landed.
    /// </summary>
    public interface IGear
    {
        GearConditions Status { get; }
        void Land(ILandingSlot location);
        void Launch();
    }

    /// <summary>
    /// Absorbs and manages damage.
    /// </summary>
    public interface IDamage
    {
        /// <summary>
        /// Transfers an amount of damage on this subsystem.
        /// </summary>
        /// <param name="amountOfDamage"></param>
        void Damage(float amountOfDamage);
        /// <summary>
        /// Measures the ability to take damage without being destroyed. 
        /// E.g.: The damage suppression of a hull is it's maxHullPoints;
        /// the damage suppression of a shield is e.g. maxShieldPower*shieldRegenerationPerSecond
        /// as a shield damage can be regenerated as opposed to hull damage.
        /// </summary>
        /// <returns></returns>
        float DamageSuppression();
    }
}