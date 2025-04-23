using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_LoadSaveFile : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] SaveManager saveManager;
        [SerializeField] ScrollRect scrollRect;

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject loadSaveFileButtonPrefab;
        [SerializeField] GameObject cancelButtonPrefab;
        [SerializeField] GameObject deleteSaveFileButtonPrefab;

        [Header("Events")]
        public UnityEvent onReturn;

        [Header("UI Modals")]
        [SerializeField] UI_Modal_ConfirmDeleteSave uI_Modal_ConfirmDeleteSave;

        [Header("Save Button Prefabs")]
        [SerializeField] GameObject saveFileButtonPrefab;

        [Header("Save File Settings")]
        [SerializeField] int MAX_SAVE_FILES_TO_LOAD = 50;

        private GameObject deleteSaveFileButtonInstance;

        void OnEnable()
        {
            starterAssetsInputs.onMenuEvent.AddListener(OnReturn);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetupFooter();

            RefreshUI();

            starterAssetsInputs.onDeleteSaveFile.AddListener(OnDeleteSaveFile);
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(OnReturn);
            starterAssetsInputs.onDeleteSaveFile.RemoveListener(OnDeleteSaveFile);
        }

        public void OnReturn()
        {
            if (!CanUseAction())
            {
                return;
            }

            onReturn?.Invoke();
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();

            deleteSaveFileButtonInstance = uI_FooterIndicator.AddFooterActionButton(deleteSaveFileButtonPrefab, Glossary.IsPortuguese() ? "Apagar Jogo" : "Delete Save File");
            HideDeleteSaveFileButton();

            uI_FooterIndicator.AddFooterActionButton(cancelButtonPrefab, Glossary.IsPortuguese() ? "Regressar" : "Cancel");
            uI_FooterIndicator.AddFooterActionButton(loadSaveFileButtonPrefab, Glossary.IsPortuguese() ? "Confirmar" : "Confirm");
        }

        public void RefreshUI()
        {
            PopulateScrollRectWithSaveFiles();
            StartCoroutine(GiveFocusNextFrame());
        }

        void PopulateScrollRectWithSaveFiles()
        {
            string[] saveFileNames = SaveUtils.GetSaveFileNames(saveManager.SAVE_FILES_FOLDER).Take(MAX_SAVE_FILES_TO_LOAD).ToArray();

            scrollRect.gameObject.SetActive(true);

            Utils.ClearScrollRect(scrollRect);

            foreach (var saveFileName in saveFileNames)
            {
                GameObject saveFileInstance = Instantiate(saveFileButtonPrefab, scrollRect.content);
                if (saveFileInstance.TryGetComponent<UI_SaveFileButton>(out var saveFileButton))
                {
                    saveFileButton.SetupButton(saveFileName, saveManager);

                    saveFileButton.onSelect.AddListener(ShowDeleteSaveFileButton);
                    saveFileButton.onDeselect.AddListener(HideDeleteSaveFileButton);
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

        void OnDeleteSaveFile()
        {
            if (!CanUseAction())
            {
                return;
            }

            if (
                EventSystem.current.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.TryGetComponent(out UI_SaveFileButton saveFileButton)
                && !string.IsNullOrEmpty(saveFileButton.saveFileName))
            {
                Debug.Log("Attempted to delete saveFileButton");
                uI_Modal_ConfirmDeleteSave.OnDeleteSaveFile(saveFileButton.saveFileName);

                // Disable scroll rect
                scrollRect.gameObject.SetActive(false);
            }
        }

        void ShowDeleteSaveFileButton()
        {
            deleteSaveFileButtonInstance.SetActive(true);
        }

        void HideDeleteSaveFileButton()
        {
            deleteSaveFileButtonInstance.SetActive(false);
        }

        bool CanUseAction()
        {
            return uI_Modal_ConfirmDeleteSave.isActiveAndEnabled == false;
        }
    }
}
