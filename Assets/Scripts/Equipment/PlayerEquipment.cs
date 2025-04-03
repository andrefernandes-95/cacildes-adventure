namespace AF
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PlayerEquipment : CharacterBaseEquipment
    {
        // TODO: Turn EquipmentDatabase private, make character base equipment have all the shared methods for AI as well
        public EquipmentDatabase equipmentDatabase;

        public PlayerManager playerManager;

        #region Equipment Getters
        public override List<AccessoryInstance> GetAccessoryInstances()
        {
            return equipmentDatabase.accessories.ToList();
        }

        public override ArmorInstance GetArmorInstance()
        {
            return equipmentDatabase.armor;
        }

        public override ArrowInstance GetArrowInstance()
        {
            return equipmentDatabase.GetCurrentArrow();
        }

        public override ConsumableInstance GetConsumableInstance()
        {
            return equipmentDatabase.GetCurrentConsumable();
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
            return equipmentDatabase.GetCurrentLeftWeapon();
        }

        public override LegwearInstance GetLegwearInstance()
        {
            return equipmentDatabase.legwear;
        }

        public override WeaponInstance GetRightHandWeapon()
        {
            return equipmentDatabase.GetCurrentRightWeapon();
        }

        public override List<ShieldInstance> GetShieldInstances()
        {
            var shields = equipmentDatabase.leftWeapons.OfType<ShieldInstance>().ToList();
            shields.AddRange(equipmentDatabase.rightWeapons.OfType<ShieldInstance>());
            return shields;
        }

        public override SpellInstance GetSpellInstance()
        {
            return equipmentDatabase.GetCurrentSpell();
        }
        #endregion


        #region Equipment Setters

        public override void SetHelmet(HelmetInstance helmetInstance)
        {
            equipmentDatabase.helmet = helmetInstance.Clone();
        }

        public override void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            equipmentDatabase.leftWeapons[slotIndex] = weaponInstance.Clone();
        }

        public override void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex)
        {
            equipmentDatabase.rightWeapons[slotIndex] = weaponInstance.Clone();
        }

        public override void ClearHelmet()
        {
            equipmentDatabase.helmet.Clear();
        }

        public override void ClearLeftWeapon(int slotIndex)
        {
            equipmentDatabase.leftWeapons[slotIndex].Clear();
        }

        public override void ClearRightWeapon(int slotIndex)
        {
            equipmentDatabase.rightWeapons[slotIndex].Clear();
        }
        #endregion
    }
}
