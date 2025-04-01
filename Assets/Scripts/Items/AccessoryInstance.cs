namespace AF
{
    [System.Serializable]
    public class AccessoryInstance : ArmorBaseInstance
    {
        public AccessoryInstance(string id, Accessory item) : base(id, item)
        {
        }

        public new AccessoryInstance Clone() => new(this.id, this.item as Accessory);
    }
}
