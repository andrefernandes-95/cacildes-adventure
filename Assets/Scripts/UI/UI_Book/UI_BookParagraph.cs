using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_BookParagraph : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI text;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        void OnEnable()
        {
            thumbnail.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }

        public void SetupTitleParagraph(string titleText)
        {
            title.text = titleText;
            title.gameObject.SetActive(true);
        }

        public void SetupImage(Sprite image)
        {
            thumbnail.sprite = image;
            thumbnail.gameObject.SetActive(true);
        }

        public void SetupParagraphText(string text)
        {
            this.text.text = text;
            this.text.gameObject.SetActive(true);
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
