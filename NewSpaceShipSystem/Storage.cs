using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X45Game.Extensions;

namespace NewSpaceShipSystem
{
    public class StorageSlot : Item
    {
        public float MaxAmountEverStored;

        public StorageSlot(string name, string description, float value, float amount)
            : base(name, description, value, amount)
        {
            MaxAmountEverStored = amount;
        }
    }

    public class Storage:Subsystem,IStorage
    {
        Dictionary<string, StorageSlot> storage = new Dictionary<string, StorageSlot>();
        
        public float MaxNumberOfItems;
        public float NumberOfItems;

        const float BigStorageValue = 10000;


        public Storage(float capacity)
            : base(3, 4, "Storage")
        {
            MaxNumberOfItems = capacity;

            StatusReport.Write(() => "Storage Capacity: " + NumberOfItems.ToKFloats() + "/" + MaxNumberOfItems + " ", () => NumberOfItems / MaxNumberOfItems > 0.9f ? Color.Red : Color.White);
            StatusReport.AddBarGraph(() => GetBarLenght(MaxNumberOfItems), () => NumberOfItems / MaxNumberOfItems, () => new Color(60, 80, 170));
            StatusReport.Write("\nStorage contents:\n");
            foreach (var item in storage)
                WriteStatusReportForNewItem(item.Value.Name);
        }


        public Item GetItem(Item item)
        {
            Debug.Assert(item.Amount >= 0);
            if (!storage.ContainsKey(item.Name) || storage[item.Name].Amount <= item.Amount) return Item.GetEmpty();
            storage[item.Name].Amount -= item.Amount;
            NumberOfItems -= item.Amount;
            return item;
        }

        public Item StoreItem(Item item)
        {
            float storableAmount = Math.Min(item.Amount, MaxNumberOfItems - NumberOfItems);

            if (storage.ContainsKey(item.Name)) storage[item.Name].Amount += storableAmount;
            else
            {
                storage.Add(item.Name, new StorageSlot(item.Name, item.Description, item.Value, storableAmount));
                WriteStatusReportForNewItem(item.Name);
            }

            if (storage[item.Name].Amount > storage[item.Name].MaxAmountEverStored) storage[item.Name].MaxAmountEverStored = storage[item.Name].Amount;

            item.Amount -= storableAmount;
            NumberOfItems += storableAmount;
            return item;
        }

        void WriteStatusReportForNewItem(string name)
        {
            StatusReport.Write(() => storage[name].Name + " (" + (storage[name].Value * storage[name].Amount).ToKFloats() + ") " + storage[name].Amount + " ");
            StatusReport.AddBarGraph(() => GetBarLenght(storage[name].MaxAmountEverStored), () => storage[name].Amount / storage[name].MaxAmountEverStored, () => new Color(60, 80, 170));
            StatusReport.Write("\n");
        }

        /// <summary>
        /// Resets the maxAmountEverStored and kicks out items with amount=0.
        /// </summary>
        public void CleanUpStorage()
        {
            var keys = storage.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                storage[keys[i]].MaxAmountEverStored = storage[keys[i]].Amount;
                if (storage[keys[i]].Amount == 0) storage.Remove(keys[i]);
            }

            InitStatusReport();
            StatusReport.Write(() => "Storage Capacity: " + NumberOfItems.ToKFloats() + "/" + MaxNumberOfItems + " ", () => NumberOfItems/MaxNumberOfItems>0.9f?Color.Red:Color.White);
            StatusReport.AddBarGraph(()=>GetBarLenght(MaxNumberOfItems), () => NumberOfItems / MaxNumberOfItems, () => new Color(60, 80, 170));
            StatusReport.Write("\nStorage contents:\n");
            foreach (var item in storage)
                WriteStatusReportForNewItem(item.Value.Name);
        }

        private int GetBarLenght(float amount)
        {
            return (int)MathHelper.Lerp(1, 20, (float)Math.Sin(MathHelper.PiOver2 * (Math.Min(1, amount / BigStorageValue))));
        }
    }
}
