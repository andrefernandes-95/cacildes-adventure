using UnityEngine;
using DG.Tweening;

namespace AF
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Window : MonoBehaviour
    {
        public GUIManager gUIManager;

        [Header("Fade Settings")]
        public float fadeDuration = 0.25f;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            gUIManager.SetHasActiveGUI(true);
            canvasGroup.alpha = 0f;
            canvasGroup.DOKill(); // Cancel any previous tweens
            canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            gUIManager.SetHasActiveGUI(false);
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
