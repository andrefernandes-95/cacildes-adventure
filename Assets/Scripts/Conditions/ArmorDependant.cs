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
        public CharacterBaseManager characterToTest;

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
                evaluationResult = characterToTest.characterBaseEquipment.IsNaked();
            }
            else if (requireAllPiecesToBeEquipped)
            {
                evaluationResult = characterToTest.characterBaseEquipment.GetHelmetInstance().HasItem(helmet)
                && characterToTest.characterBaseEquipment.GetArmorInstance().HasItem(armor)
                && characterToTest.characterBaseEquipment.GetLegwearInstance().HasItem(legwear)
                && characterToTest.characterBaseEquipment.GetGauntletInstance().HasItem(gauntlet);
            }
            else if (requireNoneOfThePiecesToBeEquipped)
            {
                evaluationResult =
                    !(characterToTest.characterBaseEquipment.GetHelmetInstance().HasItem(helmet) == false
                    || characterToTest.characterBaseEquipment.GetArmorInstance().HasItem(armor) == false
                    || characterToTest.characterBaseEquipment.GetLegwearInstance().HasItem(legwear) == false
                    || characterToTest.characterBaseEquipment.GetGauntletInstance().HasItem(gauntlet) == false);
            }
            else if (requireOnlyTorsoArmorToBeEquipped)
            {
                evaluationResult = characterToTest.characterBaseEquipment.GetArmorInstance().HasItem(armor);
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
