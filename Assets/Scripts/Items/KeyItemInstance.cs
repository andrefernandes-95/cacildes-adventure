namespace AF
{
    [System.Serializable]
    public class KeyItemInstance : ItemInstance
    {
        public KeyItemInstance(string id, KeyItem item) : base(id, item)
        {
        }

        public new KeyItemInstance Clone() => new(this.id, this.item as KeyItem);
    }
}
