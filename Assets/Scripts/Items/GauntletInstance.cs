namespace AF
{
    [System.Serializable]
    public class GauntletInstance : ArmorBaseInstance
    {
        public GauntletInstance(string id, Gauntlet item) : base(id, item)
        {
        }

        public new GauntletInstance Clone() => new(this.id, this.item as Gauntlet);
    }
}
