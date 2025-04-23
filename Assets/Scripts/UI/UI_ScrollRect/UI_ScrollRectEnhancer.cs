namespace AF
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(ScrollRect))]
    public class UI_ScrollRectEnhancer : MonoBehaviour
    {
        ScrollRect scrollRect => GetComponent<ScrollRect>();
        StarterAssetsInputs starterAssetsInputs;

        void Awake()
        {
            starterAssetsInputs = FindAnyObjectByType<StarterAssetsInputs>(FindObjectsInactive.Include);
        }

        private void OnEnable()
        {
            starterAssetsInputs.onUpArrowPressed.AddListener(CheckScroll);
            starterAssetsInputs.onDownArrowPressed.AddListener(CheckScroll);
        }

        private void OnDisable()
        {
            starterAssetsInputs.onUpArrowPressed.AddListener(CheckScroll);
            starterAssetsInputs.onDownArrowPressed.AddListener(CheckScroll);
        }
        private void CheckScroll()
        {
            // Get the currently selected UI element
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;


            if (selectedObject != null)
            {
                // Check if it's part of the scrollRect's content
                RectTransform selectedTransform = selectedObject.GetComponent<RectTransform>();
                if (selectedTransform != null && selectedTransform.IsChildOf(scrollRect.content))
                {
                    ScrollToSelectable(selectedTransform);
                }
            }
        }

        private void ScrollToSelectable(RectTransform target)
        {
            Canvas.ForceUpdateCanvases(); // Ensure layout is up to date

            RectTransform content = scrollRect.content;
            RectTransform viewport = scrollRect.viewport;

            // Get world position of the target's center
            Vector3 targetWorldCenter = target.TransformPoint(target.rect.center);

            // Get local delta from viewport center to target center
            Vector3 viewportLocalCenter = viewport.TransformPoint(viewport.rect.center);
            Vector3 localDelta = viewport.InverseTransformPoint(targetWorldCenter) - viewport.InverseTransformPoint(viewportLocalCenter);

            // Only scroll vertically
            Vector2 newAnchoredPosition = content.anchoredPosition - new Vector2(0, localDelta.y);

            content.anchoredPosition = newAnchoredPosition;
        }
    }
}
