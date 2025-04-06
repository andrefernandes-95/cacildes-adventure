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
                ownedItems.Clear();

            }
        }
#endif

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

        /*
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
        }*/

        /*
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
            }
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
*/
    }
}
