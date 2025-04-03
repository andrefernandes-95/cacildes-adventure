namespace AF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AYellowpaper.SerializedCollections;

    public class CharacterInventory : CharacterBaseInventory
    {
        public SerializedDictionary<Item, List<ItemInstance>> ownedItems = new();

        public override List<ItemInstance> AddItem(Item itemToAdd, int quantity)
        {
            List<ItemInstance> itemsAdded = new();

            for (int i = 0; i < quantity; i++)
            {
                ItemInstance toAdd = null;

                if (ownedItems.ContainsKey(itemToAdd))
                {
                    toAdd = InventoryUtils.ItemToItemInstance<ItemInstance>(itemToAdd);

                    ownedItems[itemToAdd].Add(toAdd);
                }
                else
                {
                    ownedItems.Add(itemToAdd, new() { toAdd });
                }

                itemsAdded.Add(toAdd);
            }

            return itemsAdded;
        }

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

        public ItemInstance GetFirst(Item itemToFind)
        {
            if (!ownedItems.ContainsKey(itemToFind))
            {
                return null;
            }

            return ownedItems[itemToFind].FirstOrDefault(ownedItemInstance => ownedItemInstance.HasItem(itemToFind));
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
