using AF.Music;
using AF.Stats;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentStatusEffectApplied : MonoBehaviour
    {
        public UIDocument uIDocument;
        VisualElement root;

        VisualElement background;
        Label label;

        private void Start()
        {
            root = uIDocument.rootVisualElement;

            background = root.Q<VisualElement>("StatusColor");
            label = root.Q<Label>();
            background.style.opacity = 0;
            label.style.opacity = 0;
            root.pickingMode = PickingMode.Ignore;
            root.focusable = false;
        }


        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="statusEffectLabel"></param>
        public void Display(StatusEffect statusEffect)
        {
            label.text = statusEffect.GetAppliedName();
            background.style.backgroundColor = statusEffect.barColor;

            // Ensure elements are visible initially
            background.style.opacity = 1;
            label.style.opacity = 1;

            // Play fade-in animation
            DOTween.To(() => background.resolvedStyle.opacity, x => background.style.opacity = x, 0, 1).SetEase(Ease.Flash);
            DOTween.To(() => label.resolvedStyle.opacity, x => label.style.opacity = x, 0, 10).SetEase(Ease.InQuad);
        }
    }

}
