namespace AF
{
    using System;
    using System.Linq;
    using AF.Health;
    using AF.Stats;
    using AF.StatusEffects;

    public static class EquipmentUtils
    {

        public enum AttributeType { VITALITY, ENDURANCE, DEXTERITY, STRENGTH, INTELLIGENCE, REPUTATION }
        public enum AccessoryAttributeType { HEALTH_BONUS, STAMINA_BONUS, MANA_BONUS }

        public static int GetPoiseChangeFromItem(int playerMaxPoiseHits, EquipmentDatabase equipmentDatabase, ItemInstance itemToEquip)
        {
            int currentPoise = playerMaxPoiseHits;

            // Get the equipped poise bonus based on the type of armor
            int equippedPoiseBonus = itemToEquip switch
            {
                HelmetInstance _ => equipmentDatabase.helmet.Exists() ? equipmentDatabase.helmet.GetItem<Helmet>().poiseBonus : 0,
                ArmorInstance _ => equipmentDatabase.armor.Exists() ? equipmentDatabase.armor.GetItem<Armor>().poiseBonus : 0,
                GauntletInstance _ => equipmentDatabase.gauntlet.Exists() ? equipmentDatabase.gauntlet.GetItem<Gauntlet>().poiseBonus : 0,
                LegwearInstance _ => equipmentDatabase.legwear.Exists() ? equipmentDatabase.legwear.GetItem<Legwear>().poiseBonus : 0,
                _ => 0
            };

            // Subtract equipped poise bonus from current poise, ensuring it doesn't go negative
            currentPoise = Math.Max(0, currentPoise - equippedPoiseBonus);

            // Get the new poise bonus from the item (armorBase)
            int itemPoiseBonus = itemToEquip switch
            {
                HelmetInstance helmet => helmet.GetItem<Helmet>().poiseBonus,
                ArmorInstance armor => armor.GetItem<Armor>().poiseBonus,
                GauntletInstance gauntlet => gauntlet.GetItem<Gauntlet>().poiseBonus,
                LegwearInstance legwear => legwear.GetItem<Legwear>().poiseBonus,
                AccessoryInstance accessory => equipmentDatabase.IsAccessoryEquiped(accessory) ? 0 : accessory.GetItem<Accessory>().poiseBonus,
                _ => 0
            };

            // Return the updated poise (current poise + new item poise bonus)
            return currentPoise + itemPoiseBonus;
        }

        public static int GetPostureChangeFromItem(int playerMaxPostureDamage, EquipmentDatabase equipmentDatabase, ItemInstance itemInstanceToEquip)
        {
            int currentPosture = playerMaxPostureDamage;

            // Get the equipped posture bonus based on armor type
            int equippedPostureBonus = itemInstanceToEquip switch
            {
                HelmetInstance _ => equipmentDatabase.helmet.Exists() ? equipmentDatabase.helmet.GetItem<Helmet>().postureBonus : 0,
                ArmorInstance _ => equipmentDatabase.armor.Exists() ? equipmentDatabase.armor.GetItem<Armor>().postureBonus : 0,
                GauntletInstance _ => equipmentDatabase.gauntlet.Exists() ? equipmentDatabase.gauntlet.GetItem<Gauntlet>().postureBonus : 0,
                LegwearInstance _ => equipmentDatabase.legwear.Exists() ? equipmentDatabase.legwear.GetItem<Legwear>().postureBonus : 0,
                _ => 0
            };

            // Subtract equipped posture bonus from current posture, ensure it's non-negative
            currentPosture = Math.Max(0, currentPosture - equippedPostureBonus);

            // Get the new posture bonus from the item (armorBase)
            int itemPostureBonus = itemInstanceToEquip switch
            {
                HelmetInstance helmet => helmet != null && helmet.Exists() ? helmet.GetItem<Helmet>().postureBonus : 0,
                ArmorInstance armor => armor != null && armor.Exists() ? armor.GetItem<Armor>().postureBonus : 0,
                GauntletInstance gauntlet => gauntlet != null && gauntlet.Exists() ? gauntlet.GetItem<Gauntlet>().postureBonus : 0,
                LegwearInstance legwear => legwear != null && legwear.Exists() ? legwear.GetItem<Legwear>().postureBonus : 0,
                AccessoryInstance accessory => equipmentDatabase.IsAccessoryEquiped(accessory)
                        ? 0
                        : accessory.GetItem<Accessory>().postureBonus,
                _ => 0
            };

            // Return the updated posture (current posture + new item posture bonus)
            return currentPosture + itemPostureBonus;
        }

