namespace AF
{
    [System.Serializable]
    public class UpgradeMaterialInstance : ItemInstance
    {
        public UpgradeMaterialInstance(string id, UpgradeMaterial item) : base(id, item)
        {
        }

        public new UpgradeMaterialInstance Clone() => new(this.id, this.item as UpgradeMaterial);

    }
}
