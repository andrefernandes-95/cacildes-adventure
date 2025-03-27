namespace AF
{
    using System;
    using System.Linq;
    using AF.Stats;
    using AF.StatusEffects;

    public static class EquipmentUtils
    {

        public enum AttributeType { VITALITY, ENDURANCE, DEXTERITY, STRENGTH, INTELLIGENCE, REPUTATION }
        public enum AccessoryAttributeType { HEALTH_BONUS, STAMINA_BONUS, MANA_BONUS }

        public static int GetPoiseChangeFromItem(int playerMaxPoiseHits, EquipmentDatabase equipmentDatabase, Item itemToEquip)
        {
            int currentPoise = playerMaxPoiseHits;

            // Get the equipped poise bonus based on the type of armor
            int equippedPoiseBonus = itemToEquip switch
            {
                Helmet _ => equipmentDatabase.helmet?.poiseBonus ?? 0,
                Armor _ => equipmentDatabase.armor?.poiseBonus ?? 0,
                Gauntlet _ => equipmentDatabase.gauntlet?.poiseBonus ?? 0,
                Legwear _ => equipmentDatabase.legwear?.poiseBonus ?? 0,
                _ => 0
            };

            // Subtract equipped poise bonus from current poise, ensuring it doesn't go negative
            currentPoise = Math.Max(0, currentPoise - equippedPoiseBonus);

            // Get the new poise bonus from the item (armorBase)
            int itemPoiseBonus = itemToEquip switch
            {
                Helmet helmet => helmet.poiseBonus,
                Armor armor => armor.poiseBonus,
                Gauntlet gauntlet => gauntlet.poiseBonus,
                Legwear legwear => legwear.poiseBonus,
                Accessory accessory => equipmentDatabase.IsAccessoryEquiped(accessory) ? 0 : accessory.poiseBonus,
                _ => 0
            };

            // Return the updated poise (current poise + new item poise bonus)
            return currentPoise + itemPoiseBonus;
        }

        public static int GetPostureChangeFromItem(int playerMaxPostureDamage, EquipmentDatabase equipmentDatabase, Item itemToEquip)
        {
            int currentPosture = playerMaxPostureDamage;

            // Get the equipped posture bonus based on armor type
            int equippedPostureBonus = itemToEquip switch
            {
                Helmet _ => equipmentDatabase.helmet?.postureBonus ?? 0,
                Armor _ => equipmentDatabase.armor?.postureBonus ?? 0,
                Gauntlet _ => equipmentDatabase.gauntlet?.postureBonus ?? 0,
                Legwear _ => equipmentDatabase.legwear?.postureBonus ?? 0,
                _ => 0
            };

            // Subtract equipped posture bonus from current posture, ensure it's non-negative
            currentPosture = Math.Max(0, currentPosture - equippedPostureBonus);

            // Get the new posture bonus from the item (armorBase)
            int itemPostureBonus = itemToEquip switch
            {
                Helmet helmet => helmet.postureBonus,
                Armor armor => armor.postureBonus,
                Gauntlet gauntlet => gauntlet.postureBonus,
                Legwear legwear => legwear.postureBonus,
                Accessory accessory => equipmentDatabase.IsAccessoryEquiped(accessory) ? 0 : accessory.postureBonus,
                _ => 0
            };

            // Return the updated posture (current posture + new item posture bonus)
            return currentPosture + itemPostureBonus;
        }

        public static int GetElementalAttackForCurrentWeapon(Weapon weapon, WeaponElementType elementType, AttackStatManager attackStatManager, int playerCurrentReputation)
        {
            if (weapon == null) return 0;

            return elementType switch
            {
                WeaponElementType.Fire => weapon.GetWeaponFireAttack(attackStatManager),
                WeaponElementType.Frost => weapon.GetWeaponFrostAttack(attackStatManager),
                WeaponElementType.Lightning => weapon.GetWeaponLightningAttack(playerCurrentReputation, attackStatManager),
                WeaponElementType.Darkness => weapon.GetWeaponDarknessAttack(playerCurrentReputation, attackStatManager),
                WeaponElementType.Magic => weapon.GetWeaponMagicAttack(attackStatManager),
                WeaponElementType.Water => weapon.GetWeaponWaterAttack(attackStatManager),
                _ => 0
            };
        }

        public static int GetElementalDefenseFromItem(ArmorBase armorBase, WeaponElementType weaponElementType, DefenseStatManager defenseStatManager, EquipmentDatabase equipmentDatabase)
        {
            int baseElementalDefense = weaponElementType switch
            {
                WeaponElementType.Fire => (int)defenseStatManager.GetFireDefense(),
                WeaponElementType.Frost => (int)defenseStatManager.GetFrostDefense(),
                WeaponElementType.Lightning => (int)defenseStatManager.GetLightningDefense(),
                WeaponElementType.Magic => (int)defenseStatManager.GetMagicDefense(),
                WeaponElementType.Darkness => (int)defenseStatManager.GetDarknessDefense(),
                WeaponElementType.Water => (int)defenseStatManager.GetWaterDefense(),
                WeaponElementType.None => (int)defenseStatManager.GetDefenseAbsorption(),
                _ => 0
            };

            ArmorBase equippedArmor = armorBase switch
            {
                Helmet => equipmentDatabase.helmet,
                Armor => equipmentDatabase.armor,
                Gauntlet => equipmentDatabase.gauntlet,
                Legwear => equipmentDatabase.legwear,
                Accessory accessory when !equipmentDatabase.IsAccessoryEquiped(accessory) => accessory,
                _ => null
            };

            int currentDefenseFromItem = 0;
            if (equippedArmor != null)
            {
                currentDefenseFromItem = weaponElementType switch
                {
                    WeaponElementType.Fire => (int)equippedArmor.fireDefense,
                    WeaponElementType.Frost => (int)equippedArmor.frostDefense,
                    WeaponElementType.Lightning => (int)equippedArmor.lightningDefense,
                    WeaponElementType.Magic => (int)equippedArmor.magicDefense,
                    WeaponElementType.Darkness => (int)equippedArmor.darkDefense,
                    WeaponElementType.Water => (int)equippedArmor.waterDefense,
                    WeaponElementType.None => (int)equippedArmor.physicalDefense,
                    _ => 0
                };
            }

            int newValue = Math.Max(0, baseElementalDefense - currentDefenseFromItem);

            int newDefenseFromItem = equipmentDatabase.IsEquipped(armorBase) ? 0 : weaponElementType switch
            {
                WeaponElementType.Fire => (int)armorBase.fireDefense,
                WeaponElementType.Frost => (int)armorBase.frostDefense,
                WeaponElementType.Lightning => (int)armorBase.lightningDefense,
                WeaponElementType.Magic => (int)armorBase.magicDefense,
                WeaponElementType.Darkness => (int)armorBase.darkDefense,
                WeaponElementType.Water => (int)armorBase.waterDefense,
                WeaponElementType.None => (int)armorBase.physicalDefense,
                _ => 0
            };

            return newValue + newDefenseFromItem;
        }

        public static float GetEquipLoadFromItem(Item itemToEquip, float currentWeightPenalty, EquipmentDatabase equipmentDatabase)
        {
            // Define a function to retrieve the current speed penalty from an equipped item.
            Func<Item, float> GetSpeedPenalty = (item) =>
            {
                if (item == null)
                    return 0;

                if (item is Accessory accessory)
                    return accessory.speedPenalty;

                if (item is Weapon weapon)
                    return weapon.speedPenalty;

                if (item is ArmorBase armor)
                    return armor.speedPenalty;

                return 0;
            };

            // Adjust the weight penalty by the currently equipped item based on type.
            switch (itemToEquip)
            {
                case Weapon weapon:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.GetCurrentWeapon());
                    return Math.Max(0, currentWeightPenalty) + weapon.speedPenalty;

                case Shield shield:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.GetCurrentShield());
                    return Math.Max(0, currentWeightPenalty) + shield.speedPenalty;

                case Helmet helmet:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.helmet);
                    return Math.Max(0, currentWeightPenalty) + helmet.speedPenalty;

                case Armor armor:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.armor);
                    return Math.Max(0, currentWeightPenalty) + armor.speedPenalty;

                case Gauntlet gauntlet:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.gauntlet);
                    return Math.Max(0, currentWeightPenalty) + gauntlet.speedPenalty;

                case Legwear legwear:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.legwear);
                    return Math.Max(0, currentWeightPenalty) + legwear.speedPenalty;

                case Accessory accessory:
                    // Sum speed penalties of all equipped accessories.
                    currentWeightPenalty -= equipmentDatabase.accessories.Sum(GetSpeedPenalty);
                    return Math.Max(0, currentWeightPenalty) + accessory.speedPenalty;

                default:
                    return 0f;
            }
        }

        public static int GetAttributeFromEquipment(ArmorBase armorBase, AttributeType attributeType, StatsBonusController playerStatsBonusController, EquipmentDatabase equipmentDatabase)
        {
            // Get current value based on attribute type
            int currentValue = attributeType switch
            {
                AttributeType.VITALITY => playerStatsBonusController.GetCurrentVitality(),
                AttributeType.ENDURANCE => playerStatsBonusController.GetCurrentEndurance(),
                AttributeType.STRENGTH => playerStatsBonusController.GetCurrentStrength(),
                AttributeType.DEXTERITY => playerStatsBonusController.GetCurrentDexterity(),
                AttributeType.INTELLIGENCE => playerStatsBonusController.GetCurrentIntelligence(),
                AttributeType.REPUTATION => playerStatsBonusController.GetCurrentReputation(),
                _ => 0 // Fallback for safety
            };

            // Determine bonus from the armor base and currently equipped item
            int bonusFromEquipment = 0;
            int valueFromCurrentEquipment = 0;

            // Retrieve bonus from armorBase
            if (!equipmentDatabase.IsEquipped(armorBase))
            {
                bonusFromEquipment = attributeType switch
                {
                    AttributeType.VITALITY => armorBase.vitalityBonus,
                    AttributeType.ENDURANCE => armorBase.enduranceBonus,
                    AttributeType.STRENGTH => armorBase.strengthBonus,
                    AttributeType.DEXTERITY => armorBase.dexterityBonus,
                    AttributeType.INTELLIGENCE => armorBase.intelligenceBonus,
                    AttributeType.REPUTATION => armorBase.reputationBonus,
                    _ => 0 // Fallback for safety
                };
            }

            // Check currently equipped items
            if (armorBase is Helmet && equipmentDatabase.helmet != null)
            {
                valueFromCurrentEquipment = equipmentDatabase.helmet switch
                {
                    Helmet equippedHelmet => attributeType switch
                    {
                        AttributeType.VITALITY => equippedHelmet.vitalityBonus,
                        AttributeType.ENDURANCE => equippedHelmet.enduranceBonus,
                        AttributeType.STRENGTH => equippedHelmet.strengthBonus,
                        AttributeType.DEXTERITY => equippedHelmet.dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedHelmet.intelligenceBonus,
                        AttributeType.REPUTATION => equippedHelmet.reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is Armor && equipmentDatabase.armor != null)
            {
                valueFromCurrentEquipment = equipmentDatabase.armor switch
                {
                    Armor equippedArmor => attributeType switch
                    {
                        AttributeType.VITALITY => equippedArmor.vitalityBonus,
                        AttributeType.ENDURANCE => equippedArmor.enduranceBonus,
                        AttributeType.STRENGTH => equippedArmor.strengthBonus,
                        AttributeType.DEXTERITY => equippedArmor.dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedArmor.intelligenceBonus,
                        AttributeType.REPUTATION => equippedArmor.reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is Gauntlet && equipmentDatabase.gauntlet != null)
            {
                valueFromCurrentEquipment = equipmentDatabase.gauntlet switch
                {
                    Gauntlet equippedGauntlet => attributeType switch
                    {
                        AttributeType.VITALITY => equippedGauntlet.vitalityBonus,
                        AttributeType.ENDURANCE => equippedGauntlet.enduranceBonus,
                        AttributeType.STRENGTH => equippedGauntlet.strengthBonus,
                        AttributeType.DEXTERITY => equippedGauntlet.dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedGauntlet.intelligenceBonus,
                        AttributeType.REPUTATION => equippedGauntlet.reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is Legwear && equipmentDatabase.legwear != null)
            {
                valueFromCurrentEquipment = equipmentDatabase.legwear switch
                {
                    Legwear equippedLegwear => attributeType switch
                    {
                        AttributeType.VITALITY => equippedLegwear.vitalityBonus,
                        AttributeType.ENDURANCE => equippedLegwear.enduranceBonus,
                        AttributeType.STRENGTH => equippedLegwear.strengthBonus,
                        AttributeType.DEXTERITY => equippedLegwear.dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedLegwear.intelligenceBonus,
                        AttributeType.REPUTATION => equippedLegwear.reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is Accessory)
            {
                // Loop through each accessory in the accessories collection
                foreach (var equippedAccessory in equipmentDatabase.accessories)
                {
                    // Switch based on the specific type of attribute for the accessory
                    valueFromCurrentEquipment += attributeType switch
                    {
                        AttributeType.VITALITY => equippedAccessory?.vitalityBonus ?? 0,
                        AttributeType.ENDURANCE => equippedAccessory?.enduranceBonus ?? 0,
                        AttributeType.STRENGTH => equippedAccessory?.strengthBonus ?? 0,
                        AttributeType.DEXTERITY => equippedAccessory?.dexterityBonus ?? 0,
                        AttributeType.INTELLIGENCE => equippedAccessory?.intelligenceBonus ?? 0,
                        AttributeType.REPUTATION => equippedAccessory?.reputationBonus ?? 0,
                        _ => 0
                    };
                }
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }

        public static int GetStatusEffectResistanceFromEquipment(
            ArmorBase itemToEquip,
            StatusEffect statusEffect,
            PlayerStatusController playerStatusController,
            EquipmentDatabase equipmentDatabase)
        {
            // Get current value based on attribute type
            int currentValue = playerStatusController.GetResistanceForStatusEffect(statusEffect);

            // Determine bonus from the armor base and currently equipped item
            int bonusFromEquipment = 0;
            int valueFromCurrentEquipment = 0;

            // Retrieve bonus from armorBase
            if (itemToEquip != null)
            {
                ArmorBase.StatusEffectResistance match = itemToEquip.statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                bonusFromEquipment = (int)(match?.resistanceBonus ?? 0);
            }

            // Check currently equipped items
            if (itemToEquip is Helmet && equipmentDatabase.helmet != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.helmet.statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is Armor && equipmentDatabase.armor != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.armor.statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is Gauntlet && equipmentDatabase.gauntlet != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.gauntlet.statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is Legwear && equipmentDatabase.legwear != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.legwear.statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }


        public static int GetAttributeFromAccessory(Accessory accessory, AccessoryAttributeType attributeType, PlayerManager playerManager, EquipmentDatabase equipmentDatabase)
        {
            // Get current value based on attribute type
            int currentValue = attributeType switch
            {
                AccessoryAttributeType.HEALTH_BONUS => playerManager.health.GetMaxHealth(),
                AccessoryAttributeType.STAMINA_BONUS => playerManager.staminaStatManager.GetMaxStamina(),
                AccessoryAttributeType.MANA_BONUS => playerManager.manaManager.GetMaxMana(),
                _ => 0 // Fallback for safety
            };

            // Determine bonus from the accessory and currently equipped item
            int bonusFromEquipment = 0;
            int valueFromCurrentEquipment = 0;

            // Retrieve bonus from accessory if not equipped
            if (accessory != null && !equipmentDatabase.accessories.Contains(accessory))
            {
                bonusFromEquipment = attributeType switch
                {
                    AccessoryAttributeType.HEALTH_BONUS => accessory.healthBonus,
                    AccessoryAttributeType.STAMINA_BONUS => accessory.staminaBonus,
                    AccessoryAttributeType.MANA_BONUS => accessory.magicBonus,
                    _ => 0 // Fallback for safety
                };
            }

            // Loop through each accessory in the accessories collection
            foreach (var equippedAccessory in equipmentDatabase.accessories)
            {
                // Switch based on the specific type of attribute for the accessory
                valueFromCurrentEquipment += attributeType switch
                {
                    AccessoryAttributeType.HEALTH_BONUS => equippedAccessory?.healthBonus ?? 0,
                    AccessoryAttributeType.STAMINA_BONUS => equippedAccessory?.staminaBonus ?? 0,
                    AccessoryAttributeType.MANA_BONUS => equippedAccessory?.magicBonus ?? 0,
                    _ => 0
                };
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }
    }
}
