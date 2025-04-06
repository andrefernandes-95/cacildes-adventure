using System;
using AF.Events;
using AF.Inventory;
using AF.Stats;
using DG.Tweening;
using TigerForge;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF
{

    public class UIDocumentPlayerHUDV2 : MonoBehaviour
    {

        UIDocument uIDocument => GetComponent<UIDocument>();
        public VisualElement root;

        VisualElement healthContainer;
        VisualElement healthFill;
        VisualElement staminaContainer;
        VisualElement staminaFill;
        VisualElement manaContainer;
        VisualElement manaFill;

        [Header("Graphic Settings")]
        public float healthContainerBaseWidth = 180;
        public float staminaContainerBaseWidth = 150;
        public float manaContainerBaseWidth = 150;
        float _containerMultiplierPerLevel = 10f;

        Label quickItemName, arrowsLabel;
        IMGUIContainer shieldBlockedIcon;


        [Header("Components")]
        public StatsBonusController playerStatsBonusController;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;
        public InventoryDatabase inventoryDatabase;
        public PlayerStatsDatabase playerStatsDatabase;
        public QuestsDatabase questsDatabase;
        public GameSettings gameSettings;

        [Header("Unequipped Textures")]
        public Texture2D unequippedSpellSlot;
        public Texture2D unequippedWeaponSlot;
        public Texture2D unequippedConsumableSlot;
        public Texture2D unequippedShieldSlot;
        public Texture2D unequippedArrowSlot;

        [Header("Components")]
        public PlayerManager playerManager;
        public SyntyCharacterModelManager equipmentGraphicsHandler;

        IMGUIContainer spellSlotContainer, consumableSlotContainer, weaponSlotContainer, shieldSlotContainer;

        [Header("Animations")]
        public Vector3 popEffectWhenSwitchingSlots = new Vector3(0.8f, 0.8f, 0.8f);

        VisualElement leftGamepad, alpha1, upGamepad, alpha2, rightGamepad, alpha3, downGamepad, alpha4;
        VisualElement useKeyboard, useGamepad, useXbox;
        VisualElement equipmentContainer;

        VisualElement KeyboardActions, GamepadActions;

        Label currentObjectiveLabel, currentObjectiveValue, combatStanceIndicatorLabel;

        public StarterAssetsInputs starterAssetsInputs;

        [Header("Localization")]
        public LocalizedString oneHandIndicator_LocalizedString;
        public LocalizedString twoHandIndicator_LocalizedString;

        public LocalizedString dodgeLabel; // Dodge
        public LocalizedString jumpLabel; // Jump
        public LocalizedString toggle1Or2Handing; // Toggle 1/2 Handing
        public LocalizedString heavyAttack; // Heavy Attack
        public LocalizedString sprint; // Sprint

        PlayerHealth playerHealth;

        public Color staminaOriginalColor;
        public Color manaOriginalColor;

        public Color highlightColor;

        private void Awake()
        {

            EventManager.StartListening(
                EventMessages.ON_EQUIPMENT_CHANGED,
                UpdateEquipment);

            EventManager.StartListening(
                EventMessages.ON_QUEST_TRACKED,
                UpdateQuestTracking);

            EventManager.StartListening(
                EventMessages.ON_QUESTS_PROGRESS_CHANGED,
                UpdateQuestTracking);

            EventManager.StartListening(EventMessages.ON_USE_CUSTOM_INPUT_CHANGED, UpdateInputsHUD);

            EventManager.StartListening(EventMessages.ON_PLAYER_HUD_VISIBILITY_CHANGED, EvaluatePlayerHUD);

            EventManager.StartListening(EventMessages.ON_TWO_HANDING_CHANGED, UpdateCombatStanceIndicator);
        }

        void EvaluatePlayerHUD()
        {
            if (gameSettings.ShouldShowPlayerHUD())
            {
                ShowControlHints();
            }
            else
            {
                HideControlHints();
            }
        }

        private void OnEnable()
        {
            playerHealth = playerManager.health as PlayerHealth;
            this.root = this.uIDocument.rootVisualElement;

            healthContainer = root.Q<VisualElement>("Health");
            healthFill = root.Q<VisualElement>("HealthFill");
            staminaContainer = root.Q<VisualElement>("Stamina");
            staminaFill = root.Q<VisualElement>("StaminaFill");
            manaContainer = root.Q<VisualElement>("Mana");
            manaFill = root.Q<VisualElement>("ManaFill");

            quickItemName = root.Q<Label>("QuickItemName");
            arrowsLabel = root.Q<Label>("ArrowsLabel");

            spellSlotContainer = root.Q<IMGUIContainer>("SpellSlot");
            consumableSlotContainer = root.Q<IMGUIContainer>("ConsumableSlot");
            weaponSlotContainer = root.Q<IMGUIContainer>("WeaponSlot");
            shieldSlotContainer = root.Q<IMGUIContainer>("ShieldSlot");

            shieldBlockedIcon = shieldSlotContainer.Q<IMGUIContainer>("Blocked");

            leftGamepad = shieldSlotContainer.Q<VisualElement>("Gamepad");
            alpha1 = shieldSlotContainer.Q<VisualElement>("Keyboard");

            upGamepad = spellSlotContainer.Q<VisualElement>("Gamepad");
            alpha2 = spellSlotContainer.Q<VisualElement>("Keyboard");

            rightGamepad = weaponSlotContainer.Q<VisualElement>("Gamepad");
            alpha3 = weaponSlotContainer.Q<VisualElement>("Keyboard");

            downGamepad = consumableSlotContainer.Q<VisualElement>("Gamepad");
            alpha4 = consumableSlotContainer.Q<VisualElement>("Keyboard");

            useKeyboard = consumableSlotContainer.Q<VisualElement>("UseKeyboard");
            useGamepad = consumableSlotContainer.Q<VisualElement>("UseGamepad");
            useXbox = consumableSlotContainer.Q<VisualElement>("UseXbox");

            equipmentContainer = root.Q<VisualElement>("EquipmentContainer");

            currentObjectiveLabel = root.Q<Label>("CurrentObjectiveLabel");
            currentObjectiveValue = root.Q<Label>("CurrentObjectiveValue");
            currentObjectiveLabel.style.display = DisplayStyle.None;
            currentObjectiveValue.text = "";

            KeyboardActions = root.Q<VisualElement>("KeyboardActions");
            GamepadActions = root.Q<VisualElement>("GamepadActions");

            combatStanceIndicatorLabel = root.Q<Label>("CombatStanceIndicator");

            if (playerManager.TryGetThirdPersonController(out ThirdPersonController thirdPersonController) && thirdPersonController.water != null)
            {
                root.Q<VisualElement>("SwimmingIndicator").style.display = DisplayStyle.Flex;
            }
            else
            {
                root.Q<VisualElement>("SwimmingIndicator").style.display = DisplayStyle.None;
            }

            // This seems to run on every frame
            //InputSystem.onDeviceChange += HandleDeviceChangeCallback;

            Load();
        }

        void Load()
        {
            UpdateEquipment();
            UpdateQuestTracking();
            EvaluatePlayerHUD();
            UpdateCombatStanceIndicator();
            UpdateInputsHUD();
        }

        void UpdateCombatStanceIndicator()
        {
            if (equipmentDatabase.isTwoHanding)
            {
                combatStanceIndicatorLabel.text = twoHandIndicator_LocalizedString.GetLocalizedString();
            }
            else
            {
                combatStanceIndicatorLabel.text = oneHandIndicator_LocalizedString.GetLocalizedString();
            }
        }

        /*
                private void OnDisable()
                {
                    InputSystem.onDeviceChange -= HandleDeviceChangeCallback;
                }

                void HandleDeviceChangeCallback(InputDevice device, InputDeviceChange change)
                {
                    HandleDeviceChange();
                }*/

        void HandleDeviceChange()
        {
            KeyboardActions.style.display = Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;
            GamepadActions.style.display = Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
            UpdateEquipment();
        }

        void UpdateInputsHUD()
        {
            if (gameSettings.UseCustomInputs() && !string.IsNullOrEmpty(gameSettings.GetDodgeOverrideBindingPayload()))
            {
                root.Q<Label>("DodgeKeyLabel").text = dodgeLabel.GetLocalizedString() + ": " + starterAssetsInputs.GetCurrentKeyBindingForAction("Dodge");
                root.Q<VisualElement>("DodgeKeyIcon").style.display = DisplayStyle.None;
            }
            else
            {
                root.Q<Label>("DodgeKeyLabel").text = dodgeLabel.GetLocalizedString();
                root.Q<VisualElement>("DodgeKeyIcon").style.display = DisplayStyle.Flex;
            }

            if (gameSettings.UseCustomInputs() && !string.IsNullOrEmpty(gameSettings.GetJumpOverrideBindingPayload()))
            {
                root.Q<Label>("JumpKeyLabel").text = jumpLabel.GetLocalizedString() + ": " + starterAssetsInputs.GetCurrentKeyBindingForAction("Jump");
                root.Q<VisualElement>("JumpKeyIcon").style.display = DisplayStyle.None;
            }
            else
            {
                root.Q<Label>("JumpKeyLabel").text = jumpLabel.GetLocalizedString();
                root.Q<VisualElement>("JumpKeyIcon").style.display = DisplayStyle.Flex;
            }

            if (gameSettings.UseCustomInputs() && !string.IsNullOrEmpty(gameSettings.GetTwoHandModeOverrideBindingPayload()))
            {
                root.Q<Label>("ToggleHandsKeyLabel").text = toggle1Or2Handing.GetLocalizedString() + ": " + starterAssetsInputs.GetCurrentKeyBindingForAction("Tab");
                root.Q<VisualElement>("ToggleHandsKeyIcon").style.display = DisplayStyle.None;
            }
            else
            {
                root.Q<Label>("ToggleHandsKeyLabel").text = toggle1Or2Handing.GetLocalizedString();
                root.Q<VisualElement>("ToggleHandsKeyIcon").style.display = DisplayStyle.Flex;
            }

            if (gameSettings.UseCustomInputs() && !string.IsNullOrEmpty(gameSettings.GetHeavyAttackOverrideBindingPayload()))
            {
                root.Q<Label>("HeavyAttackKeyLabel").text = heavyAttack.GetLocalizedString() + ": " + starterAssetsInputs.GetCurrentKeyBindingForAction("HeavyAttack");
                root.Q<VisualElement>("HeavyAttackKeyIcon").style.display = DisplayStyle.None;
            }
            else
            {
                root.Q<Label>("HeavyAttackKeyLabel").text = heavyAttack.GetLocalizedString();
                root.Q<VisualElement>("HeavyAttackKeyIcon").style.display = DisplayStyle.Flex;
            }

            if (gameSettings.UseCustomInputs() && !string.IsNullOrEmpty(gameSettings.GetSprintOverrideBindingPayload()))
            {
                root.Q<Label>("SprintKeyLabel").text = sprint.GetLocalizedString() + ": " + starterAssetsInputs.GetCurrentKeyBindingForAction("Sprint");
                root.Q<VisualElement>("SprintKeyIcon").style.display = DisplayStyle.None;
            }
            else
            {
                root.Q<Label>("SprintKeyLabel").text = sprint.GetLocalizedString();
                root.Q<VisualElement>("SprintKeyIcon").style.display = DisplayStyle.Flex;
            }


            HandleDeviceChange();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetHUD_RootOpacity(float value)
        {
            root.style.opacity = value;
        }

        public void HideHUD()
        {
            SetHUD_RootOpacity(0);
        }

        public void ShowHUD()
        {
            SetHUD_RootOpacity(1);
        }

        public void ShowControlHints()
        {
            KeyboardActions.style.opacity = 1;
            GamepadActions.style.opacity = 1;
        }

        public void HideControlHints()
        {
            KeyboardActions.style.opacity = 0;
            GamepadActions.style.opacity = 0;
        }

        private void Update()
        {
            healthContainer.style.width = (healthContainerBaseWidth +
                playerManager.characterBaseStats.GetVitality() * _containerMultiplierPerLevel) * (playerHealth.hasHealthCutInHalf ? .5f : 1f);

            staminaContainer.style.width = staminaContainerBaseWidth + ((
                playerManager.characterBaseStats.GetEndurance()) * _containerMultiplierPerLevel);

            manaContainer.style.width = manaContainerBaseWidth + ((
                playerManager.characterBaseStats.GetIntelligence()) * _containerMultiplierPerLevel);

            this.healthFill.style.width = new Length(playerManager.health.GetCurrentHealthPercentage() * ((playerHealth.hasHealthCutInHalf ? .5f : 1f)), LengthUnit.Percent);
            this.staminaFill.style.width = new Length(playerManager.staminaStatManager.GetCurrentStaminaPercentage(), LengthUnit.Percent);
            this.manaFill.style.width = new Length(playerManager.manaManager.GetCurrentManaPercentage(), LengthUnit.Percent);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ShowEquipment()
        {
            equipmentContainer.visible = true;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void HideEquipment()
        {
            equipmentContainer.visible = false;
        }

        public void UpdateEquipment()
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            quickItemName.text = "";
            arrowsLabel.text = "";


            alpha1.style.display = Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;
            alpha2.style.display = Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;
            alpha3.style.display = Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;
            alpha4.style.display = Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;

            leftGamepad.style.display = Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
            upGamepad.style.display = Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
            rightGamepad.style.display = Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
            downGamepad.style.display = Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;

            /*            useGamepad.style.display = equipmentDatabase.GetCurrentConsumable() != null && Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
                        useXbox.style.display = equipmentDatabase.GetCurrentConsumable() != null && Gamepad.current != null ? DisplayStyle.Flex : DisplayStyle.None;
                        useKeyboard.style.display = equipmentDatabase.GetCurrentConsumable() != null && Gamepad.current == null ? DisplayStyle.Flex : DisplayStyle.None;

                        if (equipmentDatabase.IsBowEquipped())
                        {
                            arrowsLabel.text = equipmentDatabase.GetCurrentArrow().Exists()
                                ? equipmentDatabase.GetCurrentArrow().GetItem<Arrow>().GetName() + " (" + inventoryDatabase.GetItemAmount(equipmentDatabase.GetCurrentArrow().GetItem<Arrow>()) + ")"
                                : "";

                            spellSlotContainer.style.backgroundImage = equipmentDatabase.GetCurrentArrow().Exists()
                                ? new StyleBackground(equipmentDatabase.GetCurrentArrow().GetItem<Arrow>().sprite)
                                : new StyleBackground(unequippedArrowSlot);
                        }
                        else
                        {
                            spellSlotContainer.style.backgroundImage = equipmentDatabase.GetCurrentSpell().Exists()
                                ? new StyleBackground(equipmentDatabase.GetCurrentSpell().GetItem<Spell>().sprite)
                                : new StyleBackground(unequippedSpellSlot);
                        }


                        shieldSlotContainer.style.backgroundImage = equipmentDatabase.GetCurrentLeftWeapon().Exists()
                            ? new StyleBackground(equipmentDatabase.GetCurrentLeftWeapon().GetItem<Weapon>().sprite)
                            : new StyleBackground(unequippedShieldSlot);

                        shieldBlockedIcon.style.display = equipmentDatabase.IsBowEquipped() || equipmentDatabase.IsStaffEquipped()
                            ? DisplayStyle.Flex
                            : DisplayStyle.None;

                        weaponSlotContainer.style.backgroundImage = equipmentDatabase.GetCurrentRightWeapon().Exists()
                            ? new StyleBackground(equipmentDatabase.GetCurrentRightWeapon().GetItem<Weapon>().sprite)
                            : new StyleBackground(unequippedWeaponSlot);

                        quickItemName.text = equipmentDatabase.GetCurrentConsumable().Exists()
                            ? equipmentDatabase.GetCurrentConsumable().GetItem<Consumable>().GetName() + $" ({inventoryDatabase.GetItemAmount(equipmentDatabase.GetCurrentConsumable().GetItem<Consumable>())})"
                            : "";

                        if (equipmentDatabase.GetCurrentConsumable() is Card)
                        {
                            consumableSlotContainer.style.height = 55;
                            consumableSlotContainer.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                            consumableSlotContainer.style.borderTopWidth = 0;
                            consumableSlotContainer.style.borderBottomWidth = 0;
                            consumableSlotContainer.style.borderLeftWidth = 0;
                            consumableSlotContainer.style.borderRightWidth = 0;

                        }
                        else
                        {
                            consumableSlotContainer.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
                            consumableSlotContainer.style.height = 45;
                            consumableSlotContainer.style.borderTopWidth = new StyleFloat(1);
                            consumableSlotContainer.style.borderBottomWidth = new StyleFloat(1);
                            consumableSlotContainer.style.borderLeftWidth = new StyleFloat(1);
                            consumableSlotContainer.style.borderRightWidth = new StyleFloat(1);
                        }

                        consumableSlotContainer.style.backgroundImage = equipmentDatabase.GetCurrentConsumable().Exists()
                            ? new StyleBackground(equipmentDatabase.GetCurrentConsumable().GetItem<Consumable>().sprite)
                            : new StyleBackground(unequippedConsumableSlot);*/
        }

        public void OnSwitchWeapon()
        {
            UIUtils.PlayPopAnimation(weaponSlotContainer, popEffectWhenSwitchingSlots);
            UpdateEquipment();
        }
        public void OnSwitchShield()
        {
            UIUtils.PlayPopAnimation(shieldSlotContainer, popEffectWhenSwitchingSlots);
            UpdateEquipment();
        }
        public void OnSwitchConsumable()
        {
            UIUtils.PlayPopAnimation(consumableSlotContainer, popEffectWhenSwitchingSlots);
            UpdateEquipment();
        }
        public void OnSwitchSpell()
        {
            UIUtils.PlayPopAnimation(spellSlotContainer, popEffectWhenSwitchingSlots);
            UpdateEquipment();
        }

        public bool IsEquipmentDisplayed()
        {
            if (!root.visible)
            {
                return false;
            }

            return equipmentContainer.visible;
        }

        void UpdateQuestTracking()
        {
            currentObjectiveLabel.style.display = DisplayStyle.None;
            currentObjectiveValue.text = "";

            string currentQuestObjective = questsDatabase.GetCurrentTrackedQuestObjective();

            if (!string.IsNullOrEmpty(currentQuestObjective))
            {
                currentObjectiveValue.text = currentQuestObjective;
                currentObjectiveLabel.style.display = DisplayStyle.Flex;
            }
        }

        public void DisplayInsufficientStamina()
        {
            DisplayInsufficientBarBackgroundColor(staminaOriginalColor, staminaFill, staminaContainer);
        }

        public void DisplayInsufficientMana()
        {
            DisplayInsufficientBarBackgroundColor(manaOriginalColor, manaFill, manaContainer);
        }

        void DisplayInsufficientBarBackgroundColor(Color originalColor, VisualElement target, VisualElement targetContainer)
        {
            Color blinkColor = Color.red; // Change to Color.grey if needed

            // Sequence for the blink effect
            Sequence blinkSequence = DOTween.Sequence();
            blinkSequence.Append(
                DOTween.To(() => (Color)target.style.backgroundColor.value,
                           x => target.style.backgroundColor = new StyleColor(x),
                           blinkColor, 0.5f)
                       .SetEase(Ease.InOutFlash))
                       .OnComplete(() =>
                       {
                           target.style.backgroundColor = originalColor;
                       });
        }

        public enum ControlKey
        {
            None,
            Move,
            Interact,
            Sprint,
            Jump,
            Dodge,
            ToggleHands,
            Attack,
            BlockParryAim,
            LockOn,
            HeavyAttack,
            MainMenu,
        }

        public void HighlightKey(ControlKey controlKey)
        {
            DisableHighlights();
            VisualElement target;

            if (Gamepad.current != null)
            {
                target = GamepadActions.Q(controlKey.ToString());
            }
            else
            {
                target = KeyboardActions.Q(controlKey.ToString());
            }
            target.style.backgroundColor = highlightColor;
            target.style.paddingBottom = 10;
            target.style.paddingLeft = 10;
            target.style.paddingTop = 10;
            target.style.paddingRight = 10;
        }

        public void DisableHighlights()
        {
            foreach (ControlKey key in Enum.GetValues(typeof(ControlKey)))
            {
                VisualElement keyboardTarget = KeyboardActions.Q(key.ToString());
                if (keyboardTarget != null)
                {
                    keyboardTarget.style.backgroundColor = new Color(255, 255, 255, 0);
                    keyboardTarget.style.paddingBottom = 1;
                    keyboardTarget.style.paddingLeft = 1;
                    keyboardTarget.style.paddingTop = 1;
                    keyboardTarget.style.paddingRight = 1;
                }
                VisualElement gamepadTarget = GamepadActions.Q(key.ToString());
                if (gamepadTarget != null)
                {
                    gamepadTarget.style.backgroundColor = new Color(255, 255, 255, 0);
                    gamepadTarget.style.paddingBottom = 1;
                    gamepadTarget.style.paddingLeft = 1;
                    gamepadTarget.style.paddingTop = 1;
                    gamepadTarget.style.paddingRight = 1;
                }
            }
        }

    }
}
