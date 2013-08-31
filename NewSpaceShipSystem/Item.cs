using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    public class Item//TODO: Implement item types with different properties, e.g. "Powerful Bullets" which deal more damage than normal "Bullets" but can be used as a substitute for "Bullets"
    {
        public readonly string Name;
        public readonly string Description;
        public readonly float Value;
        public float Amount;

        public Item(string name,string description, float value, float amount)
        {
            Name = name;
            Description = description;
            Value = value;
            Amount = amount;
        }

        public static Item GetFuel(float amount)
        {
            return new Item("Fuel","A matter-anti-matter mix storing a high amount of energy. Main power source of many spaceship engines.",1,amount);
        }

        public static Item GetBullets(float amount)
        {
            return new Item("Bullets","Ammunition for mechanical weapons. Powerful, but heavy and expensive", 3, amount);
        }

        public static Item GetIntocnitum(float amount)
        {
            return new Item("Intocnitum","A very valuable material which is used as an alloy material.", 10, amount);
        }

        public static Item GetBuildingBricks(float amount)
        {
            return new Item("Building Bricks","The main building material for spaceships.", 0.5f, amount);
        }

        public static Item GetEmpty()
        {
            return new Item("Emptiness", "So empty you won't believe it.", 0, 0);
        }
    }
}
