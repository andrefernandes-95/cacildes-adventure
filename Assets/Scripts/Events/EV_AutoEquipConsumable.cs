using System;
using System.Collections;
using AF.Inventory;

namespace AF
{

    public class EV_AutoEquipConsumable : EventBase
    {
        public EquipmentDatabase equipmentDatabase;
        public InventoryDatabase inventoryDatabase;

        public Consumable consumableToEquip;

        public override IEnumerator Dispatch()
        {
            int freeSlot = Array.FindIndex(equipmentDatabase.consumables, (slot) => slot == null);

            if (freeSlot != -1)
            {
                ConsumableInstance consumableInstance = inventoryDatabase.GetFirst(consumableToEquip) as ConsumableInstance;
                equipmentDatabase.EquipConsumable(consumableInstance, freeSlot);
            }

            yield return null;
        }
    }

}
