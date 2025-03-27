using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentChangelog : MonoBehaviour
    {

        VisualElement root;
        ScrollView scrollPanel;

        [Header("Components")]
        public Soundbank soundbank;

        [Header("UI Components")]
        public UIDocumentTitleScreen uIDocumentTitleScreen;

        [Header("Localization")]
        public LocalizedString ReturnToTitleScreen_LocalizedString;

        List<Changelog> changelogs = new();

        int currentIndex = 0;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnClose()
        {
            if (this.isActiveAndEnabled)
            {
                Close();
            }
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnShowNext()
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex - 1, 0, changelogs.Count - 1);

            soundbank.PlaySound(soundbank.uiHover);

            DrawUI();
        }
        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnShowPrevious()
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }
            soundbank.PlaySound(soundbank.uiHover);

            currentIndex = Mathf.Clamp(currentIndex + 1, 0, changelogs.Count - 1);

            DrawUI();
        }

        void HandleNavigationIndicators()
        {
            if (currentIndex <= 0 || changelogs.Count <= 1)
            {
                root.Q<VisualElement>("ShowNextHint").style.opacity = 0.2f;
            }
            else
            {
                root.Q<VisualElement>("ShowNextHint").style.opacity = 1f;
            }

            if (currentIndex < changelogs.Count - 1 && changelogs.Count > 1)
            {
                root.Q<VisualElement>("ShowPreviousHint").style.opacity = 1f;
            }
            else
            {
                root.Q<VisualElement>("ShowPreviousHint").style.opacity = 0.25f;
            }
        }

        private void OnEnable()
        {
            currentIndex = 0;

            if (changelogs != null && changelogs.Count <= 0)
            {
                changelogs = Resources.LoadAll<Changelog>("Changelogs")
                    .OrderBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .Reverse()
                    .ToList();
            }

            root = GetComponent<UIDocument>().rootVisualElement;
            scrollPanel = root.Q<ScrollView>();

            root.Q<VisualElement>("ShowNextHint").Q<Button>().RegisterCallback<ClickEvent>(ev =>
            {
                OnShowNext();
            });
            root.Q<VisualElement>("ShowPreviousHint").Q<Button>().RegisterCallback<ClickEvent>(ev =>
            {
                OnShowPrevious();
            });


            UIUtils.SetupButton(root.Q<Button>("CloseBtn"), () =>
            {
                Close();
            }, soundbank);


            DrawUI();
        }

        void DrawUI()
        {
            HandleNavigationIndicators();
            scrollPanel.Clear();

            Changelog currentChangelog = changelogs[currentIndex];

            root.Q<Label>("ExpansionLabel").style.display = currentChangelog.updateType == Changelog.UpdateType.EXPANSION ? DisplayStyle.Flex : DisplayStyle.None;
            root.Q<Label>("BigUpdateLabel").style.display = currentChangelog.updateType == Changelog.UpdateType.BIG_UPDATE ? DisplayStyle.Flex : DisplayStyle.None;

            root.Q<Label>("Version").text = currentChangelog.name;

            root.Q<VisualElement>("Thumbnail").style.display = DisplayStyle.None;
            root.Q<Label>("SmallDescription").text = "";
            root.Q<Label>("Date").text = currentChangelog.date;

            if (currentChangelog.changelogThumbnail != null)
            {
                root.Q<VisualElement>("Thumbnail").style.backgroundImage = new StyleBackground(currentChangelog.changelogThumbnail);
                root.Q<VisualElement>("Thumbnail").style.display = DisplayStyle.Flex;
            }

            if (currentChangelog.smallDescription.IsEmpty == false)
            {
                root.Q<Label>("SmallDescription").text = currentChangelog.smallDescription.GetLocalizedString();
                root.Q<Label>("SmallDescription").style.display = DisplayStyle.Flex;
            }

            if (currentChangelog.additions.Count() > 0)
            {
                var sectionTitleClone = new Label
                {
                    text = LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Additions")
                };
                sectionTitleClone.AddToClassList("label-text");
                scrollPanel.Add(sectionTitleClone);

                foreach (var addition in currentChangelog.additions)
                {
                    CreateSection(addition.GetLocalizedString());
                }
            }

            if (currentChangelog.improvements.Count() > 0)
            {
                var sectionTitleClone = new Label
                {
                    text = LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Improvements")
                };
                sectionTitleClone.AddToClassList("label-text");
                sectionTitleClone.style.marginTop = currentChangelog.additions.Count() > 0 ? 10 : 0;
                scrollPanel.Add(sectionTitleClone);

                foreach (var improvement in currentChangelog.improvements)
                {
                    CreateSection(improvement.GetLocalizedString());
                }
            }

            if (currentChangelog.bugfixes.Count() > 0)
            {
                var sectionTitleClone = new Label
                {
                    text = LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Bugfixes")
                };
                sectionTitleClone.AddToClassList("label-text");
                sectionTitleClone.style.marginTop = currentChangelog.additions.Count() > 0 || currentChangelog.improvements.Count() > 0
                    ? 10 : 0;

                scrollPanel.Add(sectionTitleClone);

                foreach (var bugfix in currentChangelog.bugfixes)
                {
                    CreateSection(bugfix.GetLocalizedString());
                }
            }

            if (scrollPanel.childCount > 0)
            {
                scrollPanel.Children().ElementAt(0).Focus();
            }
        }

        void CreateSection(string value)
        {
            var additionLabel = new Label
            {
                text = "-" + value
            };
            additionLabel.AddToClassList("label-text");
            additionLabel.style.unityFontStyleAndWeight = FontStyle.Normal;
            additionLabel.style.fontSize = 12;

            additionLabel.RegisterCallback<FocusInEvent>((ev) =>
            {
                scrollPanel.ScrollTo(additionLabel);
            });

            scrollPanel.Add(additionLabel);
        }

        void Close()
        {
            uIDocumentTitleScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
