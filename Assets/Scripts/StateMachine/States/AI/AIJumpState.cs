using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "AI Jump State", menuName = "States / AI / New Jump State")]
    public class AIJumpState : AIState
    {
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 5f;
        bool hasEnteredState = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            characterManager.animator.SetBool(AnimatorParametersConstants.Jump, true);

            if (!hasEnteredState)
            {
                hasEnteredState = true;

            }

            if (characterManager.characterGravity.VerticalVelocity >= 0)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.fallState);
            }

            if (characterManager.targetManager.currentTarget != null)
            {
                characterManager.RotateTowards(characterManager.targetManager.currentTarget.transform);
                characterManager.characterController.Move(characterManager.transform.forward * 5f * Time.deltaTime);
            }

            return this;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);
            characterBaseManager.animator.SetBool(AnimatorParametersConstants.Jump, false);
            hasEnteredState = false;
        }
    }
}
