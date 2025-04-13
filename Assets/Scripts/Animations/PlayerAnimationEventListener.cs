using AF.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace AF.Animations
{
    public class PlayerAnimationEventListener : MonoBehaviour, IAnimationEventListener
    {
        public PlayerManager playerManager;

        [Header("Unity Events")]
        public UnityEvent onLeftFootstep;
        public UnityEvent onRightFootstep;
        [HideInInspector] public UnityEvent onHitboxesClosed;
        public Cinemachine.CinemachineImpulseSource cinemachineImpulseSource;

        [Header("Components")]
        public AudioSource combatAudioSource;
        public Soundbank soundbank;


        public void OpenLeftWeaponHitbox()
        {
            if (playerManager.playerWeaponsManager.equippedLeftWeaponInstance != null)
            {
                playerManager.playerWeaponsManager.equippedLeftWeaponInstance.OpenDamageCollider();
            }

            DisableRotation();
        }

        public void CloseLeftWeaponHitbox()
        {
            if (playerManager.playerWeaponsManager.equippedLeftWeaponInstance != null)
            {
                playerManager.playerWeaponsManager.equippedLeftWeaponInstance.CloseDamageCollider();
            }

            AllowCombos();
        }

        public void OpenRightWeaponHitbox()
        {
            if (playerManager.playerWeaponsManager.equippedRightWeaponInstance != null)
            {
                playerManager.playerWeaponsManager.equippedRightWeaponInstance.OpenDamageCollider();
            }

            DisableRotation();
        }

        public void CloseRightWeaponHitbox()
        {
            if (playerManager.playerWeaponsManager.equippedRightWeaponInstance != null)
            {
                playerManager.playerWeaponsManager.equippedRightWeaponInstance.CloseDamageCollider();
            }

            AllowCombos();
        }

        public void OpenLeftFootHitbox()
        {

            DisableRotation();
        }

        public void CloseLeftFootHitbox()
        {
        }

        public void OpenRightFootHitbox()
        {

            DisableRotation();
        }

        public void CloseRightFootHitbox()
        {
        }
        public void EnableRotation()
        {
            playerManager.EnableCanRotate();
        }

        public void DisableRotation()
        {
            if (playerManager.combatManager.isJumpAttacking)
            {
                return;
            }

            playerManager.DisableCanRotate();
        }

        public void EnableRootMotion()
        {
            playerManager.animator.applyRootMotion = true;
        }

        public void DisableRootMotion()
        {
            playerManager.animator.applyRootMotion = false;
        }

        public void FaceTarget()
        {

        }

        public void SetAnimatorBool_True(string parameterName)
        {
            playerManager.animator.SetBool(parameterName, true);
        }

        public void SetAnimatorBool_False(string parameterName)
        {
            playerManager.animator.SetBool(parameterName, false);
        }

        public void OnSpellCast()
        {
            playerManager.playerShootingManager.CastSpell();
        }

        public void OnFireArrow()
        {
            playerManager.playerShootingManager.OnShoot();
        }

        public void OnFireMultipleArrows()
        {
            playerManager.playerShootingManager.ShootWithoutClearingProjectilesAndSpells(false);
        }

        public void OnLeftFootstep()
        {

            onLeftFootstep?.Invoke();
        }

        public void OnRightFootstep()
        {
            onRightFootstep?.Invoke();
        }

        public void OnCloth()
        {
            if (playerManager.thirdPersonController.Grounded)
            {
                soundbank.PlaySound(soundbank.dodge, combatAudioSource);
            }
            else
            {
                soundbank.PlaySound(soundbank.cloth, combatAudioSource);
            }
        }

        public void OnImpact()
        {
            soundbank.PlaySound(soundbank.impact, combatAudioSource);
        }

        public void OnBuff()
        {

        }

        public void OpenCombo()
        {

        }

        public void OnThrow()
        {
            playerManager.projectileSpawner.ThrowProjectile();
        }

        public void OnBlood()
        {
            throw new System.NotImplementedException();
        }

        public void OnShakeCamera()
        {
            cinemachineImpulseSource.GenerateImpulse();
        }

        public void ShowShield()
        {
        }

        public void DropIKHelper()
        {
            playerManager.SetCanUseIK_False();
        }

        public void UseIKHelper()
        {
            playerManager.SetCanUseIK_True();
        }

        public void SetCanTakeDamage_False()
        {
            playerManager.damageReceiver.SetCanTakeDamage(false);
        }

        public void OnWeaponSpecial()
        {
        }

        public void MoveTowardsTarget()
        {
        }

        public void StopMoveTowardsTarget()
        {
        }

        public void OnSwim()
        {
            playerManager.thirdPersonController.OnSwimAnimationEvent();
        }

        public void PauseAnimation()
        {
        }

        public void ResumeAnimation()
        {
        }


        public void StopIframes()
        {
            playerManager.dodgeController.StopIframes();
        }
        public void EnableIframes()
        {
            playerManager.dodgeController.EnableIframes();
        }

        public void OnCard()
        {
            playerManager.playerCardManager.UseCurrentCard();
        }

        public void OnExecuted()
        {
        }

        public void OnExecuting()
        {
            playerManager.executionerManager.OnExecuting();
        }

        public void ShowRifleWeapon()
        {
        }

        public void HideRifleWeapon()
        {
        }

        public void IsFetchingArrow()
        {
            playerManager.isBusy = false;
        }

        public void ApplyJumpVelocity()
        {
            float bonusJumpHeight = playerManager.IsSprinting() ? .5f : 0f;
            playerManager.characterGravity.Jump(bonusJumpHeight);
            playerManager.EnableCanMove();
        }

        public void EnableCanMove()
        {
            playerManager.EnableCanMove();
        }

        public void DisableCanMove()
        {
            playerManager.DisableCanMove();
        }

        public void WarmupSpell()
        {
            if (playerManager.characterBaseMagicManager.currentChargeableSpellAttackAction != null)
            {
                playerManager.characterBaseMagicManager.currentChargeableSpellAttackAction.WarmupSpell(playerManager);
            }
        }

        public void CastSpell()
        {
            if (playerManager.characterBaseMagicManager.currentChargeableSpellAttackAction != null)
            {
                playerManager.characterBaseMagicManager.currentChargeableSpellAttackAction.CastSpell(playerManager);
            }
        }

        public void AllowCombos()
        {
            playerManager.combatManager.allowCombos = true;
        }
    }
}
