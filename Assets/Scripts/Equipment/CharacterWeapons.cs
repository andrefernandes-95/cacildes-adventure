using System.Collections.Generic;
using System.Linq;
using AF.Events;
using AF.Health;
using AF.Stats;
using TigerForge;
using UnityEngine;
using UnityEngine.Localization;

namespace AF.Equipment
{
    public class CharacterWeapons : MonoBehaviour
    {
        [Header("Current Weapons")]
        public WorldWeapon equippedRightWeaponInstance;
        public WorldWeapon equippedLeftWeaponInstance;

        [Header("Unarmed Weapon Prefab")]
        public UnarmedWorldWeapon unarmedWeaponPrefab;

        [Header("Character Transform Refs")]
        [SerializeField] Transform rightWeaponHandler;
        [SerializeField] Transform leftWeaponHandler;

        [Header("Components")]
        public CharacterBaseManager character;

        void Awake()
        {
            if (unarmedWeaponPrefab != null)
            {
                equippedRightWeaponInstance = Instantiate(unarmedWeaponPrefab, rightWeaponHandler);
                equippedLeftWeaponInstance = Instantiate(unarmedWeaponPrefab, leftWeaponHandler);

                character.UpdateAttackAnimations(unarmedWeaponPrefab.rightLightAttacks.ToArray());
                character.UpdateAttackAnimations(unarmedWeaponPrefab.leftLightAttacks.ToArray());
            }
        }

