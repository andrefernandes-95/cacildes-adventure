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
        public ArrowInstance[] arrows = new ArrowInstance[2];
        public SpellInstance[] spells = new SpellInstance[5];
        public ConsumableInstance[] consumables = new ConsumableInstance[10];

        [Header("Defensive Gear")]
        public HelmetInstance helmet;
        public ArmorInstance armor;
        public GauntletInstance gauntlet;
        public LegwearInstance legwear;

        [Header("Accessories")]
        public AccessoryInstance[] accessories = new AccessoryInstance[4];

        public int currentWeaponIndex, currentShieldIndex, currentConsumableIndex, currentSpellIndex, currentArrowIndex = 0;


        #region Equipment Getters
        public override List<AccessoryInstance> GetAccessoryInstances()
        {
            return accessories.ToList();
        }

        public override ArmorInstance GetArmorInstance()
        {
            return armor;
        }

        public override ArrowInstance GetArrowInstance()
        {
            return arrows[currentArrowIndex];
        }

        public override ConsumableInstance GetConsumableInstance()
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
            return leftWeapons[currentShieldIndex];
        }

        public override LegwearInstance GetLegwearInstance()
        {
            return legwear;
        }

        public override WeaponInstance GetRightHandWeapon()
        {
            return rightWeapons[currentWeaponIndex];
        }

        public override List<ShieldInstance> GetShieldInstances()
        {
            var shields = rightWeapons.OfType<ShieldInstance>().ToList();
            shields.AddRange(leftWeapons.OfType<ShieldInstance>());
            return shields;
        }

        public override SpellInstance GetSpellInstance()
        {
            return spells[currentSpellIndex];
        }
        #endregion

        #region Equipment Setters
        public override void SetHelmet(HelmetInstance helmetInstance)
        {
            helmet = helmetInstance.Clone();
        }

        public override void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            leftWeapons[slotIndex] = weaponInstance.Clone();
        }

        public override void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            rightWeapons[slotIndex] = weaponInstance.Clone();
        }

        public override void ClearHelmet()
        {
            helmet.Clear();
        }

        public override void ClearLeftWeapon(int slotIndex)
        {
            leftWeapons[slotIndex].Clear();
        }

        public override void ClearRightWeapon(int slotIndex)
        {
            rightWeapons[slotIndex].Clear();
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

        public override ArrowInstance GetArrowInSlot(int slot)
        {
            return arrows[slot];
        }

        public override AccessoryInstance GetAccessoryInSlot(int slot)
        {
            return accessories[slot];
        }

        public override ConsumableInstance GetConsumableInSlot(int slot)
        {
            return consumables[slot];
        }
        #endregion

    }
}
