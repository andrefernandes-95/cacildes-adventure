using AF.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace AF.Conditions
{
    public class ItemDependant : MonoBehaviour
    {
        public Item item;

        public CharacterBaseManager character;

        [Header("Events")]
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        private void Awake()
        {
            Evaluate();
        }

        public void Evaluate()
        {
            if (character.characterBaseInventory.HasItem(item))
            {
                onTrue?.Invoke();
            }
            else
            {
                onFalse?.Invoke();
            }
        }
    }
}
