using System.Collections;
using AF.Events;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class CharacterRollManager : MonoBehaviour
    {
        // Animation hash values
        public readonly int hashRoll = Animator.StringToHash("Roll");
        public readonly int hashBackStep = Animator.StringToHash("BackStep");

        [Header("Components")]
        public CharacterBaseManager character;
        public LockOnManager lockOnManager;
        public UIManager uIManager;

        [Header("Stamina Settings")]
        public int dodgeCost = 15;

        [Header("In-game flags")]
        public bool isDodging = false;

        public float maxRequestForRollDuration = 0.4f;
        [HideInInspector] public float currentRequestForRollDuration = Mathf.Infinity;

        [Header("Dodge Attacks")]
        public int dodgeAttackBonus = 30;

        [Header("Unity Events")]
        public UnityEvent onDodge;

        public void ResetStates()
        {
            isDodging = false;
        }

        public void EnableIframes()
        {
            isDodging = true;

            onDodge?.Invoke();
        }

        public void StopIframes()
        {
            // Has Finished Dodging
            EventManager.EmitEvent(EventMessages.ON_PLAYER_DODGING_FINISHED);

            ResetStates();
        }

        public void AttemptRoll()
        {
            AttemptToPerformDodge(false);
        }

        public void AttemptBackstep()
        {
            AttemptToPerformDodge(true);
        }

        void AttemptToPerformDodge(bool isBackstep)
        {
            if (!CanDodge())
            {
                return;
            }

            //character.staminaStatManager.DecreaseStamina(dodgeCost);
            //character.playerBlockInput.OnBlockInput_Cancelled();

            isDodging = true;

            if (isBackstep)
            {
                character.PlayBusyHashedAnimationWithRootMotion(hashBackStep);
            }
            else
            {
                HandleRoll();
                onDodge?.Invoke();
            }
        }

        void HandleRoll()
        {
            character.PlayBusyHashedAnimationWithRootMotion(hashRoll);

            /*
            if (playerManager.equipmentGraphicsHandler.IsHeavyWeight())
            {
                StartCoroutine(StopHeavyRollRootmotion());
            }
            else if (playerManager.equipmentGraphicsHandler.IsMidWeight())
            {
                StartCoroutine(StopMidRollRootmotion());
            }*/
        }

        IEnumerator StopMidRollRootmotion()
        {
            yield return new WaitForSeconds(0.75f);
            character.animator.applyRootMotion = false;
        }

        IEnumerator StopHeavyRollRootmotion()
        {
            yield return new WaitForSeconds(0.3f);
            isDodging = false;

            yield return new WaitForSeconds(0.3f);
            character.animator.applyRootMotion = false;
        }

        public bool ShouldBackstep()
        {
            return false;
            ///            return character //character.starterAssetsInputs.move == Vector2.zero && playerManager.thirdPersonController.isSliding == false;
        }

        private bool CanDodge()
        {
            if (isDodging)
            {
                return false;
            }

            if (character.IsBusy())
            {
                return false;
            }

            /*
                        if (character.climbController.climbState != ClimbState.NONE)
                        {
                            return false;
                        }

                        if (character.playerCombatController.isCombatting)
                        {
                            return false;
                        }

                        if (!character.thirdPersonController.Grounded || !character.thirdPersonController.canMove)
                        {
                            return false;
                        }

                        if (!character.staminaStatManager.HasEnoughStaminaForAction(dodgeCost))
                        {
                            return false;
                        }*/

            if (uIManager.IsShowingGUI())
            {
                return false;
            }

            return true;
        }
    }
}
