namespace AF
{
    using System;
    using System.Collections.Generic;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public abstract class CharacterBaseInventory : MonoBehaviour
    {
        public abstract List<ItemInstance> AddItem(Item item, int quantity);
        public abstract void RemoveItem(Item item);

        public abstract bool HasItem(Item item);
        public abstract int GetItemQuantity(Item item);

        public abstract SerializedDictionary<Item, List<ItemInstance>> GetInventory();

        string GenerateItemId()
        {
            return Guid.NewGuid().ToString();
        }

        public WeaponInstance AddWeapon(Weapon weapon, Dictionary<Item, List<ItemInstance>> characterInventory)
        {
            WeaponInstance addedItem = new WeaponInstance(GenerateItemId(), weapon, 0);

            if (characterInventory.ContainsKey(weapon))
            {
                characterInventory[weapon].Add(addedItem);
            }
            else
            {
                characterInventory.Add(weapon, new() { addedItem });
            }

            return addedItem;
        }

        public HelmetInstance AddHelmet(Helmet helmet, Dictionary<Item, List<ItemInstance>> characterInventory)
        {
            HelmetInstance addedItem = new HelmetInstance(GenerateItemId(), helmet);

            if (characterInventory.ContainsKey(helmet))
            {
                characterInventory[helmet].Add(addedItem);
            }
            else
            {
                characterInventory.Add(helmet, new() { addedItem });
            }

            return addedItem;
        }
    }
}
