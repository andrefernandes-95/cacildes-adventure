using System.Collections.Generic;
using System.Linq;
using AF.Events;
using AF.StatusEffects;
using TigerForge;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static AF.ArmorBase;

namespace AF.Stats
{
    public class StatsBonusController : MonoBehaviour
    {
        [Header("Attribute Bonus")]
        public int healthBonus = 0;
        public int magicBonus = 0;
        public int staminaBonus = 0;
        public float staminaRegenerationBonus = 0f;
        public bool shouldRegenerateMana = false;

        [Header("Stats Bonus")]
        public int vitalityBonus = 0;
        public int enduranceBonus = 0;
        public int strengthBonus = 0;
        public int dexterityBonus = 0;
        public int intelligenceBonus = 0;
        public int vitalityBonusFromConsumable = 0;
        public int enduranceBonusFromConsumable = 0;
        public int strengthBonusFromConsumable = 0;
        public int dexterityBonusFromConsumable = 0;
        public int intelligenceBonusFromConsumable = 0;

        [Header("Elemental Defenses Bonus")]
        public float fireDefenseBonus = 0;
        public float frostDefenseBonus = 0;
        public float lightningDefenseBonus = 0;
        public float magicDefenseBonus = 0;
        public float darkDefenseBonus = 0;
        public float waterDefenseBonus = 0;

        [Header("Equipment Modifiers")]
        public float weightPenalty = 0f;
        public int equipmentPoise = 0;
        public float equipmentPhysicalDefense = 0;
        public bool ignoreWeaponRequirements = false;

        [Header("Gold & Experience")]
        public float additionalCoinPercentage = 0;

        [Header("Block & Parry")]
        public int parryPostureDamageBonus = 0;
        public float parryPostureWindowBonus = 0;

        [Header("Posture")]
        public int postureBonus = 0;
        public float postureDecreaseRateBonus = 0f;

        [Header("Shop Discounts")]
        public int reputationBonus = 0;
        public float discountPercentage = 0f;

        [Header("Skills & Spells")]
        public float spellDamageBonusMultiplier = 0f;

        [Header("Locomotion")]
        public int movementSpeedBonus = 0;

        [Header("Chances")]
        public bool chanceToRestoreHealthUponDeath = false;
        public bool chanceToNotLoseItemUponConsumption = false;

        [Header("Combat")]
        public float projectileMultiplierBonus = 0f;
        public bool canRage = false;
        public float backStabAngleBonus = 0f;
        public bool increaseAttackPowerWhenUnarmed = false;
        public bool increaseAttackPowerTheLowerTheReputation = false;
        public bool increaseAttackPowerWithLowerHealth = false;
        public float twoHandAttackBonusMultiplier = 0f;
        public float heavyAttackBonusMultiplier = 0f;
        public float jumpAttackBonusMultiplier = 0f;
        public float slashDamageMultiplier = 0f;
        public float pierceDamageMultiplier = 0f;
        public float bluntDamageMultiplier = 0f;
        public float footDamageMultiplier = 0f;
        public float physicalAttackBonus = 0f;

        [Header("Increase Next Attack Damage?")]
        public bool increaseNextAttackDamage = false;
        public float nextAttackMultiplierFactor = 1.3f;

        [Header("Status Controller")]
        public CharacterBaseManager character;
        public StatusController statusController;

        [Header("Databases")]
        public UIDocumentPlayerGold uIDocumentPlayerGold;
        public NotificationManager notificationManager;

        [Header("Status Effect Resistances")]
        public Dictionary<StatusEffect, float> statusEffectCancellationRates = new();

        private void Awake()
        {
            // TODO: This needs to be a character event, not global, otherwise it will run every time the player changes his equipment!

            /*
            EventManager.StartListening(EventMessages.ON_SHIELD_EQUIPMENT_CHANGED, () =>
            {
                RecalculateEquipmentBonus();
            });*/
        }


