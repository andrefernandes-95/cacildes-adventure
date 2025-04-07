using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        [ReadOnly][SerializeField] bool filterForWeapons = false;
        [ReadOnly][SerializeField] bool filterForShields = false;
        [ReadOnly][SerializeField] bool filterForArrows = false;
        [ReadOnly][SerializeField] bool filterForSkills = false;
        [ReadOnly][SerializeField] bool filterForAccessories = false;
        [ReadOnly][SerializeField] bool filterForConsumables = false;
        [ReadOnly][SerializeField] bool filterForHelmets = false;
        [ReadOnly][SerializeField] bool filterForArmors = false;
        [ReadOnly][SerializeField] bool filterForGauntlets = false;
        [ReadOnly][SerializeField] bool filterForBoots = false;
        [ReadOnly][SerializeField] bool noFilter = false;

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

        [Header("UI Components")]
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] GameObject ui_itemButtonPrefab;

        private void OnEnable()
        {
            CollectItems();

            PreselectFilterButton();

            PopulateScrollRect();
        }

        void OnDisable()
        {
            ClearItems();

            // Reset isAttemptingToEquip state as well
            ResetIsAttemptingToEquipState();
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

        void ResetFilters()
        {
            filterForWeapons = filterForShields = filterForArrows = filterForSkills =
            filterForAccessories = filterForConsumables = filterForHelmets = filterForArmors =
            filterForGauntlets = filterForBoots = noFilter = false;
        }

        public void FilterForWeapons()
        {
            ResetFilters();
            filterForWeapons = true;
            Refresh();
        }

        public void FilterForShields()
        {
            ResetFilters();
            filterForShields = true;
            Refresh();
        }
        public void FilterForArrows()
        {
            ResetFilters();
            filterForArrows = true;
            Refresh();
        }
        public void FilterForSkills()
        {
            ResetFilters();
            filterForSkills = true;
            Refresh();
        }
        public void FilterForAccessories()
        {
            ResetFilters();
            filterForAccessories = true;
            Refresh();
        }
        public void FilterForConsumables()
        {
            ResetFilters();
            filterForConsumables = true;
            Refresh();
        }
        public void FilterForHelmets()
        {
            ResetFilters();
            filterForHelmets = true;
            Refresh();
        }
        public void FilterForArmors()
        {
            ResetFilters();
            filterForArmors = true;
            Refresh();
        }
        public void FilterForGauntlets()
        {
            ResetFilters();
            filterForGauntlets = true;
            Refresh();
        }
        public void FilterForBoots()
        {
            ResetFilters();
            filterForBoots = true;
            Refresh();
        }
        public void ClearFilters()
        {
            ResetFilters();
            noFilter = true;
            Refresh();
        }


        void PreselectFilterButton()
        {
            if (filterForWeapons)
            {
                filterForWeaponsButton.GetComponent<Button>().Select();
            }
            else if (filterForShields)
            {
                filterForShieldsButton.GetComponent<Button>().Select();
            }
            else if (filterForArrows)
            {
                filterForArrowsButton.GetComponent<Button>().Select();
            }
            else if (filterForSkills)
            {
                filterForSkillsButton.GetComponent<Button>().Select();
            }
            else if (filterForConsumables)
            {
                filterForConsumablesButton.GetComponent<Button>().Select();
            }
            else if (filterForAccessories)
            {
                filterForAccessoriesButton.GetComponent<Button>().Select();
            }
            else if (filterForHelmets)
            {
                filterForHelmetsButton.GetComponent<Button>().Select();
            }
            else if (filterForArmors)
            {
                filterForArmorsButton.GetComponent<Button>().Select();
            }
            else if (filterForGauntlets)
            {
                filterForGauntletsButton.GetComponent<Button>().Select();
            }
            else if (filterForBoots)
            {
                filterForBootsButton.GetComponent<Button>().Select();
            }
            else if (noFilter)
            {
                noFiltersButton.GetComponent<Button>().Select();
            }
        }

        void PopulateScrollRect()
        {
            foreach (Transform child in scrollRect.content.transform)
            {
                Destroy(child.gameObject);
            }

            if (filterForWeapons)
            {
                PopulateWeapons();
            }
            else if (filterForShields)
            {
                PopulateShields();
            }
            else if (filterForArrows)
            {
                PopulateArrows();
            }
            else if (filterForSkills)
            {
                PopulateSkills();
            }
            else if (filterForConsumables)
            {
                PopulateConsumables();
            }
            else if (filterForAccessories)
            {
                PopulateAccessories();
            }
            else if (filterForHelmets)
            {
                PopulateHelmets();
            }
            else if (filterForArmors)
            {
                PopulateArmors();
            }
            else if (filterForGauntlets)
            {
                PopulateGauntlets();
            }
            else if (filterForBoots)
            {
                PopulateBoots();
            }
            else if (noFilter)
            {
                PopulateWeapons();
                PopulateShields();
                PopulateArrows();
                PopulateSkills();
                PopulateHelmets();
                PopulateArmors();
                PopulateGauntlets();
                PopulateBoots();
                PopulateAccessories();
                PopulateConsumables();
                PopulateCraftingMaterials();
                PopulateUpgradeMaterials();
                PopulateKeyItems();
            }
        }

        void PopulateWeapons()
        {
            foreach (WeaponInstance weaponInstance in weaponInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = weaponInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateShields()
        {
            foreach (ShieldInstance shieldInstance in shieldInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = shieldInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateArrows()
        {
            foreach (KeyValuePair<Arrow, List<ArrowInstance>> stackableArrow in stackableArrowInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.stackableItem = stackableArrow.Key;
                uI_ItemButton.stackableItemAmount = stackableArrow.Value.Count;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateSkills()
        {
            foreach (SpellInstance spellInstance in spellInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = spellInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateConsumables()
        {
            foreach (KeyValuePair<Consumable, List<ConsumableInstance>> stackableConsumable in stackableConsumableInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.stackableItem = stackableConsumable.Key;
                uI_ItemButton.stackableItemAmount = stackableConsumable.Value.Count;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateAccessories()
        {
            foreach (AccessoryInstance accessoryInstance in accessoryInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = accessoryInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateHelmets()
        {
            foreach (HelmetInstance helmetInstance in helmetInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = helmetInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateArmors()
        {
            foreach (ArmorInstance armorInstance in armorInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = armorInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateGauntlets()
        {
            foreach (GauntletInstance gauntletInstance in gauntletInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = gauntletInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateBoots()
        {
            foreach (LegwearInstance legwearInstance in legwearInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = legwearInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateUpgradeMaterials()
        {
            foreach (KeyValuePair<UpgradeMaterial, List<UpgradeMaterialInstance>> stackableUpgradeMaterial in stackableUpgradeMaterialInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.stackableItem = stackableUpgradeMaterial.Key;
                uI_ItemButton.stackableItemAmount = stackableUpgradeMaterial.Value.Count;
                uI_ItemButton.SetupButton();
            }
        }
        void PopulateCraftingMaterials()
        {
            foreach (KeyValuePair<CraftingMaterial, List<CraftingMaterialInstance>> stackableCraftingMaterial in stackableCraftMaterialInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.stackableItem = stackableCraftingMaterial.Key;
                uI_ItemButton.stackableItemAmount = stackableCraftingMaterial.Value.Count;
                uI_ItemButton.SetupButton();
            }
        }

        void PopulateKeyItems()
        {
            foreach (KeyItemInstance keyItemInstance in keyItemInstances)
            {
                GameObject itemButton = Instantiate(ui_itemButtonPrefab, scrollRect.content);
                UI_ItemButton uI_ItemButton = itemButton.GetComponent<UI_ItemButton>();
                uI_ItemButton.uI_CharacterInventory = uI_CharacterInventory;
                uI_ItemButton.itemInstance = keyItemInstance;
                uI_ItemButton.SetupButton();
            }
        }

        void ResetIsAttemptingToEquipState()
        {
            SetIsAttemptingToEquipItems(false, -1);
            isAttemptingToEquipRightWeapon = false;
            isAttemptingToEquipLeftWeapon = false;
        }

        public void SetIsAttemptingToEquipItems(bool isAttemptingToEquipItems, int slotToEquipItemTo)
        {
            this.isAttemptingToEquipItems = isAttemptingToEquipItems;
            equippingSlotIndex = slotToEquipItemTo;
        }

    }
}
