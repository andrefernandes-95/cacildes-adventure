using TMPro;
using UnityEngine;

namespace AF
{
    public class UI_RebindInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private StarterAssetsInputs starterAssetsInputs;
        [SerializeField] UI_Modal_RebindKey uI_Modal_RebindKey;

        [Header("Current Assigned Key")]
        [SerializeField] private TextMeshProUGUI value;

        [Header("Action")]
        [SerializeField] private string actionName = "Jump";
        [SerializeField] private string enTitle = "Jump";
        [SerializeField] private string ptTitle = "Saltar";

        private void OnEnable()
        {
            Refresh();
        }

        void Refresh()
        {
            value.text = $"[{starterAssetsInputs.GetHumanReadableCurrentKeyBinding(actionName)}]";
        }

        public void OnRebind()
        {
            string title = enTitle;
            if (Glossary.IsPortuguese())
            {
                title = ptTitle;
            }

            title += $" [{starterAssetsInputs.GetHumanReadableCurrentKeyBinding(actionName)}]";


            uI_Modal_RebindKey.OnRebindKey(actionName, title, Refresh);
        }
    }
}
