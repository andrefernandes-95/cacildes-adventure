using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AF
{
    [CreateAssetMenu(fileName = "UI Input", menuName = "Data / New UI Input", order = 0)]
    public class UI_Input : ScriptableObject
    {
        [Tooltip("Must match the action name in StarterAssetsInput")]
        public string actionName;
        public Sprite ps4Icon;
        public Sprite xboxIcon;
        public string GetCurrentKeyBinding(PlayerInput playerInput)
        {
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput is null.");
                return "";
            }

            var action = playerInput.actions[actionName];
            if (action == null)
            {
                Debug.LogError($"{actionName} not found in playerInput.actions");
                return "";
            }

            var binding = action.bindings.FirstOrDefault(b => !b.isComposite && !b.isPartOfComposite);
            if (binding == null)
            {
                Debug.LogWarning($"No valid binding found for action: {actionName}");
                return "";
            }

            return InputControlPath.ToHumanReadableString(
                binding.effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            ).ToUpper();
        }
    }
}
