namespace AF
{
    public class ArmorBaseInstance : ItemInstance
    {
        public ArmorBaseInstance(string id, ArmorBase item) : base(id, item)
        {
        }

        public new ArmorBaseInstance Clone() => new(this.id, this.item as ArmorBase);
    }
}
