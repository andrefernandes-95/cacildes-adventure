using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{
    public class UI_KeyboardKey : MonoBehaviour
    {
        [Header("Optional Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

        void OnEnable()
        {
            // Try to find StarterAssetsInputs if not assigned in the inspector
            if (starterAssetsInputs == null)
            {
                starterAssetsInputs = FindAnyObjectByType<StarterAssetsInputs>();
            }

            // Ensure both StarterAssetsInputs and PlayerInput exist
            if (starterAssetsInputs == null || starterAssetsInputs.playerInput == null)
            {
                Debug.LogError("Could not find a valid StarterAssetsInputs or PlayerInput in the scene!");
                return;
            }

            // Subscribe to control scheme change event
            starterAssetsInputs.playerInput.controlsChangedEvent.AddListener(OnControlSchemeChanged);

            // Check the current control scheme on enable
            OnControlSchemeChanged(starterAssetsInputs.playerInput);
        }

        void OnDisable()
        {
            // Unsubscribe to avoid potential issues when re-enabling
            if (starterAssetsInputs != null && starterAssetsInputs.playerInput != null)
            {
                starterAssetsInputs.playerInput.controlsChangedEvent.RemoveListener(OnControlSchemeChanged);
            }
        }

        private void OnControlSchemeChanged(PlayerInput playerInput)
        {
            // Optional debug for tracking input changes
            Debug.Log($"Control Scheme Changed: {playerInput.currentControlScheme}");

            // Show this UI element only if using Keyboard & Mouse
            gameObject.SetActive(playerInput.currentControlScheme == "KeyboardMouse");
        }
    }
}
