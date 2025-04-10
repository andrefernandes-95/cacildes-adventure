using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace AF
{
    public static class UIUtils
    {
        #region UI Toolkit Stuff - Will be deprecated soon
        public static void SetupButton(Button button, UnityAction callback, Soundbank soundbank)
        {
            SetupButton(button, callback, null, null, true, soundbank);
        }

        public static void SetupButton(
            Button button,
            UnityAction callback,
            UnityAction onFocusInCallback,
            UnityAction onFocusOutCallback,
            bool hasPopupAnimation,
            Soundbank soundbank)
        {
            button.RegisterCallback<ClickEvent>(ev =>
            {
                if (hasPopupAnimation)
                {
                    PlayPopAnimation(button);
                }

                soundbank.PlaySound(soundbank.uiHover);
                callback.Invoke();
            });
            button.RegisterCallback<NavigationSubmitEvent>(ev =>
            {
                if (hasPopupAnimation)
                {
                    PlayPopAnimation(button);
                }

                soundbank.PlaySound(soundbank.uiDecision);
                callback.Invoke();
            });

            button.RegisterCallback<FocusInEvent>(ev =>
            {
                if (hasPopupAnimation)
                {
                    PlayPopAnimation(button);
                }

                soundbank.PlaySound(soundbank.uiHover);
                onFocusInCallback?.Invoke();
            });
            button.RegisterCallback<PointerEnterEvent>(ev =>
            {

                soundbank.PlaySound(soundbank.uiHover);
                onFocusInCallback?.Invoke();
            });


            button.RegisterCallback<FocusOutEvent>(ev =>
            {
                onFocusOutCallback?.Invoke();
            });
            button.RegisterCallback<PointerOutEvent>(ev =>
            {
                onFocusOutCallback?.Invoke();
            });
        }

        public static void PlayPopAnimation(VisualElement button)
        {
            PlayPopAnimation(button, Vector3.zero);
        }

        public static void PlayPopAnimation(VisualElement button, Vector3 startingScale)
        {
            button.transform.scale = Vector3.one;

            DOTween.To(
                () => startingScale,
                scale => button.transform.scale = scale,
                Vector3.one,
                0.5f
            ).SetEase(Ease.OutElastic);
        }

        public static void ScrollToLastPosition(int currentIndex, ScrollView scrollView, UnityAction onFinish)
        {
            VisualElement lastElement = null;

            int lastScrollElementIndex = currentIndex;

            if (lastScrollElementIndex != -1 && scrollView?.childCount > 0)
            {
                while (lastScrollElementIndex >= 0 && lastScrollElementIndex + 1 < scrollView.childCount && lastElement == null)
                {
                    lastElement = scrollView?.ElementAt(lastScrollElementIndex + 1);

                    if (lastElement != null)
                    {
                        lastElement.Focus();
                        scrollView.ScrollTo(lastElement);
                        break;
                    }
                    else
                    {
                        lastScrollElementIndex--;
                    }
                }

            }

            onFinish();
        }

        #endregion


        /// <summary>
        /// Plays a pop animation (scale up and back) on the target GameObject.
        /// </summary>
        /// <param name="target">The GameObject to animate</param>
        /// <param name="popScale">How big the "pop" gets (default 1.2x)</param>
        /// <param name="duration">Total duration of the animation</param>
        public static void PlayPopEffect(this GameObject target, float popScale = 1.2f, float duration = 0.25f)
        {
            if (target == null) return;

            Transform t = target.transform;

            t.DOKill(); // Kill any existing tweens to avoid overlap
            t.localScale = Vector3.zero;

            // Pop scale up, then bounce back to 1
            Sequence popSeq = DOTween.Sequence();
            popSeq.Append(t.DOScale(popScale, duration * 0.4f).SetEase(Ease.OutBack));
            popSeq.Append(t.DOScale(1f, duration * 0.6f).SetEase(Ease.OutQuad));
        }
    }
}
