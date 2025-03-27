using System;
using System.Collections.Generic;
using System.Linq;
using AF.Inventory;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF.Pickups
{
    public class AddItemUtil : MonoBehaviour
    {
        [Header("Data")]

        [Header("Effect Instances")]
        [SerializedDictionary("Item", "Quantity")]
        public SerializedDictionary<Item, ItemAmount> itemsToAdd;

        // Scene References
        UIDocumentReceivedItemPrompt uIDocumentReceivedItemPrompt;
        PlayerInventory playerInventory;
        Soundbank soundbank;


        public void OnAddItem()
        {
            GetUIDocumentReceivedItemPrompt().gameObject.SetActive(true);

            List<UIDocumentReceivedItemPrompt.ItemsReceived> itemsToDisplay = new();

            foreach (var item in itemsToAdd)
            {
                if (item.Value.chanceToGet != 100)
                {
                    int chance = UnityEngine.Random.Range(0, 100);
                    if (chance > item.Value.chanceToGet)
                    {
                        continue;
                    }
                }

                bool isCard = item.Key is Card;

                itemsToDisplay.Add(new()
                {
                    itemName = item.Key.GetName(),
                    quantity = item.Value.amount,
                    sprite = item.Key.sprite,
                    isCard = isCard
                });

                GetPlayerInventory().AddItem(item.Key, item.Value.amount);
            }

            GetUIDocumentReceivedItemPrompt().DisplayItemsReceived(itemsToDisplay);

            GetSoundbank().PlaySound(GetSoundbank().uiItemReceived);
        }

        UIDocumentReceivedItemPrompt GetUIDocumentReceivedItemPrompt()
        {
            if (uIDocumentReceivedItemPrompt == null)
            {
                uIDocumentReceivedItemPrompt = FindAnyObjectByType<UIDocumentReceivedItemPrompt>(FindObjectsInactive.Include);
            }

            return uIDocumentReceivedItemPrompt;
        }

        Soundbank GetSoundbank()
        {
            if (soundbank == null)
            {
                soundbank = FindAnyObjectByType<Soundbank>(FindObjectsInactive.Include);
            }

            return soundbank;
        }

        PlayerInventory GetPlayerInventory()
        {
            if (playerInventory == null)
            {
                playerInventory = FindAnyObjectByType<PlayerInventory>(FindObjectsInactive.Include);
            }

            return playerInventory;
        }
    }
}
