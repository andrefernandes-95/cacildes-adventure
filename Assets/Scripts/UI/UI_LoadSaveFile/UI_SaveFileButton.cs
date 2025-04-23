using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_SaveFileButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public string saveFileName = "";

        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI date;
        [SerializeField] TextMeshProUGUI location;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        public void SetupButton(string saveFileName, SaveManager saveManager)
        {
            this.saveFileName = saveFileName;

            Texture2D screenshotThumbnail = SaveUtils.GetScreenshotFilePath(saveManager.SAVE_FILES_FOLDER, saveFileName);
            thumbnail.sprite = UIUtils.CreateSpriteFromTexture(screenshotThumbnail);

            date.text = "";
            location.text = saveFileName;
        }

        public void OnClickLoadGame()
        {

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
