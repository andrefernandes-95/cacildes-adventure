using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace AF.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Database", menuName = "System/New Inventory Database", order = 0)]
    public class InventoryDatabase : ScriptableObject
    {

        [Header("Inventory")]
        [SerializedDictionary("Item", "Quantity")]
        public SerializedDictionary<Item, List<ItemInstance>> ownedItems = new();

        // public SerializedDictionary<Item, ItemAmount> defaultItems = new();

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;


#if UNITY_EDITOR
        private void OnEnable()
        {
            // No need to populate the list; it's serialized directly
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                // Clear the list when exiting play mode
                Clear();
            }
        }
#endif
        public void Clear()
        {
            ownedItems.Clear();
        }

        public void SetDefaultItems()
        {
            /*  ownedItems.Clear();

              foreach (var defaultItem in defaultItems)
              {
                  ownedItems.Add(defaultItem.Key, defaultItem.Value);

                  if (defaultItem.Key is Armor armor)
                  {
                      equipmentDatabase.EquipArmor(armor);
                  }
                  else if (defaultItem.Key is Legwear legwear)
                  {
                      equipmentDatabase.EquipLegwear(legwear);
                  }
              }
              */
        }

        public void ReplenishItems()
        {
            foreach (var itemEntry in ownedItems)
            {
                if (itemEntry.Key is Consumable)
                {
                    foreach (ConsumableInstance consumableInstance in itemEntry.Value)
                    {

                    }
                }
            }
        }

        public void AddItem(Item itemToAdd)
        {
            AddItem(itemToAdd, 1);
        }

        public List<ItemInstance> AddItem(Item itemToAdd, int quantity)
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


        public ItemInstance GetFirst(Item itemToFind)
        {
            if (!ownedItems.ContainsKey(itemToFind))
            {
                return null;
            }

            return ownedItems[itemToFind].FirstOrDefault(ownedItemInstance => ownedItemInstance.HasItem(itemToFind));
        }

        public void RemoveItem(Item itemToRemove)
        {
            ItemInstance itemInstanceToRemove = GetFirst(itemToRemove);
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

                if (GetItemAmount(itemInstance.GetItem<Consumable>()) > 0)
                {
                    ownedItems[itemInstance.GetItem<Consumable>()].Remove(itemInstance);
                    return;
                }
            }

            ownedItems[itemInstance.GetItem<Item>()].Remove(itemInstance);

            // If not last arrow, do not unequip item
            if (itemInstance is ArrowInstance arrowInstance && GetItemAmount(arrowInstance.GetItem<Arrow>()) > 0)
            {
                return;
            }

            UnequipItemToRemove(itemInstance);
        }

        void UnequipItemToRemove(ItemInstance item) => equipmentDatabase.UnequipItem(item);

        public ItemInstance FindItemById(string id) => ownedItems.SelectMany(entry => entry.Value).FirstOrDefault(ownedItem => ownedItem.id.Equals(id));

        public int GetItemAmount(Item itemToFind)
        {
            if (!ownedItems.ContainsKey(itemToFind))
            {
                return 0;
            }

            return ownedItems[itemToFind].Count;
        }

        public bool HasItem(Item itemToFind)
        {
            return this.ownedItems.ContainsKey(itemToFind);
        }

        public int GetWeaponsCount()
        {
            return ownedItems.Count(x => x.Key is Weapon);
        }

        public int GetSpellsCount()
        {
            return ownedItems.Count(x => x.Key is Spell);
        }
        public void AddFromSerializedItem(SerializedItem serializedItem)
        {
            Item item = Resources.Load<Item>(serializedItem.itemPath);

            if (item is Weapon weapon)
            {
                WeaponInstance weaponInstance = new(Guid.NewGuid().ToString(), weapon, serializedItem.level);

                if (ownedItems.ContainsKey(weapon))
                {
                    ownedItems[weapon].Add(weaponInstance);
                }
                else
                {
                    ownedItems.Add(weapon, new() { weaponInstance });
                }
            }
            else if (item is Shield shield)
            {
                ShieldInstance shieldInstance = new(Guid.NewGuid().ToString(), shield);
                AddItemInstance(shield, shieldInstance);
            }
            else if (item is Arrow arrow)
            {
                ArrowInstance arrowInstance = new(Guid.NewGuid().ToString(), arrow);
                AddItemInstance(arrow, arrowInstance);
            }
            else if (item is Spell spell)
            {
                SpellInstance spellInstance = new(Guid.NewGuid().ToString(), spell);
                AddItemInstance(spell, spellInstance);
            }
            else if (item is Consumable consumable)
            {
                ConsumableInstance consumableInstance = new(Guid.NewGuid().ToString(), consumable)
                {
                    wasUsed = serializedItem.wasUsed
                };
                AddItemInstance(consumable, consumableInstance);
            }
            else if (item is Armor armor)
            {
                ArmorInstance armorInstance = new(Guid.NewGuid().ToString(), armor);
                AddItemInstance(armor, armorInstance);
            }
            else if (item is Helmet helmet)
            {
                HelmetInstance helmetInstance = new(Guid.NewGuid().ToString(), helmet);
                AddItemInstance(helmet, helmetInstance);
            }
            else if (item is Gauntlet gauntlet)
            {
                GauntletInstance gauntletInstance = new(Guid.NewGuid().ToString(), gauntlet);
                AddItemInstance(gauntlet, gauntletInstance);
            }
            else if (item is Legwear legwear)
            {
                LegwearInstance legwearInstance = new(Guid.NewGuid().ToString(), legwear);
                AddItemInstance(legwear, legwearInstance);
            }
            else if (item is Accessory accessory)
            {
                AccessoryInstance accessoryInstance = new(Guid.NewGuid().ToString(), accessory);
                AddItemInstance(accessory, accessoryInstance);
            }
            else if (item is CraftingMaterial craftingMaterial)
            {
                CraftingMaterialInstance craftingMaterialInstance = new(Guid.NewGuid().ToString(), craftingMaterial);
                AddItemInstance(craftingMaterial, craftingMaterialInstance);
            }
            else if (item is UpgradeMaterial upgradeMaterial)
            {
                UpgradeMaterialInstance upgradeMaterialInstance = new(Guid.NewGuid().ToString(), upgradeMaterial);
                AddItemInstance(upgradeMaterial, upgradeMaterialInstance);
            }
            /*
            else if (item is Gemstone gemstone)
            {
                GemstoneInstance gemstoneInstance = new(serializedItem.id, gemstone);
                AddItemInstance(gemstone, gemstoneInstance);
            }*/
            else
            {
                ItemInstance itemInstance = new(Guid.NewGuid().ToString(), item);
                AddItemInstance(item, itemInstance);
            }
        }

        private void AddItemInstance<T>(T item, ItemInstance instance) where T : Item
        {
            if (ownedItems.ContainsKey(item))
            {
                ownedItems[item].Add(instance);
            }
            else
            {
                ownedItems.Add(item, new() { instance });
            }
        }
    }
}
