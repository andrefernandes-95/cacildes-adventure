using System.Collections;
using UnityEngine;

namespace AF
{
    public class EV_PlayAnimation : EventBase
    {
        public Animator animator;

        public string animationName;

        public float crossFadeTime = 0.1f;

        [Header("Player Settings")]
        public bool usePlayerAnimator = false;
        public bool useBusyAnimation = false;

        PlayerManager playerManager;
        public Transform teleportPlayerToThisTransform;

        public override IEnumerator Dispatch()
        {
            yield return null;

            if (usePlayerAnimator)
            {
                if (playerManager == null)
                {
                    playerManager = FindAnyObjectByType<PlayerManager>(FindObjectsInactive.Include);
                }

                animator = playerManager.animator;

                if (teleportPlayerToThisTransform != null)
                {
                    playerManager.playerComponentManager.TeleportPlayer(teleportPlayerToThisTransform);
                }

            }

            if (crossFadeTime <= 0)
            {
                if (usePlayerAnimator && useBusyAnimation)
                {
                    playerManager.PlayBusyAnimation(animationName);
                }
                else
                {
                    animator.Play(animationName);
                }
            }
            else
            {
                if (usePlayerAnimator && useBusyAnimation)
                {
                    playerManager.PlayCrossFadeBusyAnimationWithRootMotion(animationName, crossFadeTime);
                }
                else
                {
                    animator.CrossFade(animationName, crossFadeTime);
                }
            }

            yield return null;

        }
    }
}
