using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_Modal_ConfirmDeleteSave : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] UI_LoadSaveFile uI_LoadSaveFile;
        [SerializeField] SaveManager saveManager;

        [Header("Footer")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject cancelButtonPrefab;

        [Header("Sounds")]
        [SerializeField] AudioClip onDeletedSaveSfx;
        [SerializeField] AudioClip onCancelSfx;
        [SerializeField] Soundbank soundbank;

        [Header("UI Save File Info")]
        [SerializeField] Image saveFileThumbnail;
        [SerializeField] TextMeshProUGUI saveFileLabelDate;
        [SerializeField] TextMeshProUGUI saveFileLabelName;

        [Header("Event System")]
        [SerializeField] GameObject defaultSelectedGameObject;

        string saveFileNameToDelete = string.Empty;

        private void OnEnable()
        {
            inputs.onMenuEvent.AddListener(Close);

            EventSystem.current.SetSelectedGameObject(defaultSelectedGameObject);
        }

        private void OnDisable()
        {
            saveFileNameToDelete = string.Empty;
            inputs.onMenuEvent.RemoveListener(Close);
        }

        public void OnDeleteSaveFile(string saveFileName)
        {
            this.saveFileNameToDelete = saveFileName;

            uI_FooterIndicator.Refresh();

            uI_FooterIndicator.AddFooterActionButton(
                cancelButtonPrefab, Glossary.IsPortuguese() ? "Cancelar" : "Cancel");

            SetupSaveFileInfo(saveFileName);

            gameObject.SetActive(true);
        }

        void SetupSaveFileInfo(string saveFileName)
        {
            saveFileLabelDate.text = "";
            saveFileLabelName.text = saveFileName;

            Texture2D screenshotThumbnail = SaveUtils.GetScreenshotFilePath(saveManager.SAVE_FILES_FOLDER, saveFileName);
            saveFileThumbnail.sprite = UIUtils.CreateSpriteFromTexture(screenshotThumbnail);
        }

        public void Close()
        {
            uI_LoadSaveFile.RefreshUI();
            gameObject.SetActive(false);
        }

        public void OnCancel()
        {
            soundbank.PlaySound(onCancelSfx);

            Close();
        }

        public void OnConfirmDeleteSaveFile()
        {
            soundbank.PlaySound(onDeletedSaveSfx);

            if (!string.IsNullOrEmpty(saveFileNameToDelete))
            {
                saveManager.DeleteSaveFile(saveFileNameToDelete);
            }

            Close();
        }
    }
}
