using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_QuestListItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {

        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI questTitle;

        [SerializeField] GameObject questTrackedIndicator;
        [SerializeField] GameObject questCompletedIndicator;

        [Header("Quest")]
        [HideInInspector] public QuestParent quest;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        UI_QuestPreview uI_QuestPreview;

        public void SetupButton(QuestParent quest, UI_QuestPreview uI_QuestPreview)
        {
            this.quest = quest;
            this.uI_QuestPreview = uI_QuestPreview;

            UpdateUI();
        }

        void UpdateUI()
        {
            if (quest == null) return;

            thumbnail.sprite = UIUtils.CreateSpriteFromTexture(quest.questIcon as Texture2D);

            questTitle.text = Glossary.IsPortuguese() ? quest.portugueseName : quest.englishName;

            questTrackedIndicator.SetActive(false);
            questCompletedIndicator.SetActive(false);

            if (quest.IsCompleted())
            {
                questCompletedIndicator.SetActive(true);
            }

            if (quest.questsDatabase.IsQuestTracked(quest))
            {
                questTrackedIndicator.SetActive(true);
            }
        }

        public void OnClickQuest()
        {
            if (uI_QuestPreview.quest == quest)
            {
                uI_QuestPreview.quest = null;
                uI_QuestPreview.Hide();
                return;
            }

            uI_QuestPreview.quest = quest;
            uI_QuestPreview.Show();
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }
    }
}
