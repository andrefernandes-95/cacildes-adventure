
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    [System.Serializable]
    public class CardEffectInstance : MonoBehaviour
    {
        public Card card;

        [Header("Consumable Events")]
        public UnityEvent onCardStart;
        public UnityEvent onCardUse;
        public UnityEvent onCardEnd;


    }
}
