using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class UI_PrimaryButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private AudioSource audioSource => GetComponent<AudioSource>();
        private RectTransform rectTransform => GetComponent<RectTransform>();

        public AudioClip hover;
        public AudioClip click;

        [Header("Animation Settings")]
        public float popScale = 0.9f;
        public float popDuration = 0.1f;

        [Header("Cursor Settings")]
        public Texture2D hoverCursorTexture;
        public Vector2 hotspot = Vector2.zero;
        public CursorMode cursorMode = CursorMode.Auto;

        private void Awake()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayHover();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClick();
            DoPop();
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlayHover();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PlayClick();
            DoPop();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hoverCursorTexture, hotspot, cursorMode);
            PlayHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, hotspot, cursorMode);
        }

        private void PlayHover()
        {
            if (hover != null)
                audioSource.PlayOneShot(hover);
        }

        private void PlayClick()
        {
            if (click != null)
                audioSource.PlayOneShot(click);
        }

        private void DoPop()
        {
            rectTransform.DOKill(); // cancel any ongoing tweens
            rectTransform.localScale = Vector3.one; // reset scale
            rectTransform
                .DOScale(popScale, popDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    rectTransform.DOScale(1f, popDuration).SetEase(Ease.OutBack);
                });
        }
    }
}
