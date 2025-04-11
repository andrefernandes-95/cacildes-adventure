using UnityEngine;
using DG.Tweening;

namespace AF
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        [Header("GUI Settings")]
        public GUIManager guiManager;
        public bool addToGuiManager = true;

        [Header("Fade Settings")]
        public float fadeDuration = 0.25f;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            if (addToGuiManager)
            {
                guiManager.PushWindow(this);
            }

            canvasGroup.alpha = 1;
        }

        private void OnDisable()
        {
            if (addToGuiManager)
            {
                guiManager.RemoveWindow(this);
            }

            canvasGroup.alpha = 0;
        }
    }
}
