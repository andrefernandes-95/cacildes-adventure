using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{

    public class RightBumper_InputHandler : MonoBehaviour
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
                action.action.Enable(); // Ensure it's active
            }
        }

        private void OnDisable()
        {
            if (action != null)
            {
                action.action.performed -= OnPerformed;
                action.action.Disable();
            }
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            if (!starterAssetsInputs.CanUseInput())
            {
                return;
            }

            starterAssetsInputs.rightHandBumper = true;
            playerManager.combatManager.currentAttackingMember = AttackingMember.RIGHT_HAND;
        }

    }
}
