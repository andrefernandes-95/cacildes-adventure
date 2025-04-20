using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{
    public class UI_RemappableKeyLabel : MonoBehaviour
    {
        PlayerInput playerInput;

        [SerializeField] string actionId = "";

        [SerializeField] TextMeshProUGUI keyLabel;

        private void Awake()
        {
            playerInput = FindAnyObjectByType<PlayerInput>();
        }

        private void OnEnable()
        {
            keyLabel.text = GetCurrentKeyBinding();
        }

        public string GetCurrentKeyBinding()
        {
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput is null.");
                return "";
            }

            var action = playerInput.actions[actionId];
            if (action == null)
            {
                Debug.LogError($"{actionId} not found in playerInput.actions");
                return "";
            }

            var binding = action.bindings.FirstOrDefault(b => !b.isComposite && !b.isPartOfComposite);
            if (binding == null)
            {
                Debug.LogWarning($"No valid binding found for action: {actionId}");
                return "";
            }

            return InputControlPath.ToHumanReadableString(
                binding.effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            ).ToUpper();
        }
    }
}
