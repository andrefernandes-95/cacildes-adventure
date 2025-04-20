using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class UI_Modal_RebindKey : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] UI_Settings uI_Settings;
        [Header("Footer")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;
        [SerializeField] GameObject cancelButtonPrefab;
        [Header("UI")]
        [SerializeField] TextMeshProUGUI title;

        [Header("Sounds")]
        [SerializeField] AudioClip onRebindedSfx;
        [SerializeField] AudioClip onCancelSfx;
        [SerializeField] Soundbank soundbank;

        public void OnRebindKey(string actionName, string titleLabel, UnityAction callback)
        {
            title.text = titleLabel;
            uI_FooterIndicator.Refresh();
            uI_FooterIndicator.AddFooterActionButton(
                cancelButtonPrefab, Glossary.IsPortuguese() ? "Cancelar" : "Cancel");
            gameObject.SetActive(true);
            StartCoroutine(ListenForKey(actionName, callback));
        }

        IEnumerator ListenForKey(string actionName, UnityAction callback)
        {
            yield return null;

            if (string.IsNullOrEmpty(actionName))
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return inputs.Rebind(actionName,
            (bindingPayload) =>
           {
               gameSettings.SetJumpOverrideBindingPayload(bindingPayload);
               callback?.Invoke();
               soundbank.PlaySound(onRebindedSfx);
               gameObject.SetActive(false);
           }, Close);

        }

        public void Close()
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            soundbank.PlaySound(onCancelSfx);
            StopAllCoroutines();
            uI_Settings.SetupFooter();
            gameObject.SetActive(false);
        }
    }
}
