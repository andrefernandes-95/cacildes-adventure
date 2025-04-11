using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{

    public class LeftTrigger_InputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference action; // Drag & drop your Sprint action here
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [Header("Components")]
        [SerializeField] PlayerManager playerManager;

        private void OnEnable()
        {
            if (action != null)
            {
                action.action.performed += OnPerformed;
                action.action.canceled += OnCanceled;
                action.action.Enable(); // Ensure it's active
            }
        }

        private void OnDisable()
        {
            if (action != null)
            {
                action.action.performed -= OnPerformed;
                action.action.canceled -= OnCanceled;
                action.action.Disable();
            }
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            if (!starterAssetsInputs.CanUseInput())
            {
                return;
            }

            starterAssetsInputs.leftHandTrigger = true;
            playerManager.combatManager.currentAttackingMember = AttackingMember.LEFT_HAND;
            playerManager.playerBlockController.SetIsBlocking(true);
        }

        private void OnCanceled(InputAction.CallbackContext context)
        {
            if (!starterAssetsInputs.CanUseInput())
            {
                return;
            }

            starterAssetsInputs.leftHandTrigger = false;
            playerManager.playerBlockController.ResetIsBlocking();
        }
    }
}
