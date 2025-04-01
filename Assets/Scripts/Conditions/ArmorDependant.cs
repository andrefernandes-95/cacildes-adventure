using AF.Events;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

namespace AF.Conditions
{
    public class ArmorDependant : MonoBehaviour
    {
        [Header("Equipment Conditions")]
        public Helmet helmet;
        public Armor armor;
        public Gauntlet gauntlet;
        public Legwear legwear;

        [Header("Settings")]
        public bool requireOnlyTorsoArmorToBeEquipped = false;
        public bool requireAllPiecesToBeEquipped = false;
        public bool requireNoneOfThePiecesToBeEquipped = false;

        [Header("Naked Conditions")]
        public bool requirePlayerToBeNaked = false;

        [Header("Databases")]
        public EquipmentDatabase equipmentDatabase;

        [Header("Events")]
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        private void Awake()
        {
            EventManager.StartListening(EventMessages.ON_EQUIPMENT_CHANGED, Evaluate);

            Evaluate();
        }

        public void Evaluate()
        {
            bool evaluationResult = false;

            if (requirePlayerToBeNaked)
            {
                evaluationResult = equipmentDatabase.IsPlayerNaked();
            }
            else if (requireAllPiecesToBeEquipped)
            {
                evaluationResult = equipmentDatabase.helmet.HasItem(helmet)
                && equipmentDatabase.armor.HasItem(armor)
                && equipmentDatabase.legwear.HasItem(legwear)
                && equipmentDatabase.gauntlet.HasItem(gauntlet);
            }
            else if (requireNoneOfThePiecesToBeEquipped)
            {
                evaluationResult =
                    !(equipmentDatabase.helmet.HasItem(helmet) == false
                    || equipmentDatabase.armor.HasItem(armor) == false
                    || equipmentDatabase.legwear.HasItem(legwear) == false
                    || equipmentDatabase.gauntlet.HasItem(gauntlet) == false);
            }
            else if (requireOnlyTorsoArmorToBeEquipped)
            {
                evaluationResult = equipmentDatabase.armor.HasItem(armor);
            }

            Utils.UpdateTransformChildren(transform, evaluationResult);

            if (evaluationResult)
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