        (Weapon, Weapon) GetCurrentWeapons()
        {
            Weapon currentRightWeapon = character.characterBaseEquipment.GetRightHandWeapon().Exists()
                ? character.characterBaseEquipment.GetRightHandWeapon().GetItem<Weapon>()
                : null;

            Weapon currentLeftWeapon = character.characterBaseEquipment.GetLeftHandWeapon().Exists()
                ? character.characterBaseEquipment.GetLeftHandWeapon().GetItem<Weapon>()
                : null;

            return (currentRightWeapon, currentLeftWeapon);
        }

        (Shield, Shield) GetCurrentShield()
        {
            Shield currentRightShield = character.characterBaseEquipment.GetRightHandWeapon().Exists()
                ? character.characterBaseEquipment.GetRightHandWeapon().GetItem<Shield>()
                : null;

            Shield currentLeftShield = character.characterBaseEquipment.GetLeftHandWeapon().Exists()
                ? character.characterBaseEquipment.GetLeftHandWeapon().GetItem<Shield>()
                : null;

            return (currentRightShield, currentLeftShield);
        }

        Helmet GetCurrentHelmet()
        {
            Helmet currentHelmet = character.characterBaseEquipment.GetHelmetInstance().Exists()
                ? character.characterBaseEquipment.GetHelmetInstance().GetItem<Helmet>()
                : null;

            return currentHelmet;
        }

        Armor GetCurrentArmor()
        {
            Armor currentArmor = character.characterBaseEquipment.GetArmorInstance().Exists()
                ? character.characterBaseEquipment.GetArmorInstance().GetItem<Armor>()
                : null;

            return currentArmor;
        }

        Gauntlet GetCurrentGauntlets()
        {
            Gauntlet currentGauntlets = character.characterBaseEquipment.GetGauntletInstance().Exists()
                ? character.characterBaseEquipment.GetGauntletInstance().GetItem<Gauntlet>()
                : null;

            return currentGauntlets;
        }

        Legwear GetCurrentLegwears()
        {
            Legwear currentLegwears = character.characterBaseEquipment.GetLegwearInstance().Exists()
                ? character.characterBaseEquipment.GetLegwearInstance().GetItem<Legwear>()
                : null;

            return currentLegwears;
        }

        public List<Accessory> GetCurrentAccessories()
        {
            List<Accessory> accessories = new();

            List<AccessoryInstance> equippedAccessoryInstances = character.characterBaseEquipment.GetAccessoryInstances().ToList();

            foreach (AccessoryInstance accessoryInstance in equippedAccessoryInstances)
            {
                if (accessoryInstance.Exists())
                {
                    accessories.Add(accessoryInstance.GetItem<Accessory>());
                }
            }

            return accessories;
        }

        List<Shield> GetCurrentShields()
        {
            List<Shield> shields = new();

            List<ShieldInstance> equippedShieldInstances = character.characterBaseEquipment.GetShieldInstances().ToList();

            foreach (ShieldInstance shieldInstance in equippedShieldInstances)
            {
                if (shieldInstance.Exists())
                {
                    shields.Add(shieldInstance.GetItem<Shield>());
                }
            }

            return shields;
        }


        public void RecalculateEquipmentBonus()
        {
            (Weapon currentRightWeapon, Weapon currentLeftWeapon) = GetCurrentWeapons();
            (Shield currentRightShield, Shield currentLeftShield) = GetCurrentShield();
            Helmet currentHelmet = GetCurrentHelmet();
            Armor currentArmor = GetCurrentArmor();
            Gauntlet currentGauntlet = GetCurrentGauntlets();
            Legwear currentLegwear = GetCurrentLegwears();
            List<Accessory> currentAccessories = GetCurrentAccessories();

            UpdateStatusEffectCancellationRates();
            UpdateWeightPenalty(currentRightWeapon, currentLeftWeapon, currentRightShield, currentLeftShield,
            currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories);

            UpdateArmorPoise(currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories);

            UpdateEquipmentPhysicalDefense(currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories);
            UpdateStatusEffectResistances(currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories);
            UpdateAttributes(currentRightWeapon, currentLeftWeapon, currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories, currentRightShield, currentLeftShield);
            UpdateAdditionalCoinPercentage(currentHelmet, currentArmor, currentGauntlet, currentLegwear, currentAccessories);
        }

