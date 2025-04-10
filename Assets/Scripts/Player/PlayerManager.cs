using AF.Animations;
using AF.Equipment;
using AF.Events;
using AF.Footsteps;
using AF.Health;
using AF.Inventory;
using AF.Ladders;
using AF.Reputation;
using AF.Shooting;
using AF.StateMachine;
using TigerForge;
using UnityEngine;

namespace AF
{
    public class PlayerManager : CharacterBaseManager
    {
        public ThirdPersonController thirdPersonController;
        public CharacterWeapons playerWeaponsManager;
        public ClimbController climbController;
        public CharacterRollManager dodgeController;
        public PlayerLevelManager playerLevelManager;
        public PlayerAchievementsManager playerAchievementsManager;
        public CombatNotificationsController combatNotificationsController;
        public StaminaStatManager staminaStatManager;
        public CharacterBaseMagicManager manaManager;
        public PlayerInventory playerInventory;
        public FavoriteItemsManager favoriteItemsManager;
        public PlayerShooter playerShootingManager;
        public ProjectileSpawner projectileSpawner;
        public SyntyCharacterModelManager equipmentGraphicsHandler;
        public FootstepListener footstepListener;
        public PlayerComponentManager playerComponentManager;
        public EventNavigator eventNavigator;
        public PlayerBlockInput playerBlockInput;
        public PlayerBlockController playerBlockController;
        public StarterAssetsInputs starterAssetsInputs;
        public PlayerAnimationEventListener playerAnimationEventListener;
        public PlayerBackstabController playerBackstabController;
        public TwoHandingController twoHandingController;
        public LockOnManager lockOnManager;
        public PlayerReputation playerReputation;
        public PlayerAppearance playerAppearance;
        public RageManager rageManager;
        public PlayerCardManager playerCardManager;
        public ExecutionerManager executionerManager;
        public UIDocumentPlayerHUDV2 uIDocumentPlayerHUDV2;
        public PlayerStateMachine playerStateMachine;
        public PlayerCamera playerCamera;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;

        public EquipmentDatabase equipmentDatabase;

        // Animator Overrides
        RuntimeAnimatorController defaultAnimatorController;

        [Header("IK Helpers")]
        bool _canUseWeaponIK = true;


        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            starterAssetsInputs.onRightHandAttack.AddListener(() =>
            {
                combatManager.currentAttackingMember = AttackingMember.RIGHT_HAND;
            });

            starterAssetsInputs.onLeftHandAttack.AddListener(() =>
            {
                combatManager.currentAttackingMember = AttackingMember.LEFT_HAND;
            });
        }


        public override void ResetStates()
        {
            // First, reset all flags before calling the handlers
            isBusy = false;
            animator.applyRootMotion = false;
            canMove = true;
            canRotate = true;

            SetCanUseIK_True();

            // TODO: Remove
            if (TryGetThirdPersonController(out ThirdPersonController tps))
            {
                tps.canRotateCharacter = true;
            }

            playerInventory.FinishItemConsumption();
            combatManager.ResetStates();
            playerShootingManager.ResetStates();

            dodgeController.ResetStates();
            playerInventory.ResetStates();
            characterPosture.ResetStates();
            characterPoise.ResetStates();
            damageReceiver.ResetStates();

            rageManager.ResetStates();

            playerComponentManager.ResetStates();

            playerWeaponsManager.ResetStates();
            playerWeaponsManager.ShowEquipment();

            playerBlockInput.CheckQueuedInput();


            playerBlockController.ResetStates();

            characterBaseAttackManager.ResetStates();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!dodgeController.isDodging)
            {
                return;
            }

