using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_Settings : MonoBehaviour
    {
        [Header("Event System")]
        public GameObject defaultSelectedButton;

        [Header("Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [SerializeField] GameSettings gameSettings;

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject confirmButtonPrefab;
        [SerializeField] GameObject cancelButtonPrefab;

        [Header("Events")]
        public UnityEvent onReturn;

        [Header("Settings Sub-menus")]
        [SerializeField] GameObject defaultSettingsPage;
        [SerializeField] GameObject displayControlsSettingsPage;
        [SerializeField] GameObject changeControlsSettingsPage;
        [SerializeField] GameObject gameplaySettingsPage;
        [SerializeField] GameObject graphicsSettingsPage;
        [SerializeField] GameObject languageSettingsPage;
        [SerializeField] GameObject audioSettingsPage;

        [Header("UI Modals")]
        [SerializeField] UI_Modal_RebindKey uI_Modal_RebindKey;

        void OnEnable()
        {
            starterAssetsInputs.onMenuEvent.AddListener(OnReturn);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UpdateUI();

            StartCoroutine(GiveFocusNextFrame());

            SetupFooter();

            OpenDefaultSettings();
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(OnReturn);

            // Save Settings
            gameSettings.SaveSettings();
        }

        void UpdateUI()
        {
        }


        public void OnReturn()
        {
            if (uI_Modal_RebindKey.isActiveAndEnabled)
            {
                return;
            }

            onReturn?.Invoke();
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();
            uI_FooterIndicator.AddFooterActionButton(cancelButtonPrefab, Glossary.IsPortuguese() ? "Regressar" : "Cancel");
            uI_FooterIndicator.AddFooterActionButton(confirmButtonPrefab, Glossary.IsPortuguese() ? "Confirmar" : "Confirm");
        }

        void DisableAllSubMenus()
        {
            defaultSettingsPage.SetActive(false);
            displayControlsSettingsPage.SetActive(false);
            changeControlsSettingsPage.SetActive(false);
            gameplaySettingsPage.SetActive(false);
            graphicsSettingsPage.SetActive(false);
            languageSettingsPage.SetActive(false);
            audioSettingsPage.SetActive(false);
        }

        public void OpenDefaultSettings()
        {
            DisableAllSubMenus();
            defaultSettingsPage.SetActive(true);
        }
        public void OpenDisplayControlsSettings()
        {
            DisableAllSubMenus();
            displayControlsSettingsPage.SetActive(true);
        }
        public void OpenChangeControlsSettings()
        {
            DisableAllSubMenus();
            changeControlsSettingsPage.SetActive(true);
        }
        public void OpenGameplaySettings()
        {
            DisableAllSubMenus();
            gameplaySettingsPage.SetActive(true);
        }
        public void OpenGraphicsSettings()
        {
            DisableAllSubMenus();
            graphicsSettingsPage.SetActive(true);
        }
        public void OpenLanguageSettings()
        {
            DisableAllSubMenus();
            languageSettingsPage.SetActive(true);
        }
        public void OpenAudioSettings()
        {
            DisableAllSubMenus();
            audioSettingsPage.SetActive(true);
        }
    }
}
