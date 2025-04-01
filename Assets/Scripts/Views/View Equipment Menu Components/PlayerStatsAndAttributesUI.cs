
namespace AF
{
    using AF.Health;
    using AF.Stats;
    using AF.StatusEffects;
    using UnityEngine;
    using UnityEngine.Localization.Settings;
    using UnityEngine.UIElements;

    public class PlayerStatsAndAttributesUI : MonoBehaviour
    {
        [Header("Components")]
        public PlayerManager playerManager;
        public StatsBonusController playerStatsBonusController;
        public EquipmentGraphicsHandler equipmentGraphicsHandler;
        public AttackStatManager attackStatManager;
        public DefenseStatManager defenseStatManager;

        [Header("UI Documents")]
        public UIDocument uIDocument;
        public VisualElement root;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;
        public EquipmentDatabase equipmentDatabase;

        [HideInInspector] public bool shouldRerender = true;

        public StatusEffect poison, bleed, burnt, frostbite, paralysis, fear, curse, drowning;

        private void OnEnable()
        {
            if (shouldRerender)
            {
                shouldRerender = false;

                SetupRefs();
            }
        }

        void SetupRefs()
        {
            root = uIDocument.rootVisualElement;
        }

        public void DrawStats(ItemInstance item)
        {
            root.Q<VisualElement>("PlayerName").Q<Label>().text = playerManager.playerAppearance.GetPlayerName();

            SetLocalizedLabel("Level", "Level ", "Nível ", playerStatsDatabase.GetCurrentLevel());
            SetLocalizedLabel("Gold", " Gold ", " Ouro ", playerStatsDatabase.gold);
            SetGoldForNextLevelLabel();

            // TODO: Get the damage of both left and right weapons

            Damage baseAttackDamage = attackStatManager.GetAttackDamage(null);
            int baseAttack = baseAttackDamage.GetTotalDamage();

            Damage itemAttackDamage = attackStatManager.GetAttackDamage(equipmentDatabase.GetCurrentRightWeapon());
            int itemAttack = itemAttackDamage.GetTotalDamage();

            // TODO: I think this code is wrong
            if (item is AccessoryInstance)
            {
                // If previewing some item that augments physical bonus, show it here

                /*if (item is Weapon weapon)
                {
                    return (int)attackStatManager.GetWeaponAttack(weapon);
                }
                else if (item is Accessory accessory && equipmentDatabase.IsAccessoryEquiped(accessory))
                {
                    return baseAttack + accessory.physicalAttackBonus;
                }*/
            }

            // Physical and Elemental Defenses
            int basePhysicalDefense = (int)defenseStatManager.GetDefenseAbsorption();
            var itemDefenses = GetItemDefenses(item);

            int basePoise = playerManager.characterPoise.GetMaxPoiseHits();
            int itemPoise = EquipmentUtils.GetPoiseChangeFromItem(basePoise, equipmentDatabase, item);

            int basePosture = playerManager.characterPosture.GetMaxPostureDamage();
            int itemPosture = EquipmentUtils.GetPostureChangeFromItem(basePosture, equipmentDatabase, item);

            float baseEquipLoad = equipmentGraphicsHandler.GetEquipLoad();
            float itemEquipLoad = EquipmentUtils.GetEquipLoadFromItem(item, baseEquipLoad, equipmentDatabase);

            var playerBaseStats = GetPlayerBaseStats();
            var itemBonusStats = GetItemBonusStats(item);

            // Setting Labels for each stat
            SetStatLabel("Vitality", playerBaseStats.vitality, itemBonusStats.vitality);
            SetStatLabel("Endurance", playerBaseStats.endurance, itemBonusStats.endurance);
            SetStatLabel("Strength", playerBaseStats.strength, itemBonusStats.strength);
            SetStatLabel("Dexterity", playerBaseStats.dexterity, itemBonusStats.dexterity);
            SetStatLabel("Intelligence", playerBaseStats.intelligence, itemBonusStats.intelligence);


            SetStatLabel("Health",
                playerManager.health.GetMaxHealth(), itemBonusStats.healthBonus, "" + (int)playerManager.health.GetCurrentHealth());
            SetStatLabel("Stamina",
                playerManager.staminaStatManager.GetMaxStamina(), itemBonusStats.staminaBonus, "" + (int)playerManager.playerStatsDatabase.currentStamina);
            SetStatLabel("Mana",
                playerManager.manaManager.GetMaxMana(), itemBonusStats.magicBonus, "" + (int)playerManager.playerStatsDatabase.currentMana);

            SetStatLabel("Poise", basePoise, itemPoise);
            SetStatLabel("Posture", basePosture, itemPosture);
            SetStatLabel("Reputation", playerBaseStats.reputation, itemBonusStats.reputation);

            SetWeightLoadLabel("WeightLoad", baseEquipLoad, itemEquipLoad);

            SetStatLabel("PhysicalAttack", baseAttack, itemAttack);

            WeaponInstance weapon = item as WeaponInstance;
            SetAttackLabels(weapon, "FireAttack", WeaponElementType.Fire);
            SetAttackLabels(weapon, "FrostAttack", WeaponElementType.Frost);
            SetAttackLabels(weapon, "LightningAttack", WeaponElementType.Lightning);
            SetAttackLabels(weapon, "MagicAttack", WeaponElementType.Magic);
            SetAttackLabels(weapon, "DarknessAttack", WeaponElementType.Darkness);
            SetAttackLabels(weapon, "WaterAttack", WeaponElementType.Water);

            SetStatLabel("PhysicalDefense", basePhysicalDefense, itemDefenses.physical);
            SetStatLabel("FireDefense", (int)playerManager.defenseStatManager.GetFireDefense(), itemDefenses.fire);
            SetStatLabel("FrostDefense", (int)playerManager.defenseStatManager.GetFrostDefense(), itemDefenses.frost);
            SetStatLabel("LightningDefense", (int)playerManager.defenseStatManager.GetLightningDefense(), itemDefenses.lightning);
            SetStatLabel("MagicDefense", (int)playerManager.defenseStatManager.GetMagicDefense(), itemDefenses.magic);
            SetStatLabel("DarknessDefense", (int)playerManager.defenseStatManager.GetDarknessDefense(), itemDefenses.darkness);
            SetStatLabel("WaterDefense", (int)playerManager.defenseStatManager.GetWaterDefense(), itemDefenses.water);

            DrawStatusEffectLabel("Poison", poison, item);
            DrawStatusEffectLabel("Bleed", bleed, item);
            DrawStatusEffectLabel("Burnt", burnt, item);
            DrawStatusEffectLabel("Frostbite", frostbite, item);
            DrawStatusEffectLabel("Paralysis", paralysis, item);
            DrawStatusEffectLabel("Fear", fear, item);
            DrawStatusEffectLabel("Curse", curse, item);
            DrawStatusEffectLabel("Drowning", drowning, item);
        }

