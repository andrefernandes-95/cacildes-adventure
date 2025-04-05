using UnityEngine;

namespace AF
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

        private void Awake()
        {
            starterAssetsInputs.onNewMenuEvent.AddListener(ToggleMainMenu);

            gameObject.SetActive(false);
        }

        void ToggleMainMenu()
        {
            if (isActiveAndEnabled)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                // Prevent camera from rotating when disabling inputs
                starterAssetsInputs.look = Vector2.zero;
            }
        }
    }
}
