namespace AF
{
    [System.Serializable]
    public class ShieldInstance : WeaponInstance
    {

        public ShieldInstance(string id, Shield item, int level = 1) : base(id, item, level)
        {
        }

        public new ShieldInstance Clone() => new(this.id, this.item as Shield);
    }
}
