using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public abstract class CharacterBaseEquipment : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] CharacterBaseManager characterBaseManager;

        [Header("Default Weapons")]
        [SerializeField] Weapon[] defaultRightHandWeapons = new Weapon[3];
        [SerializeField] Weapon[] defaultLeftHandWeapons = new Weapon[3];
        [SerializeField] Arrow[] defaultArrows = new Arrow[2];
        [SerializeField] int defaultArrowNumber = 30;

        [Header("Default Skills")]
        [SerializeField] Spell[] defaultSpells = new Spell[5];

        [Header("Default Consumables")]
        [SerializeField] Consumable[] defaultConsumables = new Consumable[10];
        [SerializeField] int minConsumableAmount = 1;
        [SerializeField] int maxConsumableAmount = 1;

        [Header("Default Gear")]
        [SerializeField] Helmet defaultHelmet;
        [SerializeField] Armor defaultArmor;
        [SerializeField] Gauntlet defaultGauntlet;
        [SerializeField] Legwear defaultLegwear;
        [SerializeField] Accessory[] defaultAccessories = new Accessory[4];

        private void Awake()
        {
            SetupDefaultEquipment();
        }

        void SetupDefaultEquipment()
        {
            int rightSlotIndex = 0;
            foreach (Weapon weapon in defaultRightHandWeapons)
            {
                if (weapon != null)
                {
                    WeaponInstance addedWeapon = characterBaseManager.characterBaseInventory.AddWeapon(weapon, characterBaseManager.characterBaseInventory.GetInventory());
                    characterBaseManager.characterWeapons.EquipWeapon(addedWeapon, rightSlotIndex, false);
                }

                rightSlotIndex++;
            }

            int leftSlotIndex = 0;
            foreach (Weapon weapon in defaultLeftHandWeapons)
            {
                if (weapon != null)
                {
                    WeaponInstance addedWeapon = characterBaseManager.characterBaseInventory.AddWeapon(weapon, characterBaseManager.characterBaseInventory.GetInventory());
                    characterBaseManager.characterWeapons.EquipWeapon(addedWeapon, leftSlotIndex, false);
                }

                leftSlotIndex++;
            }

            foreach (Arrow arrow in defaultArrows)
            {
                if (arrow != null)
                {
                    characterBaseManager.characterBaseInventory.AddItem(arrow, defaultArrowNumber);
                }
            }
            foreach (Spell spell in defaultSpells)
            {
                if (spell != null)
                {
                    characterBaseManager.characterBaseInventory.AddItem(spell, 1);
                }
            }
            foreach (Consumable consumable in defaultConsumables)
            {
                if (consumable != null)
                {
                    characterBaseManager.characterBaseInventory.AddItem(consumable, Random.Range(minConsumableAmount, maxConsumableAmount));
                }
            }
            foreach (Accessory accessory in defaultAccessories)
            {
                if (accessory != null)
                {
                    characterBaseManager.characterBaseInventory.AddItem(accessory, 1);
                }
            }

            if (defaultHelmet != null)
            {
                HelmetInstance addedHelmet = characterBaseManager.characterBaseInventory.AddHelmet(defaultHelmet, characterBaseManager.characterBaseInventory.GetInventory());
                EquipHelmet(addedHelmet);
            }

            if (defaultArmor != null)
            {
                characterBaseManager.characterBaseInventory.AddItem(defaultArmor, 1);
            }

            if (defaultGauntlet != null)
            {
                characterBaseManager.characterBaseInventory.AddItem(defaultGauntlet, 1);
            }

            if (defaultLegwear != null)
            {
                characterBaseManager.characterBaseInventory.AddItem(defaultLegwear, 1);
            }
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

        public abstract void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex);
        public abstract void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex);

        public abstract void ClearRightWeapon(int slotIndex);
        public abstract void ClearLeftWeapon(int slotIndex);

        public abstract void SetHelmet(HelmetInstance helmetInstance);
        public abstract void ClearHelmet();

        public void EquipWeapon(WeaponInstance weapon, int slotIndex, bool isRightHand)
        {
            if (isRightHand)
            {
                SetRightWeapon(weapon, slotIndex);
            }
            else
            {
                SetLeftWeapon(weapon, slotIndex);
            }
        }

        public void UnequipWeapon(int slotIndex, bool isRightHand)
        {
            if (isRightHand)
            {
                ClearRightWeapon(slotIndex);
            }
            else
            {
                ClearLeftWeapon(slotIndex);
            }

        }

        public void EquipHelmet(HelmetInstance helmetInstance)
        {
            if (helmetInstance.Exists())
            {
                helmetInstance.GetItem<Helmet>().OnEquip(characterBaseManager);
            }

            SetHelmet(helmetInstance);
        }

        public void UnequipHelmet()
        {
            if (GetHelmetInstance() != null && GetHelmetInstance().Exists())
            {
                GetHelmetInstance().GetItem<Helmet>().OnUnequip(characterBaseManager);
            }

            ClearHelmet();
        }

    }
}
