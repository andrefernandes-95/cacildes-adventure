using System.Collections.Generic;
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
using AF.Stats;
using TigerForge;
using UnityEngine;

namespace AF
{
    public class PlayerManager : CharacterBaseManager
    {
        public ThirdPersonController thirdPersonController;
        public PlayerWeaponsManager playerWeaponsManager;
        public ClimbController climbController;
        public DodgeController dodgeController;
        public StatsBonusController statsBonusController;
        public PlayerLevelManager playerLevelManager;
        public PlayerAchievementsManager playerAchievementsManager;
        public CombatNotificationsController combatNotificationsController;
        public PlayerCombatController playerCombatController;
        public StaminaStatManager staminaStatManager;
        public ManaManager manaManager;
        public DefenseStatManager defenseStatManager;
        public AttackStatManager attackStatManager;
        public PlayerInventory playerInventory;
        public FavoriteItemsManager favoriteItemsManager;
        public PlayerShooter playerShootingManager;
        public ProjectileSpawner projectileSpawner;
        public EquipmentGraphicsHandler equipmentGraphicsHandler;
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
        protected AnimatorOverrideController animatorOverrideController;
        RuntimeAnimatorController defaultAnimatorController;

        [Header("IK Helpers")]
        bool _canUseWeaponIK = true;


        private void Awake()
        {
            SetupAnimRefs();
        }

        void SetupAnimRefs()
        {
            if (defaultAnimatorController == null)
            {
                defaultAnimatorController = animator.runtimeAnimatorController;
            }
            if (animatorOverrideController == null)
            {
                animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            }
        }

        public override void ResetStates()
        {
            // First, reset all flags before calling the handlers
            isBusy = false;
            animator.applyRootMotion = false;
            SetCanUseIK_True();

            if (TryGetThirdPersonController(out ThirdPersonController tps))
            {
                tps.canRotateCharacter = true;
            }

            playerInventory.FinishItemConsumption();
            playerCombatController.ResetStates();
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

            attackStatManager.ResetStates();
        }

        public override Damage GetAttackDamage()
        {
            Damage attackDamage = attackStatManager.GetAttackDamage();

            if (playerBlockController.isCounterAttacking)
            {
                attackDamage.damageType = DamageType.COUNTER_ATTACK;
            }

            if (playerCardManager.HasCard() && playerCardManager.currentCard.useDamage)
            {
                return playerCardManager.CombineDamageWithCard(attackDamage);
            }

            return attackDamage;
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
            SetupAnimRefs();

            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            var clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);
            animator.runtimeAnimatorController = defaultAnimatorController;

            Weapon currentWeapon = equipmentDatabase.GetCurrentWeapon();
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
            }
        }

        void UpdateAnimationOverrides(Animator animator, AnimationClipOverrides clipOverrides, System.Collections.Generic.List<AnimationOverride> clips)
        {
            foreach (var animationOverride in clips)
            {
                clipOverrides[animationOverride.animationName] = animationOverride.animationClip;
                animatorOverrideController.ApplyOverrides(clipOverrides);
            }

            animator.runtimeAnimatorController = animatorOverrideController;

            RefreshAnimationOverrideState();
        }

        public void RefreshAnimationOverrideState()
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
    }
}
