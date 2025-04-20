using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class UI_ToggleEnhancer : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, IPointerExitHandler, ISubmitHandler
    {
        public AudioClip hoverSound;
        public AudioClip checkedClick;
        public AudioClip uncheckedClick;

        public Texture2D hoverCursor;
        public Vector2 cursorHotspot = Vector2.zero;

        private AudioSource audioSource => GetComponent<AudioSource>();
        private Toggle toggle;

        void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(hoverSound);
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetCursor();
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlaySound(hoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            OnClicked();
        }

        void OnClicked()
        {

            if (toggle == null)
                return;

            PlaySound(toggle.isOn ? checkedClick : uncheckedClick);
        }

        private void OnDisable()
        {
            ResetCursor();
        }

        private void ResetCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
