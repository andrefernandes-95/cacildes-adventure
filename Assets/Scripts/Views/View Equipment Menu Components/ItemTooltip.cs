namespace AF
{
    using System;
    using AF.Health;
    using UnityEditor.SearchService;
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.UIElements;

    public class ItemTooltip : MonoBehaviour
    {
        public PlayerManager playerManager;

        public const string TOOLTIP = "ItemTooltip";
        VisualElement tooltip;
        VisualElement itemInfo;
        VisualElement tooltipItemSprite;
        Label tooltipItemDescription;

        VisualElement tooltipEffectsContainer;

        public VisualTreeAsset itemEffectTooltipEntry;

        [Header("Components")]
        public AttackStatManager attackStatManager;
        public RecipesDatabase recipesDatabase;

        [Header("UI Documents")]
        public UIDocument uIDocument;
        public VisualElement root;

        [HideInInspector] public bool shouldRerender = true;

        [Header("Colors")]
        public Color fire = new Color(255, 102, 42);
        public Color frost = new Color(96, 202, 255);
        public Color lightning = new Color(255, 239, 95);
        public Color magic = new Color(241, 96, 255);
        public Color darkness = new Color(92, 76, 248);
        public Color water = new Color(92, 76, 248);
        public Color requirementsNotMet;


        public Texture2D weaponPhysicalAttackSprite, weaponScalingSprite, weightPenaltySprite, holyWeaponSprite,
        fireSprite, frostSprite, lightningSprite, magicSprite, darknessSprite, bluntSprite, pierceSprite, slashSprite,
        statusEffectsSprite, defenseAbsorptionSprite, poiseSprite, postureSprite, goldCoinSprite, reputationSprite, barterSprite,
        vitalitySprite, enduranceSprite, intelligenceSprite, strengthSprite, dexteritySprite, blacksmithSprite,
        pushForceSprite, heavyAttackSprite, staminaCostSprite, bossTokenSprite, replenishableSprite, spellCastSprite,
        upgradeMaterialSprite, craftingMaterialSprite, projectileSprite, requirementsSprite, waterSprite, cardSprite;

        [Header("Localization")]

        // +{0}% Equip Load
        public LocalizedString equipLoadTooltip_Label;
        // +{0} Poise Points
        public LocalizedString poiseTooltip_Label;
        // +{0} Posture Points
        public LocalizedString postureTooltip_Label;

        // +{0}% gold found on enemies
        public LocalizedString goldFoundOnEnemiesTooltip_Label;

        /*
        +{0} Final Damage\n\n
        Explanation: \n
        +{1} Weapon Base Damage\n
        +{2} ATK [STR Scaling: {3}]\n
        +{4} ATK [DEX Scaling: {5}]\n
        +{6} ATK [INT Scaling: {7}]\n
        */
        public LocalizedString damageExplanationLabel;

        // Holy Weapon
        public LocalizedString holyWeaponLabel;
        // +{0} Fire ATK
        public LocalizedString fireAttackLabel;
        public LocalizedString frostAttackLabel;
        public LocalizedString lightningAttackLabel;
        public LocalizedString magicAttackLabel;
        public LocalizedString darknessAttackLabel;
        public LocalizedString waterAttackLabel;

        // "Damage Type: Blunt"
        public LocalizedString damageTypeBluntLabel;
        public LocalizedString damageTypePierceLabel;
        public LocalizedString damageTypeSlashLabel;

        // "+{0} Push Force"
        public LocalizedString pushForceLabel;
        // "+{0} Posture Damage"
        public LocalizedString postureDamageLabel;

        // "+{0} Heavy Attack Bonus"
        public LocalizedString heavyAttackBonusLabel;
        // "{0} Light ATK Stamina Cost {1} Heavy ATK Stamina Cost"
        public LocalizedString staminaCostLabel;

        // "Ignores enemy shields"
        public LocalizedString ignoresEnemyShields;

        //"Can not be parried"
        public LocalizedString canNotBeParried;

        // "%{0} Physical Damage Absorption When Blocking"
        public LocalizedString physicalDamageAbsorptionWhenBlocking;

        //  "Double coins per enemy kill"
        public LocalizedString doubleCoinsPerEnemyKill;

        //+{0}HP restored with each hit
        public LocalizedString hpRestoredWithEachHit;

        //"{0} Stamina Cost Per Block"
        public LocalizedString staminaCostPerBlock;

        // "%{0} Fire Damage Absorption"
        public LocalizedString fireDMGAbsorption;
        public LocalizedString frostDMGAbsorption;
        public LocalizedString lightningDMGAbsorption;
        public LocalizedString magicDMGAbsorption;
        public LocalizedString darknessDMGAbsorption;
        public LocalizedString waterDMGAbsorption;
        // "{0}% Posture Damage Absorption"
        public LocalizedString postureDamageAbsorptionLabel;
        // "{0}% Slash Damage Absorption"
        public LocalizedString slashDamageAbsorptionLabel;

        // "{0}% Pierce Damage Absorption"
        public LocalizedString pierceDamageAbsorptionLabel;

        // "{0}% Blunt Damage Absorption"
        public LocalizedString bluntDamageAbsorptionLabel;

        // "{0} Physical DMG dealt to enemies per block"
        public LocalizedString physicalDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString fireDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString frostDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString lightningDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString magicDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString darknessDmgDealtToEnemiesPerBlockLabel;
        public LocalizedString waterDmgDealtToEnemiesPerBlockLabel;
        // "+{0} Parry Window Duration Bonus"
        public LocalizedString parryWindowDurationBonusLabel;
        // "+{0} Posture DMG per Parry"
        public LocalizedString postureDamagePerParryLabel;
        // "+{0} Vitality"
        public LocalizedString vitalityBonus;
        public LocalizedString enduranceBonus;
        public LocalizedString intelligenceBonus;
        // "+{0}% Stamina Regen. Speed Bonus"
        public LocalizedString staminaRegenSpeedBonus;
        // $"+{0} Physical Defense"
        public LocalizedString physicalDefenseBonus;
        // $"+{0} Fire Defense"
        public LocalizedString fireDefenseBonus;
        public LocalizedString frostDefenseBonus;
        public LocalizedString lightningDefenseBonus;
        public LocalizedString magicDefenseBonus;
        public LocalizedString darknessDefenseBonus;
        public LocalizedString waterDefenseBonus;
        // "+{0} Strength"
        public LocalizedString strengthBonus;
        public LocalizedString dexterityBonus;
        // "{0} Reputation"
        public LocalizedString reputationBonus;
        // "+{0}% Better Prices"
        public LocalizedString betterPrices;

        // "{0} Physical DMG dealt to attacking enemies"
        public LocalizedString physicalDamageDealtToAttackingEnemies;
        public LocalizedString fireDamageDealtToAttackingEnemies;
        public LocalizedString frostDamageDealtToAttackingEnemies;
        public LocalizedString lightningDamageDealtToAttackingEnemies;
        public LocalizedString magicDamageDealtToAttackingEnemies;
        public LocalizedString darknessDamageDealtToAttackingEnemies;

        // "x{0}% damage on projectiles"
        public LocalizedString damageOnProjectilesBonus;

        // "+{0} Health Points"
        public LocalizedString healthPoints;
        public LocalizedString manaPoints;
        public LocalizedString staminaPoints;
        // "+{0} Physical Attack Damage"
        public LocalizedString physicalAttackDamage;
        public LocalizedString jumpAttackDamage;
        // "Attack increases with lower health"
        public LocalizedString attackIncreasesWithLowerHealth;
        // "Attack decreases with lower reputation"
        public LocalizedString attackIncreasesWithLowerReputation;
        // "+{0}% Spell Damage"
        public LocalizedString moreSpellDamage;
        // "Chance to receive double coins from fallen enemies"
        public LocalizedString chanceToDoubleCoinsFromFallenEnemies;
        // "+{0} Posture Decrease Rate Bonus"
        public LocalizedString postureDecreaseRateBonus;

        // "+{0} Backstab Angle Bonus"
        public LocalizedString backStabAngleBonus;
        // "{0} Mana Points required to cast"
        public LocalizedString manaPointsRequiredToCast;

        // "Crafting material (Use in a alchemy table)"
        public LocalizedString craftingMaterialLabel;

        //  "Weapon upgrade material (Give to a blacksmith)"
        public LocalizedString upgradeMaterialLabel;

        //  "Boss token. Someone might be interested in this item."
        public LocalizedString bossTokenLabel;

        // Consume to receive ${0} coins
        public LocalizedString consumeToReceive;

        // Item usage replenishes when resting at a bonfire
        public LocalizedString itemUsageReplenishesWhenRestingAtABonfire;

        // Attack bonus when two handing
        public LocalizedString multiplierWhenTwoHanding;
        public LocalizedString lostUponUse;


        private void OnEnable()
        {
            if (shouldRerender)
            {
                shouldRerender = false;

                SetupRefs();
            }

            tooltip.style.display = DisplayStyle.Flex;
        }

        private void OnDisable()
        {
            tooltip.style.display = DisplayStyle.None;
        }

        public void SetupRefs()
        {
            root = uIDocument.rootVisualElement;

            tooltip = root.Q<VisualElement>(TOOLTIP);
            itemInfo = tooltip.Q<VisualElement>("ItemInfo");
            tooltipItemSprite = root.Q<VisualElement>("ItemTooltipContainer").Q<VisualElement>("ItemSprite");
            tooltipItemDescription = root.Q<VisualElement>("ItemTooltipContainer").Q<Label>();
            tooltipEffectsContainer = tooltip.Q<VisualElement>("ItemAttributes");
        }

        public void PrepareTooltipForItem(ItemInstance itemInstance)
        {
            enabled = true;

            Item item = itemInstance.GetItem<Item>();

            tooltipEffectsContainer.Clear();
            tooltipItemSprite.style.backgroundImage = new StyleBackground(item.sprite);


            if (item is Card)
            {
                tooltipItemSprite.style.width = new StyleLength(100);
                tooltipItemSprite.style.height = new StyleLength(120);
                tooltipItemSprite.style.borderBottomWidth = 0;
                tooltipItemSprite.style.borderTopWidth = 0;
                tooltipItemSprite.style.borderLeftWidth = 0;
                tooltipItemSprite.style.borderRightWidth = 0;
                tooltipItemSprite.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                tooltip.style.width = new StyleLength(400);
            }
            else
            {
                tooltipItemSprite.style.width = new StyleLength(60);
                tooltipItemSprite.style.height = new StyleLength(60);
                tooltipItemSprite.style.borderBottomWidth = new StyleFloat(1);
                tooltipItemSprite.style.borderTopWidth = new StyleFloat(1);
                tooltipItemSprite.style.borderLeftWidth = new StyleFloat(1);
                tooltipItemSprite.style.borderRightWidth = new StyleFloat(1);
                tooltipItemSprite.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
                tooltip.style.width = new StyleLength(300);
            }

            string itemName = item.GetName().ToUpper();

            tooltipItemDescription.text = itemName + " \n" + '"' + item.GetDescription() + '"';

            if (itemInstance is WeaponInstance weaponInstance)
            {
                itemName += " +" + weaponInstance.level;

                DrawWeaponEffects(weaponInstance);
            }
            else if (itemInstance is ShieldInstance shieldInstance)
            {
                DrawShield(shieldInstance);
            }
            else if (itemInstance is ArmorBaseInstance armorBaseInstance)
            {
                DrawArmorBase(armorBaseInstance);

                if (itemInstance is AccessoryInstance accessoryInstance)
                {
                    DrawAccessory(accessoryInstance);
                }
            }
            else if (itemInstance is ConsumableInstance consumableInstance)
            {
                DrawConsumable(consumableInstance);
            }
            else if (itemInstance is ArrowInstance arrowInstance)
            {
                DrawArrow(arrowInstance);
            }
            else if (itemInstance is SpellInstance spellInstance)
            {
                DrawSpell(spellInstance);
            }
            else if (itemInstance is UpgradeMaterialInstance upgradeMaterialInstance)
            {
                DrawUpgradeMaterial(upgradeMaterialInstance);
            }
            else if (itemInstance is CraftingMaterialInstance craftingMaterialInstance)
            {
                DrawCraftingMaterial(craftingMaterialInstance);
            }
        }

        public void DisplayTooltip(Button parentButton)
        {
            // Get the button's position and size in screen space
            float buttonY = parentButton.worldBound.y;
            float buttonWidth = parentButton.resolvedStyle.width;

            // Get the tooltip's size
            float tooltipSize = tooltip.resolvedStyle.height;

            // Calculate the target position for the tooltip
            Vector2 tooltipPosition = new Vector2(buttonWidth, buttonY / 2);

            // Check if the tooltip would exceed the screen height
            float screenHeight = root.resolvedStyle.height;
            if (tooltipPosition.y + tooltipSize > screenHeight)
            {
                // Adjust the position to be above the button if it would be outside the screen
                float tooltipOffset = tooltipSize;
                tooltipPosition.y = Mathf.Max(buttonY - tooltipOffset, 0f);
            }

            // Position the tooltip
            tooltip.style.display = DisplayStyle.Flex;
        }

        void CreateEquipLoadTooltip(float speedPenalty)
        {
            if (speedPenalty <= 0)
            {
                return;
            }

            CreateTooltip(
                weightPenaltySprite,
                Color.white,
                String.Format(
                    equipLoadTooltip_Label.GetLocalizedString(),
                    Math.Round(speedPenalty * 100, 2))
            );
        }

        void CreatePoiseTooltip(int poiseBonus)
        {
            if (poiseBonus <= 0)
            {
                return;
            }

            CreateTooltip(
                poiseSprite,
                Color.white,
                String.Format(poiseTooltip_Label.GetLocalizedString(), poiseBonus));
        }
        void CreatePostureTooltip(int postureBonus)
        {
            if (postureBonus <= 0)
            {
                return;
            }

            CreateTooltip(
                postureSprite,
                Color.white,
                String.Format(postureTooltip_Label.GetLocalizedString(), postureBonus));
        }

        void CreateAdditionalGoldTooltip(float additionalCoinPercentage)
        {
            if (additionalCoinPercentage <= 0)
            {
                return;
            }

            CreateTooltip(
                goldCoinSprite,
                Color.white,
                String.Format(goldFoundOnEnemiesTooltip_Label.GetLocalizedString(), additionalCoinPercentage));
        }

        void CreateStatTooltip(int statBonus, string statName, Texture2D statSprite)
        {
            if (statBonus != 0)
            {
                CreateTooltip(statSprite, Color.white, String.Format(statName, statBonus));
            }
        }

        void DrawElementAttackTooltips(
            int fireAttack, int frostAttack, int lightningAttack, int magicAttack, int darknessAttack, int waterAttack
        )
        {
            if (fireAttack > 0)
            {
                CreateTooltip(
                    fireSprite,
                    fire,
                    String.Format(
                        fireAttackLabel.GetLocalizedString(),
                        fireAttack));
            }

            if (frostAttack > 0)
            {
                CreateTooltip(
                    frostSprite,
                    frost,
                    String.Format(
                        frostAttackLabel.GetLocalizedString(),
                        frostAttack));
            }

            if (lightningAttack > 0)
            {
                CreateTooltip(
                lightningSprite,
                lightning,
                String.Format(
                    lightningAttackLabel.GetLocalizedString(),
                    lightningAttack));
            }

            if (magicAttack > 0)
            {
                CreateTooltip(
                magicSprite,
                magic,
                String.Format(
                    magicAttackLabel.GetLocalizedString(),
                    magicAttack));
            }

            if (darknessAttack > 0)
            {
                CreateTooltip(
                darknessSprite,
                darkness,
                String.Format(
                    darknessAttackLabel.GetLocalizedString(),
                    darknessAttack));
            }

            if (waterAttack > 0)
            {
                CreateTooltip(
                waterSprite,
                water,
                String.Format(
                    waterAttackLabel.GetLocalizedString(),
                    waterAttack));
            }
        }

        void DrawDamageTooltips(Damage damage)
        {
            if (damage.weaponAttackType == WeaponAttackType.Blunt)
            {
                CreateTooltip(bluntSprite, Color.white, damageTypeBluntLabel.GetLocalizedString());
            }
            if (damage.weaponAttackType == WeaponAttackType.Pierce)
            {
                CreateTooltip(pierceSprite, Color.white, damageTypePierceLabel.GetLocalizedString());
            }
            if (damage.weaponAttackType == WeaponAttackType.Slash)
            {
                CreateTooltip(slashSprite, Color.white, damageTypeSlashLabel.GetLocalizedString());
            }

            if (damage.pushForce > 0)
            {
                CreateTooltip(pushForceSprite, Color.white, String.Format(
                    pushForceLabel.GetLocalizedString(), damage.pushForce));
            }

            if (damage.postureDamage > 0)
            {
                CreateTooltip(postureSprite, Color.white, String.Format(
                    postureDamageLabel.GetLocalizedString(), damage.postureDamage));
            }

            if (damage.ignoreBlocking)
            {
                CreateTooltip(defenseAbsorptionSprite, Color.white, ignoresEnemyShields.GetLocalizedString());
            }

            if (damage.canNotBeParried)
            {
                CreateTooltip(defenseAbsorptionSprite, Color.white, canNotBeParried.GetLocalizedString());
            }
        }

        void DrawWeaponEffects(WeaponInstance weaponInstance)
        {
            Weapon weapon = weaponInstance.GetItem<Weapon>();

            if (weapon.HasRequirements())
            {
                CreateTooltip(
                    requirementsSprite,
                    weapon.AreRequirementsMet(playerManager) ? Color.white : requirementsNotMet,
                    weapon.DrawRequirements(playerManager));
            }

            var currentWeaponDamage = weapon.GetCurrentDamage(playerManager, weaponInstance.level);

            int playerStrength = playerManager.characterBaseStats.GetStrength();
            int playerDexterity = playerManager.characterBaseStats.GetDexterity();
            int playerIntelligence = playerManager.characterBaseStats.GetIntelligence();

            string damageExplanation = String.Format(
                damageExplanationLabel.GetLocalizedString(),
                currentWeaponDamage.physical,
                currentWeaponDamage.physical,
                weapon.GetStrengthBonusFromWeapon(playerManager),
                weapon.strengthScaling,
                weapon.GetDexterityBonusFromWeapon(playerManager),
                weapon.dexterityScaling,
                weapon.GetIntelligenceBonusFromWeapon(playerManager),
                weapon.intelligenceScaling
            );

            CreateTooltip(weaponPhysicalAttackSprite, Color.white, damageExplanation);

            CreateEquipLoadTooltip(weapon.speedPenalty);

            if (weapon.isHolyWeapon)
            {
                CreateTooltip(holyWeaponSprite, Color.white, holyWeaponLabel.GetLocalizedString());
            }

            DrawElementAttackTooltips(
                currentWeaponDamage.fire, currentWeaponDamage.frost, currentWeaponDamage.lightning, currentWeaponDamage.magic,
                currentWeaponDamage.darkness, currentWeaponDamage.water
            );

            DrawDamageTooltips(currentWeaponDamage);

            if (currentWeaponDamage.statusEffects != null && currentWeaponDamage.statusEffects.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, weapon.GetFormattedStatusDamages());
            }

            CreateTooltip(
                staminaCostSprite,
                Color.white,
                String.Format(staminaCostLabel.GetLocalizedString(), weapon.lightAttackStaminaCost, weapon.heavyAttackStaminaCost));

            /*
                        if (weapon.canBeUpgraded && CraftingUtils.CanBeUpgradedFurther(weaponInstance))
                        {
                            CreateTooltip(blacksmithSprite, Color.white, CraftingUtils.GetMaterialCostForNextLevel(weaponInstance));
                        }*/

            if (weapon.blockAbsorption != 1)
            {
                CreateTooltip(defenseAbsorptionSprite, Color.white,
                    String.Format(
                        physicalDamageAbsorptionWhenBlocking.GetLocalizedString(),
                        100 - (weapon.blockAbsorption * 100)));
            }

            /*if (weapon.coinMultiplierPerFallenEnemy != 1f)
            {
                CreateTooltip(goldCoinSprite, Color.white, doubleCoinsPerEnemyKill.GetLocalizedString());
            }*/

            if (weapon.healthRestoredWithEachHit > 0)
            {
                CreateTooltip(vitalitySprite, Color.white, String.Format(
                    hpRestoredWithEachHit.GetLocalizedString(),
                    weapon.healthRestoredWithEachHit));
            }
        }

        void DrawShield(ShieldInstance shieldInstance)
        {
            Shield shield = shieldInstance.GetItem<Shield>();

            if (shield.blockStaminaCost != 1)
            {
                CreateTooltip(
                    staminaCostSprite,
                    Color.white,
                    String.Format(staminaCostPerBlock.GetLocalizedString(), shield.blockStaminaCost));
            }

            if (shield.physicalAbsorption != 1)
            {
                CreateTooltip(
                    defenseAbsorptionSprite,
                    Color.white,
                    String.Format(physicalDamageAbsorptionWhenBlocking.GetLocalizedString(), 100 - (shield.physicalAbsorption * 100)));
            }

            if (shield.fireAbsorption != 1)
            {
                CreateTooltip(
                    fireSprite,
                    fire,
                    String.Format(fireDMGAbsorption.GetLocalizedString(), 100 - (shield.fireAbsorption * 100)));
            }
            if (shield.frostAbsorption != 1)
            {
                CreateTooltip(
                    frostSprite,
                    frost,
                    String.Format(frostDMGAbsorption.GetLocalizedString(), 100 - (shield.frostAbsorption * 100)));
            }
            if (shield.lightiningAbsorption != 1)
            {
                CreateTooltip(
                    lightningSprite,
                    lightning,
                    String.Format(lightningDMGAbsorption.GetLocalizedString(), 100 - (shield.lightiningAbsorption * 100)));
            }
            if (shield.magicAbsorption != 1)
            {
                CreateTooltip(
                    magicSprite,
                    magic,
                    String.Format(
                        lightningDMGAbsorption.GetLocalizedString(),
                         100 - (shield.magicAbsorption * 100)));
            }
            if (shield.darknessAbsorption != 1)
            {
                CreateTooltip(
                    darknessSprite,
                    darkness,
                    String.Format(
                        darknessDMGAbsorption.GetLocalizedString(),
                         100 - (shield.darknessAbsorption * 100)));
            }

            if (shield.statusEffectBlockResistances != null && shield.statusEffectBlockResistances.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, shield.GetFormattedStatusResistances());
            }

            if (shield.statusEffectCancellationRates != null && shield.statusEffectCancellationRates.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, shield.GetFormattedStatusCancellationRates());
            }

            if (shield.poiseBonus != 0)
            {
                CreatePoiseTooltip(shield.poiseBonus);
            }

            if (shield.postureBonus != 0)
            {
                CreatePostureTooltip(shield.postureBonus);
            }

            if (shield.postureDamageAbsorption != 1)
            {

                CreateTooltip(
                    postureSprite,
                    Color.white,
                    String.Format(
                        postureDamageAbsorptionLabel.GetLocalizedString(),
                         100 - (shield.postureDamageAbsorption * 100)));

            }
            if (shield.slashDamageAbsorption != 1)
            {
                CreateTooltip(
                    slashSprite,
                    Color.white,
                    String.Format(
                        slashDamageAbsorptionLabel.GetLocalizedString(),
                         100 - (shield.slashDamageAbsorption * 100)));
            }
            if (shield.pierceDamageAbsorption != 1)
            {
                CreateTooltip(
                    pierceSprite,
                    Color.white,
                    String.Format(
                        pierceDamageAbsorptionLabel.GetLocalizedString(),
                         100 - (shield.pierceDamageAbsorption * 100)));
            }
            if (shield.bluntDamageAbsorption != 1)
            {
                CreateTooltip(
                    bluntSprite,
                    Color.white,
                    String.Format(
                        bluntDamageAbsorptionLabel.GetLocalizedString(),
                         100 - (shield.bluntDamageAbsorption * 100)));
            }

            if (shield.canDamageEnemiesOnShieldAttack)
            {

                if (shield.damageDealtToEnemiesUponBlocking.physical != 0)
                {
                    CreateTooltip(
                        weaponPhysicalAttackSprite,
                        Color.white,
                        String.Format(
                            physicalDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.physical));
                }

                if (shield.damageDealtToEnemiesUponBlocking.fire != 0)
                {
                    CreateTooltip(
                        fireSprite,
                        fire,
                        String.Format(
                            fireDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.fire));
                }

                if (shield.damageDealtToEnemiesUponBlocking.frost != 0)
                {

                    CreateTooltip(
                        frostSprite,
                        frost,
                        String.Format(
                            frostDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.frost));

                }

                if (shield.damageDealtToEnemiesUponBlocking.lightning != 0)
                {
                    CreateTooltip(
                        lightningSprite,
                        lightning,
                        String.Format(
                            lightningDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.lightning));
                }

                if (shield.damageDealtToEnemiesUponBlocking.magic != 0)
                {
                    CreateTooltip(
                        magicSprite,
                        magic,
                        String.Format(
                            magicDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.magic));
                }

                if (shield.damageDealtToEnemiesUponBlocking.darkness != 0)
                {
                    CreateTooltip(
                        darknessSprite,
                        darkness,
                        String.Format(
                            darknessDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.darkness));
                }

                if (shield.damageDealtToEnemiesUponBlocking.water != 0)
                {
                    CreateTooltip(
                        waterSprite,
                        water,
                        String.Format(
                            darknessDmgDealtToEnemiesPerBlockLabel.GetLocalizedString(),
                            shield.damageDealtToEnemiesUponBlocking.water));
                }

                if (shield.damageDealtToEnemiesUponBlocking.statusEffects != null && shield.damageDealtToEnemiesUponBlocking.statusEffects.Length > 0)
                {
                    CreateTooltip(statusEffectsSprite, Color.white, shield.GetFormattedStatusAttacks());
                }
            }

            if (shield.parryWindowBonus != 0)
            {

                CreateTooltip(
                    defenseAbsorptionSprite,
                    Color.white,
                    String.Format(
                        parryWindowDurationBonusLabel.GetLocalizedString(),
                        shield.parryWindowBonus));
            }

            if (shield.parryPostureDamageBonus != 0)
            {
                CreateTooltip(
                    defenseAbsorptionSprite,
                    Color.white,
                    String.Format(
                        postureDamagePerParryLabel.GetLocalizedString(),
                        shield.parryPostureDamageBonus));
            }

            if (shield.vitalityBonus != 0)
            {
                CreateTooltip(
                    vitalitySprite,
                    Color.white,
                    String.Format(
                        vitalityBonus.GetLocalizedString(),
                        shield.vitalityBonus));
            }

            if (shield.enduranceBonus != 0)
            {
                CreateTooltip(
                    enduranceSprite,
                    Color.white,
                    String.Format(
                        enduranceBonus.GetLocalizedString(),
                        shield.enduranceBonus));
            }

            if (shield.intelligenceBonus != 0)
            {
                CreateTooltip(
                    intelligenceSprite,
                    Color.white,
                    String.Format(
                        intelligenceBonus.GetLocalizedString(),
                        shield.intelligenceBonus));
            }

            if (shield.staminaRegenBonus != 1)
            {
                CreateTooltip(
                    staminaCostSprite,
                    Color.white,
                    String.Format(
                        staminaRegenSpeedBonus.GetLocalizedString(),
                        shield.staminaRegenBonus));
            }

            CreateEquipLoadTooltip(shield.speedPenalty);
        }

        void DrawArmorBase(ArmorBaseInstance armorBaseInstance)
        {
            ArmorBase armor = armorBaseInstance.GetItem<ArmorBase>();

            if (armor.physicalDefense > 0)
            {
                CreateTooltip(
                    defenseAbsorptionSprite,
                    Color.white,
                    String.Format(
                        physicalDefenseBonus.GetLocalizedString(),
                        armor.physicalDefense));
            }

            if (armor.speedPenalty > 0)
            {
                CreateEquipLoadTooltip(armor.speedPenalty);
            }

            if (armor.fireDefense > 0)
            {
                CreateTooltip(
                    fireSprite,
                    fire,
                    String.Format(
                        fireDefenseBonus.GetLocalizedString(),
                        armor.fireDefense));

            }
            if (armor.frostDefense > 0)
            {
                CreateTooltip(
                    frostSprite,
                    frost,
                    String.Format(
                        frostDefenseBonus.GetLocalizedString(),
                        armor.frostDefense));
            }
            if (armor.lightningDefense > 0)
            {
                CreateTooltip(
                    lightningSprite,
                    lightning,
                    String.Format(
                        lightningDefenseBonus.GetLocalizedString(),
                        armor.lightningDefense));
            }
            if (armor.magicDefense > 0)
            {
                CreateTooltip(
                    magicSprite,
                    magic,
                    String.Format(
                        magicDefenseBonus.GetLocalizedString(),
                        armor.magicDefense));
            }
            if (armor.darkDefense > 0)
            {
                CreateTooltip(
                    darknessSprite,
                    darkness,
                    String.Format(
                        darknessDefenseBonus.GetLocalizedString(),
                        armor.darkDefense));
            }
            if (armor.waterDefense > 0)
            {
                CreateTooltip(
                    waterSprite,
                    water,
                    String.Format(
                        waterDefenseBonus.GetLocalizedString(),
                        armor.waterDefense));
            }

            if (armor.poiseBonus != 0)
            {
                CreatePoiseTooltip(armor.poiseBonus);
            }

            if (armor.postureBonus != 0)
            {
                CreatePostureTooltip(armor.postureBonus);
            }

            if (armor.statusEffectResistances != null && armor.statusEffectResistances.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, armor.GetFormattedStatusResistances());
            }

            if (armor.statusEffectCancellationRates != null && armor.statusEffectCancellationRates.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, armor.GetFormattedStatusCancellationRates());
            }

            CreateAdditionalGoldTooltip(armor.additionalCoinPercentage);

            CreateStatTooltip(armor.vitalityBonus, vitalityBonus.GetLocalizedString(), vitalitySprite);
            CreateStatTooltip(armor.enduranceBonus, enduranceBonus.GetLocalizedString(), enduranceSprite);
            CreateStatTooltip(armor.intelligenceBonus, intelligenceBonus.GetLocalizedString(), intelligenceSprite);
            CreateStatTooltip(armor.strengthBonus, strengthBonus.GetLocalizedString(), strengthSprite);
            CreateStatTooltip(armor.dexterityBonus, dexterityBonus.GetLocalizedString(), dexteritySprite);

            if (armor.reputationBonus > 0)
            {
                CreateTooltip(
                    reputationSprite,
                    Color.white,
                    String.Format(
                        reputationBonus.GetLocalizedString(),
                        $"+{armor.reputationBonus}"));
            }
            else if (armor.reputationBonus < 0)
            {
                CreateTooltip(
                    reputationSprite,
                    Color.white,
                    String.Format(
                        reputationBonus.GetLocalizedString(),
                        $"{armor.reputationBonus}"));
            }

            if (armor.discountPercentage > 0)
            {
                CreateTooltip(
                    barterSprite,
                    Color.white,
                    String.Format(
                        betterPrices.GetLocalizedString(),
                        Math.Round(armor.discountPercentage * 100, 2)
                ));
            }

            if (armor.canDamageEnemiesUponAttack)
            {
                if (armor.damageDealtToEnemiesUponAttacked.physical != 0)
                {
                    CreateTooltip(
                        weaponPhysicalAttackSprite,
                        Color.white,
                        String.Format(
                            physicalDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.physical
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.fire != 0)
                {
                    CreateTooltip(
                        fireSprite,
                        fire,
                        String.Format(
                            fireDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.fire
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.frost != 0)
                {

                    CreateTooltip(
                        frostSprite,
                        frost,
                        String.Format(
                            frostDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.frost
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.lightning != 0)
                {
                    CreateTooltip(
                        lightningSprite,
                        lightning,
                        String.Format(
                            lightningDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.lightning
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.magic != 0)
                {
                    CreateTooltip(
                        magicSprite,
                        magic,
                        String.Format(
                            magicDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.magic
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.darkness != 0)
                {
                    CreateTooltip(
                        darknessSprite,
                        darkness,
                        String.Format(
                            darknessDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.darkness
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.water != 0)
                {
                    CreateTooltip(
                        waterSprite,
                        water,
                        String.Format(
                            magicDamageDealtToAttackingEnemies.GetLocalizedString(),
                            armor.damageDealtToEnemiesUponAttacked.water
                    ));
                }

                if (armor.damageDealtToEnemiesUponAttacked.statusEffects != null && armor.damageDealtToEnemiesUponAttacked.statusEffects.Length > 0)
                {
                    CreateTooltip(statusEffectsSprite, Color.white, armor.GetFormattedDamageDealtToEnemiesUpponAttacked());
                }
            }

            if (armor.projectileMultiplierBonus > 0)
            {
                CreateTooltip(
                    projectileSprite,
                    Color.white,
                    String.Format(
                        damageOnProjectilesBonus.GetLocalizedString(),
                        armor.projectileMultiplierBonus
                ));
            }
        }

        void DrawAccessory(AccessoryInstance accessoryInstance)
        {
            Accessory accessory = accessoryInstance.GetItem<Accessory>();

            if (accessory.GetShortDescription() != null && accessory.GetShortDescription().Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, accessory.GetShortDescription());
            }

            if (accessory.healthBonus > 0)
            {
                CreateTooltip(
                    vitalitySprite,
                    Color.white,
                    String.Format(
                        healthPoints.GetLocalizedString(),
                        accessory.healthBonus
                ));
            }
            if (accessory.magicBonus > 0)
            {
                CreateTooltip(
                    magicSprite,
                    Color.white,
                    String.Format(
                        manaPoints.GetLocalizedString(),
                        accessory.magicBonus
                ));
            }
            if (accessory.staminaBonus > 0)
            {
                CreateTooltip(
                    enduranceSprite,
                    Color.white,
                    String.Format(
                        staminaPoints.GetLocalizedString(),
                        accessory.staminaBonus
                ));
            }
            if (accessory.physicalAttackBonus > 0)
            {
                CreateTooltip(
                    weaponPhysicalAttackSprite,
                    Color.white,
                    String.Format(
                        physicalAttackDamage.GetLocalizedString(),
                        accessory.physicalAttackBonus
                ));
            }
            if (accessory.jumpAttackBonus > 0)
            {
                CreateTooltip(
                    weaponPhysicalAttackSprite,
                    Color.white,
                    String.Format(
                        jumpAttackDamage.GetLocalizedString(),
                        accessory.jumpAttackBonus
                ));
            }
            if (accessory.increaseAttackPowerWithLowerHealth)
            {
                CreateTooltip(weaponPhysicalAttackSprite, Color.white, attackIncreasesWithLowerHealth.GetLocalizedString());
            }
            if (accessory.increaseAttackPowerTheLowerTheReputation)
            {
                CreateTooltip(weaponPhysicalAttackSprite, Color.white, attackIncreasesWithLowerReputation.GetLocalizedString());
            }
            if (accessory.postureDamagePerParry > 0)
            {
                CreateTooltip(
                    postureSprite,
                    Color.white,
                    String.Format(
                        postureDamagePerParryLabel.GetLocalizedString(),
                        accessory.postureDamagePerParry
                ));
            }
            if (accessory.spellDamageBonusMultiplier > 0)
            {
                CreateTooltip(
                    magicSprite,
                    Color.white,
                    String.Format(
                        moreSpellDamage.GetLocalizedString(),
                        accessory.spellDamageBonusMultiplier
                ));
            }
            if (accessory.chanceToDoubleCoinsFromFallenEnemies)
            {
                CreateTooltip(goldCoinSprite, Color.white, chanceToDoubleCoinsFromFallenEnemies.GetLocalizedString());
            }
            if (accessory.postureDecreaseRateBonus > 0)
            {
                CreateTooltip(
                    postureSprite,
                    Color.white,
                    String.Format(
                        postureDecreaseRateBonus.GetLocalizedString(),
                        accessory.postureDecreaseRateBonus
                ));
            }
            if (accessory.backStabAngleBonus > 0)
            {
                CreateTooltip(
                    postureSprite,
                    Color.white,
                    String.Format(
                        backStabAngleBonus.GetLocalizedString(),
                        accessory.backStabAngleBonus
                ));
            }
            if (accessory.twoHandAttackBonusMultiplier > 0)
            {
                CreateTooltip(
                    slashSprite,
                    Color.white,
                    "x" + accessory.twoHandAttackBonusMultiplier + "% " + multiplierWhenTwoHanding.GetLocalizedString()
                );
            }
        }

        void DrawSpell(SpellInstance spellInstance)
        {
            Spell spell = spellInstance.GetItem<Spell>();

            Damage spellDamage = spell.projectile != null ? spell.projectile.GetComponent<Projectile>()?.damage : null;

            if (spellDamage != null)
            {
                DrawElementAttackTooltips(
                    spellDamage.fire, spellDamage.frost, spellDamage.lightning, spellDamage.magic,
                    spellDamage.darkness, spellDamage.water
                );

                DrawDamageTooltips(spellDamage);
            }

            if (spell.HasRequirements())
            {
                CreateTooltip(
                    requirementsSprite,
                    spell.AreRequirementsMet(playerManager.characterBaseStats) ? Color.white : requirementsNotMet,
                    spell.DrawRequirements(playerManager.characterBaseStats));
            }

            if (spell.GetShortDescription() != null && spell.GetShortDescription().Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, spell.GetShortDescription());
            }

            if (spell.manaCostPerCast > 0)
            {
                CreateTooltip(
                    spellCastSprite,
                    Color.white,
                    String.Format(
                        manaPointsRequiredToCast.GetLocalizedString(),
                        spell.manaCostPerCast
                ));
            }

            if (spell.staminaCostPerCast > 0)
            {
                CreateTooltip(
                    staminaCostSprite,
                    Color.white,
                    Glossary.IsPortuguese()
                        ? $"{spell.staminaCostPerCast} pontos de stamina para lançar"
                        : $"{spell.staminaCostPerCast} stamina points to cast"
                );
            }

            if (spell.statusEffects != null && spell.statusEffects.Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, spell.GetFormattedAppliedStatusEffects());
            }

            /*            if (spell.isFaithSpell)
                        {
                            CreateTooltip(holyWeaponSprite, Color.white, $"Faith Spell (improves with reputation)");
                        }*/
        }

        void DrawArrow(ArrowInstance arrowInstance)
        {
            Arrow arrow = arrowInstance.GetItem<Arrow>();

            Damage arrowDamage = arrow.projectile != null ? arrow.projectile.GetComponent<Projectile>()?.damage : null;

            if (arrowDamage != null)
            {
                if (arrowDamage.physical > 0)
                {
                    string damageExplanation = Glossary.IsPortuguese()
                        ? $"+{arrowDamage.physical} Dano Físico"
                        : $"+{arrowDamage.physical} Physical Damage";

                    CreateTooltip(weaponPhysicalAttackSprite, Color.white, damageExplanation);
                }

                DrawElementAttackTooltips(
                    arrowDamage.fire, arrowDamage.frost, arrowDamage.lightning, arrowDamage.magic,
                    arrowDamage.darkness, arrowDamage.water
                );

                DrawDamageTooltips(arrowDamage);

                if (arrow.staminaCostPerCast > 0)
                {
                    CreateTooltip(
                        staminaCostSprite,
                        Color.white,
                        Glossary.IsPortuguese()
                            ? $"{arrow.staminaCostPerCast} pontos de stamina por disparo"
                            : $"{arrow.staminaCostPerCast} stamina points to shoot"
                    );
                }
            }
        }

        void DrawCraftingMaterial(CraftingMaterialInstance craftingMaterialInstance)
        {
            CraftingMaterial craftingMaterial = craftingMaterialInstance.GetItem<CraftingMaterial>();

            if (craftingMaterial.GetShortDescription() != null && craftingMaterial.GetShortDescription().Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, craftingMaterial.GetShortDescription());
            }

            CreateTooltip(craftingMaterialSprite, Color.white, craftingMaterialLabel.GetLocalizedString());
        }

        void DrawUpgradeMaterial(UpgradeMaterialInstance upgradeMaterialInstance)
        {
            UpgradeMaterial upgradeMaterial = upgradeMaterialInstance.GetItem<UpgradeMaterial>();

            if (upgradeMaterial.GetShortDescription() != null && upgradeMaterial.GetShortDescription().Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, upgradeMaterial.GetShortDescription());
            }

            CreateTooltip(upgradeMaterialSprite, Color.white, upgradeMaterialLabel.GetLocalizedString());
        }

        void DrawConsumable(ConsumableInstance consumableInstance)
        {
            Consumable consumable = consumableInstance.GetItem<Consumable>();

            if (consumable.GetShortDescription() != null && consumable.GetShortDescription().Length > 0)
            {
                CreateTooltip(statusEffectsSprite, Color.white, consumable.GetShortDescription());
            }
            if (consumable.statusEffectsWhenConsumed != null && consumable.statusEffectsWhenConsumed.Length > 0)
            {
                string consumableText = consumable.GetFormattedAppliedStatusEffects();

                if (string.IsNullOrEmpty(consumableText) == false)
                {
                    CreateTooltip(statusEffectsSprite, Color.white, consumableText);
                }
            }
            if (consumable.isBossToken)
            {
                CreateTooltip(bossTokenSprite, Color.white, bossTokenLabel.GetLocalizedString());
            }
            if (consumable.canBeConsumedForGold)
            {
                CreateTooltip(goldCoinSprite, Color.white, String.Format(consumeToReceive.GetLocalizedString(), consumable.value));
            }
            if (consumable.shouldNotRemoveOnUse)
            {
                CreateTooltip(replenishableSprite, Color.white, itemUsageReplenishesWhenRestingAtABonfire.GetLocalizedString());
            }
            if (consumable is Card card)
            {
                CreateTooltip(cardSprite, Color.white, LocalizationSettings.StringDatabase.GetLocalizedString("Glossary", "IsCard"));

                if (card.commonlyFoundDescription.IsEmpty == false)
                {
                    CreateTooltip(cardSprite, Color.white, card.commonlyFoundDescription.GetLocalizedString());
                }

                if (card.manaRequired > 0)
                {
                    CreateTooltip(
                        spellCastSprite,
                        Color.white,
                        String.Format(
                            manaPointsRequiredToCast.GetLocalizedString(),
                            card.manaRequired
                    ));
                }

                if (card.staminaRequired > 0)
                {
                    CreateTooltip(
                        staminaCostSprite,
                        Color.white,
                        String.Format(
                            staminaCostLabel.GetLocalizedString(),
                            card.staminaRequired,
                            card.staminaRequired
                    ));
                }
                if (!card.isRenewable)
                {
                    CreateTooltip(
                        cardSprite,
                        Color.white,
                        lostUponUse.GetLocalizedString()
                    );
                }
            }

        }
        public void CreateTooltip(Texture2D sprite, Color color, string description)
        {
            VisualElement clone = itemEffectTooltipEntry.CloneTree();

            VisualElement icon = clone.Q<VisualElement>("Icon");
            icon.style.backgroundImage = new StyleBackground(sprite);
            icon.style.unityBackgroundImageTintColor = color;
            icon.style.borderTopColor = color;
            icon.style.borderLeftColor = color;
            icon.style.borderRightColor = color;
            icon.style.borderBottomColor = color;

            Label text = clone.Q<Label>();
            text.text = description;
            text.style.color = color;

            tooltipEffectsContainer.Add(clone);
        }
    }
}