        public static int GetElementalAttackForCurrentWeapon(WeaponInstance weaponInstance, WeaponElementType elementType, CharacterBaseAttackManager attackStatManager, int playerCurrentReputation)
        {
            if (weaponInstance == null || !weaponInstance.Exists())
            {
                return 0;
            }
            Weapon weapon = weaponInstance.GetItem<Weapon>();

            Damage weaponDamage = attackStatManager.CalculateDamageOutput(weapon.damage);

            return elementType switch
            {
                WeaponElementType.Fire => weaponDamage.fire,
                WeaponElementType.Frost => weaponDamage.frost,
                WeaponElementType.Lightning => weaponDamage.lightning,
                WeaponElementType.Darkness => weaponDamage.darkness,
                WeaponElementType.Magic => weaponDamage.magic,
                WeaponElementType.Water => weaponDamage.water,
                _ => 0
            };
        }

        public static int GetElementalDefenseFromItem(ArmorBaseInstance armorBaseInstance, WeaponElementType weaponElementType, CharacterBaseDefenseManager defenseStatManager, EquipmentDatabase equipmentDatabase)
        {
            int baseElementalDefense = weaponElementType switch
            {
                WeaponElementType.Fire => (int)defenseStatManager.damagedAbsorbed.fire,
                WeaponElementType.Frost => (int)defenseStatManager.damagedAbsorbed.frost,
                WeaponElementType.Lightning => (int)defenseStatManager.damagedAbsorbed.lightning,
                WeaponElementType.Magic => (int)defenseStatManager.damagedAbsorbed.magic,
                WeaponElementType.Darkness => (int)defenseStatManager.damagedAbsorbed.darkness,
                WeaponElementType.Water => (int)defenseStatManager.damagedAbsorbed.water,
                WeaponElementType.None => (int)defenseStatManager.damagedAbsorbed.physical,
                _ => 0
            };

            ArmorBaseInstance equippedArmorInstance = armorBaseInstance switch
            {
                HelmetInstance => equipmentDatabase.helmet,
                ArmorInstance => equipmentDatabase.armor,
                GauntletInstance => equipmentDatabase.gauntlet,
                LegwearInstance => equipmentDatabase.legwear,
                AccessoryInstance accessory when !equipmentDatabase.IsAccessoryEquiped(accessory) => accessory,
                _ => null
            };

            int currentDefenseFromItem = 0;
            if (equippedArmorInstance != null && equippedArmorInstance.Exists())
            {
                ArmorBase equippedArmor = equippedArmorInstance.GetItem<ArmorBase>();

                currentDefenseFromItem = weaponElementType switch
                {
                    WeaponElementType.Fire => (int)equippedArmor.damageAbsorbed.fire,
                    WeaponElementType.Frost => (int)equippedArmor.damageAbsorbed.frost,
                    WeaponElementType.Lightning => (int)equippedArmor.damageAbsorbed.lightning,
                    WeaponElementType.Magic => (int)equippedArmor.damageAbsorbed.magic,
                    WeaponElementType.Darkness => (int)equippedArmor.damageAbsorbed.darkness,
                    WeaponElementType.Water => (int)equippedArmor.damageAbsorbed.water,
                    WeaponElementType.None => (int)equippedArmor.damageAbsorbed.physical,
                    _ => 0
                };
            }

            int newValue = Math.Max(0, baseElementalDefense - currentDefenseFromItem);

            ArmorBase armorBase = armorBaseInstance.Exists() ? armorBaseInstance.GetItem<ArmorBase>() : null;

            int newDefenseFromItem = equipmentDatabase.IsEquipped(armorBaseInstance) ? 0 : weaponElementType switch
            {
                WeaponElementType.Fire => (int)armorBase.damageAbsorbed.fire,
                WeaponElementType.Frost => (int)armorBase.damageAbsorbed.frost,
                WeaponElementType.Lightning => (int)armorBase.damageAbsorbed.lightning,
                WeaponElementType.Magic => (int)armorBase.damageAbsorbed.magic,
                WeaponElementType.Darkness => (int)armorBase.damageAbsorbed.darkness,
                WeaponElementType.Water => (int)armorBase.damageAbsorbed.water,
                WeaponElementType.None => (int)armorBase.damageAbsorbed.physical,
                _ => 0
            };

            return newValue + newDefenseFromItem;
        }

