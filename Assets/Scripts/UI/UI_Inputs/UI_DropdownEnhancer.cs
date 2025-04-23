using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class UI_DropdownEnhancer : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        public AudioClip selectSound;
        public AudioClip valueChangedSound;

        public Texture2D hoverCursor;
        public Vector2 cursorHotspot = Vector2.zero;

        private AudioSource audioSource => GetComponent<AudioSource>();
        [SerializeField] TMP_Dropdown dropdown;

        private void Awake()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(selectSound);
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlaySound(selectSound);
        }

        private void OnDropdownValueChanged(int index)
        {
            if (index != -1)
            {
                PlaySound(valueChangedSound);
            }
        }

        private void OnDisable()
        {
            ResetCursor();
        }

        private void OnMouseExit()
        {
            ResetCursor();
        }

        private void ResetCursor()
        {
            Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto);
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