        void UpdateStatusEffectCancellationRates()
        {
            statusEffectCancellationRates.Clear();

            List<ArmorBaseInstance> itemInstances = new() {
                character.characterBaseEquipment.GetHelmetInstance(),
                character.characterBaseEquipment.GetArmorInstance(),
                character.characterBaseEquipment.GetGauntletInstance(),
                character.characterBaseEquipment.GetLegwearInstance()
            };

            itemInstances.AddRange(character.characterBaseEquipment.GetAccessoryInstances());

            foreach (var itemInstance in itemInstances)
            {
                if (!itemInstance.Exists())
                {
                    continue;
                }

                StatusEffectCancellationRate[] statusEffectCancellationRates = itemInstance.GetItem<ArmorBase>().statusEffectCancellationRates;
                if (statusEffectCancellationRates.Length > 0)
                {
                    EvaluateItemResistance(statusEffectCancellationRates);
                }
            }

            foreach (ShieldInstance shieldInstance in character.characterBaseEquipment.GetShieldInstances())
            {
                if (!shieldInstance.Exists())
                {
                    continue;
                }

                StatusEffectCancellationRate[] statusEffectCancellationRates = shieldInstance.GetItem<Shield>().statusEffectCancellationRates;
                if (statusEffectCancellationRates.Length > 0)
                {
                    EvaluateItemResistance(statusEffectCancellationRates);
                }
            }
        }

        void EvaluateItemResistance(StatusEffectCancellationRate[] itemStatusEffectCancellationRates)
        {
            foreach (var statusEffectCancellationRate in itemStatusEffectCancellationRates)
            {
                if (statusEffectCancellationRates.ContainsKey(statusEffectCancellationRate.statusEffect))
                {
                    statusEffectCancellationRates[statusEffectCancellationRate.statusEffect] += statusEffectCancellationRate.amountToCancelPerSecond;
                }
                else
                {
                    statusEffectCancellationRates.Add(statusEffectCancellationRate.statusEffect, statusEffectCancellationRate.amountToCancelPerSecond);
                }
            }
        }

        void UpdateWeightPenalty(Weapon rightWeapon, Weapon leftWeapon, Shield rightShield, Shield leftShield,
        Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear, List<Accessory> accessories)
        {
            weightPenalty = 0f;

            if (rightWeapon != null)
            {
                weightPenalty += rightWeapon.speedPenalty;
            }
            if (leftWeapon != null)
            {
                weightPenalty += leftWeapon.speedPenalty;
            }
            if (rightShield != null)
            {
                weightPenalty += rightShield.speedPenalty;
            }
            if (leftShield != null)
            {
                weightPenalty += leftShield.speedPenalty;
            }
            if (helmet != null)
            {
                weightPenalty += helmet.speedPenalty;
            }
            if (armor != null)
            {
                weightPenalty += armor.speedPenalty;
            }
            if (gauntlet != null)
            {
                weightPenalty += gauntlet.speedPenalty;
            }
            if (legwear != null)
            {
                weightPenalty += legwear.speedPenalty;
            }

            weightPenalty += accessories.Sum(x => x == null ? 0 : x.speedPenalty);

            weightPenalty = Mathf.Max(0, weightPenalty); // Ensure weightPenalty is non-negative
        }

        void UpdateArmorPoise(Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear, List<Accessory> accessories)
        {
            equipmentPoise = 0;

            if (helmet != null)
            {
                equipmentPoise += helmet.poiseBonus;
            }
            if (armor != null)
            {
                equipmentPoise += armor.poiseBonus;
            }
            if (gauntlet != null)
            {
                equipmentPoise += gauntlet.poiseBonus;
            }
            if (legwear != null)
            {
                equipmentPoise += legwear.poiseBonus;
            }

            equipmentPoise += accessories.Sum(x => x == null ? 0 : x.poiseBonus);
        }

        void UpdateEquipmentPhysicalDefense(Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear, List<Accessory> accessories)
        {
            equipmentPhysicalDefense = 0f;

            if (helmet != null)
            {
                equipmentPhysicalDefense += helmet.physicalDefense;
            }

            if (armor != null)
            {
                equipmentPhysicalDefense += armor.physicalDefense;
            }

            if (gauntlet != null)
            {
                equipmentPhysicalDefense += gauntlet.physicalDefense;
            }

            if (legwear != null)
            {
                equipmentPhysicalDefense += legwear.physicalDefense;
            }

            equipmentPhysicalDefense += accessories.Sum(x => x == null ? 0 : x.physicalDefense);
        }

