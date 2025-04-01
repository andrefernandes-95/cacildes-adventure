using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AF.Animations;
using AF.Characters;
using AF.Combat;
using AF.Equipment;
using AF.Health;
using AF.StateMachine;
using AF.Stats;
using AF.StatusEffects;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{
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

        // Animator Overrides
        protected AnimatorOverrideController animatorOverrideController;
        RuntimeAnimatorController defaultAnimatorController;

        public float animatorSpeed = 1f;
        float defaultAnimatorSpeed = 1f;

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

        public abstract Damage GetAttackDamage();

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
    }
}
