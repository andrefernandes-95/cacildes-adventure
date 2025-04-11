namespace AF
{
    using TMPro;
    using UnityEngine;

    public class UI_CharacterDamagePopup : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageText;

        private void Awake()
        {
            Hide();
        }

        public void Show(int damage)
        {
            damageText.text = "-" + damage.ToString();
            Refresh();
        }

        void Refresh()
        {
            Hide();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
