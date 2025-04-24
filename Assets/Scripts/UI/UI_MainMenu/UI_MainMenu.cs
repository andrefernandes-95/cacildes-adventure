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
        [SerializeField] UI_SettingsTab uI_SettingsTab;
        [SerializeField] UI_QuestsTab ui_QuestsTab;

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

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
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
            else if (defaultIsSystem)
            {
                OpenSettingsTab();
            }
            else if (defaultIsQuests)
            {
                OpenQuestsTab();
            }
        }

        void ToggleMainMenu()
        {
            if (isActiveAndEnabled)
            {

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Activate player hud if exiting main menu
                uI_PlayerHUD.gameObject.SetActive(true);

                gameObject.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

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
            uI_SettingsTab.gameObject.SetActive(false);
            ui_QuestsTab.gameObject.SetActive(false);
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
        public void OpenSettingsTab()
        {
            DisableAllTabs();
            uI_SettingsTab.gameObject.SetActive(true);
            settingsTabNavbarButton.Select();
        }
        public void OpenQuestsTab()
        {
            DisableAllTabs();
            ui_QuestsTab.gameObject.SetActive(true);
            questsTabNavbarButton.Select();
        }

    }
}
