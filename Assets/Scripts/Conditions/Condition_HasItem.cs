using AF.Inventory;

namespace AF.Conditions
{
    public class Condition_HasItem : ConditionBase
    {
        public CharacterBaseManager character;
        public Item requiredItem;

        public override bool IsConditionMet()
        {
            return character.characterBaseInventory.HasItem(requiredItem);
        }
    }
}
