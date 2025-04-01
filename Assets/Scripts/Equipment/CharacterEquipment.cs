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
    }
}
