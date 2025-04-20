using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class SliderEnhancer : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        public AudioClip hoverSound;
        public AudioClip selectSound;
        public AudioClip releaseSound;

        public Texture2D hoverCursor;
        public Vector2 cursorHotspot = Vector2.zero;

        private AudioSource audioSource => GetComponent<AudioSource>();

        private float lastReportedValue = 0f;
        private float stepThreshold = 0.5f;
        private float lastSoundTime = 0f;
        private float soundCooldown = 0.1f; // 100ms between sounds

        private void Awake()
        {
            GetComponent<Slider>().onValueChanged.AddListener((value) =>
            {
                if (Time.unscaledTime - lastSoundTime > soundCooldown)

                    if (Mathf.Abs(value - lastReportedValue) >= stepThreshold)
                    {
                        PlaySound(releaseSound);
                        lastReportedValue = value;
                    }
            });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(hoverSound);
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlaySound(selectSound);
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
