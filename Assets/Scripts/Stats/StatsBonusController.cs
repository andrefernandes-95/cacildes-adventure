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
        [Header("Bonus")]
        public int healthBonus = 0;
        public int magicBonus = 0;
        public int staminaBonus = 0;

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
        public float fireDefenseBonus = 0;
        public float frostDefenseBonus = 0;
        public float lightningDefenseBonus = 0;
        public float magicDefenseBonus = 0;
        public float darkDefenseBonus = 0;
        public float waterDefenseBonus = 0;
        public float additionalCoinPercentage = 0;
        public int parryPostureDamageBonus = 0;
        public float parryPostureWindowBonus = 0;
        public int reputationBonus = 0;
        public float discountPercentage = 0f;
        public float spellDamageBonusMultiplier = 0f;
        public int postureBonus = 0;
        public int movementSpeedBonus = 0;

        public float postureDecreaseRateBonus = 0f;

        public float staminaRegenerationBonus = 0f;
        public bool chanceToRestoreHealthUponDeath = false;
        public bool chanceToNotLoseItemUponConsumption = false;
        public float projectileMultiplierBonus = 0f;
        public bool canRage = false;
        public float backStabAngleBonus = 0f;
        public bool shouldRegenerateMana = false;
        public bool increaseAttackPowerWhenUnarmed = false;
        public float twoHandAttackBonusMultiplier = 0f;
        public float slashDamageMultiplier = 0f;
        public float pierceDamageMultiplier = 0f;
        public float bluntDamageMultiplier = 0f;
        public float footDamageMultiplier = 0f;

        [Header("Equipment Modifiers")]
        public float weightPenalty = 0f;
        public int equipmentPoise = 0;
        public float equipmentPhysicalDefense = 0;
        public bool ignoreWeaponRequirements = false;

        [Header("Status Controller")]
        public StatusController statusController;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;
        public PlayerStatsDatabase playerStatsDatabase;
        public UIDocumentPlayerGold uIDocumentPlayerGold;
        public NotificationManager notificationManager;

        [Header("Status Effect Resistances")]
        public Dictionary<StatusEffect, float> statusEffectCancellationRates = new();



        private void Awake()
        {
            EventManager.StartListening(EventMessages.ON_SHIELD_EQUIPMENT_CHANGED, () =>
            {
                RecalculateEquipmentBonus();
            });
        }

        public void RecalculateEquipmentBonus()
        {
            UpdateStatusEffectCancellationRates();
            UpdateWeightPenalty();
            UpdateArmorPoise();
            UpdateEquipmentPhysicalDefense();
            UpdateStatusEffectResistances();
            UpdateAttributes();
            UpdateAdditionalCoinPercentage();
        }

        void UpdateStatusEffectCancellationRates()
        {
            statusEffectCancellationRates.Clear();
            List<ArmorBase> items = new() {
                equipmentDatabase.helmet, equipmentDatabase.armor, equipmentDatabase.gauntlet, equipmentDatabase.legwear,
            };
            items.AddRange(equipmentDatabase.accessories);

            foreach (var item in items)
            {
                if (item != null && item.statusEffectCancellationRates != null && item.statusEffectCancellationRates.Length > 0)
                {
                    EvaluateItemResistance(item.statusEffectCancellationRates);
                }
            }

            foreach (var item in equipmentDatabase.shields)
            {
                if (item != null && item.statusEffectCancellationRates != null && item.statusEffectCancellationRates.Length > 0)
                {
                    EvaluateItemResistance(item.statusEffectCancellationRates);
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

        void UpdateWeightPenalty()
        {
            weightPenalty = 0f;

            if (equipmentDatabase.GetCurrentWeapon() != null)
            {
                weightPenalty += equipmentDatabase.GetCurrentWeapon().speedPenalty;
            }
            if (equipmentDatabase.GetCurrentShield() != null)
            {
                weightPenalty += equipmentDatabase.GetCurrentShield().speedPenalty;
            }
            if (equipmentDatabase.helmet != null)
            {
                weightPenalty += equipmentDatabase.helmet.speedPenalty;
            }
            if (equipmentDatabase.armor != null)
            {
                weightPenalty += equipmentDatabase.armor.speedPenalty;
            }
            if (equipmentDatabase.gauntlet != null)
            {
                weightPenalty += equipmentDatabase.gauntlet.speedPenalty;
            }
            if (equipmentDatabase.legwear != null)
            {
                weightPenalty += equipmentDatabase.legwear.speedPenalty;
            }

            weightPenalty += equipmentDatabase.accessories.Sum(x => x == null ? 0 : x.speedPenalty);

            weightPenalty = Mathf.Max(0, weightPenalty); // Ensure weightPenalty is non-negative
        }

        void UpdateArmorPoise()
        {
            equipmentPoise = 0;

            if (equipmentDatabase.helmet != null)
            {
                equipmentPoise += equipmentDatabase.helmet.poiseBonus;
            }
            if (equipmentDatabase.armor != null)
            {
                equipmentPoise += equipmentDatabase.armor.poiseBonus;
            }
            if (equipmentDatabase.gauntlet != null)
            {
                equipmentPoise += equipmentDatabase.gauntlet.poiseBonus;
            }
            if (equipmentDatabase.legwear != null)
            {
                equipmentPoise += equipmentDatabase.legwear.poiseBonus;
            }

            equipmentPoise += equipmentDatabase.accessories.Sum(x => x == null ? 0 : x.poiseBonus);
        }

        void UpdateEquipmentPhysicalDefense()
        {
            equipmentPhysicalDefense = 0f;

            if (equipmentDatabase.helmet != null)
            {
                equipmentPhysicalDefense += equipmentDatabase.helmet.physicalDefense;
            }

            if (equipmentDatabase.armor != null)
            {
                equipmentPhysicalDefense += equipmentDatabase.armor.physicalDefense;
            }

            if (equipmentDatabase.gauntlet != null)
            {
                equipmentPhysicalDefense += equipmentDatabase.gauntlet.physicalDefense;
            }

            if (equipmentDatabase.legwear != null)
            {
                equipmentPhysicalDefense += equipmentDatabase.legwear.physicalDefense;
            }

            equipmentPhysicalDefense += equipmentDatabase.accessories.Sum(x => x == null ? 0 : x.physicalDefense);
        }

        void UpdateStatusEffectResistances()
        {
            statusController.statusEffectResistanceBonuses.Clear();

            HandleStatusEffectEntries(equipmentDatabase.helmet?.statusEffectResistances);
            HandleStatusEffectEntries(equipmentDatabase.armor?.statusEffectResistances);
            HandleStatusEffectEntries(equipmentDatabase.gauntlet?.statusEffectResistances);
            HandleStatusEffectEntries(equipmentDatabase.legwear?.statusEffectResistances);

            var accessoryResistances = equipmentDatabase.accessories
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

        void UpdateAttributes()
        {
            ResetAttributes();

            ApplyWeaponAttributes(equipmentDatabase.GetCurrentWeapon());

            ApplyEquipmentAttributes(equipmentDatabase.helmet);
            ApplyEquipmentAttributes(equipmentDatabase.armor);
            ApplyEquipmentAttributes(equipmentDatabase.gauntlet);
            ApplyEquipmentAttributes(equipmentDatabase.legwear);

            ApplyAccessoryAttributes();

            ApplyShieldAttributes();
        }

        void ResetAttributes()
        {

            healthBonus = magicBonus = staminaBonus = vitalityBonus = enduranceBonus = strengthBonus = dexterityBonus = intelligenceBonus = 0;
            fireDefenseBonus = frostDefenseBonus = lightningDefenseBonus = magicDefenseBonus = darkDefenseBonus = waterDefenseBonus = discountPercentage = spellDamageBonusMultiplier = 0;
            reputationBonus = parryPostureDamageBonus = postureBonus = movementSpeedBonus = 0;

            parryPostureWindowBonus = staminaRegenerationBonus = postureDecreaseRateBonus = projectileMultiplierBonus = backStabAngleBonus = 0f;

            shouldRegenerateMana = chanceToRestoreHealthUponDeath = canRage = chanceToNotLoseItemUponConsumption = increaseAttackPowerWhenUnarmed = false;

            twoHandAttackBonusMultiplier = slashDamageMultiplier = pierceDamageMultiplier = bluntDamageMultiplier = footDamageMultiplier = 0f;
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

        void ApplyAccessoryAttributes()
        {
            foreach (var accessory in equipmentDatabase.accessories)
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

                    if (accessory.twoHandAttackBonusMultiplier > 0)
                    {
                        twoHandAttackBonusMultiplier += accessory.twoHandAttackBonusMultiplier;
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
                }
            }
        }

        void ApplyShieldAttributes()
        {
            Shield currentShield = equipmentDatabase.GetCurrentShield();
            if (currentShield != null)
            {

                parryPostureWindowBonus += currentShield.parryWindowBonus;
                parryPostureDamageBonus += currentShield.parryPostureDamageBonus;
                staminaRegenerationBonus += currentShield.staminaRegenBonus;
            }
        }

        void UpdateAdditionalCoinPercentage()
        {
            additionalCoinPercentage = GetEquipmentCoinPercentage(equipmentDatabase.helmet)
                                   + GetEquipmentCoinPercentage(equipmentDatabase.armor)
                                   + GetEquipmentCoinPercentage(equipmentDatabase.gauntlet)
                                   + GetEquipmentCoinPercentage(equipmentDatabase.legwear)
                                   + equipmentDatabase.accessories.Sum(x => x == null ? 0 : x.additionalCoinPercentage);
        }

        float GetEquipmentCoinPercentage(ArmorBase equipment)
        {
            return equipment != null ? equipment.additionalCoinPercentage : 0f;
        }

        public bool ShouldDoubleCoinFromFallenEnemy()
        {
            bool hasDoublingCoinAccessoryEquipped = equipmentDatabase.accessories.Any(acc => acc != null && acc.chanceToDoubleCoinsFromFallenEnemies);

            if (equipmentDatabase.GetCurrentWeapon() != null && equipmentDatabase.GetCurrentWeapon().doubleCoinsUponKillingEnemies)
            {
                return true;
            }

            if (!hasDoublingCoinAccessoryEquipped)
            {
                return false;
            }

            return Random.Range(0, 1f) <= 0.05f;
        }

        public int GetCurrentIntelligence()
        {
            return playerStatsDatabase.intelligence + intelligenceBonus + intelligenceBonusFromConsumable;
        }

        public int GetCurrentDexterity()
        {
            return playerStatsDatabase.dexterity + dexterityBonus + dexterityBonusFromConsumable;
        }

        public int GetCurrentStrength()
        {
            return playerStatsDatabase.strength + strengthBonus + strengthBonusFromConsumable;
        }

        public int GetCurrentVitality()
        {
            return playerStatsDatabase.vitality + vitalityBonus + vitalityBonusFromConsumable;
        }

        public int GetCurrentEndurance()
        {
            return playerStatsDatabase.endurance + enduranceBonus + enduranceBonusFromConsumable;
        }

        public int GetCurrentReputation()
        {
            return playerStatsDatabase.GetCurrentReputation() + reputationBonus;
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

        public void ReturnGoldAndResetStats()
        {

            int goldAmount = LevelUtils.GetRequiredExperienceForLevel(playerStatsDatabase.GetCurrentLevel());

            playerStatsDatabase.vitality = 1;
            playerStatsDatabase.endurance = 1;
            playerStatsDatabase.intelligence = 1;
            playerStatsDatabase.strength = 1;
            playerStatsDatabase.dexterity = 1;

            uIDocumentPlayerGold.AddGold(goldAmount);

            bool isPortuguese = LocalizationSettings.SelectedLocale.Identifier.Code == "pt";

            notificationManager.ShowNotification(
                isPortuguese ? "Os teus atributos foram resetados" : "Your stats have been reset",
                notificationManager.systemSuccess
            );
        }
    }
}
