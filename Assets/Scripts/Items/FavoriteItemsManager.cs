using UnityEngine;
using AF.Inventory;

namespace AF
{

    public class FavoriteItemsManager : MonoBehaviour
    {
        [Header("UI Components")]
        public UIDocumentPlayerHUDV2 uIDocumentPlayerHUDV2;
        public UIDocumentCharacterCustomization uIDocumentCharacterCustomization;

        [Header("Game Session")]
        public GameSession gameSession;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;
        public PlayerStatsDatabase playerStatsDatabase;
        public InventoryDatabase inventoryDatabase;

        [Header("Components")]
        public PlayerManager playerManager;
        public MenuManager menuManager;

        bool canSwitch = true;

        void ResetCanSwitchFlag()
        {
            canSwitch = true;
        }

        void UpdateCanSwitchFlag()
        {
            canSwitch = false;
            Invoke(nameof(ResetCanSwitchFlag), 0.1f);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnConsumableUse()
        {
            if (!CanSwitchEquipment())
            {
                return;
            }

            if (playerStatsDatabase.currentHealth <= 0)
            {
                return;
            }

            Consumable currentConsumable = playerManager.characterBaseEquipment.GetConsumable();

            if (currentConsumable == null)
            {
                return;
            }

            int itemAmount = playerManager.playerInventory.GetItemQuantity(currentConsumable);

            if (itemAmount <= 1 && !currentConsumable.isRenewable)
            {
                playerManager.characterBaseEquipment.UnequipCurrentConsumable();
            }

            playerManager.playerInventory.PrepareItemForConsuming(currentConsumable);

            uIDocumentPlayerHUDV2.UpdateEquipment();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnSwitchWeapon()
        {
            if (!CanSwitchEquipment())
            {
                return;
            }

            playerManager.characterBaseEquipment.SwitchRightWeapon();

            uIDocumentPlayerHUDV2.OnSwitchWeapon();

            UpdateCanSwitchFlag();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnSwitchShield()
        {
            if (!CanSwitchEquipment())
            {
                return;
            }

            playerManager.characterBaseEquipment.SwitchLeftWeapon();

            uIDocumentPlayerHUDV2.OnSwitchShield();

            UpdateCanSwitchFlag();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnSwitchConsumable()
        {
            if (!CanSwitchEquipment())
            {
                return;
            }

            playerManager.characterBaseEquipment.SwitchConsumable();

            uIDocumentPlayerHUDV2.OnSwitchConsumable();
            UpdateCanSwitchFlag();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnSwitchSpell()
        {
            if (!CanSwitchEquipment())
            {
                return;
            }

            playerManager.characterBaseEquipment.SwitchSkill();
            UpdateCanSwitchFlag();
        }

        bool CanSwitchEquipment()
        {
            if (!uIDocumentPlayerHUDV2.IsEquipmentDisplayed())
            {
                return false;
            }

            if (menuManager.isMenuOpen)
            {
                return false;
            }

            if (uIDocumentCharacterCustomization.isActiveAndEnabled)
            {
                return false;
            }

            if (playerManager.isBusy)
            {
                return false;
            }

            if (canSwitch == false)
            {
                return false;
            }

            return true;
        }
    }
}