        void UpdateStatusEffectResistances(Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear, List<Accessory> accessories)
        {
            statusController.statusEffectResistanceBonuses.Clear();

            HandleStatusEffectEntries(helmet?.statusEffectResistances);
            HandleStatusEffectEntries(armor?.statusEffectResistances);
            HandleStatusEffectEntries(gauntlet?.statusEffectResistances);
            HandleStatusEffectEntries(legwear?.statusEffectResistances);

            var accessoryResistances = accessories
                .Where(a => a != null)
                .SelectMany(a => a.statusEffectResistances ?? Enumerable.Empty<StatusEffectResistance>())
                .ToArray();
            HandleStatusEffectEntries(accessoryResistances);
        }

        void HandleStatusEffectEntries(StatusEffectResistance[] resistances)
        {
            if (resistances != null && resistances.Length > 0)
            {
                foreach (var resistance in resistances)
                {
                    HandleStatusEffectEntry(resistance);
                }
            }
        }

        void HandleStatusEffectEntry(StatusEffectResistance statusEffectResistance)
        {
            if (statusController.statusEffectResistanceBonuses.ContainsKey(statusEffectResistance.statusEffect))
            {
                statusController.statusEffectResistanceBonuses[statusEffectResistance.statusEffect]
                    += (int)statusEffectResistance.resistanceBonus;
            }
            else
            {
                statusController.statusEffectResistanceBonuses.Add(
                    statusEffectResistance.statusEffect, (int)statusEffectResistance.resistanceBonus);
            }
        }

        void UpdateAttributes(Weapon rightWeapon, Weapon leftWeapon, Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear,
        List<Accessory> accessories, Shield rightShield, Shield leftShield)
        {
            ResetAttributes();

            ApplyWeaponAttributes(rightWeapon);
            ApplyWeaponAttributes(leftWeapon);

            ApplyEquipmentAttributes(helmet);
            ApplyEquipmentAttributes(armor);
            ApplyEquipmentAttributes(gauntlet);
            ApplyEquipmentAttributes(legwear);

            ApplyAccessoryAttributes(accessories);

            ApplyShieldAttributes(rightShield);
            ApplyShieldAttributes(leftShield);
        }

        void ResetAttributes()
        {

            healthBonus = magicBonus = staminaBonus = vitalityBonus = enduranceBonus = strengthBonus = dexterityBonus = intelligenceBonus = 0;
            fireDefenseBonus = frostDefenseBonus = lightningDefenseBonus = magicDefenseBonus = darkDefenseBonus = waterDefenseBonus = discountPercentage = spellDamageBonusMultiplier = 0;
            reputationBonus = parryPostureDamageBonus = postureBonus = movementSpeedBonus = 0;

            parryPostureWindowBonus = staminaRegenerationBonus = postureDecreaseRateBonus = projectileMultiplierBonus = backStabAngleBonus = 0f;

            shouldRegenerateMana = chanceToRestoreHealthUponDeath = canRage = chanceToNotLoseItemUponConsumption = increaseAttackPowerWhenUnarmed =
            increaseAttackPowerTheLowerTheReputation = increaseAttackPowerWithLowerHealth = false;

            twoHandAttackBonusMultiplier = heavyAttackBonusMultiplier = jumpAttackBonusMultiplier = slashDamageMultiplier =
            pierceDamageMultiplier = bluntDamageMultiplier = footDamageMultiplier = physicalAttackBonus = 0f;
        }

        void ApplyWeaponAttributes(Weapon currentWeapon)
        {
            if (currentWeapon != null)
            {
                shouldRegenerateMana = currentWeapon.shouldRegenerateMana;
            }
        }

