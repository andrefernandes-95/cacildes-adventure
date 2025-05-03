using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_QuestsList : MonoBehaviour
    {

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        [Header("UI Components")]
        [SerializeField] ScrollRect questsListScrollRect;
        [SerializeField] GameObject questListButtonPrefab;
        [SerializeField] UI_QuestPreview uI_QuestPreview;
        [SerializeField] TextMeshProUGUI gameCompleteProgressText;

        [Header("Selected Quest")]
        public QuestsDatabase questsDatabase;

        [Header("Debug")]
        public QuestParent debugQuest;

        void Awake()
        {
            if (debugQuest != null)
            {
                debugQuest.SetProgress(0);
            }
        }

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetupFooter();

            DrawQuestsList();

            gameCompleteProgressText.text = (Glossary.IsPortuguese()
                ? "Progresso Total: " : "Overall Progress: ") + questsDatabase.GetGameCompletePercentage() + "%";
        }


        void OnDisable()
        {
        }

        public void Refresh()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();

        }

        void DrawQuestsList()
        {
            Utils.ClearScrollRect(questsListScrollRect);

            List<QuestParent> questsReceived = questsDatabase.questsReceived;

            foreach (QuestParent questReceived in questsReceived)
            {
                GameObject questButton = Instantiate(questListButtonPrefab, questsListScrollRect.content);
                questButton.GetComponent<UI_QuestListItemButton>().SetupButton(questReceived, uI_QuestPreview);
            }

            StartCoroutine(GiveFocusNextFrame());
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (
                EventSystem.current.currentSelectedGameObject == null &&
                questsListScrollRect.content.transform.childCount > 0 &&
                uI_QuestPreview.quest == null)
            {
                EventSystem.current.SetSelectedGameObject(questsListScrollRect.content.GetChild(0).gameObject);
            }
        }

    }
}
