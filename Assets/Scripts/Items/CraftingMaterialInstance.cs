namespace AF
{
    [System.Serializable]
    public class CraftingMaterialInstance : ItemInstance
    {
        public CraftingMaterialInstance(string id, CraftingMaterial item) : base(id, item)
        {
        }
        public new CraftingMaterialInstance Clone() => new(this.id, this.item as CraftingMaterial);

    }
}
