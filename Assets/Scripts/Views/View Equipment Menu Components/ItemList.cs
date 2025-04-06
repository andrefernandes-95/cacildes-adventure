using System.Collections.Generic;
using System.Linq;
using AF.Equipment;
using AF.Inventory;
using AF.Stats;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF.UI.EquipmentMenu
{
    public class ItemList : MonoBehaviour
    {
        public enum EquipmentType
        {
            WEAPON,
            SHIELD,
            ARROW,
            SPELL,
            HELMET,
            ARMOR,
            GAUNTLET,
            BOOTS,
            ACCESSORIES,
            CONSUMABLES,
            OTHER_ITEMS,
        }

        ScrollView itemsScrollView;

        Label menuLabel;

        public const string SCROLL_ITEMS_LIST = "ItemsList";

        [Header("UI Components")]
        public VisualTreeAsset itemButtonPrefab;
        public ItemTooltip itemTooltip;
        public PlayerStatsAndAttributesUI playerStatsAndAttributesUI;
        public EquipmentSlots equipmentSlots;
        [Header("UI Documents")]
        public UIDocument uIDocument;
        public VisualElement root;

        [Header("Components")]
        public MenuManager menuManager;
        public CursorManager cursorManager;
        public PlayerManager playerManager;
        public StarterAssetsInputs inputs;
        public Soundbank soundbank;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;
        public PlayerStatsDatabase playerStatsDatabase;
        public InventoryDatabase inventoryDatabase;

        Button returnButton;

        [HideInInspector] public bool shouldRerender = true;

        VisualElement itemListKeyHints, itemListGamepadHints, equipItemKeyHint, equipItemButtonHint, useItemKeyHint, useItemButtonHint;

        int lastScrollElementIndex = -1;

        public NotificationManager notificationManager;


        private void OnEnable()
        {
            if (shouldRerender)
            {
                shouldRerender = false;

                SetupRefs();
            }

            returnButton.transform.scale = new Vector3(1, 1, 1);
            root.Q<VisualElement>("ItemList").style.display = DisplayStyle.Flex;
            itemListKeyHints.style.display = DisplayStyle.None;
            itemListGamepadHints.style.display = DisplayStyle.None;
        }

        private void OnDisable()
        {
            root.Q<VisualElement>("ItemList").style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnUseItem()
        {
            /*
            if (isActiveAndEnabled && focusedItem != null && focusedItem is Consumable c)
            {
                playerManager.playerInventory.PrepareItemForConsuming(c);
            }*/
        }

        public void SetupRefs()
        {
            root = uIDocument.rootVisualElement;
            menuLabel = root.Q<Label>("MenuLabel");

            returnButton = root.Q<Button>("ReturnButton");
            UIUtils.SetupButton(returnButton, () =>
            {
                ReturnToEquipmentSlots();
            }, soundbank);

            itemsScrollView = root.Q<ScrollView>(SCROLL_ITEMS_LIST);
            itemListKeyHints = root.Q<VisualElement>("ItemListKeyboardHints");
            itemListGamepadHints = root.Q<VisualElement>("ItemListGamepadHints");
            equipItemKeyHint = itemListKeyHints.Q<VisualElement>("EquipItemKeyHint");
            equipItemButtonHint = itemListGamepadHints.Q<VisualElement>("EquipItemButtonHint");
            useItemKeyHint = itemListKeyHints.Q<VisualElement>("UseItemKeyHint");
            useItemButtonHint = itemListGamepadHints.Q<VisualElement>("UseItemButtonHint");
        }

        public void ReturnToEquipmentSlots()
        {
            itemListKeyHints.style.display = DisplayStyle.None;
            itemListGamepadHints.style.display = DisplayStyle.None;

            equipmentSlots.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        void ShowEquipmentHint()
        {
            if (Gamepad.current == null) equipItemKeyHint.style.display = DisplayStyle.Flex;
            else equipItemButtonHint.style.display = DisplayStyle.Flex;
        }

        void ShowUseItemHint()
        {
            if (Gamepad.current == null) useItemKeyHint.style.display = DisplayStyle.Flex;
            else useItemButtonHint.style.display = DisplayStyle.Flex;
        }

        public void DrawUI(EquipmentType equipmentType, int slotIndex)
        {
            menuLabel.style.display = DisplayStyle.None;
            equipItemKeyHint.style.display = DisplayStyle.None;
            equipItemButtonHint.style.display = DisplayStyle.None;
            useItemKeyHint.style.display = DisplayStyle.None;
            useItemButtonHint.style.display = DisplayStyle.None;

            if (Gamepad.current == null)
            {
                itemListKeyHints.style.display = DisplayStyle.Flex;
            }
            else
            {
                itemListGamepadHints.style.display = DisplayStyle.Flex;
            }

            if (equipmentType == EquipmentType.WEAPON)
            {
                ShowEquipmentHint();
                PopulateScrollView<Weapon>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.SHIELD)
            {
                ShowEquipmentHint();

                PopulateScrollView<Shield>(false, slotIndex, true);
            }
            else if (equipmentType == EquipmentType.ARROW)
            {
                ShowEquipmentHint();

                PopulateScrollView<Arrow>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.SPELL)
            {
                ShowEquipmentHint();

                PopulateScrollView<Spell>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.HELMET)
            {
                ShowEquipmentHint();

                PopulateScrollView<Helmet>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.ARMOR)
            {
                ShowEquipmentHint();

                PopulateScrollView<Armor>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.GAUNTLET)
            {
                ShowEquipmentHint();

                PopulateScrollView<Gauntlet>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.BOOTS)
            {
                ShowEquipmentHint();

                PopulateScrollView<Legwear>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.ACCESSORIES)
            {
                ShowEquipmentHint();

                PopulateScrollView<Accessory>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.CONSUMABLES)
            {
                ShowEquipmentHint();

                ShowUseItemHint();

                PopulateScrollView<Consumable>(false, slotIndex, false);
            }
            else if (equipmentType == EquipmentType.OTHER_ITEMS)
            {
                ShowUseItemHint();

                PopulateScrollView<Item>(true, slotIndex, false);
            }

            // Delay the focus until the next frame, required as a hack for now
            Invoke(nameof(GiveFocus), 0f);
        }

        bool IsItemEquipped(ItemInstance item, int slotIndex)
        {
            // TODO: Handle this

            if (item is WeaponInstance)
            {
                return equipmentDatabase.rightWeapons[slotIndex].IsEqualTo(item);
            }
            else if (item is ShieldInstance)
            {
                return equipmentDatabase.leftWeapons[slotIndex].IsEqualTo(item);
            }
            /*            else if (item is ArrowInstance)
                        {
                            return equipmentDatabase.arrows[slotIndex] == item;
                        }*/
            else if (item is SpellInstance)
            {
                return equipmentDatabase.spells[slotIndex].IsEqualTo(item);
            }
            else if (item is AccessoryInstance)
            {
                return equipmentDatabase.accessories[slotIndex].IsEqualTo(item);
            }
            /*
            else if (item is ConsumableInstance)
            {
                return equipmentDatabase.consumables[slotIndex].IsEqualTo(item);
            }*/
            else if (item is HelmetInstance)
            {
                return equipmentDatabase.helmet.IsEqualTo(item);
            }
            else if (item is ArmorInstance)
            {
                return equipmentDatabase.armor.IsEqualTo(item);
            }
            else if (item is GauntletInstance)
            {
                return equipmentDatabase.gauntlet.IsEqualTo(item);
            }
            else if (item is LegwearInstance)
            {
                return equipmentDatabase.legwear.IsEqualTo(item);
            }

            return false;
        }

        public bool IsKeyItem(Item item)
        {
            return !(item is Weapon || item is Shield || item is Helmet || item is Armor || item is Gauntlet || item is Legwear
                        || item is Accessory || item is Consumable || item is Spell || item is Arrow);
        }

        public bool ShouldShowItem<T>(ItemInstance itemInstance, int slotIndexToEquip, bool showOnlyKeyItems)
        {
            Item item = itemInstance.GetItem<Item>();

            if (item is not T)
            {
                return false;
            }

            if (showOnlyKeyItems && !IsKeyItem(item))
            {
                return false;
            }

            int equippedSlotIndex = -1;
            /* if (item is Weapon && itemInstance is ShieldInstance shieldInstance)
             {
                 equippedSlotIndex = equipmentDatabase.GetEquippedLeftWeaponSlot(shieldInstance);
             }
             else if (item is Arrow && itemInstance is ArrowInstance arrowInstance)
             {
                 equippedSlotIndex = equipmentDatabase.GetEquippedArrowsSlot(arrowInstance);
             }
             else if (item is Spell && itemInstance is SpellInstance spellInstance)
             {
                 equippedSlotIndex = equipmentDatabase.GetEquippedSpellSlot(spellInstance);
             }
             else if (item is Accessory && itemInstance is AccessoryInstance accessoryInstance)
             {
                 equippedSlotIndex = equipmentDatabase.GetEquippedAccessoriesSlot(accessoryInstance);
             }
             else if (item is Consumable && itemInstance is ConsumableInstance consumableInstance)
             {
                 equippedSlotIndex = equipmentDatabase.GetEquippedConsumablesSlot(consumableInstance);
             }

             if (equippedSlotIndex >= 0 && equippedSlotIndex != slotIndexToEquip)
             {
                 return false;
             }*/

            return true;
        }

        bool ShouldDisplayWeaponInstanceOnItemList(WeaponInstance weaponInstance, bool isShieldSlot, int equippedSlotIndex)
        {
            return false;
            /*
                int equippedPrimarySlot = equipmentDatabase.GetEquippedRightWeaponSlot(weaponInstance);
                int equippedSecondarySlot = equipmentDatabase.GetEquippedLeftWeaponSlot(weaponInstance);
                bool isNotEquipped = equippedSecondarySlot == -1 && equippedPrimarySlot == -1;

                if (isShieldSlot)
                {
                    if (weaponInstance.GetItem<Weapon>().IsRangeWeapon())
                    {
                        return false;
                    }
                    if (weaponInstance.GetItem<Weapon>().IsStaffWeapon())
                    {
                        return false;
                    }

                    if (isNotEquipped)
                    {
                        return true;
                    }

                    return equippedSecondarySlot == equippedSlotIndex;
                }

                if (isNotEquipped)
                {
                    return true;
                }

                return equippedPrimarySlot == equippedSlotIndex;
                */
        }

        void PopulateScrollView<ItemType>(bool showOnlyKeyItems, int slotIndex, bool isShieldSlot) where ItemType : Item
        {
            this.itemsScrollView.Clear();

            // Filter the query in one step
            List<WeaponInstance> allWeapons = inventoryDatabase.ownedItems
                .Where(entry => entry.Key is Weapon)
                .SelectMany(entry => entry.Value.OfType<WeaponInstance>())
                .ToList();

            var query = inventoryDatabase.ownedItems
                .SelectMany(entry => entry.Value.OfType<ItemInstance>())
                .Where(itemInstance =>
                {
                    if (itemInstance.GetItem<Weapon>() is not null)
                    {
                        // If shield slot or weapon slot
                        if (isShieldSlot || itemInstance.GetItem<Item>() is ItemType)
                        {
                            return ShouldDisplayWeaponInstanceOnItemList(itemInstance as WeaponInstance, isShieldSlot, slotIndex);
                        }
                    }

                    return ShouldShowItem<ItemType>(itemInstance, slotIndex, showOnlyKeyItems);
                });

            // Store stackable items directly with counts
            Dictionary<Item, (ItemInstance itemInstance, int count, VisualElement uiElement)> stackableItems = new();
            HashSet<Item> stackableProcessed = new();

            foreach (var itemInstance in query)
            {
                var item = itemInstance.GetItem<Item>();
                bool isStackable = item is Consumable || item is Arrow || showOnlyKeyItems;

                // Count stackable items in one go
                if (isStackable)
                {
                    if (stackableItems.ContainsKey(item))
                    {
                        stackableItems[item] = (stackableItems[item].itemInstance, stackableItems[item].count + 1, stackableItems[item].uiElement);
                        continue; // Skip creating a duplicate UI element for stacked items
                    }
                }

                // Create the UI element only once for stackable items or directly for non-stackable ones
                var instance = itemButtonPrefab.CloneTree();
                PopulateItemUI(instance, itemInstance, slotIndex, isShieldSlot, isStackable ? 0 : 1);

                this.itemsScrollView.Add(instance);

                // Store the stackable UI element for updating later
                if (isStackable)
                {
                    stackableItems[item] = (itemInstance, 1, instance);
                    stackableProcessed.Add(item);
                }
            }

            // Update counts for all stackable items in their respective UI elements
            foreach (var kvp in stackableItems)
            {
                var (itemInstance, count, uiElement) = kvp.Value;
                UpdateStackableItemUI(uiElement, itemInstance.GetItem<Item>(), count);
            }

            Invoke(nameof(GiveFocus), 0f);
        }



        private void PopulateItemUI(VisualElement instance, ItemInstance itemInstance, int slotIndex, bool isShieldSlot, int itemCount)
        {
            var item = itemInstance.GetItem<Item>();
            bool isEquipped = IsItemEquipped(itemInstance, slotIndex, isShieldSlot);

            // Set up item visuals
            instance.Q<VisualElement>("Sprite").style.backgroundImage = new StyleBackground(item.sprite);
            instance.Q<VisualElement>("CardSprite").style.display = item is Card ? DisplayStyle.Flex : DisplayStyle.None;

            var itemName = instance.Q<Label>("ItemName");
            itemName.text = item.GetName();
            if (itemCount > 1) itemName.text += $" ({itemCount})";

            if (isEquipped)
            {
                itemName.text += " " + LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "(Equipped)");
            }

            // Set up equipment color indicator
            var equipmentColorIndicator = GetEquipmentColorIndicator(itemInstance);
            if (equipmentColorIndicator == Color.black)
            {
                instance.Q<VisualElement>("Indicator").style.display = DisplayStyle.None;
            }
            else
            {
                instance.Q<VisualElement>("Indicator").style.backgroundColor = equipmentColorIndicator;
                instance.Q<VisualElement>("Indicator").style.display = DisplayStyle.Flex;
            }

            var btn = instance.Q<Button>("EquipButton");

            UIUtils.SetupButton(btn,
            () =>
            {

                lastScrollElementIndex = this.itemsScrollView.IndexOf(instance);

                soundbank.PlaySound(soundbank.uiEquip);

                HandleButtonClick(itemInstance, isEquipped, slotIndex, isShieldSlot, out bool ignoreRerender);

                if (!ignoreRerender)
                {
                    ReturnToEquipmentSlots();
                }
            },
            () =>
            {
                itemsScrollView.ScrollTo(instance);
                ShowTooltipAndStats(itemInstance, btn);
            },
            () =>
            {
                HideTooltipAndClearStats();
            }, false, soundbank);
        }


        private void ShowTooltipAndStats(ItemInstance itemInstance, Button btn)
        {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.PrepareTooltipForItem(itemInstance);
            itemTooltip.DisplayTooltip(btn);
            playerStatsAndAttributesUI.DrawStats(itemInstance);
        }

        private void HideTooltipAndClearStats()
        {
            itemTooltip.gameObject.SetActive(false);
            playerStatsAndAttributesUI.DrawStats(null);
        }

        bool HandleButtonClick(ItemInstance itemInstance, bool isEquipped, int slotIndex, bool isLeftHand, out bool ignoreRerender)
        {
            ignoreRerender = false;

            Item item = itemInstance.GetItem<Item>();
            return true;
            /*
            if (item is Weapon weapon)
            {
                if (!isEquipped)
                {
                    if (!weapon.AreRequirementsMet(playerManager))
                    {
                        notificationManager.ShowNotification(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Can't equip weapon. Requirements are not met."), notificationManager.systemError);
                        ignoreRerender = true;
                    }
                    else
                    {
                        if (playerManager.statsBonusController.ignoreWeaponRequirements)
                        {
                            playerManager.statsBonusController.SetIgnoreNextWeaponToEquipRequirements(false);
                        }

                        if (isLeftHand)
                        {
                            playerManager.playerWeaponsManager.EquipWeapon(itemInstance as WeaponInstance, slotIndex, false);
                        }
                        else
                        {
                            playerManager.playerWeaponsManager.EquipWeapon(itemInstance as WeaponInstance, slotIndex, true);
                        }
                    }
                }
                else
                {
                    if (isLeftHand)
                    {
                        playerManager.playerWeaponsManager.UnequipWeapon(slotIndex, false);
                    }
                    else
                    {
                        playerManager.playerWeaponsManager.UnequipWeapon(slotIndex, true);
                    }
                }
            }
            else if (itemInstance is HelmetInstance helmetInstance)
            {
                if (!isEquipped)
                {
                    playerManager.characterBaseEquipment.EquipHelmet(helmetInstance);
                }
                else
                {
                    playerManager.characterBaseEquipment.UnequipHelmet();
                }
            }
            else if (itemInstance is ArmorInstance armorInstance)
            {
                if (!isEquipped)
                {
                    playerManager.equipmentGraphicsHandler.EquipArmor(armorInstance);
                }
                else
                {
                    playerManager.equipmentGraphicsHandler.UnequipArmor();
                }
            }
            else if (itemInstance is GauntletInstance gauntletInstance)
            {
                if (!isEquipped)
                {
                    playerManager.equipmentGraphicsHandler.EquipGauntlet(gauntletInstance);
                }
                else
                {
                    playerManager.equipmentGraphicsHandler.UnequipGauntlet();
                }
            }
            else if (itemInstance is LegwearInstance legwearInstance)
            {
                if (!isEquipped)
                {
                    playerManager.equipmentGraphicsHandler.EquipLegwear(legwearInstance);
                }
                else
                {
                    playerManager.equipmentGraphicsHandler.UnequipLegwear();
                }
            }
            else if (itemInstance is AccessoryInstance accessoryInstance)
            {
                if (!isEquipped)
                {
                    playerManager.equipmentGraphicsHandler.EquipAccessory(accessoryInstance, slotIndex);
                }
                else
                {
                    playerManager.equipmentGraphicsHandler.UnequipAccessory(slotIndex);
                }
            }
            else if (itemInstance is ArrowInstance arrowInstance)
            {
                if (!isEquipped)
                {
                    equipmentDatabase.EquipArrow(arrowInstance, slotIndex);
                }
                else
                {
                    equipmentDatabase.UnequipArrow(slotIndex);
                }
            }
            else if (itemInstance is ConsumableInstance consumableInstance)
            {
                if (!isEquipped)
                {
                    equipmentDatabase.EquipConsumable(consumableInstance, slotIndex);
                }
                else
                {
                    equipmentDatabase.UnequipConsumable(slotIndex);
                }
            }
            else if (itemInstance is SpellInstance spellInstance)
            {
                if (!isEquipped)
                {
                    if (!spellInstance.GetItem<Spell>()?.AreRequirementsMet(playerManager.characterBaseStats) ?? false)
                    {
                        notificationManager.ShowNotification(
                            LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Can not equip spell. Requirements not met!"), notificationManager.systemError);
                        ignoreRerender = true;
                    }
                    else
                    {
                        equipmentDatabase.EquipSpell(spellInstance, slotIndex);
                    }
                }
                else
                {
                    equipmentDatabase.UnequipSpell(slotIndex);
                }
            }
*/
            return true;
        }

        bool IsItemEquipped(ItemInstance item, int slotIndex, bool isShieldSlot)
        {
            if (item is WeaponInstance)
            {
                return equipmentDatabase.rightWeapons[slotIndex].IsEqualTo(item) || equipmentDatabase.leftWeapons[slotIndex].IsEqualTo(item);
            }
            /*
            else if (item is ArrowInstance)
            {
                return equipmentDatabase.arrows[slotIndex].IsEqualTo(item);
            }*/
            else if (item is SpellInstance)
            {
                return equipmentDatabase.spells[slotIndex].IsEqualTo(item);
            }
            else if (item is AccessoryInstance)
            {
                return equipmentDatabase.accessories[slotIndex].IsEqualTo(item);
            }
            /*
            else if (item is ConsumableInstance)
            {
                return equipmentDatabase.consumables[slotIndex].IsEqualTo(item);
            }*/
            else if (item is HelmetInstance)
            {
                return equipmentDatabase.helmet.IsEqualTo(item);
            }
            else if (item is ArmorInstance)
            {
                return equipmentDatabase.armor.IsEqualTo(item);
            }
            else if (item is GauntletInstance)
            {
                return equipmentDatabase.gauntlet.IsEqualTo(item);
            }
            else if (item is LegwearInstance)
            {
                return equipmentDatabase.legwear.IsEqualTo(item);
            }

            return false;
        }

        private void UpdateStackableItemUI(VisualElement instance, Item item, int count)
        {
            var itemNameLabel = instance.Q<Label>("ItemName");
            itemNameLabel.text = item.GetName() + $" ({count})";
        }

        void GiveFocus()
        {
            if (lastScrollElementIndex == -1)
            {
                returnButton.Focus();
            }
            else
            {
                UIUtils.ScrollToLastPosition(
                    lastScrollElementIndex,
                    itemsScrollView,
                    () =>
                    {
                        lastScrollElementIndex = -1;
                    }
                );
            }

        }

        public Color GetEquipmentColorIndicator<T>(T item) where T : ItemInstance
        {
            bool shouldReturn = false;
            int value = 0;

            // TODO: Add support for right / left weapons
            if (item is WeaponInstance weapon)
            {
                value = playerManager.characterBaseAttackManager.CompareRightWeapon(weapon);
                shouldReturn = true;
            }
            else if (item is HelmetInstance helmet)
            {
                value = playerManager.characterBaseDefenseManager.CompareHelmet(helmet);
                shouldReturn = true;
            }
            else if (item is ArmorInstance armor)
            {
                value = playerManager.characterBaseDefenseManager.CompareArmor(armor);
                shouldReturn = true;
            }
            else if (item is GauntletInstance gauntlet)
            {
                value = playerManager.characterBaseDefenseManager.CompareGauntlets(gauntlet);
                shouldReturn = true;
            }
            else if (item is LegwearInstance legwear)
            {
                value = playerManager.characterBaseDefenseManager.CompareLegwears(legwear);
                shouldReturn = true;
            }

            if (shouldReturn)
            {
                if (value > 0) return Color.green;
                else if (value == 0) return Color.yellow;
                else if (value < 0) return Color.red;
            }

            return Color.black;
        }
    }
}
