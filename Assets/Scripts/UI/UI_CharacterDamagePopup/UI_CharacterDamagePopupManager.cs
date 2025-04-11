namespace AF
{
    using UnityEngine;

    public class UI_CharacterDamagePopupManager : MonoBehaviour
    {
        public UI_CharacterDamagePopup physicalDamagePopup;
        public UI_CharacterDamagePopup fireDamagePopup;
        public UI_CharacterDamagePopup waterDamagePopup;
        public UI_CharacterDamagePopup frostDamagePopup;
        public UI_CharacterDamagePopup magicDamagePopup;
        public UI_CharacterDamagePopup lightningDamagePopup;
        public UI_CharacterDamagePopup darknessDamagePopup;

        public void ShowPhysicalDamage(int damage)
        {
            physicalDamagePopup.Show(damage);
        }

        public void ShowFireDamage(int damage)
        {
            fireDamagePopup.Show(damage);
        }

        public void ShowWaterDamage(int damage)
        {
            waterDamagePopup.Show(damage);
        }

        public void ShowFrostDamage(int damage)
        {
            frostDamagePopup.Show(damage);
        }

        public void ShowMagicDamage(int damage)
        {
            magicDamagePopup.Show(damage);
        }

        public void ShowLightningDamage(int damage)
        {
            lightningDamagePopup.Show(damage);
        }

        public void ShowDarknessDamage(int damage)
        {
            darknessDamagePopup.Show(damage);
        }
    }
}
