using System;
using AF.Inventory;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Item / New Item")]
    public class Item : ScriptableObject
    {
        [Foldout("Obsolete")]
        [Obsolete] public LocalizedString nameLocalized;
        [Foldout("Obsolete")]
        [Obsolete] public LocalizedString descriptionLocalized;
        [Foldout("Obsolete")]
        [Obsolete] public LocalizedString shortDescriptionLocalized;

        [Header("Texts")]

        public string englishName;
        public string portugueseName;

        [TextArea(minLines: 2, maxLines: 5)] public string englishDescription;
        [TextArea(minLines: 2, maxLines: 5)] public string portugueseDescription;

        [InfoBox("Recommended to use item description objects:", EInfoBoxType.Warning)]
        public ItemDescription itemDescription;

        [HorizontalLine(color: EColor.Gray)]

        [Header("UI")]
        public Sprite sprite;

        [Header("Value")]
        public float value = 0;
        public bool isRenewable = false;

        [Tooltip("If we want to buy this item on a shop, this will override their value when trading with an NPC. E.g. Buying a boss weapon by trading a boss soul")]
        public SerializedDictionary<Item, int> tradingItemRequirements = new();

        [Header("Debug")]
        [TextAreaAttribute(minLines: 5, maxLines: 10)] public string notes;
        [TextAreaAttribute(minLines: 1, maxLines: 2)] public string location;

        public string GetName()
        {
            if (Glossary.IsPortuguese() && !string.IsNullOrEmpty(portugueseName))
            {
                return portugueseName;
            }
            else if (!string.IsNullOrEmpty(englishName))
            {
                return englishName;
            }

            if (nameLocalized != null && nameLocalized.IsEmpty == false)
            {
                return nameLocalized.GetLocalizedString();
            }

            return name;
        }


        public string GetDescription()
        {
            if (Glossary.IsPortuguese() && !string.IsNullOrEmpty(portugueseDescription))
            {
                return portugueseDescription;
            }
            else if (!string.IsNullOrEmpty(englishDescription))
            {
                return englishDescription;
            }

            if (descriptionLocalized != null && descriptionLocalized.IsEmpty == false)
            {
                return descriptionLocalized.GetLocalizedString();
            }

            return "";
        }

        public string GetShortDescription()
        {
            if (shortDescriptionLocalized != null && shortDescriptionLocalized.IsEmpty == false)
            {
                return shortDescriptionLocalized.GetLocalizedString();
            }

            return "";
        }
    }
}
