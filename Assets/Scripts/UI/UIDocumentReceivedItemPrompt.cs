
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentReceivedItemPrompt : MonoBehaviour
    {
        [System.Serializable]
        public class ItemsReceived
        {
            public string itemName = "Item Name";
            public int quantity = 0;
            public Sprite sprite = null;
            public bool isCard = false;
        }

        public UIDocument uiDocument;

        public VisualTreeAsset receivedItemPrefab;

        public Sprite placeholderCard;

        VisualElement rootPanel, cardContainer;

        bool isPopupActive = false;


        [Header("Sounds")]
        public AudioClip cardAppears;
        public AudioClip cardReveal;
        public Soundbank soundbank;

        // Callbacks
        public List<ItemsReceived> cardsToReceive = new();

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void DisplayCard(ItemsReceived card)
        {
            SetupUI();

            cardContainer.style.backgroundImage = new StyleBackground(placeholderCard);
            cardContainer.style.display = DisplayStyle.Flex;

            soundbank.PlaySound(cardAppears);

            DOTween.To(
                 () => cardContainer.contentContainer.style.opacity.value,
                 (value) => cardContainer.contentContainer.style.opacity = value,
                 1,
                 .05f
           );

            // Set the starting position below the screen
            Vector3 startPosition = new Vector3(cardContainer.contentContainer.transform.position.x,
            cardContainer.contentContainer.transform.position.y - 10, cardContainer.contentContainer.transform.position.z);

            // Set the ending position (original position)
            Vector3 endPosition = cardContainer.contentContainer.transform.position;
            DOTween.To(() => startPosition, position => cardContainer.contentContainer.transform.position = position, endPosition, 0.5f);

            FlipCard(card);

            var clone = receivedItemPrefab.CloneTree();
            clone.Q<VisualElement>("ActionButtons").Q<Label>("ItemName").text =
                LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Card") + ": " + card.itemName;
            clone.Q<Label>("ItemQuantity").text = card.quantity + "";
            clone.Q<IMGUIContainer>("ItemSprite").style.display = DisplayStyle.None;
            rootPanel.Add(clone);

            soundbank.PlaySound(cardReveal);

            isPopupActive = true;
        }

        void SetupUI()
        {
            isPopupActive = false;

            var root = uiDocument.rootVisualElement;

            cardContainer = root.Q<VisualElement>("CardContainer");
            cardContainer.style.display = DisplayStyle.None;

            if (Gamepad.current != null)
            {
                root.Q<IMGUIContainer>("KeyboardIcon").style.display = DisplayStyle.None;
                root.Q<IMGUIContainer>("GamepadIcon").style.display = DisplayStyle.Flex;
                root.Q<IMGUIContainer>("XboxIcon").style.display = DisplayStyle.Flex;
            }
            else
            {
                root.Q<IMGUIContainer>("KeyboardIcon").style.display = DisplayStyle.Flex;
                root.Q<IMGUIContainer>("GamepadIcon").style.display = DisplayStyle.None;
                root.Q<IMGUIContainer>("XboxIcon").style.display = DisplayStyle.None;
            }

            rootPanel = root.Q<VisualElement>("ReceivedItemsContainer");
            rootPanel.Clear();
        }

        public void DisplayItemsReceived(List<ItemsReceived> itemsReceived)
        {
            SetupUI();

            foreach (var itemUIEntry in itemsReceived)
            {
                if (itemUIEntry.isCard)
                {
                    cardsToReceive.Add(itemUIEntry);
                    continue;
                }

                var clone = receivedItemPrefab.CloneTree();

                clone.Q<VisualElement>("ActionButtons").Q<Label>("ItemName").text = itemUIEntry.itemName;
                clone.Q<Label>("ItemQuantity").text = itemUIEntry.quantity.ToString();
                clone.Q<IMGUIContainer>("ItemSprite").style.backgroundImage = new StyleBackground(itemUIEntry.sprite);

                rootPanel.Add(clone);
            }

            // If has cards to receive, and all items to receive are cards, just show cards
            if (cardsToReceive.Count > 0 && itemsReceived.All(x => x.isCard))
            {
                ShowLastCard();
            }

            // Force the flag to be activated in the next frame so that OnInteract doesn't overlap with another OnInteract event of the same frame (i.e. Opening a chest)
            Invoke(nameof(SetIsPopupActiveToTrue), 0f);
        }

        void SetIsPopupActiveToTrue()
        {
            isPopupActive = true;
        }

        void ShowLastCard()
        {
            if (cardsToReceive.Count <= 0)
            {
                return;
            }

            ItemsReceived lastCard = cardsToReceive.Last();

            DisplayCard(lastCard);

            cardsToReceive.Remove(lastCard);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnInteract()
        {
            if (isPopupActive)
            {
                isPopupActive = false;

                if (cardsToReceive.Count > 0)
                {
                    ShowLastCard();
                    return;
                }

                gameObject.SetActive(false);
            }
        }



        void FlipCard(ItemsReceived card)
        {
            // Perform the flip animation using DOTween
            Sequence flipSequence = DOTween.Sequence();

            // Rotate to 90 degrees
            flipSequence.Append(DOTween.To(
                () =>
                {
                    return cardContainer.contentContainer.transform.scale;
                },
                x => cardContainer.style.scale = new StyleScale(new Vector2(x.x, 1)),
                new Vector3(0, 1, 1),
                0.25f
            ).SetEase(Ease.InOutQuad));

            // Swap content or reveal new side
            flipSequence.AppendCallback(() =>
            {
                cardContainer.style.backgroundImage = new StyleBackground(card.sprite);
            });

            // Rotate to 180 degrees (show the other side)
            flipSequence.Append(DOTween.To(
                () => cardContainer.contentContainer.transform.scale,
                x => cardContainer.style.scale = new StyleScale(new Vector2(1, 1)),
                new Vector3(1, 1, 1),
                0.25f
            ).SetEase(Ease.InOutQuad));

            flipSequence.Play();
        }


    }
}
