using AF;
using UnityEditor;
using UnityEngine;
using TigerForge;
using AF.Events;
using System.Linq;
using AF.Inventory;
using System;
using CI.QuickSave;

[CreateAssetMenu(fileName = "Equipment Database", menuName = "System/New Equipment Database", order = 0)]
public class EquipmentDatabase : ScriptableObject
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

    [Header("Flags")]
    public bool isTwoHanding = false;
    public bool isUsingShield = false;

    [Header("Databases")]
    public InventoryDatabase inventoryDatabase;

    public bool shouldClearOnExit = false;

#if UNITY_EDITOR
    private void OnEnable()
    {
        // No need to populate the list; it's serialized directly
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode && shouldClearOnExit)
        {
            // Clear the list when exiting play mode
            Clear();
        }
    }
#endif

    public void Clear()
    {
        currentWeaponIndex = 0;
        currentShieldIndex = 0;
        currentConsumableIndex = 0;
        currentSpellIndex = 0;
        currentArrowIndex = 0;

        for (int i = 0; i < rightWeapons.Length; i++)
        {
            rightWeapons[i].Clear();
        }

        for (int i = 0; i < leftWeapons.Length; i++)
        {
            leftWeapons[i].Clear();
        }

        for (int i = 0; i < spells.Length; i++)
        {
            spells[i].Clear();
        }

        for (int i = 0; i < accessories.Length; i++)
        {
            accessories[i].Clear();
        }

        for (int i = 0; i < consumables.Length; i++)
        {
            consumables[i].Clear();
        }
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].Clear();
        }

        helmet.Clear();
        armor.Clear();
        gauntlet.Clear();
        legwear.Clear();
    }

    public void SwitchToNextWeapon()
    {
        currentWeaponIndex++;

        if (currentWeaponIndex >= rightWeapons.Length)
        {
            currentWeaponIndex = 0;
        }

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipWeapon(WeaponInstance weapon, int slotIndex, bool isRightHand)
    {
        if (isRightHand)
        {
            rightWeapons[slotIndex] = weapon;
        }
        else
        {
            leftWeapons[slotIndex] = weapon;
        }

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }
    public void UnequipWeapon(int slotIndex, bool isRightHand)
    {
        if (isRightHand)
        {
            rightWeapons[slotIndex].Clear();
        }
        else
        {
            leftWeapons[slotIndex].Clear();
        }

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void SwitchToNextShield()
    {
        currentShieldIndex++;

        if (currentShieldIndex >= leftWeapons.Length)
        {
            currentShieldIndex = 0;
        }

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        EventManager.EmitEvent(EventMessages.ON_SHIELD_EQUIPMENT_CHANGED);
    }

    public void EquipShield(WeaponInstance shield, int slotIndex)
    {
        leftWeapons[slotIndex] = shield.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        EventManager.EmitEvent(EventMessages.ON_SHIELD_EQUIPMENT_CHANGED);
    }

    public void UnequipShield(int slotIndex)
    {
        leftWeapons[slotIndex].Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        EventManager.EmitEvent(EventMessages.ON_SHIELD_EQUIPMENT_CHANGED);
    }

    public void SwitchToNextArrow()
    {
        currentArrowIndex = UpdateIndex(currentArrowIndex, arrows);

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipArrow(ArrowInstance arrow, int slotIndex)
    {
        arrows[slotIndex] = arrow.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }
    public void UnequipArrow(int slotIndex)
    {
        arrows[slotIndex].Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void SwitchToNextSpell()
    {
        currentSpellIndex = UpdateIndex(currentSpellIndex, spells);

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipSpell(SpellInstance spell, int slotIndex)
    {
        spells[slotIndex] = spell.Clone();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }
    public void UnequipSpell(int slotIndex)
    {
        spells[slotIndex].Clear();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void SwitchToNextConsumable()
    {
        currentConsumableIndex = UpdateIndex(currentConsumableIndex, consumables);
    }

    public void EquipConsumable(ConsumableInstance consumable, int slotIndex)
    {
        consumables[slotIndex] = consumable.Clone();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void UnequipConsumable(int slotIndex)
    {
        consumables[slotIndex].Clear();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public int UpdateIndex<T>(int index, T[] targetList)
    {
        index++;

        int nextIndexWithEquippedSlot = Array.FindIndex(targetList, index, x => x != null);

        if (nextIndexWithEquippedSlot != -1)
        {
            index = nextIndexWithEquippedSlot;
        }
        else
        {
            int fallbackIndex = Array.FindIndex(targetList, 0, x => x != null);
            index = fallbackIndex == -1 ? 0 : fallbackIndex;
        }

        return index;
    }

    public void EquipHelmet(HelmetInstance equip)
    {
        helmet = equip.Clone();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }
    public void UnequipHelmet()
    {
        helmet.Clear();

        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipArmor(ArmorInstance equip)
    {
        armor = equip.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void UnequipArmor()
    {
        armor.Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipGauntlet(GauntletInstance equip)
    {
        gauntlet = equip.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void UnequipGauntlet()
    {
        gauntlet.Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipLegwear(LegwearInstance equip)
    {
        legwear = equip.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void UnequipLegwear()
    {
        legwear.Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void EquipAccessory(AccessoryInstance accessory, int slotIndex)
    {
        accessories[slotIndex] = accessory.Clone();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public void UnequipAccessory(int slotIndex)
    {
        accessories[slotIndex].Clear();
        EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
    }

    public WeaponInstance GetCurrentRightWeapon()
    {
        return rightWeapons[currentWeaponIndex];
    }

    public WeaponInstance GetCurrentLeftWeapon()
    {
        return leftWeapons[currentShieldIndex];
    }
    public SpellInstance GetCurrentSpell()
    {
        return spells[currentSpellIndex];
    }
    public ArrowInstance GetCurrentArrow()
    {
        return arrows[currentArrowIndex];
    }
    public ConsumableInstance GetCurrentConsumable()
    {
        return consumables[currentConsumableIndex];
    }

    public bool IsAccessoryEquiped(AccessoryInstance accessory)
    {
        return accessories.Any(acc => acc.IsEqualTo(accessory));
    }
    public bool IsAccessoryEquiped(Accessory accessory)
    {
        return accessories.Any(acc => acc.HasItem(accessory));
    }

    public bool IsBowEquipped()
    {
        if (GetCurrentRightWeapon().Exists())
        {
            return GetCurrentRightWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Range;
        }
        else if (GetCurrentLeftWeapon().Exists())
        {
            return GetCurrentLeftWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Range;
        }
        return false;
    }

    public bool IsStaffEquipped()
    {
        if (GetCurrentRightWeapon().Exists())
        {
            return GetCurrentRightWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Staff;
        }
        else if (GetCurrentLeftWeapon().Exists())
        {
            return GetCurrentLeftWeapon().GetItem<Weapon>().damage.weaponAttackType == WeaponAttackType.Staff;
        }
        return false;
    }

    public bool HasEnoughCurrentArrows()
    {
        ArrowInstance currentArrowInstance = GetCurrentArrow();
        if (!currentArrowInstance.Exists())
        {
            return false;
        }

        Arrow currentArrow = currentArrowInstance.GetItem<Arrow>();

        if (currentArrow == null)
        {
            return false;
        }

        return inventoryDatabase.GetItemAmount(currentArrow) > 0;
    }

    public bool IsPlayerNaked()
    {
        return helmet.IsEmpty() && armor.IsEmpty() && legwear.IsEmpty() && gauntlet.IsEmpty();
    }

    public int GetEquippedRightWeaponSlot(WeaponInstance weapon)
    {
        return Array.IndexOf(rightWeapons, weapon);
    }

    public int GetEquippedLeftWeaponSlot(WeaponInstance weapon)
    {
        return Array.IndexOf(leftWeapons, weapon);
    }

    public int GetEquippedShieldSlot(ShieldInstance shield)
    {
        return Array.IndexOf(leftWeapons, shield);
    }
    public int GetEquippedArrowsSlot(ArrowInstance arrow)
    {
        return Array.IndexOf(arrows, arrow);
    }
    public int GetEquippedSpellSlot(SpellInstance spell)
    {
        return Array.IndexOf(spells, spell);
    }
    public int GetEquippedAccessoriesSlot(AccessoryInstance accessory)
    {
        return Array.IndexOf(accessories, accessory);
    }
    public int GetEquippedConsumablesSlot(ConsumableInstance consumable)
    {
        return Array.IndexOf(consumables, consumable);
    }

    public void UnequipItem(ItemInstance item)
    {
        // Check weapons
        for (int i = 0; i < rightWeapons.Length; i++)
        {
            if (rightWeapons[i].IsEqualTo(item))
            {
                rightWeapons[i].Clear();
                EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
            }
        }

        // Check shields
        for (int i = 0; i < leftWeapons.Length; i++)
        {
            if (leftWeapons[i].IsEqualTo(item))
            {
                leftWeapons[i].Clear();
                EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
            }
        }

        // Check helmet
        if (helmet.IsEqualTo(item))
        {
            helmet.Clear();
            EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        }

        // Check armor
        if (armor.IsEqualTo(item))
        {
            armor.Clear();
            EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        }

        // Check gauntlet
        if (gauntlet.IsEqualTo(item))
        {
            gauntlet.Clear();
            EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        }

        // Check legwear
        if (legwear.IsEqualTo(item))
        {
            legwear.Clear();
            EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
        }

        // Check arrows
        for (int i = 0; i < arrows.Length; i++)
        {
            if (arrows[i].IsEqualTo(item))
            {
                arrows[i].Clear();
                EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
            }
        }

        // Check spells
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i].IsEqualTo(item))
            {
                spells[i].Clear();
                EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
            }
        }

        // Check accessories
        for (int i = 0; i < accessories.Length; i++)
        {
            if (accessories[i].IsEqualTo(item))
            {
                accessories[i].Clear();
                EventManager.EmitEvent(EventMessages.ON_EQUIPMENT_CHANGED);
            }
        }

        // Item not found equipped
        Debug.LogWarning($"UnequipItem: Item with ID '{item.id}' not found in equipment");
    }


    public void SetIsTwoHanding(bool value)
    {
        isTwoHanding = value;

        EventManager.EmitEvent(EventMessages.ON_TWO_HANDING_CHANGED);
    }


    public bool IsEquipped(ArmorBaseInstance armorBase)
    {
        if (!armorBase.Exists())
        {
            return false;
        }

        if (armorBase is HelmetInstance)
        {
            return helmet.IsEqualTo(armorBase);
        }
        else if (armorBase is GauntletInstance)
        {
            return gauntlet.IsEqualTo(armorBase);
        }
        else if (armorBase is ArmorInstance)
        {
            return armor.IsEqualTo(armorBase);
        }
        else if (armorBase is LegwearInstance)
        {
            return legwear.IsEqualTo(armorBase);
        }
        else if (armorBase is AccessoryInstance)
        {
            return accessories.Any(acc => acc.IsEqualTo(armorBase));
        }

        return false;
    }

    public void LoadEquipmentFromSaveFile(QuickSaveReader quickSaveReader)
    {

        quickSaveReader.TryRead<int>("currentWeaponIndex", out int currentWeaponIndex);
        this.currentWeaponIndex = currentWeaponIndex;

        quickSaveReader.TryRead<int>("currentShieldIndex", out int currentShieldIndex);
        this.currentShieldIndex = currentShieldIndex;

        quickSaveReader.TryRead<int>("currentArrowIndex", out int currentArrowIndex);
        this.currentArrowIndex = currentArrowIndex;

        quickSaveReader.TryRead<int>("currentSpellIndex", out int currentSpellIndex);
        this.currentSpellIndex = currentSpellIndex;

        quickSaveReader.TryRead<int>("currentConsumableIndex", out int currentConsumableIndex);
        this.currentConsumableIndex = currentConsumableIndex;

        quickSaveReader.TryRead<string[]>("weapons", out string[] weapons);
        if (weapons != null && weapons.Length > 0)
        {
            for (int idx = 0; idx < weapons.Length; idx++)
            {
                string weaponId = weapons[idx];

                if (!string.IsNullOrEmpty(weaponId))
                {
                    if (inventoryDatabase.FindItemById(weaponId) is WeaponInstance weaponInstance)
                    {
                        EquipWeapon(weaponInstance, idx, true);
                    }
                }
            }
        }

        // Try to read shields
        quickSaveReader.TryRead<string[]>("shields", out string[] shields);
        if (shields != null && shields.Length > 0)
        {
            for (int idx = 0; idx < shields.Length; idx++)
            {
                string shieldId = shields[idx];

                if (!string.IsNullOrEmpty(shieldId))
                {
                    if (inventoryDatabase.FindItemById(shieldId) is ShieldInstance shieldInstance)
                    {
                        EquipWeapon(shieldInstance, idx, false);
                    }
                }
            }
        }

        // Try to read arrows
        quickSaveReader.TryRead<string[]>("arrows", out string[] arrows);
        if (arrows != null && arrows.Length > 0)
        {
            for (int idx = 0; idx < arrows.Length; idx++)
            {
                string arrowId = arrows[idx];

                if (!string.IsNullOrEmpty(arrowId))
                {
                    if (inventoryDatabase.FindItemById(arrowId) is ArrowInstance arrowInstance)
                    {
                        EquipArrow(arrowInstance, idx);
                    }
                }
            }
        }

        // Try to read spells
        quickSaveReader.TryRead<string[]>("spells", out string[] spells);
        if (spells != null && spells.Length > 0)
        {
            for (int idx = 0; idx < spells.Length; idx++)
            {
                string spellId = spells[idx];

                if (!string.IsNullOrEmpty(spellId))
                {
                    if (inventoryDatabase.FindItemById(spellId) is SpellInstance spellInstance)
                    {
                        EquipSpell(spellInstance, idx);
                    }
                }
            }
        }

        // Try to read accessories
        quickSaveReader.TryRead<string[]>("accessories", out string[] accessories);
        if (accessories != null && accessories.Length > 0)
        {
            for (int idx = 0; idx < accessories.Length; idx++)
            {
                string accessoryId = accessories[idx];

                if (!string.IsNullOrEmpty(accessoryId))
                {
                    if (inventoryDatabase.FindItemById(accessoryId) is AccessoryInstance accessoryInstance)
                    {
                        EquipAccessory(accessoryInstance, idx);
                    }
                }
            }
        }

        // Try to read consumables
        quickSaveReader.TryRead<string[]>("consumables", out string[] consumables);
        if (consumables != null && consumables.Length > 0)
        {
            for (int idx = 0; idx < consumables.Length; idx++)
            {
                string consumableId = consumables[idx];

                if (!string.IsNullOrEmpty(consumableId))
                {
                    if (inventoryDatabase.FindItemById(consumableId) is ConsumableInstance consumableInstance)
                    {
                        EquipConsumable(consumableInstance, idx);
                    }
                }
            }
        }

        // Try to read helmet
        quickSaveReader.TryRead<string>("helmet", out string helmetId);
        if (!string.IsNullOrEmpty(helmetId))
        {
            if (inventoryDatabase.FindItemById(helmetId) is HelmetInstance helmetInstance)
            {
                EquipHelmet(helmetInstance);
            }
        }
        else
        {
            UnequipHelmet();
        }

        // Try to read armor
        quickSaveReader.TryRead<string>("armor", out string armorId);
        if (!string.IsNullOrEmpty(armorId))
        {
            if (inventoryDatabase.FindItemById(armorId) is ArmorInstance armorInstance)
            {
                EquipArmor(armorInstance);
            }
        }
        else
        {
            UnequipArmor();
        }

        // Try to read gauntlet
        quickSaveReader.TryRead<string>("gauntlet", out string gauntletId);
        if (!string.IsNullOrEmpty(gauntletId))
        {
            if (inventoryDatabase.FindItemById(gauntletId) is GauntletInstance gauntletInstance)
            {
                EquipGauntlet(gauntletInstance);
            }
        }
        else
        {
            UnequipGauntlet();
        }

        // Try to read legwear
        quickSaveReader.TryRead<string>("legwear", out string legwearId);
        if (!string.IsNullOrEmpty(legwearId))
        {
            LegwearInstance legwearInstance = inventoryDatabase.FindItemById(legwearId) as LegwearInstance;

            if (legwearInstance != null)
            {
                EquipLegwear(legwearInstance);
            }
        }
        else
        {
            UnequipLegwear();
        }

        quickSaveReader.TryRead<bool>("isTwoHanding", out bool isTwoHanding);
        this.isTwoHanding = isTwoHanding;
    }
}