        public static float GetEquipLoadFromItem(ItemInstance itemToEquip, float currentWeightPenalty, EquipmentDatabase equipmentDatabase)
        {
            // Define a function to retrieve the current speed penalty from an equipped item.
            static float GetSpeedPenalty(ItemInstance item)
            {
                if (item == null)
                    return 0;

                if (item is AccessoryInstance accessory && accessory.Exists())
                    return accessory.GetItem<Accessory>().speedPenalty;

                if (item is WeaponInstance weapon && weapon.Exists())
                    return weapon.GetItem<Weapon>().speedPenalty;

                if (item is ArmorBaseInstance armor && armor.Exists())
                    return armor.GetItem<ArmorBase>().speedPenalty;

                return 0;
            }

            // Adjust the weight penalty by the currently equipped item based on type.
            switch (itemToEquip)
            {
                case ShieldInstance shield:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.GetCurrentLeftWeapon());
                    return Math.Max(0, currentWeightPenalty) + shield.GetItem<Shield>().speedPenalty;

                case WeaponInstance weapon:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.GetCurrentRightWeapon());
                    return Math.Max(0, currentWeightPenalty) + weapon.GetItem<Weapon>().speedPenalty;

                case HelmetInstance helmet:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.helmet);
                    return Math.Max(0, currentWeightPenalty) + helmet.GetItem<Helmet>().speedPenalty;

