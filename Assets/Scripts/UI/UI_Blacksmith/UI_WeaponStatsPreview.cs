namespace AF
{
    using AF.UI;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class UI_WeaponStatsPreview : MonoBehaviour
    {

        [Header("UI")]
        public GameObject statusEffectsContainers;

        [Header("UI - Pieces")]
        CanvasGroup canvasGroup => GetComponent<CanvasGroup>();
        public TextMeshProUGUI weaponNameLabel;
        public UI_UpgradeItemTooltip_WeaponLevel weaponLevelTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage physicalDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage fireDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage frostDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage waterDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage magicDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage lightningDamageTooltip;
        public UI_UpgradeItemTooltip_WeaponDamage darknessDamageTooltip;

        [Header("Prefabs")]
        public UI_UpgradeItemTooltip_WeaponDamage statusEffectTooltipPrefab;

        public void ShowUI(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                return;
            }

            DrawWeaponStats(weaponInstance);

            canvasGroup.alpha = 1f;
        }

        public void HideUI()
        {
            canvasGroup.alpha = 0f;
        }

        void DrawWeaponStats(WeaponInstance weaponInstance)
        {
            Utils.ClearChildren(statusEffectsContainers.transform);

            Weapon weapon = weaponInstance.GetItem<Weapon>();
            weaponNameLabel.text = weapon.GetName();

            weaponLevelTooltip.ShowTooltip(weaponInstance);

            physicalDamageTooltip.ShowTooltip(weaponInstance);

            fireDamageTooltip.ShowTooltip(weaponInstance);
            frostDamageTooltip.ShowTooltip(weaponInstance);
            waterDamageTooltip.ShowTooltip(weaponInstance);
            magicDamageTooltip.ShowTooltip(weaponInstance);
            lightningDamageTooltip.ShowTooltip(weaponInstance);
            darknessDamageTooltip.ShowTooltip(weaponInstance);

            foreach (var statusEffect in weapon.damage.statusEffects)
            {
                if (statusEffect == null) continue;

                var statusEffectTooltip = Instantiate(statusEffectTooltipPrefab, statusEffectsContainers.transform);
                statusEffectTooltip.gameObject.SetActive(true);
                statusEffectTooltip.statusEffect = statusEffect.statusEffect;
                statusEffectTooltip.ShowTooltip(weaponInstance);
            }
        }

    }
}
