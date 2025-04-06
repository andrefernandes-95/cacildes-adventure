namespace AF
{
    using System;
    using UnityEngine;

    public static class InventoryUtils
    {
        public static void AddItem(Item item, int amount, CharacterBaseInventory characterBaseInventory)
        {
            for (int i = 0; i < amount; i++)
            {
                if (item is Weapon weapon)
                {
                    characterBaseInventory.AddWeapon(weapon);
                }
                else if (item is Shield shield)
                {
                    characterBaseInventory.AddShield(shield);
                }
                else if (item is Arrow arrow)
                {
                    characterBaseInventory.AddArrow(arrow);
                }
                else if (item is Spell spell)
                {
                    characterBaseInventory.AddSkill(spell);
                }
                else if (item is Helmet helmet)
                {
                    characterBaseInventory.AddHelmet(helmet);
                }
                else if (item is Armor armor)
                {
                    characterBaseInventory.AddArmor(armor);
                }
                else if (item is Gauntlet gauntlet)
                {
                    characterBaseInventory.AddGauntlet(gauntlet);
                }
                else if (item is Legwear legwear)
                {
                    characterBaseInventory.AddLegwear(legwear);
                }
                else if (item is Accessory accessory)
                {
                    characterBaseInventory.AddAccessory(accessory);
                }
                else if (item is Consumable consumable)
                {
                    characterBaseInventory.AddConsumable(consumable);
                }
                else if (item is CraftingMaterial craftingMaterial)
                {
                    characterBaseInventory.AddCraftingMaterial(craftingMaterial);
                }
                else if (item is UpgradeMaterial upgradeMaterial)
                {
                    characterBaseInventory.AddUpgradeMaterial(upgradeMaterial);
                }
                else if (item is KeyItem keyItem)
                {
                    characterBaseInventory.AddKeyItem(keyItem);
                }
                else if (item != null)
                {
                    Debug.LogError($"Tried to add an unknown item type: {item.name}");
                }
                else if (item == null)
                {
                    Debug.LogError($"Tried to add null item");
                }
            }
        }
    }
}
