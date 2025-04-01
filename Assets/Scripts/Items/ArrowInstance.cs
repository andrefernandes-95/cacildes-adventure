namespace AF
{
    [System.Serializable]
    public class ArrowInstance : ItemInstance
    {
        public ArrowInstance(string id, Arrow item) : base(id, item)
        {
        }

        public new ArrowInstance Clone() => new(this.id, this.item as Arrow);
    }
}
