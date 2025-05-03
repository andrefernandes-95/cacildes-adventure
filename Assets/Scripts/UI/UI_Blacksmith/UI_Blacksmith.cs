using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_Blacksmith : MonoBehaviour
    {

        [Header("Components")]
        public UI_Manager uI_Manager;
        public UI_UpgradeableWeaponsList uI_UpgradeableWeaponsList;
        public StarterAssetsInputs starterAssetsInputs;

        [Header("Blacksmith Character")]
        public CharacterBaseManager characterBaseManager;

        [Header("Sounds")]
        public Soundbank soundbank;
        public AudioClip openSfx;
        public AudioClip closeSfx;

        [Header("Debug")]
        public Weapon[] weaponsToDebug;
        public UpgradeMaterial[] upgradeMaterialsToDebug;
        public int goldToDebug = 0;

        void Awake()
        {
            if (weaponsToDebug != null && weaponsToDebug.Length > 0)
            {
                foreach (Weapon weapon in weaponsToDebug)
                {
                    characterBaseManager.characterBaseInventory.AddWeapon(weapon);
                }
            }

            if (upgradeMaterialsToDebug != null && upgradeMaterialsToDebug.Length > 0)
            {
                foreach (UpgradeMaterial upgradeMaterial in upgradeMaterialsToDebug)
                {
                    characterBaseManager.characterBaseInventory.AddUpgradeMaterial(upgradeMaterial);
                }
            }

            if (goldToDebug > 0)
            {
                characterBaseManager.characterBaseGold.AddGold(goldToDebug);
            }

            uI_UpgradeableWeaponsList.Refresh();
        }

        void OnEnable()
        {
            starterAssetsInputs.onMenuEvent.AddListener(Close);

            uI_Manager.HidePlayerHUD();
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(Close);
        }

        public void Close()
        {
            soundbank.PlaySound(closeSfx);
            uI_Manager.ShowPlayerHUD();
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (characterBaseManager != null && characterBaseManager.characterBaseBlacksmithManager != null)
            {
                characterBaseManager.characterBaseBlacksmithManager.EndJob();
            }
        }
    }
}
