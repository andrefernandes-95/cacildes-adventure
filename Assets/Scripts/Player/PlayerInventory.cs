using System.Collections.Generic;
using System.Linq;
using AF.Inventory;
using AF.Ladders;
using AF.StatusEffects;
using AYellowpaper.SerializedCollections;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace AF
{
    public class PlayerInventory : CharacterBaseInventory
    {
        public Consumable currentConsumedItem;

        [Header("UI Components")]
        public NotificationManager notificationManager;
        public UIDocumentPlayerHUDV2 uIDocumentPlayerHUDV2;
        public UIDocumentPlayerGold uIDocumentPlayerGold;

        [Header("Components")]
        public PlayerManager playerManager;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;
        public InventoryDatabase inventoryDatabase;

        [Header("Flags")]
        public bool isConsumingItem = false;

        [Header("Events")]
        public UnityEvent onResetState;

        [Header("Ashes Edge Case")]
        public bool disableAshesUsage = false;
        public Item ashes;
        public UnityEvent onDisabledAshes;

        public void ResetStates()
        {
            isConsumingItem = false;
            onResetState?.Invoke();
        }


        void HandleItemAchievements(Item item)
        {
            if (item is Weapon)
            {
                int numberOfWeapons = GetAllWeaponInstances().Count;

                if (numberOfWeapons <= 0)
                {
                    playerManager.playerAchievementsManager.achievementOnAcquiringFirstWeapon.AwardAchievement();
                }
                else if (numberOfWeapons == 10)
                {
                    playerManager.playerAchievementsManager.achievementOnAcquiringTenWeapons.AwardAchievement();
                }
            }
            else if (item is Spell)
            {
                int numberOfSpells = GetAllSpellInstances().Count;

                if (numberOfSpells <= 0)
                {
                    playerManager.playerAchievementsManager.achievementOnAcquiringFirstSpell.AwardAchievement();
                }
            }
        }

        void LogAnalytic(string eventName)
        {
            if (!GameAnalytics.Initialized)
            {
                GameAnalytics.Initialize();
            }

            GameAnalytics.NewDesignEvent(eventName);
        }

        public override void RemoveItem(Item item)
        {
            ItemInstance itemInstance = GetFirst(item);

            RemoveItemInstance(itemInstance);
        }


        public void RemoveItemInstance(ItemInstance itemInstance)
        {
            if (itemInstance == null)
            {
                return;
            }

            if (itemInstance is ConsumableInstance consumableInstance)
            {
                if (consumableInstance.GetItem<Consumable>().isRenewable)
                {
                    consumableInstance.wasUsed = true;
                    return;
                }

                if (GetItemQuantity(itemInstance.GetItem<Consumable>()) > 0)
                {
                    inventoryDatabase.ownedItems[itemInstance.GetItem<Consumable>()].Remove(itemInstance);
                    return;
                }
            }

            inventoryDatabase.ownedItems[itemInstance.GetItem<Item>()].Remove(itemInstance);

            // If not last arrow, do not unequip item
            if (itemInstance is ArrowInstance arrowInstance && GetItemQuantity(arrowInstance.GetItem<Arrow>()) > 0)
            {
                return;
            }

            //UnequipItemToRemove(itemInstance);
        }

        bool CanConsumeItem(Consumable consumable)
        {
            if (isConsumingItem)
            {
                return false;
            }

            if (consumable.isRenewable && GetItemQuantity(consumable) <= 0)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Consumable depleted"),
                    notificationManager.notEnoughSpells);

                return false;
            }

            if (playerManager.combatManager.isCombatting)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);

                return false;
            }


            if (playerManager.thirdPersonController.isSwimming)
            {

                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);
                return false;
            }

            if (playerManager.characterPosture.isStunned)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);

                return false;
            }

            if (playerManager.dodgeController.isDodging)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);
                return false;
            }

            if (!playerManager.thirdPersonController.Grounded)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);
                return false;
            }

            if (playerManager.climbController.climbState != ClimbState.NONE)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);
                return false;
            }

            if (playerManager.isBusy)
            {
                return false;
            }

            if (playerStatsDatabase.currentHealth <= 0)
            {
                return false;
            }

            if (disableAshesUsage && consumable == ashes)
            {
                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can't consume item at this time."),
                    notificationManager.systemError);
                return false;
            }

            return true;
        }

        public void PrepareItemForConsuming(Consumable consumable)
        {
            if (!CanConsumeItem(consumable))
            {
                return;
            }

            this.currentConsumedItem = consumable;

            if (consumable.shouldHideEquipmentWhenConsuming)
            {
                playerManager.playerWeaponsManager.HideEquipment();
            }

            if (consumable.isBossToken || consumable.canBeConsumedForGold)
            {
                uIDocumentPlayerGold.AddGold((int)consumable.value);
            }

            if (consumable is not Card)
            {
                isConsumingItem = true;
                foreach (StatusEffect statusEffect in currentConsumedItem.statusEffectsWhenConsumed)
                {
                    playerManager.statusController.statusEffectInstances.FirstOrDefault(x => x.Key == statusEffect).Value?.onConsumeStart?.Invoke();
                }

                playerManager.playerComponentManager.DisableCharacterController();
                playerManager.playerComponentManager.DisableComponents();
            }
            else
            {
                isConsumingItem = playerManager.playerCardManager.StartCardUse(currentConsumedItem as Card);
            }
        }

        public void FinishItemConsumption()
        {
            if (currentConsumedItem == null)
            {
                return;
            }

            if (currentConsumedItem is not Card)
            {
                playerManager.playerComponentManager.EnableCharacterController();
                playerManager.playerComponentManager.EnableComponents();
            }
            else
            {
                playerManager.playerCardManager.EndCurrentCardUse();
            }

            if (currentConsumedItem.shouldNotRemoveOnUse == false)
            {
                if (playerManager.statsBonusController.chanceToNotLoseItemUponConsumption && Random.Range(0f, 1f) > 0.8f)
                {
                    notificationManager.ShowNotification(
                        LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Consumable depleted"),
                        notificationManager.notEnoughSpells);


                    notificationManager.ShowNotification(
                        LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "The item has been preserved for future use.")
                    );
                }
                else
                {
                    playerManager.playerInventory.RemoveItem(currentConsumedItem);
                }
            }

            if (currentConsumedItem.statusesToRemove != null && currentConsumedItem.statusesToRemove.Length > 0)
            {
                foreach (StatusEffect statusEffectToRemove in currentConsumedItem.statusesToRemove)
                {
                    AppliedStatusEffect appliedStatusEffect = playerManager.statusController.appliedStatusEffects.FirstOrDefault(
                        x => x.statusEffect == statusEffectToRemove);

                    if (appliedStatusEffect != null)
                    {
                        playerManager.statusController.RemoveAppliedStatus(appliedStatusEffect);
                    }
                }
            }

            foreach (StatusEffect statusEffect in currentConsumedItem.statusEffectsWhenConsumed)
            {
                // For positive effects, we override the status effect resistance to be the duration of the consumable effect
                playerManager.statusController.statusEffectResistances[statusEffect] = currentConsumedItem.effectsDurationInSeconds;

                playerManager.statusController.InflictStatusEffect(statusEffect, currentConsumedItem.effectsDurationInSeconds, true);
            }

            currentConsumedItem = null;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void AllowAshes()
        {
            disableAshesUsage = false;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void DisableAshes()
        {
            onDisabledAshes?.Invoke();
            disableAshesUsage = true;
        }


        public override int GetItemQuantity(Item item)
        {
            if (!inventoryDatabase.ownedItems.ContainsKey(item))
            {
                return 0;
            }

            return inventoryDatabase.ownedItems[item].Count;
        }

        public override bool HasItem(Item item)
        {
            return GetItemQuantity(item) > 0;
        }

        public override SerializedDictionary<Item, List<ItemInstance>> GetInventory()
        {
            return inventoryDatabase.ownedItems;
        }

        public override WeaponInstance AddWeapon(Weapon weapon)
        {
            WeaponInstance addedWeapon = base.AddWeapon(weapon);

            // Analytics
            LogAnalytic(AnalyticsUtils.OnWeaponAcquired(weapon.name));

            // Achievements
            int numberOfWeapons = GetAllWeaponInstances().Count;

            if (numberOfWeapons <= 0)
            {
                playerManager.playerAchievementsManager.achievementOnAcquiringFirstWeapon.AwardAchievement();
            }
            else if (numberOfWeapons == 10)
            {
                playerManager.playerAchievementsManager.achievementOnAcquiringTenWeapons.AwardAchievement();
            }

            return addedWeapon;
        }
    }
}
