using UnityEngine;

namespace AF
{
    public class UI_Manager : MonoBehaviour
    {
        public UI_PlayerHUD playerHUD;

        [Header("Contextual Menus")]
        public UI_Blacksmith uI_Blacksmith;
        public UI_Alchemy uI_Alchemy;

        public void ShowPlayerHUD()
        {
            playerHUD.gameObject.SetActive(true);
        }

        public void HidePlayerHUD()
        {
            playerHUD.gameObject.SetActive(false);
        }

        public bool CanUseMainMenu()
        {
            if (uI_Blacksmith.isActiveAndEnabled)
            {
                return false;
            }

            if (uI_Alchemy.isActiveAndEnabled)
            {
                return false;
            }

            return true;
        }
    }
}
