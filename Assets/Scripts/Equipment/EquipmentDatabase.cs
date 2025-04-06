using AF;
using UnityEditor;
using UnityEngine;
using TigerForge;
using AF.Events;
using System.Linq;
using AF.Inventory;
using System;
using CI.QuickSave;

/// <summary>
/// Use this to store player equipment data across scenes. Do not use methods, lets leave that to the character base weapon system
/// </summary>
[CreateAssetMenu(fileName = "Equipment Database", menuName = "System/New Equipment Database", order = 0)]
public class EquipmentDatabase : ScriptableObject
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

    public int currentRightWeaponIndex, currentLeftWeaponIndex, currentConsumableIndex, currentSkillIndex, currentArrowIndex = 0;

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
        currentLeftWeaponIndex = 0;
        currentRightWeaponIndex = 0;
        currentConsumableIndex = 0;
        currentSkillIndex = 0;
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
            consumables[i] = null;
        }
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i] = null;
        }

        helmet.Clear();
        armor.Clear();
        gauntlet.Clear();
        legwear.Clear();
    }

    // TODO: Make this logic appear first during load,
    // Then the character will just fetch the newly data from the database on start
    public void LoadEquipmentFromSaveFile(QuickSaveReader quickSaveReader)
    {
        /*
                quickSaveReader.TryRead<int>("currentWeaponIndex", out int currentWeaponIndex);
                this.currentRightWeaponIndex = currentWeaponIndex;

                quickSaveReader.TryRead<int>("currentShieldIndex", out int currentShieldIndex);
                this.currentLeftWeaponIndex = currentShieldIndex;

                quickSaveReader.TryRead<int>("currentArrowIndex", out int currentArrowIndex);
                this.currentArrowIndex = currentArrowIndex;

                quickSaveReader.TryRead<int>("currentSpellIndex", out int currentSpellIndex);
                this.currentSkillIndex = currentSpellIndex;

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
                                // TODO: Change to Player Equipment Logic
                                // EquipWeapon(weaponInstance, idx, true);
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
                                // TODO :Change to PlayerEquipment logic
                                // EquipWeapon(shieldInstance, idx, false);
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
                                //EquipArrow(arrowInstance, idx);
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
                                //   EquipSpell(spellInstance, idx);
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
                                //EquipAccessory(accessoryInstance, idx);
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
                                // EquipConsumable(consumableInstance, idx);
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
                        //EquipHelmet(helmetInstance);
                    }
                }
                else
                {
                    // UnequipHelmet();
                }

                // Try to read armor
                quickSaveReader.TryRead<string>("armor", out string armorId);
                if (!string.IsNullOrEmpty(armorId))
                {
                    if (inventoryDatabase.FindItemById(armorId) is ArmorInstance armorInstance)
                    {
                        // EquipArmor(armorInstance);
                    }
                }
                else
                {
                    // UnequipArmor();
                }

                // Try to read gauntlet
                quickSaveReader.TryRead<string>("gauntlet", out string gauntletId);
                if (!string.IsNullOrEmpty(gauntletId))
                {
                    if (inventoryDatabase.FindItemById(gauntletId) is GauntletInstance gauntletInstance)
                    {
                        //   EquipGauntlet(gauntletInstance);
                    }
                }
                else
                {
                    // UnequipGauntlet();
                }

                // Try to read legwear
                quickSaveReader.TryRead<string>("legwear", out string legwearId);
                if (!string.IsNullOrEmpty(legwearId))
                {
                    LegwearInstance legwearInstance = inventoryDatabase.FindItemById(legwearId) as LegwearInstance;

                    if (legwearInstance != null)
                    {
                        //   EquipLegwear(legwearInstance);
                    }
                }
                else
                {
                    //   UnequipLegwear();
                }

                quickSaveReader.TryRead<bool>("isTwoHanding", out bool isTwoHanding);
                this.isTwoHanding = isTwoHanding;*/
    }
}
