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
        Transform defaultRightWeaponHandler;
        [SerializeField] Transform leftWeaponHandler;
        Transform defaultLeftWeaponHandler;

        [Header("Components")]
        public CharacterBaseManager character;

        void Awake()
        {
            if (unarmedWeaponPrefab != null)
            {
                equippedRightWeaponInstance = Instantiate(unarmedWeaponPrefab, rightWeaponHandler);
                equippedLeftWeaponInstance = Instantiate(unarmedWeaponPrefab, leftWeaponHandler);

                character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.rightBumperActions.ToArray());
                character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.leftBumperActions.ToArray());
                character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.rightTriggerActions.ToArray());
            }

            defaultRightWeaponHandler = rightWeaponHandler;
            defaultLeftWeaponHandler = leftWeaponHandler;

            character.characterBaseEquipment.onSwitchingRightWeapon.AddListener(OnSwitchingRightWeapon);
            character.characterBaseEquipment.onSwitchingLeftWeapon.AddListener(OnSwitchingLeftWeapon);
        }

        void OnSwitchingRightWeapon()
        {
            WeaponInstance newRightWeapon = character.characterBaseEquipment.GetRightHandWeapon();

            // If no weapon, use unarmed
            if (newRightWeapon.IsEmpty())
            {
                UnequipWorldWeapon(true);
                return;
            }

            // Otherwise, equip current weapon
            EquipWorldWeapon(newRightWeapon, true);
        }

        void OnSwitchingLeftWeapon()
        {
            WeaponInstance newLeftWeapon = character.characterBaseEquipment.GetLeftHandWeapon();

            // If no weapon, use unarmed
            if (newLeftWeapon.IsEmpty())
            {
                UnequipWorldWeapon(false);
                return;
            }

            // Otherwise, equip current weapon
            EquipWorldWeapon(newLeftWeapon, false);
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

        public void EquipWorldWeapon(WeaponInstance weaponToEquip, bool isRightHand)
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

            Weapon weapon = clonedWeaponInstance.GetItem<Weapon>();
            if (isRightHand)
            {
                equippedRightWeaponInstance = Instantiate(weapon.worldWeapon, rightWeaponHandler);
                equippedRightWeaponInstance.damageCollider.weaponInstance = clonedWeaponInstance;
                character.UpdateAttackAnimations(weapon.rightBumperActions.ToArray());
            }
            else
            {
                equippedLeftWeaponInstance = Instantiate(weapon.worldWeapon, leftWeaponHandler);
                equippedLeftWeaponInstance.damageCollider.weaponInstance = clonedWeaponInstance;
                character.UpdateAttackAnimations(weapon.leftBumperActions.ToArray());
            }

            character.statsBonusController.RecalculateEquipmentBonus();
        }

        public void UnequipWorldWeapon(bool isRightHand)
        {
            if (isRightHand)
            {
                DestroyRightWeaponInstance();

                if (unarmedWeaponPrefab != null)
                {
                    equippedRightWeaponInstance = Instantiate(unarmedWeaponPrefab, rightWeaponHandler);
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.rightBumperActions.ToArray());
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.rightTriggerActions.ToArray());
                }
            }
            else
            {
                DestroyLeftWeaponInstance();

                if (unarmedWeaponPrefab != null)
                {
                    equippedLeftWeaponInstance = Instantiate(unarmedWeaponPrefab, leftWeaponHandler);
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.leftBumperActions.ToArray());
                    character.UpdateAttackAnimations(unarmedWeaponPrefab.actionItem.leftTriggerActions.ToArray());
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


        public void RestoreDefaultsForWeaponPivots()
        {
            if (equippedRightWeaponInstance != null)
            {
                rightWeaponHandler = defaultRightWeaponHandler;
                equippedRightWeaponInstance.transform.SetParent(rightWeaponHandler);
                equippedRightWeaponInstance.transform.localPosition = Vector3.zero;
                equippedRightWeaponInstance.transform.localEulerAngles = Vector3.zero;
            }
            if (equippedLeftWeaponInstance != null)
            {
                leftWeaponHandler = defaultRightWeaponHandler;
                equippedLeftWeaponInstance.transform.SetParent(leftWeaponHandler);
                equippedLeftWeaponInstance.transform.localPosition = Vector3.zero;
                equippedLeftWeaponInstance.transform.localEulerAngles = Vector3.zero;
            }
        }

        public void AssignWeaponPivotsToAvatar(CharacterAvatar avatar, GameObject avatarGameObject)
        {
            if (avatar == null || avatarGameObject == null)
            {
                Debug.LogError("AssignWeaponPivotsToAvatar: Invalid avatar or avatarGameObject.");
                return;
            }

            // Assign right-hand weapon pivot
            rightWeaponHandler = CreateWeaponHandler(
                avatarGameObject,
                avatar.rightWeaponBoneName,
                avatar.rightHandWeaponPivot,
                avatar.rightHandWeaponRotation,
                equippedRightWeaponInstance
            );

            // Assign left-hand weapon pivot
            leftWeaponHandler = CreateWeaponHandler(
                avatarGameObject,
                avatar.leftWeaponBoneName,
                avatar.leftHandWeaponPivot,
                avatar.leftHandWeaponRotation,
                equippedLeftWeaponInstance
            );
        }

        private Transform CreateWeaponHandler(GameObject avatarGameObject, string boneName, Vector3 position, Vector3 rotation, WorldWeapon equippedWeapon)
        {
            if (string.IsNullOrEmpty(boneName))
            {
                Debug.LogError("CreateWeaponHandler: Bone name is null or empty.");
                return null;
            }

            Transform boneTransform = Utils.FindChildByName(avatarGameObject.transform, boneName);
            if (boneTransform == null)
            {
                Debug.LogError($"CreateWeaponHandler: Cannot find bone '{boneName}' in avatar.");
                return null;
            }

            // Create a new empty game object as the weapon handler
            GameObject weaponHandlerGO = new GameObject($"{boneName}_WeaponHandler");
            Transform weaponHandlerTransform = weaponHandlerGO.transform;

            weaponHandlerTransform.SetParent(boneTransform, worldPositionStays: false);
            weaponHandlerTransform.localPosition = position;
            weaponHandlerTransform.localEulerAngles = rotation;

            // Attach the equipped weapon if available
            if (equippedWeapon != null)
            {
                equippedWeapon.transform.SetParent(weaponHandlerTransform, worldPositionStays: false);
                equippedWeapon.transform.localPosition = Vector3.zero;
                equippedWeapon.transform.localEulerAngles = Vector3.zero;
            }

            return weaponHandlerTransform;
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
