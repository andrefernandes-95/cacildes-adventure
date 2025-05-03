using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_UpgradeableWeaponsList : MonoBehaviour
    {

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        [Header("Character")]
        public CharacterBaseManager character;

        [Header("Footer Buttons")]
        public GameObject exitBlacksmithButtonPrefab;
        public GameObject confirmUpgradeButtonPrefab;
        GameObject confirmUpgradeButtonInstance;


        [Header("UI Components")]
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] UI_UpgradeableWeaponButton upgradeableWeaponItemButtonPrefab;
        [SerializeField] UI_WeaponStatsPreview uI_WeaponStatsPreview;
        [SerializeField] UI_UpgradeCostsPreview uI_UpgradeCostsPreview;

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            uI_WeaponStatsPreview.HideUI();
            uI_UpgradeCostsPreview.HideUI();

            SetupFooter();

            DrawWeaponsList();
        }

        public void Refresh()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();

            uI_FooterIndicator.AddFooterActionButton(exitBlacksmithButtonPrefab, Glossary.IsPortuguese() ? "Sair da bigorna" : "Leave anvil");
            confirmUpgradeButtonInstance = uI_FooterIndicator.AddFooterActionButton(confirmUpgradeButtonPrefab, Glossary.IsPortuguese() ? "Melhorar arma" : "Upgrade weapon");
            confirmUpgradeButtonInstance.SetActive(false);
        }

        List<WeaponInstance> GetUpgradeableWeapons()
        {
            return character.characterBaseInventory.GetAllWeaponInstances().Where(itemEntry => itemEntry.item is Weapon wp && wp.canBeUpgraded)
                .Select(itemEntry => itemEntry).ToList();
        }

        void DrawWeaponsList()
        {
            Utils.ClearScrollRect(scrollRect);

            List<WeaponInstance> upgradeableWeapons = GetUpgradeableWeapons();
            foreach (WeaponInstance upgradeableWeaponInstance in upgradeableWeapons)
            {
                GameObject upgradeableWeaponButton = Instantiate(upgradeableWeaponItemButtonPrefab.gameObject, scrollRect.content);
                UI_UpgradeableWeaponButton upgradeableWeaponButtonComponent = upgradeableWeaponButton.GetComponent<UI_UpgradeableWeaponButton>();
                upgradeableWeaponButtonComponent.weaponInstance = upgradeableWeaponInstance;
                upgradeableWeaponButtonComponent.characterBaseManager = character;

                upgradeableWeaponButtonComponent.UpdateUI();

                upgradeableWeaponButtonComponent.onSelect.AddListener(() =>
                {
                    uI_WeaponStatsPreview.ShowUI(upgradeableWeaponInstance);
                    uI_UpgradeCostsPreview.ShowUI(upgradeableWeaponInstance, character);
                    confirmUpgradeButtonInstance.SetActive(true);
                });

                upgradeableWeaponButtonComponent.onDeselect.AddListener(() =>
                {
                    uI_WeaponStatsPreview.HideUI();
                    uI_UpgradeCostsPreview.HideUI();
                    confirmUpgradeButtonInstance.SetActive(false);
                });


                upgradeableWeaponButtonComponent.onUpgradeWeapon.AddListener(() =>
                {
                    Refresh();
                });
            }

            StartCoroutine(GiveFocusNextFrame());
        }

        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (
                EventSystem.current.currentSelectedGameObject == null &&
                scrollRect.content.transform.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(scrollRect.content.GetChild(0).gameObject);
            }
        }

    }
}