        void DrawStatusEffectLabel(string elementName, StatusEffect statusEffect, ItemInstance item)
        {
            PlayerStatusController playerStatusController = playerManager.statusController as PlayerStatusController;

            SetStatLabel(
                elementName,
                playerStatusController.GetResistanceForStatusEffect(statusEffect),
                item != null
                    ? EquipmentUtils.GetStatusEffectResistanceFromEquipment(item as ArmorBaseInstance, statusEffect, playerStatusController, equipmentDatabase)
                    : 0);
        }

        private void SetStatLabel(string elementName, int baseValue, int itemValue, string currentValue = "")
        {
            string label = (!string.IsNullOrEmpty(currentValue) ?
                (currentValue + "/")
                : "") + baseValue.ToString();

            Label changeIndicator =
                  root.Q<VisualElement>(elementName).Q<Label>("ChangeIndicator");
            changeIndicator.style.display = DisplayStyle.None;

            if (itemValue > 0 && itemValue != baseValue)
            {
                if (itemValue > baseValue)
                {
                    changeIndicator.style.color = Color.green;
                }
                else if (itemValue < baseValue)
                {
                    changeIndicator.style.color = Color.red;
                }

                changeIndicator.text = " > " + itemValue;
                changeIndicator.style.display = DisplayStyle.Flex;
            }

            root.Q<VisualElement>(elementName).Q<Label>("Value").text = label;
        }

