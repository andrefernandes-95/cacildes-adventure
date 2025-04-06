namespace AF.UI.EquipmentMenu
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Localization.Settings;
    using UnityEngine.UIElements;

    public class EquipmentSlots : MonoBehaviour
    {
        [Header("Components")]
        public Soundbank soundbank;

        [Header("UI Documents")]
        public UIDocument uIDocument;
        public VisualElement root;
        public ViewEquipmentMenu viewEquipmentMenu;

        public ItemList itemList;
        public ItemTooltip itemTooltip;

        Button weaponButtonSlot1;
        Button weaponButtonSlot2;
        Button weaponButtonSlot3;

        Button secondaryWeaponButtonSlot1;
        Button secondaryWeaponButtonSlot2;
        Button secondaryWeaponButtonSlot3;

        Button arrowsButtonSlot1;
        Button arrowsButtonSlot2;

        Button spellsButtonSlot1;
        Button spellsButtonSlot2;
        Button spellsButtonSlot3;
        Button spellsButtonSlot4;
        Button spellsButtonSlot5;

        Button helmetButtonSlot;
        Button armorButtonSlot;
        Button gauntletsButtonSlot;
        Button bootsButtonSlot;

        Button accessoryButtonSlot1;
        Button accessoryButtonSlot2;
        Button accessoryButtonSlot3;
        Button accessoryButtonSlot4;

        Button consumableButtonSlot1;
        Button consumableButtonSlot2;
        Button consumableButtonSlot3;
        Button consumableButtonSlot4;
        Button consumableButtonSlot5;
        Button consumableButtonSlot6;
        Button consumableButtonSlot7;
        Button consumableButtonSlot8;
        Button consumableButtonSlot9;
        Button consumableButtonSlot10;

        Button otherItemsButton;


        [Header("Sprites")]
        public Texture2D txt_UnequipedWeapon, txt_UnequipedShield, txt_UnequipedArrow,
            txt_UnequipedSpell, txt_UnequippedHelmet, txt_UnequippedArmor, txt_UnequippedLegwear, txt_UnequippedGauntlets,
            txt_UnequippedAccessory, txt_UnequippedConsumable, txt_OtherItems;

        [Header("Player")]
        public PlayerManager playerManager;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;

        [HideInInspector] public bool shouldRerender = true;

        Button activeButton;

        private void OnEnable()
        {
            if (shouldRerender)
            {
                shouldRerender = false;

                SetupRefs();
            }

            DrawUI();
            root.Q<VisualElement>("EquipmentSlots").style.display = DisplayStyle.Flex;
        }

        private void OnDisable()
        {
            root.Q<VisualElement>("EquipmentSlots").style.display = DisplayStyle.None;
        }

        public void SetupRefs()
        {
            //            activeButton = null;

            root = uIDocument.rootVisualElement;

            weaponButtonSlot1 = root.Q<Button>("WeaponButton_Slot1");
            weaponButtonSlot2 = root.Q<Button>("WeaponButton_Slot2");
            weaponButtonSlot3 = root.Q<Button>("WeaponButton_Slot3");

            secondaryWeaponButtonSlot1 = root.Q<Button>("SecondaryWeaponButton_Slot1");
            secondaryWeaponButtonSlot2 = root.Q<Button>("SecondaryWeaponButton_Slot2");
            secondaryWeaponButtonSlot3 = root.Q<Button>("SecondaryWeaponButton_Slot3");

            arrowsButtonSlot1 = root.Q<Button>("ArrowsButton_Slot1");
            arrowsButtonSlot2 = root.Q<Button>("ArrowsButton_Slot2");

            spellsButtonSlot1 = root.Q<Button>("SpellsButton_Slot1");
            spellsButtonSlot2 = root.Q<Button>("SpellsButton_Slot2");
            spellsButtonSlot3 = root.Q<Button>("SpellsButton_Slot3");
            spellsButtonSlot4 = root.Q<Button>("SpellsButton_Slot4");
            spellsButtonSlot5 = root.Q<Button>("SpellsButton_Slot5");

            helmetButtonSlot = root.Q<Button>("HelmetButton");
            armorButtonSlot = root.Q<Button>("ArmorButton");
            gauntletsButtonSlot = root.Q<Button>("GauntletsButton");
            bootsButtonSlot = root.Q<Button>("BootsButton");

            accessoryButtonSlot1 = root.Q<Button>("AccessoriesButton_Slot1");
            accessoryButtonSlot2 = root.Q<Button>("AccessoriesButton_Slot2");
            accessoryButtonSlot3 = root.Q<Button>("AccessoriesButton_Slot3");
            accessoryButtonSlot4 = root.Q<Button>("AccessoriesButton_Slot4");

            consumableButtonSlot1 = root.Q<Button>("ConsumablesButton_Slot1");
            consumableButtonSlot2 = root.Q<Button>("ConsumablesButton_Slot2");
            consumableButtonSlot3 = root.Q<Button>("ConsumablesButton_Slot3");
            consumableButtonSlot4 = root.Q<Button>("ConsumablesButton_Slot4");
            consumableButtonSlot5 = root.Q<Button>("ConsumablesButton_Slot5");

            consumableButtonSlot6 = root.Q<Button>("ConsumablesButton_Slot6");
            consumableButtonSlot7 = root.Q<Button>("ConsumablesButton_Slot7");
            consumableButtonSlot8 = root.Q<Button>("ConsumablesButton_Slot8");
            consumableButtonSlot9 = root.Q<Button>("ConsumablesButton_Slot9");
            consumableButtonSlot10 = root.Q<Button>("ConsumablesButton_Slot10");

            otherItemsButton = root.Q<Button>("OtherItemsButton");

            AssignWeaponButtonCallbacks();
            AssignShieldButtonCallbacks();
            AssignArrowButtonCallbacks();
            AssignSpellButtonCallbacks();
            AssignArmorButtonCallbacks();
            AssignAccessoryButtonCallbacks();
            AssignConsumableButtonCallbacks();
            AssignOtherItemsButtonCallbacks();
        }

        void AssignWeaponButtonCallbacks()
        {
            Dictionary<Button, Func<WeaponInstance>> buttonDictionary = new()
            {
                { weaponButtonSlot1, () => equipmentDatabase.rightWeapons[0] },
                { weaponButtonSlot2, () => equipmentDatabase.rightWeapons[1] },
                { weaponButtonSlot3, () => equipmentDatabase.rightWeapons[2] },
            };

            int slotIndex = 0;
            foreach (var entry in buttonDictionary)
            {
                int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                UIUtils.SetupButton(entry.Key,
                    () =>
                    {
                        activeButton = entry.Key;

                        SetupEquipmentButton(ItemList.EquipmentType.WEAPON, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Weapons"));
                    },
                    () =>
                    {
                        OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Weapons"), entry.Value());
                    },
                    () =>
                    {
                        OnSlotFocusOut();
                    },
                    false,
                    soundbank);

                slotIndex++;
            }
        }

        void AssignShieldButtonCallbacks()
        {
            Dictionary<Button, Func<WeaponInstance>> buttonDictionary = new()
            {
                { secondaryWeaponButtonSlot1, () => equipmentDatabase.leftWeapons[0] },
                { secondaryWeaponButtonSlot2, () => equipmentDatabase.leftWeapons[1] },
                { secondaryWeaponButtonSlot3, () => equipmentDatabase.leftWeapons[2] },
            };

            int slotIndex = 0;
            foreach (var entry in buttonDictionary)
            {
                int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                UIUtils.SetupButton(entry.Key,
                    () =>
                    {
                        activeButton = entry.Key;

                        SetupEquipmentButton(ItemList.EquipmentType.SHIELD, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Shields"));
                    },
                    () =>
                    {
                        OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Shields"), entry.Value());
                    },
                    () =>
                    {
                        OnSlotFocusOut();
                    },
                    false,
                    soundbank);

                slotIndex++;
            }
        }

        void AssignArrowButtonCallbacks()
        {
            return;
            /*
                Dictionary<Button, Func<ArrowInstance>> buttonDictionary = new()
                {
                    { arrowsButtonSlot1, () => equipmentDatabase.arrows[0] },
                    { arrowsButtonSlot2, () => equipmentDatabase.arrows[1] },
                };

                int slotIndex = 0;
                foreach (var entry in buttonDictionary)
                {
                    int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                    UIUtils.SetupButton(entry.Key,
                        () =>
                        {
                            activeButton = entry.Key;

                            SetupEquipmentButton(ItemList.EquipmentType.ARROW, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Arrows"));
                        },
                        () =>
                        {
                            OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Arrows"), entry.Value());
                        },
                        () =>
                        {
                            OnSlotFocusOut();
                        },
                        false,
                        soundbank);

                    slotIndex++;
                }*/
        }

        void AssignSpellButtonCallbacks()
        {
            Dictionary<Button, Func<SpellInstance>> buttonDictionary = new()
            {
                { spellsButtonSlot1, () => equipmentDatabase.spells[0] },
                { spellsButtonSlot2, () => equipmentDatabase.spells[1] },
                { spellsButtonSlot3, () => equipmentDatabase.spells[2] },
                { spellsButtonSlot4, () => equipmentDatabase.spells[3] },
                { spellsButtonSlot5, () => equipmentDatabase.spells[4] }
            };

            int slotIndex = 0;
            foreach (var entry in buttonDictionary)
            {
                int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                UIUtils.SetupButton(entry.Key,
                    () =>
                    {
                        activeButton = entry.Key;
                        SetupEquipmentButton(ItemList.EquipmentType.SPELL, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Spells"));
                    },
                    () =>
                    {
                        OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Spells"), entry.Value());
                    },
                    () =>
                    {
                        OnSlotFocusOut();
                    },
                    false,
                    soundbank);

                slotIndex++;
            }
        }

        void AssignArmorButtonCallbacks()
        {
            AssignHelmetButtonCallback();
            AssignArmorButtonCallback();
            AssignGauntletsButtonCallback();
            AssignLegwearButtonCallback();
        }


        void AssignHelmetButtonCallback()
        {
            ItemInstance Get() { return equipmentDatabase.helmet; }

            UIUtils.SetupButton(helmetButtonSlot,
            () =>
            {
                activeButton = helmetButtonSlot;

                SetupEquipmentButton(ItemList.EquipmentType.HELMET, 0, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Helmet"));
            },
            () => { OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Helmet"), Get()); },
            OnSlotFocusOut,
            false,
            soundbank);
        }
        void AssignArmorButtonCallback()
        {
            ItemInstance Get() { return equipmentDatabase.armor; }

            UIUtils.SetupButton(armorButtonSlot,
            () =>
            {
                activeButton = armorButtonSlot;

                SetupEquipmentButton(ItemList.EquipmentType.ARMOR, 0, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Armor"));
            },
            () => { OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Armor"), Get()); },
            OnSlotFocusOut,
            false,
            soundbank);
        }
        void AssignGauntletsButtonCallback()
        {
            ItemInstance Get() { return equipmentDatabase.gauntlet; }

            UIUtils.SetupButton(gauntletsButtonSlot,
            () =>
            {
                activeButton = gauntletsButtonSlot;

                SetupEquipmentButton(ItemList.EquipmentType.GAUNTLET, 0, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Gauntlets"));
            },
            () => { OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Gauntlets"), Get()); },
            OnSlotFocusOut,
            false,
            soundbank);
        }
        void AssignLegwearButtonCallback()
        {
            ItemInstance Get() { return equipmentDatabase.legwear; }

            UIUtils.SetupButton(bootsButtonSlot,
            () =>
            {
                activeButton = bootsButtonSlot;

                SetupEquipmentButton(ItemList.EquipmentType.BOOTS, 0, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Boots"));
            },
            () => { OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Boots"), Get()); },
            OnSlotFocusOut,
            false,
            soundbank);
        }

        void AssignAccessoryButtonCallbacks()
        {
            Dictionary<Button, Func<AccessoryInstance>> buttonDictionary = new()
            {
                { accessoryButtonSlot1, () => equipmentDatabase.accessories[0] },
                { accessoryButtonSlot2, () => equipmentDatabase.accessories[1] },
                { accessoryButtonSlot3, () => equipmentDatabase.accessories[2] },
                { accessoryButtonSlot4, () => equipmentDatabase.accessories[3] },
            };

            int slotIndex = 0;
            foreach (var entry in buttonDictionary)
            {
                int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                UIUtils.SetupButton(entry.Key,
                    () =>
                    {
                        activeButton = entry.Key;

                        SetupEquipmentButton(ItemList.EquipmentType.ACCESSORIES, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Accessories"));
                    },
                    () =>
                    {
                        OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Accessories"), entry.Value());
                    },
                    () =>
                    {
                        OnSlotFocusOut();
                    },
                    false,
                    soundbank);

                slotIndex++;
            }
        }

        void AssignConsumableButtonCallbacks()
        {
            Dictionary<Button, Func<Consumable>> buttonDictionary = new()
            {
                { consumableButtonSlot1, () => equipmentDatabase.consumables[0] },
                { consumableButtonSlot2, () => equipmentDatabase.consumables[1] },
                { consumableButtonSlot3, () => equipmentDatabase.consumables[2] },
                { consumableButtonSlot4, () => equipmentDatabase.consumables[3] },
                { consumableButtonSlot5, () => equipmentDatabase.consumables[4] },
                { consumableButtonSlot6, () => equipmentDatabase.consumables[5] },
                { consumableButtonSlot7, () => equipmentDatabase.consumables[6] },
                { consumableButtonSlot8, () => equipmentDatabase.consumables[7] },
                { consumableButtonSlot9, () => equipmentDatabase.consumables[8] },
                { consumableButtonSlot10, () => equipmentDatabase.consumables[9] },
            };

            int slotIndex = 0;
            foreach (var entry in buttonDictionary)
            {
                int localSlotIndex = slotIndex;  // Create a local variable to capture the correct value

                UIUtils.SetupButton(entry.Key,
                    () =>
                    {
                        activeButton = entry.Key;

                        SetupEquipmentButton(ItemList.EquipmentType.CONSUMABLES, localSlotIndex, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Consumables"));
                    },
                    () =>
                    {
                        // OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "Consumables"), entry.Value());
                    },
                    () =>
                    {
                        OnSlotFocusOut();
                    },
                    false,
                    soundbank);

                slotIndex++;
            }
        }

        void AssignOtherItemsButtonCallbacks()
        {
            UIUtils.SetupButton(otherItemsButton,
            () =>
            {
                activeButton = otherItemsButton;
                SetupEquipmentButton(ItemList.EquipmentType.OTHER_ITEMS, 0, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "All Items"));
            },
            () =>
            {
                OnSlotFocus(LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "All Items"), null);
            },
            () =>
            {
                OnSlotFocusOut();
            },
            false,
            soundbank);
        }

        void DrawUI()
        {
            DrawSlotSprites();

            // Delay the focus until the next frame, required as a hack for now
            Invoke(nameof(GiveFocus), 0f);
        }

        void GiveFocus()
        {
            if (activeButton != null)
            {
                activeButton.Focus();
            }
            else
            {
                weaponButtonSlot1.Focus();
            }
        }

        void OnSlotFocus(string activeSlotMenuLabel, ItemInstance itemInstance)
        {

            string displayName = activeSlotMenuLabel;

            if (itemInstance != null && itemInstance.Exists())
            {
                displayName = itemInstance.GetItem<Item>().GetName();

                if (itemInstance is WeaponInstance weaponInstance)
                {
                    displayName += " +" + weaponInstance.level;
                }

                itemTooltip.gameObject.SetActive(true);
                itemTooltip.PrepareTooltipForItem(itemInstance);
            }

            // viewEquipmentMenu.menuFooter.DisplayTooltip(displayName);
        }

        void OnSlotFocusOut()
        {
            // viewEquipmentMenu.menuFooter.HideTooltip();

            itemTooltip.gameObject.SetActive(false);
        }

        void SetupEquipmentButton(ItemList.EquipmentType equipmentType, int slotIndex, string label)
        {
            itemList.gameObject.SetActive(true);
            itemList.DrawUI(equipmentType, slotIndex);

            this.gameObject.SetActive(false);
        }

        void DrawSlotSprites()
        {
            SetBackgroundImage(weaponButtonSlot1, equipmentDatabase.rightWeapons, 0, txt_UnequipedWeapon);
            SetBackgroundImage(weaponButtonSlot2, equipmentDatabase.rightWeapons, 1, txt_UnequipedWeapon);
            SetBackgroundImage(weaponButtonSlot3, equipmentDatabase.rightWeapons, 2, txt_UnequipedWeapon);

            SetShieldSlot(secondaryWeaponButtonSlot1, 0, txt_UnequipedShield);
            SetShieldSlot(secondaryWeaponButtonSlot2, 1, txt_UnequipedShield);
            SetShieldSlot(secondaryWeaponButtonSlot3, 2, txt_UnequipedShield);

            /*
                        SetBackgroundImage(arrowsButtonSlot1, equipmentDatabase.arrows, 0, txt_UnequipedArrow);
                        SetBackgroundImage(arrowsButtonSlot2, equipmentDatabase.arrows, 1, txt_UnequipedArrow);*/

            SetBackgroundImage(spellsButtonSlot1, equipmentDatabase.spells, 0, txt_UnequipedSpell);
            SetBackgroundImage(spellsButtonSlot2, equipmentDatabase.spells, 1, txt_UnequipedSpell);
            SetBackgroundImage(spellsButtonSlot3, equipmentDatabase.spells, 2, txt_UnequipedSpell);
            SetBackgroundImage(spellsButtonSlot4, equipmentDatabase.spells, 3, txt_UnequipedSpell);
            SetBackgroundImage(spellsButtonSlot5, equipmentDatabase.spells, 4, txt_UnequipedSpell);

            SetBackgroundImage(helmetButtonSlot, new ItemInstance[] { equipmentDatabase.helmet }, 0, txt_UnequippedHelmet);
            SetBackgroundImage(armorButtonSlot, new ItemInstance[] { equipmentDatabase.armor }, 0, txt_UnequippedArmor);
            SetBackgroundImage(bootsButtonSlot, new ItemInstance[] { equipmentDatabase.legwear }, 0, txt_UnequippedLegwear);
            SetBackgroundImage(gauntletsButtonSlot, new ItemInstance[] { equipmentDatabase.gauntlet }, 0, txt_UnequippedGauntlets);

            SetBackgroundImage(accessoryButtonSlot1, equipmentDatabase.accessories, 0, txt_UnequippedAccessory);
            SetBackgroundImage(accessoryButtonSlot2, equipmentDatabase.accessories, 1, txt_UnequippedAccessory);
            SetBackgroundImage(accessoryButtonSlot3, equipmentDatabase.accessories, 2, txt_UnequippedAccessory);
            SetBackgroundImage(accessoryButtonSlot4, equipmentDatabase.accessories, 3, txt_UnequippedAccessory);

            /*
                        SetBackgroundImage(consumableButtonSlot1, equipmentDatabase.consumables, 0, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot2, equipmentDatabase.consumables, 1, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot3, equipmentDatabase.consumables, 2, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot4, equipmentDatabase.consumables, 3, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot5, equipmentDatabase.consumables, 4, txt_UnequippedConsumable);

                        SetBackgroundImage(consumableButtonSlot6, equipmentDatabase.consumables, 5, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot7, equipmentDatabase.consumables, 6, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot8, equipmentDatabase.consumables, 7, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot9, equipmentDatabase.consumables, 8, txt_UnequippedConsumable);
                        SetBackgroundImage(consumableButtonSlot10, equipmentDatabase.consumables, 9, txt_UnequippedConsumable);*/
        }

        void SetBackgroundImage(VisualElement button, ItemInstance[] items, int index, Texture2D unequippedTexture)
        {
            if (index < items.Length)
            {
                if (items[index].Exists())
                {
                    button.style.backgroundImage = new StyleBackground(items[index].GetItem<Item>().sprite);
                }
                else
                {
                    button.style.backgroundImage = new StyleBackground(unequippedTexture);
                }
            }
        }

        void SetShieldSlot(VisualElement button, int index, Texture2D unequippedTexture)
        {
            if (index < equipmentDatabase.leftWeapons.Length && equipmentDatabase.leftWeapons[index].Exists())
            {
                button.style.backgroundImage = new StyleBackground(equipmentDatabase.leftWeapons[index].GetItem<Weapon>().sprite);
                return;
            }

            button.style.backgroundImage = new StyleBackground(unequippedTexture);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnUnequip()
        {/*
            Button focusedElement = root.focusController.focusedElement as Button;
            if (focusedElement == null) { return; }
            activeButton = focusedElement;

            // Dictionary to map buttons to unequip actions
            var unequipActions = new Dictionary<Button, System.Action>
    {
        { weaponButtonSlot1, () => playerManager.playerWeaponsManager.UnequipWeapon(0, true) },
        { weaponButtonSlot2, () => playerManager.playerWeaponsManager.UnequipWeapon(1, true) },
        { weaponButtonSlot3, () => playerManager.playerWeaponsManager.UnequipWeapon(2, true) },
        { secondaryWeaponButtonSlot1, () => playerManager.playerWeaponsManager.UnequipWeapon(0, false) },
        { secondaryWeaponButtonSlot2, () => playerManager.playerWeaponsManager.UnequipWeapon(1, false) },
        { secondaryWeaponButtonSlot3, () => playerManager.playerWeaponsManager.UnequipWeapon(2, false) },
        { arrowsButtonSlot1, () => equipmentDatabase.UnequipArrow(0) },
        { arrowsButtonSlot2, () => equipmentDatabase.UnequipArrow(1) },
        { spellsButtonSlot1, () => equipmentDatabase.UnequipSpell(0) },
        { spellsButtonSlot2, () => equipmentDatabase.UnequipSpell(1) },
        { spellsButtonSlot3, () => equipmentDatabase.UnequipSpell(2) },
        { spellsButtonSlot4, () => equipmentDatabase.UnequipSpell(3) },
        { spellsButtonSlot5, () => equipmentDatabase.UnequipSpell(4) },
        { helmetButtonSlot, () => playerManager.equipmentGraphicsHandler.UnequipHelmet() },
        { armorButtonSlot, () => playerManager.equipmentGraphicsHandler.UnequipArmor() },
        { gauntletsButtonSlot, () => playerManager.equipmentGraphicsHandler.UnequipGauntlet() },
        { bootsButtonSlot, () => playerManager.equipmentGraphicsHandler.UnequipLegwear() },
        { accessoryButtonSlot1, () => playerManager.equipmentGraphicsHandler.UnequipAccessory(0) },
        { accessoryButtonSlot2, () => playerManager.equipmentGraphicsHandler.UnequipAccessory(1) },
        { accessoryButtonSlot3, () => playerManager.equipmentGraphicsHandler.UnequipAccessory(2) },
        { accessoryButtonSlot4, () => playerManager.equipmentGraphicsHandler.UnequipAccessory(3) },
        { consumableButtonSlot1, () => equipmentDatabase.UnequipConsumable(0) },
        { consumableButtonSlot2, () => equipmentDatabase.UnequipConsumable(1) },
        { consumableButtonSlot3, () => equipmentDatabase.UnequipConsumable(2) },
        { consumableButtonSlot4, () => equipmentDatabase.UnequipConsumable(3) },
        { consumableButtonSlot5, () => equipmentDatabase.UnequipConsumable(4) },
        { consumableButtonSlot6, () => equipmentDatabase.UnequipConsumable(5) },
        { consumableButtonSlot7, () => equipmentDatabase.UnequipConsumable(6) },
        { consumableButtonSlot8, () => equipmentDatabase.UnequipConsumable(7) },
        { consumableButtonSlot9, () => equipmentDatabase.UnequipConsumable(8) },
        { consumableButtonSlot10, () => equipmentDatabase.UnequipConsumable(9) }
    };
            // Execute the matching unequip action if the button exists in the dictionary
            if (unequipActions.TryGetValue(focusedElement, out var unequipAction))
            {
                unequipAction.Invoke();
                soundbank.PlaySound(soundbank.uiCancel);
                DrawUI();
            }
            */
        }

    }
}
