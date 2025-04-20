using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace AF
{
    public class UI_TitleScreen : MonoBehaviour
    {
        [Header("Event System")]
        public GameObject defaultSelectedButton;

        [Header("UI Components")]
        [SerializeField] TextMeshProUGUI gameTitle;
        [SerializeField] TextMeshProUGUI gameVersion;

        [SerializeField] GameObject titleScreenCanvas;

        [Header("Components")]
        [SerializeField] SaveManager saveManager;
        [SerializeField] UI_PlayerHUD uI_PlayerHUD;

        [Header("Links")]
        [SerializeField] string discordLink = "https://discord.gg/JwnZMc27D2";

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void OnEnable()
        {
            UpdateUI();

            StartCoroutine(GiveFocusNextFrame());
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }

        void UpdateUI()
        {
            UpdateGameTitle();
            UpdateGameVersion();
        }

        void UpdateGameTitle()
        {
            string playerNameKey = "playerName";
            var tableEntry = LocalizationSettings.StringDatabase.GetTableEntry("UIDocuments", playerNameKey);
            bool hasPlayerName = tableEntry.Entry != null && !string.IsNullOrEmpty(tableEntry.Entry.Value);

            string playerName = hasPlayerName
                ? LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", playerNameKey)
                : "Cacildes";

            string prefix = Glossary.IsPortuguese() ? "A Aventura de " : "";
            string suffix = Glossary.IsPortuguese() ? "" : " Adventure";

            gameTitle.text = $"{prefix}{playerName}{suffix}";
        }

        void UpdateGameVersion()
        {
            string prefix = Glossary.IsPortuguese() ? "Versão" : "Version";
            gameVersion.text = $"{prefix} {Application.version}";
        }

        public void OnNewGame()
        {
            //            saveManager.ResetGameState(false);
            uI_PlayerHUD.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            titleScreenCanvas.SetActive(false);
        }

        public void OnExitGame()
        {
            Application.Quit();
        }

        public void OnOpenDiscord()
        {
            OpenWebsite(discordLink);
        }

        void OpenWebsite(string url)
        {
            Utils.LogAnalytic(AnalyticsUtils.OnUIButtonClick($"Clicked on url: {url}"));
            Application.OpenURL(url);
        }
    }
}