        void ApplyEquipmentAttributes(ArmorBase equipment)
        {
            if (equipment != null)
            {
                vitalityBonus += equipment.vitalityBonus;
                enduranceBonus += equipment.enduranceBonus;
                strengthBonus += equipment.strengthBonus;
                dexterityBonus += equipment.dexterityBonus;
                intelligenceBonus += equipment.intelligenceBonus;
                fireDefenseBonus += equipment.fireDefense;
                frostDefenseBonus += equipment.frostDefense;
                lightningDefenseBonus += equipment.lightningDefense;
                magicDefenseBonus += equipment.magicDefense;
                darkDefenseBonus += equipment.darkDefense;
                waterDefenseBonus += equipment.waterDefense;
                reputationBonus += equipment.reputationBonus;
                discountPercentage += equipment.discountPercentage;
                postureBonus += equipment.postureBonus;
                staminaRegenerationBonus += equipment.staminaRegenBonus;
                movementSpeedBonus += equipment.movementSpeedBonus;
                projectileMultiplierBonus += equipment.projectileMultiplierBonus;

                if (equipment.canRage)
                {
                    canRage = true;
                }
            }
        }

        void ApplyAccessoryAttributes(List<Accessory> accessories)
        {
            foreach (var accessory in accessories)
            {
                vitalityBonus += accessory?.vitalityBonus ?? 0;
                enduranceBonus += accessory?.enduranceBonus ?? 0;
                strengthBonus += accessory?.strengthBonus ?? 0;
                dexterityBonus += accessory?.dexterityBonus ?? 0;
                intelligenceBonus += accessory?.intelligenceBonus ?? 0;
                fireDefenseBonus += accessory?.fireDefense ?? 0;
                frostDefenseBonus += accessory?.frostDefense ?? 0;
                lightningDefenseBonus += accessory?.lightningDefense ?? 0;
                magicDefenseBonus += accessory?.magicDefense ?? 0;
                darkDefenseBonus += accessory?.darkDefense ?? 0;
                waterDefenseBonus += accessory?.waterDefense ?? 0;
                reputationBonus += accessory?.reputationBonus ?? 0;
                parryPostureDamageBonus += accessory?.postureDamagePerParry ?? 0;

                backStabAngleBonus += accessory?.backStabAngleBonus ?? 0;

                healthBonus += accessory?.healthBonus ?? 0;
                magicBonus += accessory?.magicBonus ?? 0;
                staminaBonus += accessory?.staminaBonus ?? 0;
                spellDamageBonusMultiplier += accessory?.spellDamageBonusMultiplier ?? 0;
                postureBonus += accessory?.postureBonus ?? 0;
                staminaRegenerationBonus += accessory?.staminaRegenBonus ?? 0;

                postureDecreaseRateBonus += accessory?.postureDecreaseRateBonus ?? 0;


                if (accessory != null)
                {
                    if (accessory.chanceToRestoreHealthUponDeath)
                    {
                        chanceToRestoreHealthUponDeath = true;
                    }

                    if (accessory.chanceToNotLoseItemUponConsumption)
                    {
                        chanceToNotLoseItemUponConsumption = true;
                    }

                    if (accessory.increaseAttackPowerWhenUnarmed)
                    {
                        increaseAttackPowerWhenUnarmed = true;
                    }

                    if (accessory.increaseAttackPowerTheLowerTheReputation)
                    {
                        increaseAttackPowerTheLowerTheReputation = true;
                    }

                    if (accessory.increaseAttackPowerWithLowerHealth)
                    {
                        increaseAttackPowerWithLowerHealth = true;
                    }

                    if (accessory.twoHandAttackBonusMultiplier > 0)
                    {
                        twoHandAttackBonusMultiplier += accessory.twoHandAttackBonusMultiplier;
                    }

                    if (accessory.heavyAttackBonusMultiplier > 0)
                    {
                        heavyAttackBonusMultiplier += accessory.heavyAttackBonusMultiplier;
                    }

                    if (accessory.jumpAttackBonus > 0)
                    {
                        jumpAttackBonusMultiplier += accessory.jumpAttackBonus;
                    }
                    if (accessory.footDamageMultiplier > 0)
                    {
                        footDamageMultiplier += accessory.footDamageMultiplier;
                    }

                    if (accessory.slashDamageMultiplier > 0)
                    {
                        slashDamageMultiplier += accessory.slashDamageMultiplier;
                    }

                    if (accessory.bluntDamageMultiplier > 0)
                    {
                        bluntDamageMultiplier += accessory.bluntDamageMultiplier;
                    }

                    if (accessory.pierceDamageMultiplier > 0)
                    {
                        pierceDamageMultiplier += accessory.pierceDamageMultiplier;
                    }

                    if (accessory.physicalAttackBonus > 0)
                    {
                        physicalAttackBonus += accessory.physicalAttackBonus;
                    }
                }
            }
        }

