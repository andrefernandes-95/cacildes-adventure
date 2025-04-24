using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_BookChaptersList : MonoBehaviour
    {

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        [Header("UI Components")]
        [SerializeField] ScrollRect chaptersListScrollRect;
        [SerializeField] GameObject chapterItemButtonPrefab;
        [SerializeField] UI_Book uI_Book;
        [SerializeField] TextMeshProUGUI bookTitle;
        [SerializeField] TextMeshProUGUI bookAuthor;

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetupFooter();

            DrawChapters();
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

        void SetupBookTitleAndAuthor()
        {
            if (uI_Book.book == null) return;

            bookTitle.text = Glossary.IsPortuguese() ? uI_Book.book.portugueseBookName : uI_Book.book.englishBookName;
            bookAuthor.text = Glossary.IsPortuguese() ? uI_Book.book.portugueseAuthor : uI_Book.book.englishAuthor;
        }

        void DrawChapters()
        {
            Utils.ClearScrollRect(chaptersListScrollRect);

            if (uI_Book.book == null)
            {
                return;
            }

            SetupBookTitleAndAuthor();

            foreach (BookPage bookPage in uI_Book.book.booksPages)
            {
                GameObject chapterButton = Instantiate(chapterItemButtonPrefab, chaptersListScrollRect.content);

                UI_BookChapterButton chapterButtonComponent = chapterButton.GetComponent<UI_BookChapterButton>();
                chapterButtonComponent.SetupButton(bookPage, uI_Book);

                chapterButtonComponent.GetComponent<Button>().onClick.AddListener(() =>
                {
                    uI_Book.SetBookPage(bookPage);
                });
            }

            StartCoroutine(GiveFocusNextFrame());
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (
                EventSystem.current.currentSelectedGameObject == null &&
                chaptersListScrollRect.transform.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(chaptersListScrollRect.content.GetChild(0).gameObject);
            }
        }

    }
}