            if (other.TryGetComponent<DamageReceiver>(out var damageReceiver) && damageReceiver.damageOnDodge)
            {
                damageReceiver.TakeDamage(new Damage(
                    physical: 1,
                    fire: 0,
                    frost: 0,
                    lightning: 0,
                    darkness: 0,
                    magic: 0,
                    water: 0,
                    poiseDamage: 0,
                    postureDamage: 0,
                    weaponAttackType: WeaponAttackType.Blunt,
                    statusEffects: null,
                    pushForce: 0,
                    canNotBeParried: false,
                    ignoreBlocking: false
                ));
            }
        }

        public void UpdateAnimatorOverrideControllerClips()
        {
            return;
            /*            SetupAnimRefs();

                        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

                        var clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
                        animatorOverrideController.GetOverrides(clipOverrides);
                        animator.runtimeAnimatorController = defaultAnimatorController;

                        Weapon currentWeapon = equipmentDatabase.GetCurrentRightWeapon();
                        if (currentWeapon != null)
                        {
                            if (currentWeapon.animationOverrides.Count > 0)
                            {
                                UpdateAnimationOverrides(animator, clipOverrides, currentWeapon.animationOverrides);
                            }

                            if (equipmentDatabase.isTwoHanding)
                            {
                                if (currentWeapon.twoHandOverrides != null && currentWeapon.twoHandOverrides.Count > 0)
                                {
                                    List<AnimationOverride> animationOverrides = new();
                                    animationOverrides.AddRange(currentWeapon.twoHandOverrides);
                                    animationOverrides.AddRange(currentWeapon.blockOverrides);
                                    UpdateAnimationOverrides(animator, clipOverrides, animationOverrides);
                                }
                            }
                        }*/
        }

        public override void RefreshAnimationOverrideState()
        {
            // Hack to refresh lock on while switching animations
            if (lockOnManager.isLockedOn)
            {
                LockOnRef tmp = lockOnManager.nearestLockOnTarget;
                lockOnManager.DisableLockOn();
                lockOnManager.nearestLockOnTarget = tmp;
                lockOnManager.EnableLockOn();
            }
        }

        public void UpdateAnimatorOverrideControllerClip(string animationName, AnimationClip animationClip)
        {
            var clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);

            animator.runtimeAnimatorController = defaultAnimatorController;

            clipOverrides[animationName] = animationClip;

            animatorOverrideController.ApplyOverrides(clipOverrides);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        public void SetCanUseIK_False()
        {
            _canUseWeaponIK = false;
        }

        public void SetCanUseIK_True()
        {
            _canUseWeaponIK = true;

            EventManager.EmitEvent(EventMessages.ON_CAN_USE_IK_IS_TRUE);
        }

        public bool CanUseIK()
        {
            return _canUseWeaponIK;
        }

        /// <summary>
        /// Wrapper method to access third person controller
        /// </summary>
        /// <param name="tps"></param>
        /// <returns></returns>
        public bool TryGetThirdPersonController(out ThirdPersonController tps)
        {
            tps = thirdPersonController;

            return tps != null;
        }

        public bool IsJumping()
        {
            return starterAssetsInputs.jump && characterGravity.isGrounded;
        }

        public bool IsFalling()
        {
            return !characterGravity.isGrounded;
        }

        public bool IsAttemptingToDodge()
        {
            return starterAssetsInputs.dodge;
        }

        public bool IsAttemptingToRightAttack()
        {
            return starterAssetsInputs.rightHandBumper;
        }
        public bool IsAttemptingToHeavyRightAttack()
        {
            return starterAssetsInputs.rightHandTrigger;
        }
        public bool IsAttemptingToLeftAttack()
        {
            return starterAssetsInputs.leftHandAttack;
        }
        public bool IsAttemptingAttack()
        {
            return IsAttemptingToRightAttack() || IsAttemptingToLeftAttack() || IsAttemptingToHeavyRightAttack();
        }

        public bool IsSprinting()
        {
            return starterAssetsInputs.sprint;
        }


        public bool IsMoving()
        {
            // Can control player
            return starterAssetsInputs.move != Vector2.zero;
        }

    }
}
