using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "AI Fall State", menuName = "States / AI / New Fall State")]
    public class AIFallState : BaseFallState
    {
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 5f;

        bool hasUpdatedFallBegin = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            SetInAirTimer(characterManager.animator, 0f);
            PlayFall(characterManager.animator);

            if (!hasUpdatedFallBegin)
            {
                hasUpdatedFallBegin = true;
                characterManager.characterGravity.UpdateFallBegin();
            }

            // Already grounded?
            if (characterManager.characterGravity.isGrounded)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
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

            if (characterBaseManager.characterGravity.GetFallHeight() > 1)
            {
                SetInAirTimer(characterBaseManager.animator, 1f);
            }

            hasUpdatedFallBegin = false;
        }

    }
}
