namespace AF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public abstract class CharacterBaseInventory : MonoBehaviour
    {
        public abstract void RemoveItem(Item item);

        public abstract bool HasItem(Item item);

        public abstract int GetItemQuantity(Item item);

        public abstract SerializedDictionary<Item, List<ItemInstance>> GetInventory();

        public List<WeaponInstance> GetAllWeaponInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Weapon)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<WeaponInstance>()
                .ToList();
        }
        public List<ShieldInstance> GetAllShieldInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Shield)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<ShieldInstance>()
                .ToList();
        }
        public List<ArrowInstance> GetAllArrowInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Arrow)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<ArrowInstance>()
                .ToList();
        }
        public List<SpellInstance> GetAllSpellInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Spell)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<SpellInstance>()
                .ToList();
        }
        public List<AccessoryInstance> GetAllAccessoryInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Accessory)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<AccessoryInstance>()
                .ToList();
        }
        public List<ConsumableInstance> GetAllConsumableInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Consumable)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<ConsumableInstance>()
                .ToList();
        }
        public List<HelmetInstance> GetAllHelmetInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Helmet)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<HelmetInstance>()
                .ToList();
        }
        public List<ArmorInstance> GetAllArmorInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Armor)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<ArmorInstance>()
                .ToList();
        }
        public List<GauntletInstance> GetAllGauntletInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Gauntlet)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<GauntletInstance>()
                .ToList();
        }
        public List<LegwearInstance> GetAllLegwearInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is Legwear)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<LegwearInstance>()
                .ToList();
        }
        public List<CraftingMaterialInstance> GetAllCraftingMaterialInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is CraftingMaterial)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<CraftingMaterialInstance>()
                .ToList();
        }
        public List<UpgradeMaterialInstance> GetAllUpgradeMaterialInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is UpgradeMaterial)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<UpgradeMaterialInstance>()
                .ToList();
        }
        public List<KeyItemInstance> GetAllKeyItemInstances()
        {
            return GetInventory()
                .Where(itemEntry => itemEntry.Key is KeyItem)
                .SelectMany(itemEntry => itemEntry.Value)
                .OfType<KeyItemInstance>()
                .ToList();
        }

        string GenerateItemId()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual WeaponInstance AddWeapon(Weapon weapon)
        {
            WeaponInstance addedItem = new WeaponInstance(GenerateItemId(), weapon, 0);

            if (GetInventory().ContainsKey(weapon))
            {
                GetInventory()[weapon].Add(addedItem);
            }
            else
            {
                GetInventory().Add(weapon, new() { addedItem });
            }

            return addedItem;
        }

        public virtual ShieldInstance AddShield(Shield shield)
        {
            ShieldInstance addedItem = new ShieldInstance(GenerateItemId(), shield, 0);

            if (GetInventory().ContainsKey(shield))
            {
                GetInventory()[shield].Add(addedItem);
            }
            else
            {
                GetInventory().Add(shield, new() { addedItem });
            }

            return addedItem;
        }

        public HelmetInstance AddHelmet(Helmet helmet)
        {
            HelmetInstance addedItem = new HelmetInstance(GenerateItemId(), helmet);

            if (GetInventory().ContainsKey(helmet))
            {
                GetInventory()[helmet].Add(addedItem);
            }
            else
            {
                GetInventory().Add(helmet, new() { addedItem });
            }

            return addedItem;
        }

        public ArmorInstance AddArmor(Armor armor)
        {
            ArmorInstance addedItem = new ArmorInstance(GenerateItemId(), armor);

            if (GetInventory().ContainsKey(armor))
            {
                GetInventory()[armor].Add(addedItem);
            }
            else
            {
                GetInventory().Add(armor, new() { addedItem });
            }

            return addedItem;
        }

        public GauntletInstance AddGauntlet(Gauntlet gauntlet)
        {
            GauntletInstance addedItem = new GauntletInstance(GenerateItemId(), gauntlet);

            if (GetInventory().ContainsKey(gauntlet))
            {
                GetInventory()[gauntlet].Add(addedItem);
            }
            else
            {
                GetInventory().Add(gauntlet, new() { addedItem });
            }

            return addedItem;
        }

        public LegwearInstance AddLegwear(Legwear legwear)
        {
            LegwearInstance addedItem = new LegwearInstance(GenerateItemId(), legwear);

            if (GetInventory().ContainsKey(legwear))
            {
                GetInventory()[legwear].Add(addedItem);
            }
            else
            {
                GetInventory().Add(legwear, new() { addedItem });
            }

            return addedItem;
        }

        public AccessoryInstance AddAccessory(Accessory accessory)
        {
            AccessoryInstance addedItem = new AccessoryInstance(GenerateItemId(), accessory);

            if (GetInventory().ContainsKey(accessory))
            {
                GetInventory()[accessory].Add(addedItem);
            }
            else
            {
                GetInventory().Add(accessory, new() { addedItem });
            }

            return addedItem;
        }

        public ArrowInstance AddArrow(Arrow arrow)
        {
            ArrowInstance addedItem = new ArrowInstance(GenerateItemId(), arrow);

            if (GetInventory().ContainsKey(arrow))
            {
                GetInventory()[arrow].Add(addedItem);
            }
            else
            {
                GetInventory().Add(arrow, new() { addedItem });
            }

            return addedItem;
        }

        public SpellInstance AddSkill(Spell spell)
        {
            SpellInstance addedItem = new SpellInstance(GenerateItemId(), spell);

            if (GetInventory().ContainsKey(spell))
            {
                GetInventory()[spell].Add(addedItem);
            }
            else
            {
                GetInventory().Add(spell, new() { addedItem });
            }

            return addedItem;
        }

        public ConsumableInstance AddConsumable(Consumable consumable)
        {
            ConsumableInstance addedItem = new ConsumableInstance(GenerateItemId(), consumable);

            if (GetInventory().ContainsKey(consumable))
            {
                GetInventory()[consumable].Add(addedItem);
            }
            else
            {
                GetInventory().Add(consumable, new() { addedItem });
            }

            return addedItem;
        }

        public UpgradeMaterialInstance AddUpgradeMaterial(UpgradeMaterial upgradeMaterial)
        {
            UpgradeMaterialInstance addedItem = new UpgradeMaterialInstance(GenerateItemId(), upgradeMaterial);

            if (GetInventory().ContainsKey(upgradeMaterial))
            {
                GetInventory()[upgradeMaterial].Add(addedItem);
            }
            else
            {
                GetInventory().Add(upgradeMaterial, new() { addedItem });
            }

            return addedItem;
        }

        public CraftingMaterialInstance AddCraftingMaterial(CraftingMaterial craftingMaterial)
        {
            CraftingMaterialInstance addedItem = new CraftingMaterialInstance(GenerateItemId(), craftingMaterial);

            if (GetInventory().ContainsKey(craftingMaterial))
            {
                GetInventory()[craftingMaterial].Add(addedItem);
            }
            else
            {
                GetInventory().Add(craftingMaterial, new() { addedItem });
            }

            return addedItem;
        }

        public KeyItemInstance AddKeyItem(KeyItem keyItem)
        {
            KeyItemInstance addedItem = new KeyItemInstance(GenerateItemId(), keyItem);

            if (GetInventory().ContainsKey(keyItem))
            {
                GetInventory()[keyItem].Add(addedItem);
            }
            else
            {
                GetInventory().Add(keyItem, new() { addedItem });
            }

            return addedItem;
        }

        public ItemInstance GetFirst(Item itemToFind)
        {
            if (!GetInventory().ContainsKey(itemToFind))
            {
                return null;
            }

            return GetInventory()[itemToFind].FirstOrDefault(ownedItemInstance => ownedItemInstance.HasItem(itemToFind));
        }

        public ItemInstance FindItemById(string id) => GetInventory().SelectMany(entry => entry.Value).FirstOrDefault(ownedItem => ownedItem.id.Equals(id));

        public void ReplenishItems()
        {
            foreach (var entry in GetInventory())
            {
                if (entry.Key is Consumable && entry.Key.isRenewable)
                {
                    foreach (ConsumableInstance consumableInstance in entry.Value)
                    {
                        consumableInstance.wasUsed = false;
                    }
                }
            }
        }

        public void UpdateWeaponLevel(WeaponInstance weaponInstance, int newLevel)
        {
            if (GetInventory().ContainsKey(weaponInstance.GetItem<Weapon>()))
            {
                List<WeaponInstance> weaponInstances = GetInventory()[weaponInstance.GetItem<Weapon>()].OfType<WeaponInstance>().ToList();
                var index = weaponInstances.IndexOf(weaponInstance);
                if (index != -1)
                {
                    weaponInstances[index].level = newLevel;
                }
            }
        }
    }
}
