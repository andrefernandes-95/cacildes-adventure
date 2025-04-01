namespace AF
{
    [System.Serializable]
    public class LegwearInstance : ArmorBaseInstance
    {
        public LegwearInstance(string id, Legwear item) : base(id, item)
        {
        }

        public new LegwearInstance Clone() => new(this.id, this.item as Legwear);
    }
}
