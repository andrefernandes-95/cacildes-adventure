using UnityEngine;

namespace AF
{
    public class EquipDebugUtil : MonoBehaviour
    {
        public CharacterBaseManager character;

        [Header("Debug")]
        public Helmet helmet;
        public bool equipHelmet = false;
        public bool clearHelmet = false;

        // TODO: Delete later, just for testing purposes
        void Update()
        {
            if (equipHelmet)
            {
                equipHelmet = false;

                character.characterBaseEquipment.EquipHelmet(character.characterBaseInventory.AddHelmet(helmet, character.characterBaseInventory.GetInventory()));
            }

            if (clearHelmet)
            {
                clearHelmet = false;
                character.characterBaseEquipment.UnequipHelmet();
            }
        }
    }
}
