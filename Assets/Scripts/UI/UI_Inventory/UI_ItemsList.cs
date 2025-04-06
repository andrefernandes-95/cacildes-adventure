using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    public class UI_ItemsList : MonoBehaviour
    {
        [SerializeField] UI_CharacterInventory uI_CharacterInventory;

        [Header("UI")]
        public UI_InventoryFilterButton filterForWeaponsButton;

        public UI_InventoryFilterButton filterForShieldsButton;

        public UI_InventoryFilterButton filterForArrowsButton;

        public UI_InventoryFilterButton filterForSkillsButton;

        public UI_InventoryFilterButton filterForAccessoriesButton;

        public UI_InventoryFilterButton filterForConsumablesButton;
        public UI_InventoryFilterButton filterForHelmetsButton;
        public UI_InventoryFilterButton filterForArmorsButton;
        public UI_InventoryFilterButton filterForGauntletsButton;
        public UI_InventoryFilterButton filterForBootsButton;
        public UI_InventoryFilterButton noFiltersButton;


        [Header("Equipping Filtering Options")]
        bool isAttemptingToEquipItems = false;

        public bool isAttemptingToEquipRightWeapon = false;
        public bool isAttemptingToEquipLeftWeapon = false;
        public int equippingSlotIndex = -1;

        [Header("Filters")]
        public bool filterForWeapons = false;
        public bool filterForShields = false;
        public bool filterForArrows = false;
        public bool filterForSkills = false;
        public bool filterForAccessories = false;
        public bool filterForConsumables = false;
        public bool filterForHelmets = false;
        public bool filterForArmors = false;
        public bool filterForGauntlets = false;
        public bool filterForBoots = false;
        public bool noFilter = false;

        [Header("Items to show")]
        public List<WeaponInstance> weaponInstances = new();
        public List<ShieldInstance> shieldInstances = new();
        public SerializedDictionary<Arrow, List<ArrowInstance>> stackableArrowInstances = new();
        public List<SpellInstance> spellInstances = new();
        public List<AccessoryInstance> accessoryInstances = new();
        public List<HelmetInstance> helmetInstances = new();
        public List<ArmorInstance> armorInstances = new();
        public List<GauntletInstance> gauntletInstances = new();
        public List<LegwearInstance> legwearInstances = new();
        // Consumables
        public SerializedDictionary<Consumable, List<ConsumableInstance>> stackableConsumableInstances = new();
        public List<ConsumableInstance> nonStackableConsumableInstances = new();
        // All Items
        public SerializedDictionary<CraftingMaterial, List<CraftingMaterialInstance>> stackableCraftMaterialInstances = new();
        public SerializedDictionary<UpgradeMaterial, List<UpgradeMaterialInstance>> stackableUpgradeMaterialInstances = new();
        public List<KeyItemInstance> keyItemInstances = new();

        private void OnEnable()
        {
            CollectItems();
        }

        void OnDisable()
        {
            ClearItems();
        }

        void CollectItems()
        {
            if (filterForWeapons)
            {
                CollectWeapons();
            }
            else if (filterForShields)
            {
                CollectShields();
            }
            else if (filterForArrows)
            {
                CollectArrows();
            }
            else if (filterForSkills)
            {
                CollectSpells();
            }
            else if (filterForHelmets)
            {
                CollectHelmets();
            }
            else if (filterForArmors)
            {
                CollectArmors();
            }
            else if (filterForGauntlets)
            {
                CollectGauntlets();
            }
            else if (filterForBoots)
            {
                CollectLegwears();
            }
            else if (filterForAccessories)
            {
                CollectAccessories();
            }
            else if (filterForConsumables)
            {
                CollectConsumables();
            }
            else if (noFilter)
            {
                CollectWeapons();
                CollectShields();
                CollectArrows();
                CollectSpells();
                CollectHelmets();
                CollectArmors();
                CollectGauntlets();
                CollectLegwears();
                CollectAccessories();
                CollectConsumables();
                CollectCraftingMaterials();
                CollectUpgradeMaterials();
                CollectKeyItems();
            }
        }

        void CollectWeapons()
        {
            CharacterBaseManager targetCharacter = uI_CharacterInventory.character;
            CharacterBaseInventory characterInventory = targetCharacter.characterBaseInventory;
            CharacterBaseEquipment characterEquipment = targetCharacter.characterBaseEquipment;

            // Start by collecting all weapon instances
            var allWeapons = characterInventory.GetAllWeaponInstances();

            if (!isAttemptingToEquipItems)
            {
                this.weaponInstances = allWeapons;
                return;
            }

            if (isAttemptingToEquipRightWeapon)
            {
                this.weaponInstances = allWeapons
                    // Exclude weapons currently equipped in the left hand
                    .Where(weapon => characterEquipment.GetEquippedWeaponSlot(weapon, false) == -1)
                    // Allow:
                    // - Weapons not equipped in any right-hand slot
                    // - Or, the weapon currently equipped in the right-hand slot we are trying to modify
                    .Where(weapon =>
                        weapon.IsEqualTo(characterEquipment.GetRightWeaponInSlot(equippingSlotIndex)) ||
                        characterEquipment.GetEquippedWeaponSlot(weapon, true) == -1)
                    .ToList();
            }
            else if (isAttemptingToEquipLeftWeapon)
            {
                this.weaponInstances = allWeapons
                    // Exclude weapons currently equipped in the right hand
                    .Where(weapon => characterEquipment.GetEquippedWeaponSlot(weapon, true) == -1)
                    // Allow:
                    // - Weapons not equipped in any left-hand slot
                    // - Or, the weapon currently equipped in the left-hand slot we are trying to modify
                    .Where(weapon =>
                        weapon.IsEqualTo(characterEquipment.GetLeftWeaponInSlot(equippingSlotIndex)) ||
                        characterEquipment.GetEquippedWeaponSlot(weapon, false) == -1)
                    .ToList();
            }
        }

        void CollectShields()
        {
            this.shieldInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllShieldInstances();
        }

        void CollectArrows()
        {
            List<ArrowInstance> arrowInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllArrowInstances();

            SerializedDictionary<Arrow, List<ArrowInstance>> stackableArrows = new();
            foreach (ArrowInstance arrowInstance in arrowInstances)
            {
                Arrow arrow = arrowInstance.GetItem<Arrow>();
                if (stackableArrows.ContainsKey(arrow))
                {
                    stackableArrows[arrow].Add(arrowInstance);
                }
                else
                {
                    stackableArrows.Add(arrow, new() { arrowInstance });
                }
            }

            if (!isAttemptingToEquipItems)
            {

                this.stackableArrowInstances = stackableArrows;
                return;
            }

            CharacterBaseEquipment characterEquipment = uI_CharacterInventory.character.characterBaseEquipment;

            this.stackableArrowInstances = (SerializedDictionary<Arrow, List<ArrowInstance>>)this.stackableArrowInstances.Where(arrowEntry =>
            {
                for (int i = 0; i < characterEquipment.GetArrows().Length; i++)
                {
                    if (i == equippingSlotIndex)
                        continue;

                    if (characterEquipment.GetArrowInSlot(i) == arrowEntry.Key)
                        return false; // Arrow is already in another slot
                }

                return true; // Arrow is not equipped anywhere else
            });
        }


        void CollectSpells()
        {
            this.spellInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllSpellInstances();
        }

        void CollectAccessories()
        {
            this.accessoryInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllAccessoryInstances();
        }

        void CollectHelmets()
        {
            this.helmetInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllHelmetInstances();
        }

        void CollectArmors()
        {
            this.armorInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllArmorInstances();
        }

        void CollectGauntlets()
        {
            this.gauntletInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllGauntletInstances();
        }

        void CollectLegwears()
        {
            this.legwearInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllLegwearInstances();
        }

        void CollectConsumables()
        {
            List<ConsumableInstance> consumableInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllConsumableInstances();

            List<ConsumableInstance> nonStackableConsumables = new();
            SerializedDictionary<Consumable, List<ConsumableInstance>> stackableConsumables = new();

            foreach (ConsumableInstance consumableInstance in consumableInstances)
            {
                Consumable consumable = consumableInstance.GetItem<Consumable>();

                if (consumable.isStackable)
                {
                    if (stackableConsumables.ContainsKey(consumable))
                    {
                        stackableConsumables[consumable].Add(consumableInstance);
                    }
                    else
                    {
                        stackableConsumables.Add(consumable, new() { consumableInstance });
                    }
                }
                else
                {
                    nonStackableConsumables.Add(consumableInstance);
                }
            }

            this.nonStackableConsumableInstances = nonStackableConsumables;
            this.stackableConsumableInstances = stackableConsumables;
        }

        void CollectUpgradeMaterials()
        {
            List<UpgradeMaterialInstance> upgradeMaterials = uI_CharacterInventory.character.characterBaseInventory.GetAllUpgradeMaterialInstances();

            SerializedDictionary<UpgradeMaterial, List<UpgradeMaterialInstance>> stackableUpgradeMaterials = new();
            foreach (UpgradeMaterialInstance upgradeMaterialInstance in upgradeMaterials)
            {
                UpgradeMaterial upgradeMaterial = upgradeMaterialInstance.GetItem<UpgradeMaterial>();
                if (stackableUpgradeMaterials.ContainsKey(upgradeMaterial))
                {
                    stackableUpgradeMaterials[upgradeMaterial].Add(upgradeMaterialInstance);
                }
                else
                {
                    stackableUpgradeMaterials.Add(upgradeMaterial, new() { upgradeMaterialInstance });
                }
            }

            this.stackableUpgradeMaterialInstances = stackableUpgradeMaterials;
        }

        void CollectCraftingMaterials()
        {
            List<CraftingMaterialInstance> craftingMaterials = uI_CharacterInventory.character.characterBaseInventory.GetAllCraftingMaterialInstances();

            SerializedDictionary<CraftingMaterial, List<CraftingMaterialInstance>> stackableCraftingMaterials = new();
            foreach (CraftingMaterialInstance craftingMaterialInstance in craftingMaterials)
            {
                CraftingMaterial craftingMaterial = craftingMaterialInstance.GetItem<CraftingMaterial>();
                if (stackableCraftingMaterials.ContainsKey(craftingMaterial))
                {
                    stackableCraftingMaterials[craftingMaterial].Add(craftingMaterialInstance);
                }
                else
                {
                    stackableCraftingMaterials.Add(craftingMaterial, new() { craftingMaterialInstance });
                }
            }

            this.stackableCraftMaterialInstances = stackableCraftingMaterials;
        }

        void CollectKeyItems()
        {
            this.keyItemInstances = uI_CharacterInventory.character.characterBaseInventory.GetAllKeyItemInstances();
        }

        void ClearItems()
        {
            weaponInstances.Clear();
            shieldInstances.Clear();
            stackableArrowInstances.Clear();
            spellInstances.Clear();
            accessoryInstances.Clear();
            helmetInstances.Clear();
            armorInstances.Clear();
            gauntletInstances.Clear();
            legwearInstances.Clear();
            stackableConsumableInstances.Clear();
            nonStackableConsumableInstances.Clear();
            stackableCraftMaterialInstances.Clear();
            stackableUpgradeMaterialInstances.Clear();
            keyItemInstances.Clear();
        }

        void Refresh()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public bool IsAttemptingToEquipItems()
        {
            return isAttemptingToEquipItems;
        }

        public void SetIsAttemptingToEquipItems(bool isAttemptingToEquipItems)
        {
            this.isAttemptingToEquipItems = isAttemptingToEquipItems;
            Refresh();
        }

    }
}
