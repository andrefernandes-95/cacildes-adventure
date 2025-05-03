namespace AF
{
    using AF.UI;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class UI_UpgradeCostsPreview : MonoBehaviour
    {
        [Header("UI")]
        public GameObject requiredMaterialsContainer;
        CanvasGroup canvasGroup => GetComponent<CanvasGroup>();

        [Header("UI - Pieces")]
        public UI_GoldRequiredTooltip goldRequiredTooltip;

        [Header("Prefabs")]
        public UI_UpgradeRequiredItemTooltip requiredItemTooltipPrefab;


        public void ShowUI(WeaponInstance weaponInstance, CharacterBaseManager character)
        {
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                return;
            }

            DrawUpgradeCosts(weaponInstance, character);

            canvasGroup.alpha = 1f;
        }

        public void HideUI()
        {
            canvasGroup.alpha = 0f;
        }

        void DrawUpgradeCosts(WeaponInstance weaponInstance, CharacterBaseManager character)
        {
            Utils.ClearChildren(requiredMaterialsContainer.transform);

            Weapon weapon = weaponInstance.GetItem<Weapon>();

            if (weapon.TryGetNextWeaponUpgradeLevel(out WeaponUpgradeLevel weaponUpgradeForNextLevel))
            {
                if (weaponUpgradeForNextLevel == null)
                {
                    gameObject.SetActive(false);
                    return;
                }

                goldRequiredTooltip.ShowTooltip(character, weaponUpgradeForNextLevel.goldCostForUpgrade);

                foreach (var upgradeMaterialPair in weaponUpgradeForNextLevel.upgradeMaterials)
                {
                    if (upgradeMaterialPair.Key == null) continue;

                    var requiredItemTooltip = Instantiate(requiredItemTooltipPrefab, requiredMaterialsContainer.transform);
                    requiredItemTooltip.gameObject.SetActive(true);
                    requiredItemTooltip.ShowTooltip(character, upgradeMaterialPair.Key, upgradeMaterialPair.Value);
                }
            }
        }
    }
}
