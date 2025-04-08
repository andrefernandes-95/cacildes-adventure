using UnityEngine;

namespace AF
{
    public static class UI_EquipmentUtils
    {
        public static void EquipItem(
            CharacterBaseManager character,
            ItemInstance itemToEquip,
            Item stackableItem,
            int slot,
            bool isAttemptingToEquipRightWeapon,
            bool isAttemptingToEquipLeftWeapon
            )
        {
            // Slot to equip
            if (slot < 0)
            {
                Debug.LogError("Error: Attempting to equip something on a slot with index of -1");
                return;
            }

            CharacterBaseEquipment characterEquipment = character.characterBaseEquipment;

            // Attempting to equip weapon?
            if ((isAttemptingToEquipRightWeapon || isAttemptingToEquipLeftWeapon) && itemToEquip is WeaponInstance weaponInstance)
            {
                characterEquipment.EquipWeapon(weaponInstance, slot, isAttemptingToEquipRightWeapon);
            }
            // Attempting to equip arrows?
            else if (stackableItem is Arrow arrow)
            {
                characterEquipment.EquipArrow(arrow, slot);

            }
            // Attempting to equip skills?
            else if (itemToEquip is SpellInstance spellInstance)
            {
                characterEquipment.EquipSkill(spellInstance, slot);
            }
            // Attempting to equip accessory?
            else if (itemToEquip is AccessoryInstance accessoryInstance)
            {
                characterEquipment.EquipAccessory(accessoryInstance, slot);
            }
            // Attempting to equip consumable?
            else if (stackableItem is Consumable consumable)
            {
                characterEquipment.EquipConsumable(consumable, slot);
            }
            else if (itemToEquip is HelmetInstance helmetInstance)
            {
                characterEquipment.EquipHelmet(helmetInstance);
            }
            else if (itemToEquip is ArmorInstance armorInstance)
            {
                characterEquipment.EquipArmor(armorInstance);
            }
            else if (itemToEquip is GauntletInstance gauntletInstance)
            {
                characterEquipment.EquipGauntlets(gauntletInstance);
            }
            else if (itemToEquip is LegwearInstance legwearInstance)
            {
                characterEquipment.EquipLegwear(legwearInstance);
            }
        }
    }
}