using AF.Events;
using TigerForge;
using UnityEngine;


namespace AF
{
    public class BowAimingHelper : MonoBehaviour
    {
        public Vector3 crossBowPosition;
        public CharacterBaseManager character;

        Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.localPosition;

            EventManager.StartListening(EventMessages.ON_EQUIPMENT_CHANGED, Evaluate);

            Evaluate();
        }

        void Evaluate()
        {
            if (character == null)
            {
                return;
            }

            if (character.characterBaseEquipment.GetRightHandWeapon().Exists() && character.characterBaseEquipment.GetRightHandWeapon().GetItem<Weapon>().isCrossbow)
            {
                transform.localPosition = crossBowPosition;
            }
            else
            {
                transform.localPosition = originalPosition;
            }
        }

    }
}
