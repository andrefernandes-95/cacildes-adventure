using System.Collections;
using AF.Health;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{

    public class PlayerBackstabController : MonoBehaviour
    {
        public readonly int hashBackstabExecution = Animator.StringToHash("Backstab Execution");

        public LayerMask characterLayer;

        [Header("Options")]
        public float backStabAngle = 90f;

        [Header("FX")]
        public UnityEvent onBackstab;

        [Header("Components")]
        public PlayerManager playerManager;
        public Transform playerEyesRef;

        bool dontAllowBackstab = false;
        float maxCooldownBeforeBackstabingAgain = 5f;

        Coroutine ResetDontAllowBackstabCoroutine;

        public bool PerformBackstab()
        {
            CharacterManager enemy = GetPossibleTarget();

            if (enemy != null && CanBackstab(enemy, playerManager.GetAttackDamage()))
            {
                enemy.characterPosture.isStunned = true;
                enemy.damageReceiver.waitingForBackstab = true;

                bool isBackstabbing = false;
                // If backstab sucess
                enemy.damageReceiver.HandleIncomingDamage(playerManager, (incomeDamage) =>
                {
                    isBackstabbing = true;
                    enemy.transform.position = playerManager.transform.position;
                    playerManager.transform.rotation = enemy.transform.rotation;
                    playerManager.playerComponentManager.DisablePlayerControlAndRegainControlAfterResetStates();
                    enemy.targetManager.SetTarget(playerManager, true);

                    playerManager.PlayBusyHashedAnimationWithRootMotion(hashBackstabExecution);
                    Invoke(nameof(PlayDelayedBackstab), 0.8f);

                    DisableBackstab();
                }, false);

                return isBackstabbing;
            }

            return false;
        }

        void DisableBackstab()
        {
            dontAllowBackstab = true;

            if (ResetDontAllowBackstabCoroutine != null)
            {
                StopCoroutine(ResetDontAllowBackstabCoroutine);
            }

            ResetDontAllowBackstabCoroutine = StartCoroutine(ResetDontAllowBackstab_Coroutine());
        }

        IEnumerator ResetDontAllowBackstab_Coroutine()
        {
            yield return new WaitForSeconds(maxCooldownBeforeBackstabingAgain);
            dontAllowBackstab = false;
        }

        void PlayDelayedBackstab()
        {
            onBackstab?.Invoke();
        }

        CharacterManager GetPossibleTarget()
        {
            // Get the forward direction of the player
            Vector3 playerForward = playerEyesRef.transform.forward;

            // Cast a ray from the player's chest forward
            if (Physics.Raycast(playerEyesRef.transform.position, playerForward, out RaycastHit hit, 1f, characterLayer))
            {
                float angle = Vector3.Angle(playerEyesRef.transform.forward, hit.transform.forward);
                if (hit.transform != null && angle < backStabAngle + playerManager.statsBonusController.backStabAngleBonus)
                {
                    hit.transform.TryGetComponent<CharacterManager>(out var character);

                    return character;
                }
            }

            return null;
        }

        public bool CanBackstab(CharacterManager target, Damage incomingDamage)
        {
            if (dontAllowBackstab)
            {
                return false;
            }

            if (!target.damageReceiver.canBeBackstabbed)
            {
                return false;
            }

            if (target.characterPosture.isStunned)
            {
                return false;
            }

            if (target != null && target.health != null && target.health.GetCurrentHealth() - incomingDamage.GetTotalDamage() <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
