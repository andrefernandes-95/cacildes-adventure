using System.Linq;
using AF.Characters;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    public class ExecutionerManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public readonly string hashExecuting = "Executing";

        private AnimationClip currentExecutedClip;
        private Transform executedTransformRef;

        public CharacterFaction playerFaction;

        public float maximumRange = 3f;

        public LocalizedString targetTooFarWarning;
        public NotificationManager notificationManager;

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="animationClip"></param>
        public void SetExecutionClip(AnimationClip animationClip)
        {
            playerManager.UpdateAnimatorOverrideControllerClip(hashExecuting, animationClip);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="animationClip"></param>
        public void SetExecutedClip(AnimationClip animationClip)
        {
            this.currentExecutedClip = animationClip;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="animationClip"></param>
        public void SetExecutedTransformRef(Transform executedTransformRef)
        {
            this.executedTransformRef = executedTransformRef;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void BeginExecution()
        {
            CharacterManager target = GetClosestEnemy();

            if (target == null)
            {
                return;
            }

            playerManager.PlayBusyAnimationWithRootMotion(hashExecuting);
            target.executionManager.ReceiveExecution(
                currentExecutedClip, executedTransformRef.position, executedTransformRef.rotation, playerManager.GetAttackDamage());
        }


        CharacterManager GetClosestEnemy()
        {
            CharacterManager target = playerManager.lockOnManager.nearestLockOnTarget?.characterManager;
            if (target == null)
            {
                // Get all characters in the scene
                var allCharacters = FindObjectsByType<CharacterManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                // Filter characters by tag "Enemy"
                var enemyCharacters = allCharacters.Where(character => character.CompareTag("Enemy"));

                // Exclude the character that is the same as this character
                var filteredCharacters = enemyCharacters.Where(_character => !_character.characterFactions.Contains(playerFaction));

                // Sort characters by distance to the player
                var closestCharacter = filteredCharacters.OrderBy(
                    character => Vector3.Distance(playerManager.transform.position, character.transform.position))?.FirstOrDefault();

                if (closestCharacter != null)
                {
                    target = closestCharacter;
                }
            }

            if (target != null && Vector3.Distance(target.transform.position, playerManager.transform.position) > maximumRange)
            {
                notificationManager.ShowNotification(targetTooFarWarning.GetLocalizedString());
                return null;
            }

            return target;
        }


        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnExecuting()
        {
        }

    }
}
