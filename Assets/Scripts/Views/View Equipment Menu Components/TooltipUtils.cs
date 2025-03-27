using UnityEngine.Localization.Settings;

namespace AF
{
    public static class TooltipUtils
    {

        public static string GetWeaponPhysicalDamageExplanation(PlayerManager playerManager, Weapon weapon)
        {
            AttackStatManager attackStatManager = playerManager.attackStatManager;
            int totalPhysicalDamage = attackStatManager.GetWeaponAttack(weapon);
            int strengthAttackBonus = attackStatManager.GetStrengthBonusFromWeapon(weapon);
            int dexterityAttackBonus = (int)attackStatManager.GetDexterityBonusFromWeapon(weapon);
            int intelligenceAttackBonus = totalPhysicalDamage > 0 ? (int)attackStatManager.GetIntelligenceBonusFromWeapon(weapon) : 0;

            string damageExplanation = "";

            if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
            {
                damageExplanation += $"Total Physical Damage: {totalPhysicalDamage}\n\n";

                damageExplanation += "Explanation: \n";
                damageExplanation += $"+{weapon.GetWeaponBaseAttack()} weapon physical damage\n";

                if (strengthAttackBonus > 0)
                {
                    damageExplanation += $"+{strengthAttackBonus} from strength bonus ({weapon.strengthScaling} Scaling)\n";
                }
                if (dexterityAttackBonus > 0)
                {
                    damageExplanation += $"+{dexterityAttackBonus} from dexterity bonus ({weapon.dexterityScaling} Scaling)\n";
                }
                if (intelligenceAttackBonus > 0)
                {
                    damageExplanation += $"+{intelligenceAttackBonus} from intelligence bonus ({weapon.intelligenceScaling} Scaling)\n";
                }

                if (playerManager.equipmentDatabase.isTwoHanding)
                {
                    int twoHandBonus = (int)(attackStatManager.GetWeaponAttack(weapon) - attackStatManager.GetWeaponAttack(weapon) / playerManager.attackStatManager.twoHandAttackBonusMultiplier);

                    if (twoHandBonus > 0)
                    {
                        damageExplanation += $"+{twoHandBonus} from two-handing weapon\n";
                    }
                }

                if (!playerManager.thirdPersonController.Grounded)
                {
                    int jumpAttackBonus = (int)(attackStatManager.GetWeaponAttack(weapon) - attackStatManager.GetWeaponAttack(weapon) / playerManager.attackStatManager.jumpAttackMultiplier);

                    if (jumpAttackBonus > 0)
                    {
                        damageExplanation += $"+{jumpAttackBonus} from jump attack bonus\n";
                    }
                }
            }
            else if (LocalizationSettings.SelectedLocale.Identifier.Code == "pt")
            {

                damageExplanation += $"Dano Físico Total: {totalPhysicalDamage}\n\n";

                damageExplanation += "Explicação: \n";
                damageExplanation += $"+{weapon.GetWeaponBaseAttack()} dano físico da arma\n";

                if (strengthAttackBonus > 0)
                {
                    damageExplanation += $"+{strengthAttackBonus} de bónus de força ({weapon.strengthScaling} Escala)\n";
                }
                if (dexterityAttackBonus > 0)
                {
                    damageExplanation += $"+{dexterityAttackBonus} de bónus de destreza ({weapon.dexterityScaling} Escala)\n";
                }
                if (intelligenceAttackBonus > 0)
                {
                    damageExplanation += $"+{intelligenceAttackBonus} de bónus de inteligência ({weapon.intelligenceScaling} Escala)\n";
                }

                if (playerManager.equipmentDatabase.isTwoHanding)
                {
                    int twoHandBonus = (int)(attackStatManager.GetWeaponAttack(weapon) - attackStatManager.GetWeaponAttack(weapon) / playerManager.attackStatManager.twoHandAttackBonusMultiplier);

                    if (twoHandBonus > 0)
                    {
                        damageExplanation += $"+{twoHandBonus} por usar duas mãos\n";
                    }
                }

                if (!playerManager.thirdPersonController.Grounded)
                {
                    int jumpAttackBonus = (int)(attackStatManager.GetWeaponAttack(weapon) - attackStatManager.GetWeaponAttack(weapon) / playerManager.attackStatManager.jumpAttackMultiplier);

                    if (jumpAttackBonus > 0)
                    {
                        damageExplanation += $"+{jumpAttackBonus} por atacar no ar\n";
                    }
                }
            }

            return damageExplanation;
        }

