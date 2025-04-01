namespace AF
{
    [System.Serializable]
    public class WeaponInstance : ItemInstance
    {
        public int level = 1;

        public WeaponInstance(string id, Weapon item, int level = 1) : base(id, item)
        {
            this.level = level;
        }

        public new WeaponInstance Clone() => new(this.id, this.item as Weapon, this.level);
    }
}
