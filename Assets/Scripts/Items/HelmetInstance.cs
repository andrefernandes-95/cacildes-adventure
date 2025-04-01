namespace AF
{
    [System.Serializable]
    public class HelmetInstance : ArmorBaseInstance
    {
        public HelmetInstance(string id, Helmet item) : base(id, item)
        {
        }

        public new HelmetInstance Clone() => new(this.id, this.item as Helmet);
    }
}
