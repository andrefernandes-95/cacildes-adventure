namespace AF
{
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerEquipment : CharacterBaseEquipment
    {
        // TODO: Turn EquipmentDatabase private, make character base equipment have all the shared methods for AI as well
        public EquipmentDatabase equipmentDatabase;

        public PlayerManager playerManager;

        #region Equipment Indexes
        public override void SwitchRightWeapon()
        {
            equipmentDatabase.currentRightWeaponIndex++;

            if (equipmentDatabase.currentRightWeaponIndex >= GetRightHandWeapons().Length)
            {
                equipmentDatabase.currentRightWeaponIndex = 0;
            }
        }

        public override void SwitchLeftWeapon()
        {
            equipmentDatabase.currentLeftWeaponIndex++;

            if (equipmentDatabase.currentLeftWeaponIndex >= GetLeftHandWeapons().Length)
            {
                equipmentDatabase.currentLeftWeaponIndex = 0;
            }
        }

        public override void SwitchSkill()
        {
            equipmentDatabase.currentSkillIndex++;

            if (equipmentDatabase.currentSkillIndex >= GetSpells().Length)
            {
                equipmentDatabase.currentSkillIndex = 0;
            }
        }

        public override void SwitchConsumable()
        {
            equipmentDatabase.currentConsumableIndex++;

            if (equipmentDatabase.currentConsumableIndex >= GetConsumables().Length)
            {
                equipmentDatabase.currentConsumableIndex = 0;
            }
        }

        public override void SwitchArrow()
        {
            equipmentDatabase.currentArrowIndex++;

            if (equipmentDatabase.currentArrowIndex >= GetArrows().Length)
            {
                equipmentDatabase.currentArrowIndex = 0;
            }
        }
        #endregion

        public override List<AccessoryInstance> GetAccessoryInstances()
        {
            return equipmentDatabase.accessories.ToList();
        }

        public override ArmorInstance GetArmorInstance()
        {
            return equipmentDatabase.armor;
        }

        public override Arrow GetCurrentArrow()
        {
            return equipmentDatabase.arrows[equipmentDatabase.currentArrowIndex];
        }

        public override Consumable GetConsumable()
        {
            return equipmentDatabase.consumables[equipmentDatabase.currentConsumableIndex];
        }

        public override GauntletInstance GetGauntletInstance()
        {
            return equipmentDatabase.gauntlet;
        }

        public override HelmetInstance GetHelmetInstance()
        {
            return equipmentDatabase.helmet;
        }

        public override WeaponInstance GetLeftHandWeapon()
        {
            return equipmentDatabase.leftWeapons[equipmentDatabase.currentLeftWeaponIndex];
        }

        public override LegwearInstance GetLegwearInstance()
        {
            return equipmentDatabase.legwear;
        }

        public override WeaponInstance GetRightHandWeapon()
        {
            return equipmentDatabase.rightWeapons[equipmentDatabase.currentRightWeaponIndex];
        }

        public override List<ShieldInstance> GetShieldInstances()
        {
            var shields = equipmentDatabase.leftWeapons.OfType<ShieldInstance>().ToList();
            shields.AddRange(equipmentDatabase.rightWeapons.OfType<ShieldInstance>());
            return shields;
        }

        public override SpellInstance GetSpellInstance()
        {
            return equipmentDatabase.spells[equipmentDatabase.currentSkillIndex];
        }

        public override Arrow GetArrowInSlot(int slot)
        {
            return equipmentDatabase.arrows[slot];
        }


        public override WeaponInstance GetRightWeaponInSlot(int slot)
        {
            return equipmentDatabase.rightWeapons[slot];
        }

        public override WeaponInstance GetLeftWeaponInSlot(int slot)
        {
            return equipmentDatabase.leftWeapons[slot];
        }

        public override SpellInstance GetSpellInSlot(int slot)
        {
            return equipmentDatabase.spells[slot];
        }

        public override AccessoryInstance GetAccessoryInSlot(int slot)
        {
            return equipmentDatabase.accessories[slot];
        }

        public override Consumable GetConsumableInSlot(int slot)
        {
            return equipmentDatabase.consumables[slot];
        }

        public override WeaponInstance[] GetRightHandWeapons()
        {
            return equipmentDatabase.rightWeapons;
        }

        public override WeaponInstance[] GetLeftHandWeapons()
        {
            return equipmentDatabase.leftWeapons;
        }



        public override Arrow[] GetArrows()
        {
            return equipmentDatabase.arrows;
        }

        public override Consumable[] GetConsumables()
        {
            return equipmentDatabase.consumables;
        }

        public override SpellInstance[] GetSpells()
        {
            return equipmentDatabase.spells;
        }


        protected override void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            equipmentDatabase.rightWeapons[slotIndex] = weaponInstance.Clone();
        }

        protected override void ClearRightWeapon(int slotIndex)
        {
            equipmentDatabase.rightWeapons[slotIndex].Clear();
        }

        protected override void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            equipmentDatabase.leftWeapons[slotIndex] = weaponInstance.Clone();
        }

        protected override void ClearLeftWeapon(int slotIndex)
        {
            equipmentDatabase.leftWeapons[slotIndex].Clear();
        }

        protected override void SetHelmet(HelmetInstance helmetInstance)
        {
            equipmentDatabase.helmet = helmetInstance.Clone();
        }

        protected override void ClearHelmet()
        {
            equipmentDatabase.helmet.Clear();
        }

        protected override void SetArrow(Arrow arrow, int slotIndex)
        {
            equipmentDatabase.arrows[slotIndex] = arrow;
        }

        protected override void ClearArrow(int slotIndex)
        {
            equipmentDatabase.arrows[slotIndex] = null;
        }

        protected override void SetSkill(SpellInstance skillInstance, int slotIndex)
        {
            equipmentDatabase.spells[slotIndex] = skillInstance.Clone();
        }

        protected override void ClearSkill(int slotIndex)
        {
            equipmentDatabase.spells[slotIndex].Clear();
        }

        protected override void SetAccessory(AccessoryInstance accessoryInstance, int slotIndex)
        {
            equipmentDatabase.accessories[slotIndex] = accessoryInstance.Clone();
        }

        protected override void ClearAccessory(int slotIndex)
        {
            equipmentDatabase.accessories[slotIndex].Clear();
        }

        protected override void SetArmor(ArmorInstance armorInstance)
        {
            equipmentDatabase.armor = armorInstance.Clone();
        }

        protected override void ClearArmor()
        {
            equipmentDatabase.armor.Clear();
        }

        protected override void SetGauntlets(GauntletInstance gauntletInstance)
        {
            equipmentDatabase.gauntlet = gauntletInstance.Clone();
        }

        protected override void ClearGauntlets()
        {
            equipmentDatabase.gauntlet.Clear();
        }

        protected override void SetLegwear(LegwearInstance legwearInstance)
        {
            equipmentDatabase.legwear = legwearInstance.Clone();
        }

        protected override void ClearLegwear()
        {
            equipmentDatabase.legwear.Clear();
        }

        protected override void SetConsumable(Consumable consumable, int slotIndex)
        {
            equipmentDatabase.consumables[slotIndex] = consumable;
        }

        protected override void ClearConsumable(int slotIndex)
        {
            equipmentDatabase.consumables[slotIndex] = null;
        }

        public override void UnequipCurrentConsumable()
        {
            UnequipConsumable(equipmentDatabase.currentConsumableIndex);
        }
        public override int GetCurrentRightHandWeaponSlotIndex()
        {
            return equipmentDatabase.currentRightWeaponIndex;
        }

        public override int GetCurrentLeftHandWeaponSlotIndex()
        {
            return equipmentDatabase.currentLeftWeaponIndex;
        }

        public override int GetCurrentConsumablesSlotIndex()
        {
            return equipmentDatabase.currentConsumableIndex;
        }

        public override int GetCurrentSkillsSlotIndex()
        {
            return equipmentDatabase.currentSkillIndex;
        }

        public override int GetCurrentArrowsSlotIndex()
        {
            return equipmentDatabase.currentArrowIndex;
        }
    }
}
