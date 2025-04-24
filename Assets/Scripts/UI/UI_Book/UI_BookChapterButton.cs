using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_BookChapterButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {

        [Header("Components")]
        [SerializeField] TextMeshProUGUI bookChapterTitle;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        BookPage bookPage;
        UI_Book uI_Book;

        public void SetupButton(BookPage bookPage, UI_Book uI_Book)
        {
            this.bookPage = bookPage;
            this.uI_Book = uI_Book;

            UpdateUI();
        }

        void UpdateUI()
        {
            if (bookPage == null) return;

            bookChapterTitle.text = Glossary.IsPortuguese() ? bookPage.portugueseTitle : bookPage.englishTitle;
        }

        public void OnClickChapter()
        {
            uI_Book.currentBookPage = bookPage;
            uI_Book.ShowBookPage();
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
