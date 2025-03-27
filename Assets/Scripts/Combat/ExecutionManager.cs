using System.Collections;
using AF.Health;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    public class ExecutionManager : MonoBehaviour
    {
        public CharacterManager characterManager;

        public readonly string hashExecution = "Executed";
        public readonly string hashExecutionToDeath = "ExecutedDead";

        public bool isBeingExecuted = false;

        NotificationManager _notificationManager;

        [Header("UI")]
        public LocalizedString mustBeHumanoidWarning;

        [Header("FX")]
        public GameObject bloodFx;

        public void ResetStates()
        {
            isBeingExecuted = false;
        }

        public void ReceiveExecution(AnimationClip executionClip, Vector3 position, Quaternion rotation, Damage incomingDamage)
        {
            if (isBeingExecuted)
            {
                return;
            }

            if (!characterManager.animator.isHuman)
            {
                GetNotificationManager().ShowNotification(mustBeHumanoidWarning.GetLocalizedString());
                return;
            }

            isBeingExecuted = true;

            characterManager.Teleport(position, rotation);

            characterManager.UpdateAnimatorOverrideControllerClips(hashExecution, executionClip);
            characterManager.UpdateAnimatorOverrideControllerClips(hashExecutionToDeath, executionClip);

            characterManager.targetManager.SetPlayerAsTarget();
            characterManager.damageReceiver.ApplyDamage(incomingDamage);

            StartCoroutine(PlayAnimation());
        }

        IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(0.1f);

            if (characterManager.health.GetCurrentHealth() <= 0)
            {
                characterManager.PlayBusyAnimationWithRootMotion(hashExecutionToDeath);
            }
            else
            {
                characterManager.PlayBusyAnimationWithRootMotion(hashExecution);
            }
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnExecuted()
        {
            Instantiate(bloodFx, characterManager.transform.position, Quaternion.identity);

            isBeingExecuted = false;
        }

        NotificationManager GetNotificationManager()
        {
            if (_notificationManager == null)
            {
                _notificationManager = FindAnyObjectByType<NotificationManager>(FindObjectsInactive.Include);
            }

            return _notificationManager;
        }
    }
}
