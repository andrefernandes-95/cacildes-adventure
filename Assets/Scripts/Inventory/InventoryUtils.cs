namespace AF
{
    using System;
    using UnityEngine;

    public static class InventoryUtils
    {
        public static ItemInstance ItemToItemInstance<T>(Item itemToAdd) where T : ItemInstance
        {
            string id = Guid.NewGuid().ToString();

            var toAdd = itemToAdd switch
            {
                Shield => new ShieldInstance(id, itemToAdd as Shield),
                Weapon => new WeaponInstance(id, itemToAdd as Weapon, 0),
                Arrow => new ArrowInstance(id, itemToAdd as Arrow),
                Spell => new SpellInstance(id, itemToAdd as Spell),
                Helmet => new HelmetInstance(id, itemToAdd as Helmet),
                Gauntlet => new GauntletInstance(id, itemToAdd as Gauntlet),
                Legwear => new LegwearInstance(id, itemToAdd as Legwear),
                Armor => new ArmorInstance(id, itemToAdd as Armor),
                Accessory => new AccessoryInstance(id, itemToAdd as Accessory),
                Consumable => new ConsumableInstance(id, itemToAdd as Consumable),
                CraftingMaterial => new CraftingMaterialInstance(id, itemToAdd as CraftingMaterial),
                //Gemstone => new GemstoneInstance(id, itemToAdd),
                _ => new ItemInstance(id, itemToAdd),
            };

            return toAdd;
        }
    }
}
