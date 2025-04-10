using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] StarterAssetsInputs starterAssetsInputs;

        [Header("Tabs")]
        [SerializeField] UI_EquipmentTab uI_EquipmentTab;
        [SerializeField] UI_InventoryTab uI_InventoryTab;

        [Header("Default Tab")]
        public bool defaultIsEquipment = false;
        public bool defaultIsInventory = false;
        public bool defaultIsQuests = false;
        public bool defaultIsSystem = false;

        [Header("Navbar Buttons")]
        [SerializeField] Button equipmentTabNavbarButton;
        [SerializeField] Button inventoryTabNavbarButton;
        [SerializeField] Button questsTabNavbarButton;
        [SerializeField] Button settingsTabNavbarButton;

        [Header("UIs To Hide")]
        [SerializeField] UI_PlayerHUD uI_PlayerHUD;


        private void Awake()
        {
            starterAssetsInputs.onNewMenuEvent.AddListener(ToggleMainMenu);

            gameObject.SetActive(false);
        }

        void OpenDefaultTab()
        {
            if (defaultIsEquipment)
            {

                OpenEquipmentTab();
            }
            else if (defaultIsInventory)
            {
                OpenInventoryTab();
            }
        }

        void ToggleMainMenu()
        {
            if (isActiveAndEnabled)
            {
                // Activate player hud if exiting main menu
                uI_PlayerHUD.gameObject.SetActive(true);

                gameObject.SetActive(false);
            }
            else
            {
                // Deactivate player hud while on main menu
                uI_PlayerHUD.gameObject.SetActive(false);

                gameObject.SetActive(true);

                // Prevent camera from rotating when disabling inputs
                starterAssetsInputs.look = Vector2.zero;

                DisableAllTabs();

                OpenDefaultTab();
            }
        }

        void DisableAllTabs()
        {
            uI_EquipmentTab.gameObject.SetActive(false);
            uI_InventoryTab.gameObject.SetActive(false);
        }

        public void OpenEquipmentTab()
        {
            DisableAllTabs();
            uI_EquipmentTab.gameObject.SetActive(true);
            equipmentTabNavbarButton.Select();
        }

        public void OpenInventoryTab()
        {
            DisableAllTabs();
            uI_InventoryTab.gameObject.SetActive(true);
            inventoryTabNavbarButton.Select();
        }

    }
}
