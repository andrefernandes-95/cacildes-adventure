using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_QuestButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI questTitle;
        [SerializeField] TextMeshProUGUI questDescription;

        [SerializeField] GameObject questTrackedIndicator;
        [SerializeField] GameObject questStartedIndicator;
        [SerializeField] GameObject questCompletedIndicator;

        [Header("Quest")]
        [HideInInspector] public QuestParent quest;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        public void SetupButton(QuestParent quest)
        {
            this.quest = quest;

            UpdateUI();
        }

        void UpdateUI()
        {
            if (quest == null) return;

            thumbnail.sprite = UIUtils.CreateSpriteFromTexture(quest.questIcon as Texture2D);

            questTitle.text = Glossary.IsPortuguese() ? quest.portugueseName : quest.englishName;
            questDescription.text = Glossary.IsPortuguese() ? quest.portugueseDescription : quest.englishDescription;

            questStartedIndicator.SetActive(false);
            questTrackedIndicator.SetActive(false);
            questCompletedIndicator.SetActive(false);

            if (quest.IsCompleted())
            {
                questCompletedIndicator.SetActive(true);
            }
            else if (quest.isStarted)
            {
                questStartedIndicator.SetActive(true);
            }

            if (quest.questsDatabase.IsQuestTracked(quest))
            {
                questTrackedIndicator.SetActive(true);
            }
        }

        public void OnClickQuest()
        {
            quest.Track();

            UpdateUI();
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onSelect?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDeselect?.Invoke();
        }
    }
}
