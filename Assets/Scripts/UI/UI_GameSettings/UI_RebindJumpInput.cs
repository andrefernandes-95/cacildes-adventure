using TMPro;
using UnityEngine;

namespace AF
{
    public class UI_RebindJumpInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private StarterAssetsInputs starterAssetsInputs;
        [SerializeField] UI_Modal_RebindKey uI_Modal_RebindKey;

        [Header("Current Assigned Key")]
        [SerializeField] private TextMeshProUGUI value;

        private void OnEnable()
        {
            Refresh();
        }

        void Refresh()
        {
            value.text = $"[{starterAssetsInputs.GetHumanReadableCurrentKeyBinding("Jump")}]";
        }

        public void OnRebindJump()
        {
            string title = "Jump";
            if (Glossary.IsPortuguese())
            {
                title = "Saltar";
            }

            title += $" [{starterAssetsInputs.GetHumanReadableCurrentKeyBinding("Jump")}]";


            uI_Modal_RebindKey.OnRebindKey("Jump", title, Refresh);
        }
    }
}
