using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_Credits : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [SerializeField] ScrollRect scrollRect;

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject returnButtonPrefab;
        [SerializeField] GameObject confirmButtonPrefab;

        [Header("Events")]
        public UnityEvent onReturn;

        [Header("Save Button Prefabs")]
        [SerializeField] GameObject creditsButtonPrefab;

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
            uI_FooterIndicator.AddFooterActionButton(confirmButtonPrefab, Glossary.IsPortuguese() ? "Visitar página do autor" : "Visit author's page");
        }

        public void RefreshUI()
        {
            PopulateScrollRectWithSaveFiles();
            StartCoroutine(GiveFocusNextFrame());
        }

        void PopulateScrollRectWithSaveFiles()
        {
            Contributor[] contributors = Resources.LoadAll<Contributor>("Contributors").ToArray();

            scrollRect.gameObject.SetActive(true);

            Utils.ClearScrollRect(scrollRect);

            foreach (Contributor contributor in contributors)
            {
                GameObject saveFileInstance = Instantiate(creditsButtonPrefab, scrollRect.content);
                if (saveFileInstance.TryGetComponent<UI_CreditButton>(out var creditButton))
                {
                    creditButton.SetupButton(contributor);
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
