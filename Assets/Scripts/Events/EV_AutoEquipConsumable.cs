using System;
using System.Collections;

namespace AF
{
    public class EV_AutoEquipConsumable : EventBase
    {
        public CharacterBaseManager characterToAutoEquipConsumable;
        public Consumable consumableToEquip;

        public override IEnumerator Dispatch()
        {
            int freeSlot = Array.FindIndex(characterToAutoEquipConsumable.characterBaseEquipment.GetConsumables(), (slot) => slot == null);

            if (freeSlot != -1)
            {
                characterToAutoEquipConsumable.characterBaseEquipment.EquipConsumable(consumableToEquip, freeSlot);
            }

            yield return null;
        }
    }
}