        public void ResetStates()
        {
            CloseAllWeaponHitboxes();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void CloseAllWeaponHitboxes()
        {
            equippedRightWeaponInstance?.CloseDamageCollider();
            equippedLeftWeaponInstance?.CloseDamageCollider();
        }

        void DestroyRightWeaponInstance()
        {
            equippedRightWeaponInstance = null;

            foreach (Transform child in rightWeaponHandler.transform)
            {
                Destroy(child.gameObject);
            }
        }
        void DestroyLeftWeaponInstance()
        {
            equippedLeftWeaponInstance = null;

            foreach (Transform child in leftWeaponHandler.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void EquipWeapon(WeaponInstance weaponToEquip, int slot, bool isRightHand)
        {
            if (isRightHand)
            {
                DestroyRightWeaponInstance();
            }
            else
            {
                DestroyLeftWeaponInstance();
            }

            WeaponInstance clonedWeaponInstance = weaponToEquip.Clone();
            character.characterBaseEquipment.EquipWeapon(clonedWeaponInstance, slot, isRightHand);

            Weapon weapon = clonedWeaponInstance.GetItem<Weapon>();
            if (isRightHand)
            {
                equippedRightWeaponInstance = Instantiate(weapon.worldWeapon, rightWeaponHandler);
                equippedRightWeaponInstance.damageCollider.weaponInstance = clonedWeaponInstance;
                character.UpdateAttackAnimations(weapon.rightLightAttacks.ToArray());
            }
            else
            {
                equippedLeftWeaponInstance = Instantiate(weapon.worldWeapon, leftWeaponHandler);
                equippedLeftWeaponInstance.damageCollider.weaponInstance = clonedWeaponInstance;
                character.UpdateAttackAnimations(weapon.leftLightAttacks.ToArray());
            }

            character.statsBonusController.RecalculateEquipmentBonus();

        }

        public void UnequipWeapon(int slot, bool isRightHand)
        {
            character.characterBaseEquipment.UnequipWeapon(slot, isRightHand);

            if (isRightHand)
            {
                DestroyRightWeaponInstance();

                if (unarmedWeaponPrefab != null)
                {
                    equippedRightWeaponInstance = Instantiate(unarmedWeaponPrefab, rightWeaponHandler);
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.rightLightAttacks.ToArray());
                }
            }
            else
            {
                DestroyLeftWeaponInstance();

                if (unarmedWeaponPrefab != null)
                {
                    equippedLeftWeaponInstance = Instantiate(unarmedWeaponPrefab, leftWeaponHandler);
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.leftLightAttacks.ToArray());
                }
            }


            character.statsBonusController.RecalculateEquipmentBonus();
        }

        public void ShowEquipment()
        {
            if (equippedRightWeaponInstance != null)
            {
                equippedRightWeaponInstance.gameObject.SetActive(true);
            }
            if (equippedLeftWeaponInstance != null)
            {
                equippedLeftWeaponInstance.gameObject.SetActive(true);
            }
        }

        public void HideEquipment()
        {
            if (equippedRightWeaponInstance != null)
            {
                equippedRightWeaponInstance.gameObject.SetActive(false);
            }
            if (equippedLeftWeaponInstance != null)
            {
                equippedLeftWeaponInstance.gameObject.SetActive(false);
            }
        }

        // TODO: Handle Later
        /*
        bool CanApplyBuff()
        {
            if (currentWeaponInstance == null || currentWeaponInstance.characterWeaponBuffs == null)
            {
                notificationManager.ShowNotification(
                    CanNotApplyBuffToThisWeapon.GetLocalizedString(), notificationManager.systemError);
                return false;
            }
            else if (currentWeaponInstance.characterWeaponBuffs.HasOnGoingBuff())
            {
                notificationManager.ShowNotification(
                    WeaponIsAlreadyBuffed.GetLocalizedString(), notificationManager.systemError);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyFireToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.FIRE, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyFrostToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.FROST, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyLightningToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.LIGHTNING, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyMagicToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.MAGIC, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyDarknessToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.DARKNESS, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyPoisonToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.POISON, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplyBloodToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.BLOOD, customDuration);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ApplySharpnessToWeapon(float customDuration)
        {
            ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName.SHARPNESS, customDuration);
        }


        public void ApplyWeaponBuffToWeapon(CharacterWeaponBuffs.WeaponBuffName weaponBuffName, float customDuration)
        {
            if (!CanApplyBuff())
            {
                return;
            }

            if (customDuration > 0)
            {
                currentWeaponInstance.characterWeaponBuffs.ApplyBuff(weaponBuffName, customDuration);
            }
            else
            {
                currentWeaponInstance.characterWeaponBuffs.ApplyBuff(weaponBuffName);
            }
        }

        public Damage GetBuffedDamage(Damage weaponDamage)
        {
            if (currentWeaponInstance == null || currentWeaponInstance.characterWeaponBuffs == null || currentWeaponInstance.characterWeaponBuffs.HasOnGoingBuff() == false)
            {
                return weaponDamage;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.FIRE)
            {
                weaponDamage.fire += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.FIRE].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.FROST)
            {
                weaponDamage.frost += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.FROST].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.LIGHTNING)
            {
                weaponDamage.lightning += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.LIGHTNING].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.MAGIC)
            {
                weaponDamage.magic += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.MAGIC].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.DARKNESS)
            {
                weaponDamage.darkness += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.DARKNESS].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.WATER)
            {
                weaponDamage.water += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.WATER].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.SHARPNESS)
            {
                weaponDamage.physical += currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.SHARPNESS].damageBonus;
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.POISON)
            {
                StatusEffectEntry statusEffectToApply = new()
                {
                    statusEffect = currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.POISON].statusEffect,
                    amountPerHit = currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.POISON].statusEffectAmountToApply,
                };

                if (weaponDamage.statusEffects == null)
                {
                    weaponDamage.statusEffects = new StatusEffectEntry[] {
                        statusEffectToApply
                    };
                }
                else
                {
                    weaponDamage.statusEffects = weaponDamage.statusEffects.Append(statusEffectToApply).ToArray();
                }
            }

            if (currentWeaponInstance.characterWeaponBuffs.appliedBuff == CharacterWeaponBuffs.WeaponBuffName.BLOOD)
            {
                StatusEffectEntry statusEffectToApply = new()
                {
                    statusEffect = currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.BLOOD].statusEffect,
                    amountPerHit = currentWeaponInstance.characterWeaponBuffs.weaponBuffs[CharacterWeaponBuffs.WeaponBuffName.BLOOD].statusEffectAmountToApply,
                };

                if (weaponDamage.statusEffects == null)
                {
                    weaponDamage.statusEffects = new StatusEffectEntry[] {
                        statusEffectToApply
                    };
                }
                else
                {
                    weaponDamage.statusEffects = weaponDamage.statusEffects.Append(statusEffectToApply).ToArray();
                }
            }

            return weaponDamage;
        }

        public void EquipLeftWeapon(CharacterWeaponHitbox leftWeaponGameObject)
        {
            leftWeaponInstance = leftWeaponGameObject;
            leftWeaponInstance.gameObject.SetActive(true);
        }

        public void UnequipLeftWeapon()
        {
            if (leftWeaponInstance != null)
            {
                leftWeaponInstance.gameObject.SetActive(false);
            }

            leftWeaponInstance = null;
        }

        public int GetCurrentBlockStaminaCost()
        {
            if (playerManager.playerWeaponsManager.currentShieldInstance == null)
            {
                return playerManager.characterBlockController.unarmedStaminaCostPerBlock;
            }

            return (int)playerManager.playerWeaponsManager.currentShieldInstance.shield.blockStaminaCost;
        }

        public Damage GetCurrentShieldDefenseAbsorption(Damage incomingDamage)
        {
            if (equipmentDatabase.isTwoHanding && equipmentDatabase.GetCurrentRightWeapon() != null)
            {
                // TODO: Weapons should have block absorption too

                // incomingDamage.physical = (int)(incomingDamage.physical * equipmentDatabase.GetCurrentRightWeapon() as ShieldInstance.blockAbsorption);
                return incomingDamage;
            }
            else if (currentShieldInstance == null || currentShieldInstance.shield == null)
            {
                incomingDamage.physical = (int)(incomingDamage.physical * playerManager.characterBlockController.unarmedDefenseAbsorption);
                return incomingDamage;
            }

            return currentShieldInstance.shield.FilterDamage(incomingDamage);
        }
        public Damage GetCurrentShieldPassiveDamageFilter(Damage incomingDamage)
        {
            if (currentShieldInstance == null || currentShieldInstance.shield == null)
            {
                return incomingDamage;
            }

            return currentShieldInstance.shield.FilterPassiveDamage(incomingDamage);
        }

        public void ApplyShieldDamageToAttacker(CharacterManager attacker)
        {
            if (currentShieldInstance == null || currentShieldInstance.shield == null)
            {
                return;
            }

            currentShieldInstance.shield.AttackShieldAttacker(attacker);
        }

        public void HandleWeaponSpecial()
        {
            if (
                playerManager.playerWeaponsManager.currentWeaponInstance == null
                || playerManager.playerWeaponsManager.currentWeaponInstance.onWeaponSpecial == null
                || playerManager.playerWeaponsManager.currentWeaponInstance.weapon == null
                )
            {
                return;
            }

            if (playerManager.manaManager.playerStatsDatabase.currentMana < playerManager.playerWeaponsManager.currentWeaponInstance.weapon.manaCostToUseWeaponSpecialAttack)
            {
                //                notificationManager.ShowNotification(NotEnoughManaToUseWeaponSpecial.GetLocalizedString());
                return;
            }

            playerManager.manaManager.DecreaseMana(
                playerManager.playerWeaponsManager.currentWeaponInstance.weapon.manaCostToUseWeaponSpecialAttack
            );

            playerManager.playerWeaponsManager.currentWeaponInstance.onWeaponSpecial?.Invoke();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ThrowWeapon()
        {

        }*/

    }
}
