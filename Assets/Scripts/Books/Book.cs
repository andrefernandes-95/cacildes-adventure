using System;
using System.Linq;
using AF.Events;
using AYellowpaper.SerializedCollections;
using GameAnalyticsSDK;
using NaughtyAttributes;
using TigerForge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [System.Serializable]
    public class BookPage
    {
        [ShowAssetPreview]
        public Sprite paragraphImage;

        public string englishTitle;
        public string portugueseTitle;

        [TextArea(minLines: 6, maxLines: 15)] public string englishParagraph;
        [TextArea(minLines: 6, maxLines: 15)] public string portugueseParagraph;
    }

    [CreateAssetMenu(menuName = "Data / New Book")]

    public class Book : ScriptableObject
    {
        public Color coverColor = new Color(58, 44, 33);

        [Header("Book Name")]
        public string englishBookName;
        public string portugueseBookName;

        [Header("Author")]
        public string englishAuthor;
        public string portugueseAuthor;

        [Header("Pages")]
        public BookPage[] booksPages;

    }
}
