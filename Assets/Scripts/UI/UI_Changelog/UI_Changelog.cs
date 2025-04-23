using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_Changelog : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [SerializeField] ScrollRect scrollRect;

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject returnButtonPrefab;

        [Header("Events")]
        public UnityEvent onReturn;

        [Header("Save Button Prefabs")]
        [SerializeField] GameObject changelogButtonPrefab;

        void OnEnable()
        {
            starterAssetsInputs.onMenuEvent.AddListener(OnReturn);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetupFooter();

            RefreshUI();
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(OnReturn);
        }

        public void OnReturn()
        {
            onReturn?.Invoke();
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();

            uI_FooterIndicator.AddFooterActionButton(returnButtonPrefab, Glossary.IsPortuguese() ? "Regressar" : "Cancel");
        }

        public void RefreshUI()
        {
            PopulateScrollRectWithSaveFiles();
            StartCoroutine(GiveFocusNextFrame());
        }

        void PopulateScrollRectWithSaveFiles()
        {
            List<Changelog> changelogs = Resources.LoadAll<Changelog>("Changelogs")
                .OrderBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .Reverse()
                .ToList();


            scrollRect.gameObject.SetActive(true);

            Utils.ClearScrollRect(scrollRect);

            foreach (Changelog changelog in changelogs)
            {
                GameObject saveFileInstance = Instantiate(changelogButtonPrefab, scrollRect.content);
                if (saveFileInstance.TryGetComponent<UI_ChangelogButton>(out var changelogButton))
                {
                    changelogButton.SetupButton(changelog);
                }
            }
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (scrollRect.transform.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(scrollRect.content.GetChild(0).gameObject);
            }
        }


    }
}