        public static string GetLightiningDamageExplanation(int baseLightningAttack, int holyDamageScaleFromReputation, int intelligenceBonusFromWeapon)
        {
            string damageExplained = "";

            if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
            {
                damageExplained += $"Total Lightning Damage: {baseLightningAttack + holyDamageScaleFromReputation + intelligenceBonusFromWeapon}\n\n";

                damageExplained += "Explanation: \n";
                damageExplained += $"+{baseLightningAttack} from weapon's lightning damage";

                if (holyDamageScaleFromReputation > 0)
                {
                    damageExplained += $"\n+{holyDamageScaleFromReputation} from high reputation";
                }
                if (intelligenceBonusFromWeapon > 0)
                {
                    damageExplained += $"\n+{intelligenceBonusFromWeapon} from intelligence bonus";
                }
            }
            else
            {
                damageExplained += $"Dano de Trovão Total: {baseLightningAttack + holyDamageScaleFromReputation + intelligenceBonusFromWeapon}\n\n";

                damageExplained += "Explicação: \n";
                damageExplained += $"+{baseLightningAttack} dano de trovão da arma";

                if (holyDamageScaleFromReputation > 0)
                {
                    damageExplained += $"\n+{holyDamageScaleFromReputation} de reputação alta";
                }
                if (intelligenceBonusFromWeapon > 0)
                {
                    damageExplained += $"\n+{intelligenceBonusFromWeapon} de bónus de inteligência";
                }
            }

            return damageExplained;
        }


        public static string GetDarknessDamageExplanation(int baseAttack, int damageFromReputation, int intelligenceBonusFromWeapon)
        {
            string damageExplained = "";

            if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
            {
                damageExplained += $"Total Darkness Damage: {baseAttack + damageFromReputation + intelligenceBonusFromWeapon}\n\n";

                damageExplained += "Explanation: \n";
                damageExplained += $"+{baseAttack} from weapon's dark damage";

                if (damageFromReputation > 0)
                {
                    damageExplained += $"\n+{damageFromReputation} from low reputation";
                }
                if (intelligenceBonusFromWeapon > 0)
                {
                    damageExplained += $"\n+{intelligenceBonusFromWeapon} from intelligence bonus";
                }
            }
            else
            {
                damageExplained += $"Dano de Trevas Total: {baseAttack + damageFromReputation + intelligenceBonusFromWeapon}\n\n";

                damageExplained += "Explicação: \n";
                damageExplained += $"+{baseAttack} dano de trevas da arma";

                if (damageFromReputation > 0)
                {
                    damageExplained += $"\n+{damageFromReputation} de reputação baixa";
                }
                if (intelligenceBonusFromWeapon > 0)
                {
                    damageExplained += $"\n+{intelligenceBonusFromWeapon} de bónus de inteligência";
                }
            }

            return damageExplained;
        }

        public static string GetMagicDamageExplanation(PlayerManager playerManager, Weapon weapon)
        {
            AttackStatManager attackStatManager = playerManager.attackStatManager;
            int totalMagicDamage = weapon.GetWeaponMagicAttack(attackStatManager);
            int baseMagicDamage = weapon.GetWeaponBaseMagicAttack();
            int damageFromIntelligenceScaling = (int)attackStatManager.GetIntelligenceBonusFromWeapon(weapon);
            string damageExplanation = "";

            if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
            {
                damageExplanation += $"Total Magic Damage: {totalMagicDamage}\n\n";

                damageExplanation += "Explanation: \n";
                damageExplanation += $"+{baseMagicDamage} weapon base magic damage\n";

                if (damageFromIntelligenceScaling > 0)
                {
                    damageExplanation += $"+{damageFromIntelligenceScaling} from intelligence bonus ({weapon.intelligenceScaling} Scaling)\n";
                }
            }
            else if (LocalizationSettings.SelectedLocale.Identifier.Code == "pt")
            {
                damageExplanation += $"Dano Mágico Total: {totalMagicDamage}\n\n";

                damageExplanation += "Explicação: \n";
                damageExplanation += $"+{baseMagicDamage} dano mágico da arma\n";

                if (damageFromIntelligenceScaling > 0)
                {
                    damageExplanation += $"+{damageFromIntelligenceScaling} de bónus de inteligência ({weapon.intelligenceScaling} Escala)\n";
                }
            }

            return damageExplanation;
        }
    }
}
