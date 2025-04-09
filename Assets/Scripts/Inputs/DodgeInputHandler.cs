using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{

    public class DodgeInputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference action; // Drag & drop your Sprint action here
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

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

            starterAssetsInputs.dodge = true;
            // Reset dodge flag instantly or after a tiny delay if needed
            Invoke(nameof(ResetDodgeFlag), 0.01f);
        }

        private void ResetDodgeFlag()
        {
            starterAssetsInputs.dodge = false;
        }

    }
}
