using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public abstract class CharacterBaseEquipment : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] protected CharacterBaseManager characterBaseManager;

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


        [Header("Quick Items Switching Events")]
        public UnityEvent onSwitchingSpell;
        public UnityEvent onSwitchingConsumable;
        public UnityEvent onSwitchingRightWeapon;
        public UnityEvent onSwitchingLeftWeapon;

        [Header("Sounds")]
        public Soundpack switchWeaponsSoundpack;
        public Soundpack switchSpellsSoundpack;
        public Soundpack switchConsumablesSoundpack;

        public void SetupDefaultEquipment()
        {
            int rightSlotIndex = 0;
            foreach (Weapon weapon in defaultRightHandWeapons)
            {
                if (weapon != null)
                {
                    WeaponInstance addedWeapon = characterBaseManager.characterBaseInventory.AddWeapon(weapon);
                    EquipWeapon(addedWeapon, rightSlotIndex, false);
                }

                rightSlotIndex++;
            }

            int leftSlotIndex = 0;
            foreach (Weapon weapon in defaultLeftHandWeapons)
            {
                if (weapon != null)
                {
                    WeaponInstance addedWeapon = characterBaseManager.characterBaseInventory.AddWeapon(weapon);
                    EquipWeapon(addedWeapon, leftSlotIndex, false);
                }

                leftSlotIndex++;
            }

            for (int slot = 0; slot < defaultArrows.Length; slot++)
            {
                Arrow arrow = defaultArrows[slot];

                if (arrow == null)
                    continue;

                for (int i = 0; i < defaultArrowNumber; i++)
                {
                    characterBaseManager.characterBaseInventory.AddArrow(arrow);
                }

                characterBaseManager.characterBaseEquipment.EquipArrow(arrow, slot);
            }

            for (int slot = 0; slot < defaultSpells.Length; slot++)
            {
                Spell spell = defaultSpells[slot];

                if (spell == null)
                    continue;

                SpellInstance spellInstance = characterBaseManager.characterBaseInventory.AddSkill(spell);
                characterBaseManager.characterBaseEquipment.EquipSkill(spellInstance, slot);
            }

            for (int slot = 0; slot < defaultConsumables.Length; slot++)
            {
                Consumable consumable = defaultConsumables[slot];

                if (consumable == null)
                    continue;

                int numberOfConsumablesToAddToInventory = Random.Range(minConsumableAmount, maxConsumableAmount);

                for (int i = 0; i < numberOfConsumablesToAddToInventory; i++)
                {
                    characterBaseManager.characterBaseInventory.AddConsumable(consumable);
                }

                characterBaseManager.characterBaseEquipment.EquipConsumable(consumable, slot);
            }

            for (int slot = 0; slot < defaultAccessories.Length; slot++)
            {
                Accessory accessory = defaultAccessories[slot];

                if (accessory == null)
                    continue;

                AccessoryInstance accessoryInstance = characterBaseManager.characterBaseInventory.AddAccessory(accessory);
                characterBaseManager.characterBaseEquipment.EquipAccessory(accessoryInstance, slot);
            }

            if (defaultHelmet != null)
            {
                HelmetInstance addedHelmet = characterBaseManager.characterBaseInventory.AddHelmet(defaultHelmet);
                EquipHelmet(addedHelmet);
            }

            if (defaultArmor != null)
            {
                ArmorInstance addedArmor = characterBaseManager.characterBaseInventory.AddArmor(defaultArmor);
                EquipArmor(addedArmor);
            }

            if (defaultGauntlet != null)
            {
                GauntletInstance addedGauntlet = characterBaseManager.characterBaseInventory.AddGauntlet(defaultGauntlet);
                EquipGauntlets(addedGauntlet);
            }

            if (defaultLegwear != null)
            {
                LegwearInstance addedLegwear = characterBaseManager.characterBaseInventory.AddLegwear(defaultLegwear);
                EquipLegwear(addedLegwear);
            }
        }

        public abstract void SwitchRightWeapon();
        public abstract void SwitchLeftWeapon();
        public abstract void SwitchSkill();
        public abstract void SwitchConsumable();
        public abstract void SwitchArrow();

        public abstract WeaponInstance[] GetRightHandWeapons();
        public abstract WeaponInstance[] GetLeftHandWeapons();
        public abstract Arrow[] GetArrows();
        public abstract Consumable[] GetConsumables();
        public abstract SpellInstance[] GetSpells();
        public abstract HelmetInstance GetHelmetInstance();
        public abstract ArmorInstance GetArmorInstance();
        public abstract GauntletInstance GetGauntletInstance();
        public abstract LegwearInstance GetLegwearInstance();

        public abstract WeaponInstance GetRightHandWeapon();
        public abstract WeaponInstance GetLeftHandWeapon();
        public abstract WeaponInstance GetRightWeaponInSlot(int slot);
        public abstract WeaponInstance GetLeftWeaponInSlot(int slot);

        /// <summary>
        /// Return the slot where the given weapon is equipped. If not equipped, will return -1
        /// </summary>
        /// <param name="weaponInstance"></param>
        /// <param name="isRightHand"></param>
        /// <returns></returns>
        public int GetEquippedWeaponSlot(WeaponInstance weaponInstance, bool isRightHand)
        {
            if (isRightHand)
            {
                return System.Array.FindIndex(GetRightHandWeapons(), (_weapon) => _weapon.IsEqualTo(weaponInstance));
            }

            return System.Array.FindIndex(GetLeftHandWeapons(), (_weapon) => _weapon.IsEqualTo(weaponInstance));
        }

        public abstract List<ShieldInstance> GetShieldInstances();
        public abstract SpellInstance GetSpellInstance();
        public abstract SpellInstance GetSpellInSlot(int slot);

        public abstract Arrow GetCurrentArrow();
        public abstract Arrow GetArrowInSlot(int slot);


        public abstract List<AccessoryInstance> GetAccessoryInstances();
        public abstract AccessoryInstance GetAccessoryInSlot(int slot);

        public abstract Consumable GetConsumable();
        public abstract Consumable GetConsumableInSlot(int slot);

        protected abstract void SetRightWeapon(WeaponInstance weaponInstance, int slotIndex);
        protected abstract void SetLeftWeapon(WeaponInstance weaponInstance, int slotIndex);

        protected abstract void ClearRightWeapon(int slotIndex);
        protected abstract void ClearLeftWeapon(int slotIndex);

        public void EquipWeapon(WeaponInstance weapon, int slotIndex, bool isRightHand)
        {
            if (isRightHand)
            {

                // Is Already Equipped?
                if (GetRightWeaponInSlot(slotIndex).IsEqualTo(weapon))
                {
                    UnequipWeapon(slotIndex, true);
                    onSwitchingRightWeapon.Invoke();
                    return;
                }

                SetRightWeapon(weapon, slotIndex);
                onSwitchingRightWeapon.Invoke();
            }
            else
            {

                // Is Already Equipped?
                if (GetLeftWeaponInSlot(slotIndex).IsEqualTo(weapon))
                {
                    UnequipWeapon(slotIndex, false);
                    onSwitchingLeftWeapon.Invoke();
                    return;
                }

                SetLeftWeapon(weapon, slotIndex);
                onSwitchingLeftWeapon.Invoke();
            }

            // Now check if we need to update the weapon currently equipped in the world. Only do it if we made changes to the active slot
            bool shouldUpdateWorldWeapon = false;
            if (isRightHand && GetCurrentRightHandWeaponSlotIndex() == slotIndex)
            {
                shouldUpdateWorldWeapon = true;
            }
            else if (GetCurrentLeftHandWeaponSlotIndex() == slotIndex)
            {
                shouldUpdateWorldWeapon = true;
            }

            if (shouldUpdateWorldWeapon)
            {
                characterBaseManager.characterWeapons.EquipWorldWeapon(
                    weapon,
                    isRightHand);
            }
        }

        public void UnequipWeapon(int slotIndex, bool isRightHand)
        {
            if (isRightHand)
            {
                ClearRightWeapon(slotIndex);
                onSwitchingRightWeapon.Invoke();
            }
            else
            {
                ClearLeftWeapon(slotIndex);
                onSwitchingLeftWeapon.Invoke();
            }

            // Now check if we need to update the weapon currently equipped in the world. Only do it if we made changes to the active slot
            bool shouldUpdateWorldWeapon = false;
            if (isRightHand && GetCurrentRightHandWeaponSlotIndex() == slotIndex)
            {
                shouldUpdateWorldWeapon = true;
            }
            else if (GetCurrentLeftHandWeaponSlotIndex() == slotIndex)
            {
                shouldUpdateWorldWeapon = true;
            }

            if (shouldUpdateWorldWeapon)
            {
                characterBaseManager.characterWeapons.UnequipWorldWeapon(isRightHand);
            }
        }

        protected abstract void SetArrow(Arrow arrow, int slotIndex);
        protected abstract void ClearArrow(int slotIndex);
        public void EquipArrow(Arrow arrow, int slotIndex)
        {
            // If arrow already equipped, unequip it
            if (GetCurrentArrow() == arrow)
            {
                UnequipArrow(slotIndex);
                return;
            }

            SetArrow(arrow, slotIndex);
        }
        public void UnequipArrow(int slotIndex)
        {
            ClearArrow(slotIndex);
        }

        protected abstract void SetSkill(SpellInstance skillInstance, int slotIndex);
        protected abstract void ClearSkill(int slotIndex);
        public void EquipSkill(SpellInstance skill, int slotIndex)
        {
            // If already equipped
            if (GetSpellInstance().IsEqualTo(skill))
            {
                UnequipSkill(slotIndex);
                return;
            }

            SetSkill(skill, slotIndex);
        }
        public void UnequipSkill(int slotIndex)
        {
            ClearSkill(slotIndex);
        }

        protected abstract void SetAccessory(AccessoryInstance accessoryInstance, int slotIndex);
        protected abstract void ClearAccessory(int slotIndex);
        public void EquipAccessory(AccessoryInstance accessory, int slotIndex)
        {
            // If accessory already equipped, unequip it
            if (GetAccessoryInSlot(slotIndex).IsEqualTo(accessory))
            {
                UnequipAccessory(slotIndex);
                return;
            }

            if (accessory.Exists())
            {
                accessory.GetItem<Accessory>().OnEquip(characterBaseManager);
            }

            SetAccessory(accessory, slotIndex);
            UpdateEquipmentValues();
        }
        public void UnequipAccessory(int slotIndex)
        {
            if (GetAccessoryInSlot(slotIndex) != null && GetAccessoryInSlot(slotIndex).Exists())
            {
                GetAccessoryInSlot(slotIndex).GetItem<Accessory>().OnUnequip(characterBaseManager);
            }

            ClearAccessory(slotIndex);
            UpdateEquipmentValues();
        }

        protected abstract void SetHelmet(HelmetInstance helmetInstance);
        protected abstract void ClearHelmet();
        public void EquipHelmet(HelmetInstance helmetInstance)
        {
            if (GetHelmetInstance().IsEqualTo(helmetInstance))
            {
                UnequipHelmet();
                return;
            }

            if (helmetInstance.Exists())
            {
                helmetInstance.GetItem<Helmet>().OnEquip(characterBaseManager);
            }

            SetHelmet(helmetInstance);
            UpdateEquipmentValues();
        }
        public void UnequipHelmet()
        {
            if (GetHelmetInstance() != null && GetHelmetInstance().Exists())
            {
                GetHelmetInstance().GetItem<Helmet>().OnUnequip(characterBaseManager);
            }

            ClearHelmet();
            UpdateEquipmentValues();
        }

        protected abstract void SetArmor(ArmorInstance armorInstance);
        protected abstract void ClearArmor();
        public void EquipArmor(ArmorInstance armorInstance)
        {
            if (GetArmorInstance().IsEqualTo(armorInstance))
            {
                UnequipArmor();
                return;
            }

            if (armorInstance.Exists())
            {
                armorInstance.GetItem<Armor>().OnEquip(characterBaseManager);
            }

            SetArmor(armorInstance);
            UpdateEquipmentValues();
        }
        public void UnequipArmor()
        {
            if (GetArmorInstance() != null && GetArmorInstance().Exists())
            {
                GetArmorInstance().GetItem<Armor>().OnUnequip(characterBaseManager);
            }

            ClearArmor();
            UpdateEquipmentValues();
        }


        protected abstract void SetGauntlets(GauntletInstance gauntletInstance);
        protected abstract void ClearGauntlets();
        public void EquipGauntlets(GauntletInstance gauntletInstance)
        {
            if (GetGauntletInstance().IsEqualTo(gauntletInstance))
            {
                UnequipGauntlets();
                return;
            }

            if (gauntletInstance.Exists())
            {
                gauntletInstance.GetItem<Gauntlet>().OnEquip(characterBaseManager);
            }

            SetGauntlets(gauntletInstance);
            UpdateEquipmentValues();
        }
        public void UnequipGauntlets()
        {
            if (GetGauntletInstance() != null && GetGauntletInstance().Exists())
            {
                GetGauntletInstance().GetItem<Gauntlet>().OnUnequip(characterBaseManager);
            }

            ClearGauntlets();
            UpdateEquipmentValues();
        }

        protected abstract void SetLegwear(LegwearInstance legwearInstance);
        protected abstract void ClearLegwear();
        public void EquipLegwear(LegwearInstance legwearInstance)
        {
            if (GetLegwearInstance().IsEqualTo(legwearInstance))
            {
                UnequipLegwear();
                return;
            }

            if (legwearInstance.Exists())
            {
                legwearInstance.GetItem<Legwear>().OnEquip(characterBaseManager);
            }

            SetLegwear(legwearInstance);
            UpdateEquipmentValues();
        }
        public void UnequipLegwear()
        {
            if (GetLegwearInstance() != null && GetLegwearInstance().Exists())
            {
                GetLegwearInstance().GetItem<Legwear>().OnUnequip(characterBaseManager);
            }

            ClearLegwear();
            UpdateEquipmentValues();
        }


        protected abstract void SetConsumable(Consumable consumable, int slotIndex);
        protected abstract void ClearConsumable(int slotIndex);
        public void EquipConsumable(Consumable consumable, int slotIndex)
        {
            // If consumable is equipped, unequip it
            if (GetConsumable() == consumable)
            {
                UnequipConsumable(slotIndex);
                return;
            }

            SetConsumable(consumable, slotIndex);
        }
        public void UnequipConsumable(int slotIndex)
        {
            ClearConsumable(slotIndex);
        }
        public abstract void UnequipCurrentConsumable();



        void UpdateEquipmentValues()
        {
            characterBaseManager.statsBonusController.RecalculateEquipmentBonus();
            characterBaseManager.characterBaseDefenseManager.RecalculateDamageAbsorbed();
        }

        public bool IsAccessoryEquiped(AccessoryInstance accessory)
        {
            return GetAccessoryInstances().Any(acc => acc.IsEqualTo(accessory));
        }

        public bool IsAccessoryEquiped(Accessory accessory)
        {
            return GetAccessoryInstances().Any(acc => acc.HasItem(accessory));
        }

        public bool IsBowEquipped()
        {
            if (GetRightHandWeapon().Exists())
            {
                return GetRightHandWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Range;
            }
            else if (GetLeftHandWeapon().Exists())
            {
                return GetLeftHandWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Range;
            }
            return false;
        }

        public bool IsStaffEquipped()
        {
            if (GetRightHandWeapon().Exists())
            {
                return GetRightHandWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Staff;
            }
            else if (GetLeftHandWeapon().Exists())
            {
                return GetLeftHandWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Staff;
            }
            return false;
        }

        public bool HasEnoughCurrentArrows()
        {
            Arrow currentArrow = GetCurrentArrow();


            if (currentArrow == null)
            {
                return false;
            }

            return characterBaseManager.characterBaseInventory
                .GetItemQuantity(currentArrow) > 0;
        }

        public bool IsNaked()
        {
            return
                GetHelmetInstance().IsEmpty()
                && GetArmorInstance().IsEmpty()
                && GetGauntletInstance().IsEmpty()
                && GetLegwearInstance().IsEmpty();
        }

        public bool IsItemInstanceEquipped(ItemInstance itemInstance)
        {
            return
                GetLeftHandWeapons().Any(leftHandWeapon => leftHandWeapon.IsEqualTo(itemInstance))
                || GetRightHandWeapons().Any(rightHandWeapon => rightHandWeapon.IsEqualTo(itemInstance))
                || GetSpells().Any(spell => spell.IsEqualTo(itemInstance))
                || GetAccessoryInstances().Any(accessory => accessory.IsEqualTo(itemInstance))
                || GetHelmetInstance().IsEqualTo(itemInstance)
                || GetArmorInstance().IsEqualTo(itemInstance)
                || GetGauntletInstance().IsEqualTo(itemInstance)
                || GetLegwearInstance().IsEqualTo(itemInstance);
        }

        public bool IsItemEquipped(Item item)
        {
            return GetArrows().Any(arrow => arrow == item) || GetConsumables().Any(consumable => consumable == item);
        }

        public abstract int GetCurrentRightHandWeaponSlotIndex();
        public abstract int GetCurrentLeftHandWeaponSlotIndex();
        public abstract int GetCurrentConsumablesSlotIndex();
        public abstract int GetCurrentSkillsSlotIndex();
        public abstract int GetCurrentArrowsSlotIndex();


    }
}
