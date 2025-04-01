namespace AF
{
    [System.Serializable]
    public class ArmorInstance : ArmorBaseInstance
    {
        public ArmorInstance(string id, Armor item) : base(id, item)
        {
        }

        public new ArmorInstance Clone() => new(this.id, this.item as Armor);
    }
}