                case ArmorInstance armor:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.armor);
                    return Math.Max(0, currentWeightPenalty) + armor.GetItem<Armor>().speedPenalty;

                case GauntletInstance gauntlet:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.gauntlet);
                    return Math.Max(0, currentWeightPenalty) + gauntlet.GetItem<Gauntlet>().speedPenalty;

                case LegwearInstance legwear:
                    currentWeightPenalty -= GetSpeedPenalty(equipmentDatabase.legwear);
                    return Math.Max(0, currentWeightPenalty) + legwear.GetItem<Legwear>().speedPenalty;

                case AccessoryInstance accessory:
                    // Sum speed penalties of all equipped accessories.
                    currentWeightPenalty -= equipmentDatabase.accessories.Sum((Func<ItemInstance, float>)GetSpeedPenalty);
                    return Math.Max(0, currentWeightPenalty) + accessory.GetItem<Accessory>().speedPenalty;

                default:
                    return 0f;
            }
        }

        public static int GetAttributeFromEquipment(ArmorBaseInstance armorBase, AttributeType attributeType, CharacterBaseManager character, EquipmentDatabase equipmentDatabase)
        {
            // Get current value based on attribute type
            int currentValue = attributeType switch
            {
                AttributeType.VITALITY => character.characterBaseStats.GetVitality(),
                AttributeType.ENDURANCE => character.characterBaseStats.GetEndurance(),
                AttributeType.STRENGTH => character.characterBaseStats.GetStrength(),
                AttributeType.DEXTERITY => character.characterBaseStats.GetDexterity(),
                AttributeType.INTELLIGENCE => character.characterBaseStats.GetIntelligence(),
                AttributeType.REPUTATION => character.characterBaseStats.GetReputation(),
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
                    AttributeType.VITALITY => armorBase.GetItem<ArmorBase>().vitalityBonus,
                    AttributeType.ENDURANCE => armorBase.GetItem<ArmorBase>().enduranceBonus,
                    AttributeType.STRENGTH => armorBase.GetItem<ArmorBase>().strengthBonus,
                    AttributeType.DEXTERITY => armorBase.GetItem<ArmorBase>().dexterityBonus,
                    AttributeType.INTELLIGENCE => armorBase.GetItem<ArmorBase>().intelligenceBonus,
                    AttributeType.REPUTATION => armorBase.GetItem<ArmorBase>().reputationBonus,
                    _ => 0 // Fallback for safety
                };
            }

            // Check currently equipped items
            if (armorBase is HelmetInstance && equipmentDatabase.helmet.Exists())
            {
                valueFromCurrentEquipment = equipmentDatabase.helmet switch
                {
                    HelmetInstance equippedHelmet => attributeType switch
                    {
                        AttributeType.VITALITY => equippedHelmet.GetItem<Helmet>().vitalityBonus,
                        AttributeType.ENDURANCE => equippedHelmet.GetItem<Helmet>().enduranceBonus,
                        AttributeType.STRENGTH => equippedHelmet.GetItem<Helmet>().strengthBonus,
                        AttributeType.DEXTERITY => equippedHelmet.GetItem<Helmet>().dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedHelmet.GetItem<Helmet>().intelligenceBonus,
                        AttributeType.REPUTATION => equippedHelmet.GetItem<Helmet>().reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is ArmorInstance && equipmentDatabase.armor.Exists())
            {
                valueFromCurrentEquipment = equipmentDatabase.armor switch
                {
                    ArmorInstance equippedArmor => attributeType switch
                    {
                        AttributeType.VITALITY => equippedArmor.GetItem<Armor>().vitalityBonus,
                        AttributeType.ENDURANCE => equippedArmor.GetItem<Armor>().enduranceBonus,
                        AttributeType.STRENGTH => equippedArmor.GetItem<Armor>().strengthBonus,
                        AttributeType.DEXTERITY => equippedArmor.GetItem<Armor>().dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedArmor.GetItem<Armor>().intelligenceBonus,
                        AttributeType.REPUTATION => equippedArmor.GetItem<Armor>().reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is GauntletInstance && equipmentDatabase.gauntlet.Exists())
            {
                valueFromCurrentEquipment = equipmentDatabase.gauntlet switch
                {
                    GauntletInstance equippedGauntlet => attributeType switch
                    {
                        AttributeType.VITALITY => equippedGauntlet.GetItem<Gauntlet>().vitalityBonus,
                        AttributeType.ENDURANCE => equippedGauntlet.GetItem<Gauntlet>().enduranceBonus,
                        AttributeType.STRENGTH => equippedGauntlet.GetItem<Gauntlet>().strengthBonus,
                        AttributeType.DEXTERITY => equippedGauntlet.GetItem<Gauntlet>().dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedGauntlet.GetItem<Gauntlet>().intelligenceBonus,
                        AttributeType.REPUTATION => equippedGauntlet.GetItem<Gauntlet>().reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is LegwearInstance && equipmentDatabase.legwear.Exists())
            {
                valueFromCurrentEquipment = equipmentDatabase.legwear switch
                {
                    LegwearInstance equippedLegwear => attributeType switch
                    {
                        AttributeType.VITALITY => equippedLegwear.GetItem<Legwear>().vitalityBonus,
                        AttributeType.ENDURANCE => equippedLegwear.GetItem<Legwear>().enduranceBonus,
                        AttributeType.STRENGTH => equippedLegwear.GetItem<Legwear>().strengthBonus,
                        AttributeType.DEXTERITY => equippedLegwear.GetItem<Legwear>().dexterityBonus,
                        AttributeType.INTELLIGENCE => equippedLegwear.GetItem<Legwear>().intelligenceBonus,
                        AttributeType.REPUTATION => equippedLegwear.GetItem<Legwear>().reputationBonus,
                        _ => 0
                    },
                    _ => 0
                };
            }
            else if (armorBase is AccessoryInstance)
            {
                // Loop through each accessory in the accessories collection
                foreach (var equippedAccessory in equipmentDatabase.accessories)
                {
                    if (equippedAccessory.IsEmpty())
                    {
                        continue;
                    }

                    // Switch based on the specific type of attribute for the accessory
                    valueFromCurrentEquipment += attributeType switch
                    {
                        AttributeType.VITALITY => equippedAccessory?.GetItem<Accessory>().vitalityBonus ?? 0,
                        AttributeType.ENDURANCE => equippedAccessory?.GetItem<Accessory>().enduranceBonus ?? 0,
                        AttributeType.STRENGTH => equippedAccessory?.GetItem<Accessory>().strengthBonus ?? 0,
                        AttributeType.DEXTERITY => equippedAccessory?.GetItem<Accessory>().dexterityBonus ?? 0,
                        AttributeType.INTELLIGENCE => equippedAccessory?.GetItem<Accessory>().intelligenceBonus ?? 0,
                        AttributeType.REPUTATION => equippedAccessory?.GetItem<Accessory>().reputationBonus ?? 0,
                        _ => 0
                    };
                }
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }

        public static int GetStatusEffectResistanceFromEquipment(
            ArmorBaseInstance itemToEquip,
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
                ArmorBase.StatusEffectResistance match = itemToEquip.GetItem<ArmorBase>().statusEffectResistances
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                bonusFromEquipment = (int)(match?.resistanceBonus ?? 0);
            }

            // Check currently equipped items
            if (itemToEquip is HelmetInstance && equipmentDatabase.helmet != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.helmet.GetItem<Helmet>()?.statusEffectResistances?
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is ArmorInstance && equipmentDatabase.armor != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.armor.GetItem<Armor>()?.statusEffectResistances?
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is GauntletInstance && equipmentDatabase.gauntlet != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.gauntlet.GetItem<Gauntlet>()?.statusEffectResistances?
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }
            else if (itemToEquip is LegwearInstance && equipmentDatabase.legwear != null)
            {
                ArmorBase.StatusEffectResistance match = equipmentDatabase.legwear.GetItem<Legwear>()?.statusEffectResistances?
                    .FirstOrDefault(x => x.statusEffect == statusEffect);
                valueFromCurrentEquipment = (int)(match?.resistanceBonus ?? 0);
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }


        public static int GetAttributeFromAccessory(AccessoryInstance accessory, AccessoryAttributeType attributeType, PlayerManager playerManager, EquipmentDatabase equipmentDatabase)
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
            if (accessory != null && !equipmentDatabase.IsAccessoryEquiped(accessory))
            {
                bonusFromEquipment = attributeType switch
                {
                    AccessoryAttributeType.HEALTH_BONUS => accessory.GetItem<Accessory>().healthBonus,
                    AccessoryAttributeType.STAMINA_BONUS => accessory.GetItem<Accessory>().staminaBonus,
                    AccessoryAttributeType.MANA_BONUS => accessory.GetItem<Accessory>().magicBonus,
                    _ => 0 // Fallback for safety
                };
            }

            // Loop through each accessory in the accessories collection
            foreach (var equippedAccessory in equipmentDatabase.accessories)
            {
                // Switch based on the specific type of attribute for the accessory
                valueFromCurrentEquipment += attributeType switch
                {
                    AccessoryAttributeType.HEALTH_BONUS => equippedAccessory.GetItem<Accessory>()?.healthBonus ?? 0,
                    AccessoryAttributeType.STAMINA_BONUS => equippedAccessory.GetItem<Accessory>()?.staminaBonus ?? 0,
                    AccessoryAttributeType.MANA_BONUS => equippedAccessory.GetItem<Accessory>()?.magicBonus ?? 0,
                    _ => 0
                };
            }

            // Adjust current value by the bonuses
            currentValue = Math.Max(0, currentValue - valueFromCurrentEquipment); // Ensure non-negative
            return currentValue + bonusFromEquipment;
        }
    }
}
