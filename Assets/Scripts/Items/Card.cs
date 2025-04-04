using AF.Health;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Item / New Card")]
    public class Card : Consumable
    {
        [Header("Card Settings")]
        public int maximumCardsAllowedInInventory = 99;

        [Header("Damage Settings")]
        public bool useDamage = false;
        public Damage cardDamage;
        public bool forcePushForceToZero = false;

        [Header("Animation Clips")]
        public AnimationClip clip;

        [Header("Info")]
        public LocalizedString commonlyFoundDescription;

        [Header("Requirements")]
        public int staminaRequired = 0;
        public int manaRequired = 0;
    }
}
