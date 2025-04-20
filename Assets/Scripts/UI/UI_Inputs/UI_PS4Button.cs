using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{
    public class UI_PS4Key : MonoBehaviour
    {
        [Header("Optional Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

        void OnEnable()
        {
            if (starterAssetsInputs == null)
            {
                starterAssetsInputs = FindAnyObjectByType<StarterAssetsInputs>();
            }

            if (starterAssetsInputs == null || starterAssetsInputs.playerInput == null)
            {
                Debug.LogError("Could not find a valid StarterAssetsInputs or PlayerInput in the scene!");
                return;
            }

            starterAssetsInputs.playerInput.controlsChangedEvent.AddListener(OnControlSchemeChanged);

            OnControlSchemeChanged(starterAssetsInputs.playerInput);
        }

        void OnDisable()
        {
            if (starterAssetsInputs != null && starterAssetsInputs.playerInput != null)
            {
                starterAssetsInputs.playerInput.controlsChangedEvent.RemoveListener(OnControlSchemeChanged);
            }
        }

        private void OnControlSchemeChanged(PlayerInput playerInput)
        {
            bool isPS4 = false;

            foreach (var device in playerInput.devices)
            {
                string deviceName = device.displayName.ToLower();
                if (deviceName.Contains("dualshock") || deviceName.Contains("playstation"))
                {
                    isPS4 = true;
                    break;
                }
            }

            gameObject.SetActive(isPS4);
        }
    }
}
