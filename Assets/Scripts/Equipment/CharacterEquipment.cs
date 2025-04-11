namespace AF
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CharacterEquipment : CharacterBaseEquipment
    {
        [Header("Offensive Gear")]
        public WeaponInstance[] rightWeapons = new WeaponInstance[3];
        public WeaponInstance[] leftWeapons = new WeaponInstance[3];
        public Arrow[] arrows = new Arrow[2];
        public SpellInstance[] spells = new SpellInstance[5];
        public Consumable[] consumables = new Consumable[10];

        [Header("Defensive Gear")]
        public HelmetInstance helmet;
        public ArmorInstance armor;
        public GauntletInstance gauntlet;
        public LegwearInstance legwear;

        [Header("Accessories")]
        public AccessoryInstance[] accessories = new AccessoryInstance[4];

        [Header("Index")]
        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;
        public int currentSkillIndex = 0;
        public int currentArrowIndex = 0;
        public int currentConsumableIndex = 0;


        #region Equipment Indexes
        public override void SwitchRightWeapon()
        {
            currentRightWeaponIndex++;

            if (currentRightWeaponIndex >= GetRightHandWeapons().Length)
            {
                currentRightWeaponIndex = 0;
            }

            onSwitchingRightWeapon.Invoke();
            characterBaseManager.PlayAnimationWithCrossFade(AnimatorClipNames.SwitchRightHand);
        }

        public override void SwitchLeftWeapon()
        {
            currentLeftWeaponIndex++;

            if (currentLeftWeaponIndex >= GetLeftHandWeapons().Length)
            {
                currentLeftWeaponIndex = 0;
            }

            onSwitchingLeftWeapon.Invoke();
            characterBaseManager.PlayAnimationWithCrossFade(AnimatorClipNames.SwitchLeftHand);
        }

        public override void SwitchSkill()
        {
            currentSkillIndex++;

            if (currentSkillIndex >= GetSpells().Length)
            {
                currentSkillIndex = 0;
            }

            onSwitchingSpell.Invoke();
            characterBaseManager.PlayAnimationWithCrossFade(AnimatorClipNames.SwitchRightHand);
        }

        public override void SwitchConsumable()
        {
            currentConsumableIndex++;

            if (currentConsumableIndex >= GetConsumables().Length)
            {
                currentConsumableIndex = 0;
            }

            onSwitchingConsumable.Invoke();
            characterBaseManager.PlayAnimationWithCrossFade(AnimatorClipNames.SwitchRightHand);
        }

        public override void SwitchArrow()
        {
            currentArrowIndex++;

            if (currentArrowIndex >= GetArrows().Length)
            {
                currentArrowIndex = 0;
            }
        }
        #endregion

        #region Equipment Getters
        public override List<AccessoryInstance> GetAccessoryInstances()
        {
            return accessories.ToList();
        }

        public override ArmorInstance GetArmorInstance()
        {
            return armor;
        }

        public override Arrow GetCurrentArrow()
        {
            return arrows[currentArrowIndex];
        }

        public override Consumable GetConsumable()
        {
            return consumables[currentConsumableIndex];
        }

        public override GauntletInstance GetGauntletInstance()
        {
            return gauntlet;
        }

        public override HelmetInstance GetHelmetInstance()
        {
            return helmet;
        }

        public override WeaponInstance GetLeftHandWeapon()
        {
            return leftWeapons[currentLeftWeaponIndex];
        }

        public override LegwearInstance GetLegwearInstance()
        {
            return legwear;
        }

        public override WeaponInstance GetRightHandWeapon()
        {
            return rightWeapons[currentRightWeaponIndex];
        }

        public override List<ShieldInstance> GetShieldInstances()
        {
            var shields = rightWeapons.OfType<ShieldInstance>().ToList();
            shields.AddRange(leftWeapons.OfType<ShieldInstance>());
            return shields;
        }

        public override SpellInstance GetSpellInstance()
        {
            return spells[currentSkillIndex];
        }
        #endregion


        #region Getters By Slot Index
        public override WeaponInstance GetRightWeaponInSlot(int slot)
        {
            return rightWeapons[slot];
        }

        public override WeaponInstance GetLeftWeaponInSlot(int slot)
        {
            return leftWeapons[slot];
        }

        public override SpellInstance GetSpellInSlot(int slot)
        {
            return spells[slot];
        }

        public override Arrow GetArrowInSlot(int slot)
        {
            return arrows[slot];
        }

        public override AccessoryInstance GetAccessoryInSlot(int slot)
        {
            return accessories[slot];
        }

        public override Consumable GetConsumableInSlot(int slot)
        {
            return consumables[slot];
        }

        public override WeaponInstance[] GetRightHandWeapons()
        {
            return rightWeapons;
        }

        public override WeaponInstance[] GetLeftHandWeapons()
        {
            return leftWeapons;
        }

        public override Arrow[] GetArrows()
        {
            return arrows;
        }

        public override Consumable[] GetConsumables()
        {
            return consumables;
        }

        public override SpellInstance[] GetSpells()
        {
            return spells;
        }

        #endregion


        protected override void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            rightWeapons[slotIndex] = weaponInstance.Clone();
        }

        protected override void ClearRightWeapon(int slotIndex)
        {
            rightWeapons[slotIndex].Clear();
        }

        protected override void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            leftWeapons[slotIndex] = weaponInstance.Clone();
        }

        protected override void ClearLeftWeapon(int slotIndex)
        {
            leftWeapons[slotIndex].Clear();
        }

        protected override void SetHelmet(HelmetInstance helmetInstance)
        {
            helmet = helmetInstance.Clone();
        }

        protected override void ClearHelmet()
        {
            helmet.Clear();
        }

        protected override void SetArrow(Arrow arrow, int slotIndex)
        {
            arrows[slotIndex] = arrow;
        }

        protected override void ClearArrow(int slotIndex)
        {
            arrows[slotIndex] = null;
        }

        protected override void SetSkill(SpellInstance skillInstance, int slotIndex)
        {
            spells[slotIndex] = skillInstance.Clone();
        }

        protected override void ClearSkill(int slotIndex)
        {
            spells[slotIndex].Clear();
        }

        protected override void SetAccessory(AccessoryInstance accessoryInstance, int slotIndex)
        {
            accessories[slotIndex] = accessoryInstance.Clone();
        }

        protected override void ClearAccessory(int slotIndex)
        {
            accessories[slotIndex].Clear();
        }

        protected override void SetArmor(ArmorInstance armorInstance)
        {
            armor = armorInstance.Clone();
        }

        protected override void ClearArmor()
        {
            armor.Clear();
        }

        protected override void SetGauntlets(GauntletInstance gauntletInstance)
        {
            gauntlet = gauntletInstance.Clone();
        }

        protected override void ClearGauntlets()
        {
            gauntlet.Clear();
        }

        protected override void SetLegwear(LegwearInstance legwearInstance)
        {
            legwear = legwearInstance.Clone();
        }

        protected override void ClearLegwear()
        {
            legwear.Clear();
        }

        protected override void SetConsumable(Consumable consumable, int slotIndex)
        {
            consumables[slotIndex] = consumable;
        }

        protected override void ClearConsumable(int slotIndex)
        {
            consumables[slotIndex] = null;
        }

        public override void UnequipCurrentConsumable()
        {
            UnequipConsumable(currentConsumableIndex);
        }

        public override int GetCurrentRightHandWeaponSlotIndex()
        {
            return currentRightWeaponIndex;
        }

        public override int GetCurrentLeftHandWeaponSlotIndex()
        {
            return currentLeftWeaponIndex;
        }

        public override int GetCurrentConsumablesSlotIndex()
        {
            return currentConsumableIndex;
        }

        public override int GetCurrentSkillsSlotIndex()
        {
            return currentSkillIndex;
        }

        public override int GetCurrentArrowsSlotIndex()
        {
            return currentArrowIndex;
        }
    }
}
