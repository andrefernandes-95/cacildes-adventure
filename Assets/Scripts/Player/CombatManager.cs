using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AF.Animations;
using AF.Combat;
using AF.Health;
using AF.Ladders;
using UnityEngine;

namespace AF
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Unarmed Attacks")]
        public AttackAction lastAttackAction;
        private AttackAction lastUnarmedRightHandAttackAction;
        private AttackAction lastUnarmedLeftHandAttackAction;
        private AttackAction lastUnarmedRightFootAttackAction;
        private AttackAction lastUnarmedLeftFootAttackAction;

        [HideInInspector] public AttackingMember currentAttackingMember = AttackingMember.NONE;

        [Header("Flags")]
        public bool isCombatting = false;
        public bool isLightAttacking = false;
        public bool allowCombos = false;


        [Header("Components")]
        public CharacterBaseManager character;



        [Header("Two-Handing")]
        public bool isTwoHanding = false;
        public float twoHandingMultiplier = 1.2f;

        [Header("Heavy Attack")]
        public bool isHeavyAttacking = false;
        public float heavyAttackMultiplier = 1.3f;

        [Header("Flags")]
        public bool isJumpAttacking = false;
        public float jumpAttackMultiplier = 1.3f;


        public readonly string SpeedMultiplierHash = "SpeedMultiplier";

        [Header("Foot Damage")]
        public bool isAttackingWithFoot = false;

        [Header("Combos")]
        [SerializeField] float maxTimeBeforeResettingCombos = 1f;
        Coroutine OnGoingResetComboCoroutine;

        private void Start()
        {
            character.animator.SetFloat(SpeedMultiplierHash, 1f);
        }

        public void ResetStates()
        {
            isJumpAttacking = false;
            isHeavyAttacking = false;
            isLightAttacking = false;
            isAttackingWithFoot = false;
            allowCombos = false;

            character.animator.SetFloat(SpeedMultiplierHash, 1f);

            // Always restore the animator speed after an attack ends
            character.RestoreDefaultAnimatorSpeed();

            StopComboCoroutine();

            if (isActiveAndEnabled)
            {
                OnGoingResetComboCoroutine = StartCoroutine(ResetComboFlags());
            }
        }

        void StopComboCoroutine()
        {
            if (OnGoingResetComboCoroutine != null)
            {
                StopCoroutine(OnGoingResetComboCoroutine);
                OnGoingResetComboCoroutine = null;
            }
        }

        IEnumerator ResetComboFlags()
        {
            yield return new WaitForSeconds(maxTimeBeforeResettingCombos);
            lastAttackAction = null;
        }

        public void SetRightHandCurrentItemAction()
        {

        }

        // TODO: Change so that the current active weapons are an array (for dual wielding, we may be attacking with both weapons)
        public void AttemptAttack()
        {
            StopComboCoroutine();

            if (currentAttackingMember == AttackingMember.RIGHT_HAND)
            {
                if (character.characterBaseEquipment.GetRightHandWeapon().Exists())
                {
                    character.characterBaseEquipment.GetRightHandWeapon().GetItem<Weapon>()
                    .PerformAction(character, true, isHeavyAttacking);
                }
                else if (character.characterWeapons.equippedRightWeaponInstance is UnarmedWorldWeapon rightUnarmedWorldWeapon)
                {
                    rightUnarmedWorldWeapon.actionItem.PerformAction(character, true, isHeavyAttacking);
                }
            }
            else if (currentAttackingMember == AttackingMember.LEFT_HAND)
            {
                if (character.characterBaseEquipment.GetLeftHandWeapon().Exists())
                {
                    character.characterBaseEquipment.GetLeftHandWeapon().GetItem<Weapon>()
                    .PerformAction(character, false, isHeavyAttacking);
                }
                else if (character.characterWeapons.equippedLeftWeaponInstance is UnarmedWorldWeapon leftUnarmedWorldWeapon)
                {
                    leftUnarmedWorldWeapon.actionItem.PerformAction(character, false, isHeavyAttacking);
                }
            }


            // If unarmed

            //character.staminaStatManager.DecreaseLightAttackStamina();

            /*
                            AttackAction chosenAttackAction = null;
                            AttackAction lastAttackAction = null;
                            List<AttackAction> attackActions = new();

                            bool isRightHand = false;
                            bool isLeftHand = false;


                            switch (currentAttackingMember)
                            {
                                case AttackingMember.RIGHT_HAND:
                                    lastAttackAction = lastUnarmedRightHandAttackAction;
                                    attackActions = GetRightWeaponAttackActions();
                                    isRightHand = true;
                                    break;
                                case AttackingMember.LEFT_HAND:
                                    lastAttackAction = lastUnarmedLeftHandAttackAction;
                                    attackActions = GetLeftWeaponAttackActions();
                                    isLeftHand = true;
                                    break;
                                default:
                                    return;
                            }

                            if (attackActions == null || attackActions.Count <= 0)
                            {
                                return;
                            }

                            if (lastAttackAction == null)
                            {
                                chosenAttackAction = attackActions[0];
                            }
                            else
                            {
                                int nextAttackActionIndex = Array.IndexOf(attackActions.ToArray(), lastAttackAction) + 1;
                                if (nextAttackActionIndex >= attackActions.Count)
                                {
                                    nextAttackActionIndex = 0;
                                }

                                chosenAttackAction = attackActions[nextAttackActionIndex];
                            }

                            if (isRightHand)
                            {
                                lastUnarmedRightHandAttackAction = chosenAttackAction;
                            }
                            else if (isLeftHand)
                            {
                                lastUnarmedLeftHandAttackAction = chosenAttackAction;
                            }

                            chosenAttackAction?.Execute(character);
                        }*/
        }

        void HandleAttackSpeed()
        {
            /*
            Weapon currentWeapon = equipmentDatabase.GetCurrentWeapon();
            if (equipmentDatabase.isTwoHanding == false && currentWeapon != null && currentWeapon.oneHandAttackSpeedPenalty != 1)
            {
                animator.SetFloat(SpeedMultiplierHash, currentWeapon.oneHandAttackSpeedPenalty);
            }
            else if (equipmentDatabase.isTwoHanding && currentWeapon != null && currentWeapon.twoHandAttackSpeedPenalty != 1)
            {
                animator.SetFloat(SpeedMultiplierHash, currentWeapon.twoHandAttackSpeedPenalty);
            }
            else
            {
                animator.SetFloat(SpeedMultiplierHash, 1f);
            }*/
        }

        void HandleJumpAttack()
        {
            /*
            isHeavyAttacking = false;
            isLightAttacking = false;
            isJumpAttacking = true;

            playerManager.playerWeaponsManager.HideShield();

            playerManager.playerAnimationEventListener.OpenRightWeaponHitbox();

            playerManager.PlayCrossFadeBusyAnimationWithRootMotion(hashJumpAttack, crossFade);
            playerManager.playerComponentManager.DisableCollisionWithEnemies();*/
        }

        public void HandleHeavyAttack(bool isCardAttack)
        {
            /*
            if (isCombatting || playerManager.thirdPersonController.Grounded == false)
            {
                return;
            }

            isLightAttacking = false;
            isHeavyAttacking = true;

            playerManager.playerWeaponsManager.HideShield();


            if (heavyAttackComboIndex > GetMaxHeavyCombo())
            {
                heavyAttackComboIndex = 0;
            }

            if (isCardAttack)
            {
                playerManager.PlayBusyHashedAnimationWithRootMotion(hashSpecialAttack);
            }
            else
            {
                if (heavyAttackComboIndex == 0)
                {
                    playerManager.PlayCrossFadeBusyAnimationWithRootMotion(hashHeavyAttack1, crossFade);
                }
                else if (heavyAttackComboIndex == 1)
                {
                    playerManager.PlayCrossFadeBusyAnimationWithRootMotion(hashHeavyAttack2, crossFade);
                }
            }

            playerManager.staminaStatManager.DecreaseHeavyAttackStamina();

            HandleAttackSpeed();

            heavyAttackComboIndex++;*/
        }


        public bool CanLightAttack()
        {
            return true;
            /*
            if (!this.isActiveAndEnabled)
            {
                return false;
            }

            if (CanAttack() == false)
            {
                return false;
            }

            if (equipmentDatabase.IsStaffEquipped() || equipmentDatabase.IsBowEquipped())
            {
                return false;
            }

            return playerManager.staminaStatManager.HasEnoughStaminaForLightAttack();*/
        }

        public bool CanHeavyAttack()
        {/*
            if (CanAttack() == false)
            {
                return false;
            }

            return playerManager.staminaStatManager.HasEnoughStaminaForHeavyAttack();*/
            return false;
        }

        bool CanAttack()
        {
            return true;
            /*if (playerManager.IsBusy())
            {
                return false;
            }

            if (playerManager.characterBlockController.isBlocking)
            {
                return false;
            }

            if (menuManager.isMenuOpen)
            {
                return false;
            }

            if (playerManager.playerShootingManager.isAiming)
            {
                return false;
            }

            if (playerManager.climbController.climbState != ClimbState.NONE)
            {
                return false;
            }

            if (playerManager.dodgeController.isDodging)
            {
                return false;
            }

            if (uIManager.IsShowingGUI())
            {
                return false;
            }

            if (playerManager.thirdPersonController.isSwimming)
            {
                return false;
            }

            return true;*/
        }

        private void OnDisable()
        {
            ResetStates();
        }


        public void HandlePlayerAttack(IDamageable damageable, Weapon weapon)
        {/*
            if (damageable is not DamageReceiver damageReceiver)
            {
                return;
            }

            if (playerManager.playerBlockController.isCounterAttacking)
            {
                playerManager.playerBlockController.onCounterAttack?.Invoke();
            }

            damageReceiver?.health?.onDamageFromPlayer?.Invoke();

            if (weapon != null && damageReceiver?.health?.weaponRequiredToKill != null && damageReceiver.health.weaponRequiredToKill == weapon)
            {
                damageReceiver.health.hasBeenHitWithRequiredWeapon = true;
            }

            if (weapon != null)
            {
                playerManager.attackStatManager.attackSource = AttackStatManager.AttackSource.WEAPON;
            }
            else
            {
                playerManager.attackStatManager.attackSource = AttackStatManager.AttackSource.UNARMED;
            }*/
        }

        public void ResetIsHeavyAttacking()
        {
            isHeavyAttacking = false;
            character.animator.SetBool(AnimatorParametersConstants.IsCharging, false);
        }


    }
}
