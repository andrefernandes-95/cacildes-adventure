using System;
using System.Collections;

namespace AF
{

    public class EV_AutoEquipConsumable : EventBase
    {
        public EquipmentDatabase equipmentDatabase;

        public Consumable consumableToEquip;

        public override IEnumerator Dispatch()
        {
            int freeSlot = Array.FindIndex(equipmentDatabase.consumables, (slot) => slot == null);

            if (freeSlot != -1)
            {
                equipmentDatabase.EquipConsumable(consumableToEquip, freeSlot);
            }

            yield return null;
        }
    }

}
