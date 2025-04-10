using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_InputButton : MonoBehaviour
    {
        [Header("Optional Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

        [Header("Keyboard")]
        [SerializeField] GameObject keyboardContainer;
        [SerializeField] TextMeshProUGUI keyboardLabel;
        [SerializeField] Image keyboardKeyIcon;

        [Header("Gamepad")]
        [SerializeField] GameObject gamepadButtonContainer;

        [Header("PS4")]
        [SerializeField] Image ps4Icon;

        [Header("Xbox")]
        [SerializeField] Image xboxIcon;

        [Header("Desired Input")]
        public UI_Input uI_Input;

        void OnEnable()
        {
            // If UI_InputButton is on a prefab which does not have access to the Core prefab, we may need to search for starterAssetsInputs before we can use it
            if (starterAssetsInputs == null)
            {
                starterAssetsInputs = FindAnyObjectByType<StarterAssetsInputs>();
            }

            if (starterAssetsInputs == null)
            {
                // This should never happen in theory as long as the Core prefab is present in the scene
                Debug.LogError("Could not find a StarterAssetsInput on the scene!");
                return;
            }

            DisableAll();

            HandleIcon();
        }

        void HandleIcon()
        {
            if (starterAssetsInputs.playerInput.currentControlScheme == "KeyboardMouse")
            {
                EnableKeyboard();
            }
            else if (starterAssetsInputs.playerInput.currentControlScheme == "")
            {
                EnablePS4();
            }
            else if (starterAssetsInputs.playerInput.currentControlScheme == "")
            {
                EnableXbox();
            }
        }

        void DisableAll()
        {
            keyboardContainer.SetActive(false);
            xboxIcon.gameObject.SetActive(false);
            ps4Icon.gameObject.SetActive(false);
            gamepadButtonContainer.gameObject.SetActive(false);
        }

        void EnableKeyboard()
        {
            if (uI_Input.useKeyboardIcon)
            {
                keyboardKeyIcon.sprite = uI_Input.keyboardIcon;
                keyboardLabel.text = "";
            }
            else
            {
                keyboardLabel.text = uI_Input.GetCurrentKeyBinding(starterAssetsInputs.playerInput);
                keyboardKeyIcon.sprite = null;
            }

            keyboardContainer.SetActive(true);
        }

        void EnablePS4()
        {
            ps4Icon.sprite = uI_Input.ps4Icon;

            ps4Icon.gameObject.SetActive(true);
            gamepadButtonContainer.gameObject.SetActive(true);
        }

        void EnableXbox()
        {
            xboxIcon.sprite = uI_Input.xboxIcon;

            xboxIcon.gameObject.SetActive(true);
            gamepadButtonContainer.gameObject.SetActive(true);
        }
    }
}
