using DG.Tweening;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentTitleScreen : MonoBehaviour
    {
        UIDocument document => GetComponent<UIDocument>();

        [Header("Components")]
        public TitleScreenManager titleScreenManager;
        public CursorManager cursorManager;
        public UIDocumentTitleScreenCredits uIDocumentTitleScreenCredits;
        public UIDocumentChangelog uIDocumentChangelog;
        public UIDocumentTitleScreenOptions uIDocumentTitleScreenOptions;
        public UIDocumentTitleScreenSaveFiles uIDocumentTitleScreenSaveFiles;
        public Soundbank soundbank;
        public SaveManager saveManager;

        [Header("Game Session")]
        public GameSession gameSession;

        VisualElement root;

        // Tutorial
        public readonly string tutorialSceneName = "Tutorials";

        void LogAnalytic(string eventName)
        {
            if (!GameAnalytics.Initialized)
            {
                GameAnalytics.Initialize();
            }

            GameAnalytics.NewDesignEvent(eventName);
        }
        private void PopIn(Button button)
        {
            // Animate scale to 1.2x size (pop-in)
            button.experimental.animation.Scale(1.2f, 400)
                .OnCompleted(() => PopOut(button));  // After pop-in, call PopOut()
        }

        private void PopOut(Button button)
        {
            // Animate scale back to original size (pop-out)
            button.experimental.animation.Scale(1f, 400)
                .OnCompleted(() => PopIn(button));  // After pop-out, call PopIn() again for looping
        }

        private void OnEnable()
        {
            root = document.rootVisualElement;

            var versionLabel = root.Q<Label>("Version");
            versionLabel.text = Application.version;

            Button newGameButton = root.Q<Button>("NewGameButton");
            Button continueButton = root.Q<Button>("ContinueButton");
            Button loadGameButton = root.Q<Button>("LoadGameButton");
            Button playTutorialButton = root.Q<Button>("PlayTutorialButton");
            Button optionsButton = root.Q<Button>("OptionsButton");
            Button controlsButton = root.Q<Button>("ControlsButton");
            Button creditsButton = root.Q<Button>("CreditsButton");
            Button changelogButton = root.Q<Button>("ChangelogButton");
            Button exitButton = root.Q<Button>("ExitButton");
            Button btnGithub = root.Q<Button>("btnGithub");
            Button joinDiscordButton = root.Q<Button>("JoinDiscord");
            Button gameGuideButton = root.Q<Button>("GuidesButton");
            //            Button btnDiscord = root.Q<Button>("btnDiscord");
            Button btnWebsite = root.Q<Button>("btnWebsite");
            Button btnYoutube = root.Q<Button>("btnYoutube");
            Button btnTwitter = root.Q<Button>("btnTwitter");
            btnTwitter.style.display = DisplayStyle.None;

            Button btnInstagram = root.Q<Button>("btnInstagram");

            UIUtils.SetupButton(newGameButton, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("NewGame"));

                saveManager.ResetGameState(false);
                titleScreenManager.StartGame();
                gameObject.SetActive(false);
            }, soundbank);

            continueButton.SetEnabled(saveManager.HasSavedGame());

            UIUtils.SetupButton(continueButton, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("ContinueSavedGame"));
                saveManager.LoadLastSavedGame(false);
                gameObject.SetActive(false);
            }, soundbank);

            UIUtils.SetupButton(loadGameButton, () =>
            {
                uIDocumentTitleScreenSaveFiles.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }, soundbank);


            UIUtils.SetupButton(joinDiscordButton, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Discord"));
                Application.OpenURL("https://discord.gg/JwnZMc27D2");

                joinDiscordButton.Focus();
            }, soundbank);

            joinDiscordButton.style.scale = new Scale(Vector3.one); // Set initial scale
            joinDiscordButton.RegisterCallback<GeometryChangedEvent>(evt => PopIn(joinDiscordButton));


            UIUtils.SetupButton(playTutorialButton, () =>
            {
                saveManager.fadeManager.FadeIn(1f, () =>
                {
                    SceneManager.LoadScene(tutorialSceneName);
                });
            }, soundbank);

            UIUtils.SetupButton(creditsButton, () =>
            {
                uIDocumentTitleScreenCredits.gameObject.SetActive(true);
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("ShowCredits"));
                gameObject.SetActive(false);
            }, soundbank);

            UIUtils.SetupButton(changelogButton, () =>
            {
                uIDocumentChangelog.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }, soundbank);

            UIUtils.SetupButton(optionsButton, () =>
            {
                uIDocumentTitleScreenOptions.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }, soundbank);

            UIUtils.SetupButton(gameGuideButton, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Game Guides"));
                Application.OpenURL("https://steamcommunity.com/app/2617740/guides/");

                gameGuideButton.Focus();
            }, soundbank);

            UIUtils.SetupButton(exitButton, () =>
            {
                Application.Quit();
            }, soundbank);

            UIUtils.SetupButton(btnGithub, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Github"));

                Application.OpenURL("https://github.com/cacildesadventure/cacildes-adventure");
            }, soundbank);

            /*
                        UIUtils.SetupButton(btnDiscord, () =>
                        {
                            LogAnalytic(AnalyticsUtils.OnUIButtonClick("Discord"));
                            Application.OpenURL("https://discord.gg/JwnZMc27D2");
                        }, soundbank);*/


            /*            UIUtils.SetupButton(btnTwitter, () =>
                        {
                            LogAnalytic(AnalyticsUtils.OnUIButtonClick("Twitter"));

                            Application.OpenURL("https://twitter.com/CacildesGame");
                        }, soundbank);*/

            UIUtils.SetupButton(btnWebsite, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Website"));

                Application.OpenURL("http://cacildesadventure.com/");
            }, soundbank);

            UIUtils.SetupButton(btnYoutube, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Youtube"));

                Application.OpenURL("https://www.youtube.com/@CacildesAdventure");
            }, soundbank);

            UIUtils.SetupButton(btnInstagram, () =>
            {
                LogAnalytic(AnalyticsUtils.OnUIButtonClick("Instagram"));

                Application.OpenURL("https://www.instagram.com/cacildes_adventure/");
            }, soundbank);


            cursorManager.ShowCursor();

            // Delay the focus until the next frame, required as an hack for now
            Invoke(nameof(GiveFocus), 0f);
        }

        void GiveFocus()
        {
            Button newGameButton = root.Q<Button>("NewGameButton");

            newGameButton.Focus();
        }
    }
}
