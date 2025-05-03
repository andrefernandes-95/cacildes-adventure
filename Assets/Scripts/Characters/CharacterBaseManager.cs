using System;
using System.Collections.Generic;
using System.Linq;
using AF.Animations;
using AF.Characters;
using AF.Combat;
using AF.Equipment;
using AF.Health;
using AF.Stats;
using AF.StatusEffects;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public abstract class CharacterBaseManager : MonoBehaviour
    {

        [Header("Components")]
        public Animator animator;
        public NavMeshAgent agent;
        public CharacterController characterController;

        [Header("Audio Sources")]
        public AudioSource combatAudioSource;

        [Header("Faction")]
        public CharacterFaction[] characterFactions;

        [Header("Flags")]
        public bool isBusy = false;
        public bool isConfused = false;
        public bool canMove = true;
        public bool canRotate = true;
        public bool isTakingDamage = false;

        [Header("Settings")]
        public float rotationSpeed = 6f;

        [Header("Transform References")]
        public Transform lockOnReference; // Player can have a lock on to make calculations easier for line casts, or future multiplayer

        [Header("Components")]
        public StatusController statusController;
        public CharacterBaseHealth health;
        public CharacterAbstractPosture characterPosture;
        public CharacterAbstractPoise characterPoise;
        public CharacterAbstractBlockController characterBlockController;
        public DamageReceiver damageReceiver;
        public CharacterPushController characterPushController;
        public CharacterGravity characterGravity;
        public CharacterRollManager characterRollManager;
        public CharacterEffectsManager characterEffectsManager;
        public CharacterSoundManager characterSoundManager;
        public CombatManager combatManager;
        public CharacterBaseStats characterBaseStats;
        public CharacterBaseEquipment characterBaseEquipment;
        public StatsBonusController statsBonusController;
        public CharacterWeapons characterWeapons;
        public CharacterBaseInventory characterBaseInventory;
        public SyntyCharacterModelManager syntyCharacterModelManager;
        public CharacterBaseAppearance characterBaseAppearance;
        public CharacterBaseAvatar characterBaseAvatar;
        public CharacterBaseDefenseManager characterBaseDefenseManager;
        public CharacterBaseAttackManager characterBaseAttackManager;
        public CharacterBaseGold characterBaseGold;

        // We need a reference to the base class so we can access certain methods that are common to both player and AI
        public CharacterBaseStateMachine characterBaseStateMachine;

        public CharacterBaseMagicManager characterBaseMagicManager;
        public CharacterBaseTargetManager characterBaseTargetManager;
        public CharacterBaseTwoHandingManager characterBaseTwoHandingManager;

        [Header("Job Componentes")]
        public CharacterBaseBlacksmithManager characterBaseBlacksmithManager;

        [Header("Character UIs")]
        public UI_CharacterDamagePopupManager uI_CharacterDamagePopupManager;

        // Animator Overrides
        protected AnimatorOverrideController animatorOverrideController;
        RuntimeAnimatorController defaultAnimatorController;

        public float animatorSpeed = 1f;
        float defaultAnimatorSpeed = 1f;

        public CinemachineImpulseSource cinemachineImpulseSource => GetComponent<CinemachineImpulseSource>();

        protected virtual void Awake()
        {
            // Order of execution is important here

            // Calculate base defense absorption
            characterBaseDefenseManager.RecalculateDamageAbsorbed();

            // Calculate base attack
            characterBaseAttackManager.RecalculateDamages();

            // Initialize synty models
            syntyCharacterModelManager.Initialize();

            // Then evaluate default equipment
            characterBaseEquipment.SetupDefaultEquipment();

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


        public abstract void ResetStates();

        public void RestoreDefaultAnimatorSpeed()
        {
            animator.speed = 1f;
        }

        public bool IsBusy()
        {
            return isBusy;
        }

        public void SetIsBusy(bool value)
        {
            isBusy = value;
        }

        public void UpdateAttackAnimations(AttackAction[] attackActions)
        {
            SetupAnimRefs();

            var clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);

            List<AnimationOverride> animationOverrides = new();
            foreach (var attack in attackActions)
            {
                if (attack.attackAnimations.Count > 0)
                {
                    foreach (var attackClip in attack.attackAnimations)
                    {
                        AnimationOverride animationOverride = new() { animationClip = attackClip.Value, animationName = attackClip.Key.name };
                        animationOverrides.Add(animationOverride);
                    }
                }
            }

            UpdateAnimationOverrides(animator, clipOverrides, animationOverrides);
        }

        void UpdateAnimationOverrides(Animator animator, AnimationClipOverrides clipOverrides, List<AnimationOverride> clips)
        {
            foreach (var animationOverride in clips)
            {
                clipOverrides[animationOverride.animationName] = animationOverride.animationClip;
                animatorOverrideController.ApplyOverrides(clipOverrides);
            }

            animator.runtimeAnimatorController = animatorOverrideController;

            RefreshAnimationOverrideState();
        }

        public virtual void RefreshAnimationOverrideState()
        {
            /*            // Hack to refresh lock on while switching animations
                        if (lockOnManager.isLockedOn)
                        {
                            LockOnRef tmp = lockOnManager.nearestLockOnTarget;
                            lockOnManager.DisableLockOn();
                            lockOnManager.nearestLockOnTarget = tmp;
                            lockOnManager.EnableLockOn();
                        }*/
        }

        public void PlayAnimationWithCrossFade(string animationName)
        {
            PlayAnimationWithCrossFade(animationName, false, false, 0.2f);
        }

        public void PlayAnimationWithCrossFade(string animationName, bool isBusy, bool applyRootMotion, float crossFade)
        {
            this.isBusy = isBusy;
            animator.applyRootMotion = applyRootMotion;

            animator.CrossFade(animationName, 0.2f);
        }

        public void PlayBusyAnimation(string animationName)
        {
            isBusy = true;
            animator.Play(animationName);
        }

        public void PlayBusyAnimationWithRootMotion(string animationName)
        {
            animator.applyRootMotion = true;
            PlayBusyAnimation(animationName);
        }

        public void SetAnimatorFloat(string parameterName, float value, float blendTime = 0.2f)
        {
            animator.SetFloat(parameterName, value, blendTime, Time.deltaTime);
        }

        public void SetAnimatorBool(string parameterName, bool value)
        {
            animator.SetBool(parameterName, value);
        }

        public void PlayCrossFadeBusyAnimationWithRootMotion(string animationName, float crossFade)
        {
            animator.applyRootMotion = true;
            isBusy = true;
            animator.CrossFade(animationName, crossFade);
        }

        #region Hashed Animations
        public void PlayBusyHashedAnimationWithRootMotion(int hashedAnimationName)
        {
            animator.applyRootMotion = true;
            PlayBusyHashedAnimation(hashedAnimationName);
        }

        public void PlayBusyHashedAnimation(int animationName)
        {
            isBusy = true;
            animator.Play(animationName);
        }
        #endregion

        public bool IsFromSameFaction(CharacterBaseManager target)
        {
            return target != null && characterFactions != null
                && characterFactions.Length > 0
                && characterFactions.Any(thisCharactersFaction =>
                    target.characterFactions != null && target.characterFactions.Length > 0 && target.characterFactions.Contains(thisCharactersFaction));

        }


        public void SetIsConfused(bool value)
        {
            this.isConfused = value;
        }

        public void ResetIsConfused()
        {
            this.isConfused = false;
        }

        public void Move(float targetSpeed, Quaternion rotation)
        {
            if (!canMove)
            {
                return;
            }

            Vector3 targetDirection = rotation * Vector3.forward;

            characterController.Move(
                            targetDirection.normalized * (targetSpeed * Time.deltaTime));
        }

        public void EnableCanMove()
        {
            canMove = true;
        }

        public void DisableCanMove()
        {
            canMove = false;
        }

        public void EnableCanRotate()
        {
            canRotate = true;
        }

        public void DisableCanRotate()
        {
            canRotate = false;
        }

        public void RotateTowards(Transform target)
        {

            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            transform.transform.rotation = Quaternion.Slerp(transform.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        public void TeleportToPosition(Vector3 position)
        {
            characterController.enabled = false;
            transform.position = position;
            characterController.enabled = true;
        }
    }
}
