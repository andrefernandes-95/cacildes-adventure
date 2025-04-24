using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_QuestPreview : MonoBehaviour
    {
        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject confirmButtonPrefab;
        GameObject confirmButtonInstance;

        [Header("UI Components")]
        [SerializeField] GameObject questDetailsContent;
        [SerializeField] ScrollRect questDetailsScrollRect;
        [SerializeField] GameObject questButtonPrefab;
        [SerializeField] GameObject questObjectiveButtonPrefab;

        [Header("Sfx")]
        [SerializeField] AudioClip openQuestDetailsSfx;
        [SerializeField] Soundbank soundbank;

        [Header("Selected Quest")]
        public QuestParent quest;


        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;



            Hide();
        }

        void OnDisable()
        {
            quest = null;
        }

        public void Refresh()
        {
            DrawQuestPreview();
        }


        void DrawQuestPreview()
        {
            Utils.ClearScrollRect(questDetailsScrollRect);

            if (quest == null)
            {
                return;
            }

            GameObject questButton = Instantiate(questButtonPrefab, questDetailsScrollRect.content);
            UI_QuestButton uI_QuestButton = questButton.GetComponent<UI_QuestButton>();

            uI_QuestButton.SetupButton(quest);

            uI_QuestButton.onSelect.AddListener(() =>
            {
                ShowTrackQuestFooterButton();
            });

            uI_QuestButton.onDeselect.AddListener(() =>
            {
                uI_FooterIndicator.Refresh();
            });

            foreach (QuestObjective questObjective in quest.GetActiveObjectives())
            {
                GameObject questObjectiveButton = Instantiate(questObjectiveButtonPrefab, questDetailsScrollRect.content);
                UI_QuestObjectiveButton uI_QuestObjectiveButton = questObjectiveButton.GetComponent<UI_QuestObjectiveButton>();

                uI_QuestObjectiveButton.SetupButton(questObjective);
                uI_QuestObjectiveButton.onSelect.AddListener(() =>
                {
                    ShowRevealLocationFooterButton();
                });

                uI_QuestObjectiveButton.onDeselect.AddListener(() =>
                {
                    uI_FooterIndicator.Refresh();
                });
            }

            StartCoroutine(GiveFocusNextFrame());
        }
        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (EventSystem.current.currentSelectedGameObject == null && questDetailsScrollRect.content.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(questDetailsScrollRect.content.GetChild(0).gameObject);
            }
        }

        void ShowTrackQuestFooterButton()
        {
            uI_FooterIndicator.AddFooterActionButton(confirmButtonPrefab, Glossary.IsPortuguese() ? "Rastrear Missão" : "Track Quest");
        }

        void ShowRevealLocationFooterButton()
        {
            uI_FooterIndicator.AddFooterActionButton(confirmButtonPrefab, Glossary.IsPortuguese() ? "Revelar / Esconder Localização" : "Reveal / Hide Location");
        }

        public void Show()
        {
            questDetailsContent.SetActive(true);
            Refresh();
            soundbank.PlaySound(openQuestDetailsSfx);
        }

        public void Hide()
        {
            questDetailsContent.SetActive(false);
        }
    }
}
