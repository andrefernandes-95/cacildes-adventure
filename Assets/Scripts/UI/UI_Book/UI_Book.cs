using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_Book : MonoBehaviour
    {
        [HideInInspector] public Book book;

        [Header("Components")]
        public UI_BookChaptersList chaptersList;
        public UI_FooterIndicator uI_FooterIndicator;
        public GameObject closeBookButtonPrefab;
        public StarterAssetsInputs starterAssetsInputs;

        [Header("Book Page")]
        public ScrollRect paragraphsScrollRect;
        public GameObject paragraphPrefab;

        public BookPage currentBookPage;

        public int maxChunkSize = 500;

        [Header("Debug")]
        public Book debugBook;

        [Header("Sounds")]
        public Soundbank soundbank;
        public AudioClip openPageSfx;
        public AudioClip closeBookSfx;

        void Awake()
        {
            if (debugBook != null)
            {
                OpenBook(debugBook);
            }
        }

        void OnEnable()
        {
            uI_FooterIndicator.AddFooterActionButton(closeBookButtonPrefab, Glossary.IsPortuguese() ? "Fechar Livro" : "Close Bok");
            starterAssetsInputs.onMenuEvent.AddListener(Close);
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(Close);

        }

        public void OpenBook(Book book)
        {
            if (book == null || book.booksPages.Length <= 0)
            {
                Debug.LogError("Book is null or has no pages.");
                return;
            }

            this.book = book;

            SetBookPage(book.booksPages[0]);
            chaptersList.Refresh();
        }

        public void SetBookPage(BookPage bookPage)
        {
            currentBookPage = bookPage;
            ShowBookPage();

            soundbank.PlaySound(openPageSfx);
        }

        public void ShowBookPage()
        {
            if (currentBookPage == null) return;

            Utils.ClearScrollRect(paragraphsScrollRect);

            List<string> paragraphs = new List<string>();
            string text = Glossary.IsPortuguese() ? currentBookPage.portugueseParagraph : currentBookPage.englishParagraph;

            int start = 0;

            while (start < text.Length)
            {
                int end = Mathf.Min(start + maxChunkSize, text.Length);

                // Try to find the last special character before the limit
                int splitPos = -1;
                for (int i = end; i > start; i--)
                {
                    if (".,;:!?-".Contains(text[i - 1]))
                    {
                        splitPos = i;
                        break;
                    }
                }

                // If no special character was found, just cut at maxChunkSize
                if (splitPos == -1) splitPos = end;

                paragraphs.Add(text.Substring(start, splitPos - start).Trim());
                start = splitPos;
            }

            for (int i = 0; i < paragraphs.Count; i++)
            {
                GameObject paragraph = Instantiate(paragraphPrefab, paragraphsScrollRect.content);
                UI_BookParagraph uI_BookParagraph = paragraph.GetComponent<UI_BookParagraph>();

                if (i == 0)
                {
                    uI_BookParagraph.SetupImage(currentBookPage.paragraphImage);
                    uI_BookParagraph.SetupTitleParagraph(
                        Glossary.IsPortuguese() ? currentBookPage.portugueseTitle : currentBookPage.englishTitle);
                }

                paragraph.GetComponent<UI_BookParagraph>().SetupParagraphText(paragraphs[i]);
            }
        }

        public void Close()
        {
            soundbank.PlaySound(closeBookSfx);
            gameObject.SetActive(false);
        }
    }
}
