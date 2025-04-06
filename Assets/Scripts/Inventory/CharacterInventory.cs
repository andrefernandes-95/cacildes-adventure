namespace AF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AYellowpaper.SerializedCollections;

    public class CharacterInventory : CharacterBaseInventory
    {
        public SerializedDictionary<Item, List<ItemInstance>> ownedItems = new();

        public override void RemoveItem(Item item)
        {
            ItemInstance itemInstanceToRemove = GetFirst(item);
            RemoveItemInstance(itemInstanceToRemove);
        }

        public void RemoveItemInstance(ItemInstance itemInstance)
        {
            if (itemInstance == null)
            {
                return;
            }

            if (itemInstance is ConsumableInstance consumableInstance)
            {
                if (consumableInstance.GetItem<Consumable>().isRenewable)
                {
                    consumableInstance.wasUsed = true;
                    return;
                }

                if (GetItemQuantity(itemInstance.GetItem<Consumable>()) > 0)
                {
                    ownedItems[itemInstance.GetItem<Consumable>()].Remove(itemInstance);
                    return;
                }
            }

            ownedItems[itemInstance.GetItem<Item>()].Remove(itemInstance);

            // If not last arrow, do not unequip item
            if (itemInstance is ArrowInstance arrowInstance && GetItemQuantity(arrowInstance.GetItem<Arrow>()) > 0)
            {
                return;
            }

            //UnequipItemToRemove(itemInstance);
        }

        public override int GetItemQuantity(Item item)
        {
            if (ownedItems.ContainsKey(item))
            {
                return ownedItems[item].Count;
            }

            return 0;
        }

        public override bool HasItem(Item item)
        {
            return GetItemQuantity(item) > 0;
        }

        public override SerializedDictionary<Item, List<ItemInstance>> GetInventory()
        {
            return ownedItems;
        }
    }
}