        void ApplyShieldAttributes(Shield shield)
        {
            if (shield != null)
            {
                parryPostureWindowBonus += shield.parryWindowBonus;
                parryPostureDamageBonus += shield.parryPostureDamageBonus;
                staminaRegenerationBonus += shield.staminaRegenBonus;
            }
        }

        void UpdateAdditionalCoinPercentage(Helmet helmet, Armor armor, Gauntlet gauntlet, Legwear legwear, List<Accessory> accessories)
        {
            additionalCoinPercentage = GetEquipmentCoinPercentage(helmet)
                                   + GetEquipmentCoinPercentage(armor)
                                   + GetEquipmentCoinPercentage(gauntlet)
                                   + GetEquipmentCoinPercentage(legwear)
                                   + accessories.Sum(x => x == null ? 0 : x.additionalCoinPercentage);
        }

        float GetEquipmentCoinPercentage(ArmorBase equipment)
        {
            return equipment != null ? equipment.additionalCoinPercentage : 0f;
        }

        public bool ShouldDoubleCoinFromFallenEnemy()
        {
            (Weapon rightWeapon, Weapon leftWeapon) = GetCurrentWeapons();
            List<Accessory> accessories = GetCurrentAccessories();

            bool hasDoublingCoinAccessoryEquipped = accessories.Any(acc => acc != null && acc.chanceToDoubleCoinsFromFallenEnemies);

            if (rightWeapon != null && rightWeapon.doubleCoinsUponKillingEnemies)
            {
                return true;
            }

            if (leftWeapon != null && leftWeapon.doubleCoinsUponKillingEnemies)
            {
                return true;
            }

            if (!hasDoublingCoinAccessoryEquipped)
            {
                return false;
            }

            return Random.Range(0, 1f) <= 0.05f;
        }

        public int GetCurrentIntelligenceBonus()
        {
            return intelligenceBonus + intelligenceBonusFromConsumable;
        }

        public int GetCurrentDexterityBonus()
        {
            return dexterityBonus + dexterityBonusFromConsumable;
        }

        public int GetCurrentStrengthBonus()
        {
            return strengthBonus + strengthBonusFromConsumable;
        }

        public int GetCurrentVitalityBonus()
        {
            return vitalityBonus + vitalityBonusFromConsumable;
        }

        public int GetCurrentEnduranceBonus()
        {
            return enduranceBonus + enduranceBonusFromConsumable;
        }

        public int GetCurrentReputationBonus()
        {
            return reputationBonus;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetStatsFromConsumable(int value)
        {
            this.vitalityBonusFromConsumable = value;
            this.enduranceBonusFromConsumable = value;
            this.strengthBonusFromConsumable = value;
            this.dexterityBonusFromConsumable = value;
            this.intelligenceBonusFromConsumable = value;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetIgnoreNextWeaponToEquipRequirements(bool value)
        {
            ignoreWeaponRequirements = value;
        }

        // TODO: Stuff related to rebirth, move it to its own proper class
        public void ReturnGoldAndResetStats()
        {
            int goldAmount = LevelUtils.GetRequiredExperienceForLevel(character.characterBaseStats.GetCurrentLevel());
            character.characterBaseStats.ResetStats();

            // TODO: Override in PlayerStatsBonusController

            uIDocumentPlayerGold.AddGold(goldAmount);

            bool isPortuguese = LocalizationSettings.SelectedLocale.Identifier.Code == "pt";

            notificationManager.ShowNotification(
                isPortuguese ? "Os teus atributos foram resetados" : "Your stats have been reset",
                notificationManager.systemSuccess
            );
        }
    }
}
