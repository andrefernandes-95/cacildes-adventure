using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(Button))]
    public class UI_QuestObjectiveButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI objectiveLocation;
        [SerializeField] TextMeshProUGUI objectiveDescription;

        [SerializeField] GameObject objectiveStartedIndicator;
        [SerializeField] GameObject objectiveCompletedIndicator;

        Button button => GetComponent<Button>();

        [Header("Quest")]
        [HideInInspector] public QuestObjective questObjective;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        bool revealLocation = false;

        public void SetupButton(QuestObjective questObjective)
        {
            this.questObjective = questObjective;

            UpdateUI();
        }

        void UpdateUI()
        {
            if (questObjective == null) return;

            thumbnail.sprite = questObjective.objectiveImage;

            objectiveDescription.text = Glossary.IsPortuguese() ? questObjective.ptObjective : questObjective.enObjective;

            UpdateObjective();

            objectiveStartedIndicator.SetActive(false);
            objectiveCompletedIndicator.SetActive(false);

            if (questObjective.quest.IsObjectiveCompleted(questObjective))
            {
                objectiveCompletedIndicator.SetActive(true);
            }
            else
            {
                objectiveStartedIndicator.SetActive(true);
            }
        }

        public void OnClickQuestObjective()
        {
            revealLocation = !revealLocation;
            UpdateObjective();
        }

        void UpdateObjective()
        {
            objectiveLocation.text = Glossary.IsPortuguese() ? "Localização: ??? (Clica para revelar)" : "Location: ??? (Click to reveal)";

            if (revealLocation && questObjective.location != null)
            {
                objectiveLocation.text = Glossary.IsPortuguese() ? questObjective.location.portugueseSceneName : questObjective.location.englishSceneName;
            }
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
