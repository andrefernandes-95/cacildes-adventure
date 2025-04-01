using System.Collections;
using AF.Music;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace AF
{

    public class EV_RemoveItem : EventBase
    {
        public Item item;

        public int amount = 1;

        public bool showNotificationText = true;

        public AudioClip pickUpSfx;

        [Header("Components")]
        public BGMManager bgmManager;
        public Soundbank soundbank;
        public NotificationManager notificationManager;
        public PlayerManager playerManager;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(RemoveItem());
        }

        IEnumerator RemoveItem()
        {
            for (int i = 0; i < amount; i++)
            {
                playerManager.playerInventory.RemoveItem(item);
            }

            if (pickUpSfx != null)
            {
                bgmManager.PlaySound(pickUpSfx, null);
            }

            if (showNotificationText)
            {
                soundbank.PlaySound(soundbank.uiCancel);

                notificationManager.ShowNotification(
                    LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Used x") +
                    +amount + " " + item.GetName() + "", item.sprite);
            }

            yield return null;
        }
    }

}
