using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public abstract class CharacterBaseEquipment : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] CharacterBaseManager characterBaseManager;


        [Header("Default Equipment")]
        public WeaponInstance[] defaultRightHandWeapons = new WeaponInstance[3];
        public WeaponInstance[] defaultLeftHandWeapons = new WeaponInstance[3];
        public ArrowInstance[] defaultArrows = new ArrowInstance[2];
        public SpellInstance[] defaultSpells = new SpellInstance[5];
        public ConsumableInstance[] defaultConsumables = new ConsumableInstance[10];
        public AccessoryInstance[] defaultAccessories = new AccessoryInstance[4];
        public HelmetInstance defaultHelmet;
        public ArmorInstance defaultArmor;
        public GauntletInstance defaultGauntlet;
        public LegwearInstance defaultLegwear;

        private void Awake()
        {
        }

        public abstract WeaponInstance GetRightHandWeapon();
        public abstract WeaponInstance GetLeftHandWeapon();
        public abstract List<ShieldInstance> GetShieldInstances();
        public abstract SpellInstance GetSpellInstance();
        public abstract ArrowInstance GetArrowInstance();
        public abstract HelmetInstance GetHelmetInstance();
        public abstract ArmorInstance GetArmorInstance();
        public abstract GauntletInstance GetGauntletInstance();
        public abstract LegwearInstance GetLegwearInstance();
        public abstract List<AccessoryInstance> GetAccessoryInstances();
        public abstract ConsumableInstance GetConsumableInstance();

    }
}
