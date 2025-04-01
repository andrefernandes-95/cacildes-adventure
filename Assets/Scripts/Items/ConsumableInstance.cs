namespace AF
{
    [System.Serializable]
    public class ConsumableInstance : ItemInstance
    {
        public bool wasUsed = false;

        public ConsumableInstance(string id, Consumable item) : base(id, item)
        {
        }

        public new ConsumableInstance Clone() => new(this.id, this.item as Consumable);
    }
}