        void SetGoldForNextLevelLabel()
        {
            string goldLabel = " " + LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Gold" : "Ouro";

            root.Q<VisualElement>("GoldForNextLevel").Q<Label>("Label").text =
                playerManager.playerLevelManager.GetRequiredExperienceForNextLevel() + " " + goldLabel;
            Label description =
            root.Q<VisualElement>("GoldForNextLevel").Q<Label>("Description");

            bool hasEnoughGoldForLevellingUp = false;
            string enoughGoldLabel = " " + LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Level up available"
                : "Subida de nível disponível";
            string notEnoughGoldLabel = " " + LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Amount for next level"
                : "Necessário para próximo nível";

            description.text = hasEnoughGoldForLevellingUp ? enoughGoldLabel : notEnoughGoldLabel;
            description.style.opacity = hasEnoughGoldForLevellingUp ? 1 : 0.5f;
        }
        private void SetWeightLoadLabel(string elementName, float baseValue, float itemValue)
        {
            // Format baseValue and itemValue as percentages with two decimal places
            string formattedBaseValue = (baseValue * 100).ToString("F2") + "%";
            string formattedItemValue = (itemValue * 100).ToString("F2") + "%";

            string label = formattedBaseValue + $" ({equipmentGraphicsHandler.GetWeightLoadLabel(baseValue)})";

            Label changeIndicator =
                  root.Q<VisualElement>(elementName).Q<Label>("ChangeIndicator");
            changeIndicator.style.display = DisplayStyle.None;

            if (itemValue > 0 && itemValue != baseValue)
            {
                if (itemValue < baseValue)
                {
                    changeIndicator.style.color = Color.green;
                }
                else if (itemValue > baseValue)
                {
                    changeIndicator.style.color = Color.red;
                }

                changeIndicator.text = " > " + formattedItemValue + $" ({equipmentGraphicsHandler.GetWeightLoadLabel(itemValue)})";
                changeIndicator.style.display = DisplayStyle.Flex;
            }

            root.Q<VisualElement>(elementName).Q<Label>("Value").text = label;
        }

        private void SetLocalizedLabel(string elementName, string enLabel, string ptLabel, int value)
        {
            string label = LocalizationSettings.SelectedLocale.Identifier.Code == "pt" ? ptLabel : enLabel;
            root.Q<VisualElement>(elementName).Q<Label>().text = label + value;
        }

        private (int vitality, int endurance, int intelligence, int strength, int dexterity, int reputation) GetPlayerBaseStats()
        {
            return (
                playerManager.characterBaseStats.GetVitality(),
                playerManager.characterBaseStats.GetEndurance(),
                playerManager.characterBaseStats.GetIntelligence(),
                playerManager.characterBaseStats.GetStrength(),
                playerManager.characterBaseStats.GetDexterity(),
                playerManager.characterBaseStats.GetReputation()
            );
        }

        private (int vitality, int endurance, int strength, int dexterity, int intelligence, int reputation,
        int healthBonus, int staminaBonus, int magicBonus) GetItemBonusStats(ItemInstance item)
        {
            if (item is ArmorBaseInstance armor)
            {
                return (
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.VITALITY, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.ENDURANCE, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.STRENGTH, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.DEXTERITY, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.INTELLIGENCE, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromEquipment(armor, EquipmentUtils.AttributeType.REPUTATION, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromAccessory(armor as AccessoryInstance, EquipmentUtils.AccessoryAttributeType.HEALTH_BONUS, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromAccessory(armor as AccessoryInstance, EquipmentUtils.AccessoryAttributeType.STAMINA_BONUS, playerManager, equipmentDatabase),
                    EquipmentUtils.GetAttributeFromAccessory(armor as AccessoryInstance, EquipmentUtils.AccessoryAttributeType.MANA_BONUS, playerManager, equipmentDatabase)
                );
            }
            return (0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        private void SetAttackLabels(WeaponInstance itemInstance, string labelName, WeaponElementType elementType)
        {
            // TODO: Do this for both hands
            WeaponInstance currentEquippedWeapon = null;

            if (equipmentDatabase.GetCurrentRightWeapon().Exists())
            {
                currentEquippedWeapon = equipmentDatabase.GetCurrentRightWeapon();
            }

            int baseValue = EquipmentUtils.GetElementalAttackForCurrentWeapon(
                currentEquippedWeapon, elementType, playerManager.attackStatManager, playerManager.characterBaseStats.GetReputation());

            int itemValue = EquipmentUtils.GetElementalAttackForCurrentWeapon(
                itemInstance, elementType, playerManager.attackStatManager, playerManager.characterBaseStats.GetReputation());

            SetStatLabel(labelName, baseValue, itemValue);
        }

        private (int physical, int fire, int frost, int lightning, int magic, int darkness, int water) GetItemDefenses(ItemInstance item)
        {
            if (item is ArmorBaseInstance armorBase && !(item is AccessoryInstance acc && equipmentDatabase.IsAccessoryEquiped(acc)))
            {
                return (
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.None, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Fire, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Frost, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Lightning, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Magic, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Darkness, defenseStatManager, equipmentDatabase),
                    EquipmentUtils.GetElementalDefenseFromItem(armorBase, WeaponElementType.Water, defenseStatManager, equipmentDatabase)
                );
            }
            return (0, -1, -1, -1, -1, -1, -1);
        }
    }
}
