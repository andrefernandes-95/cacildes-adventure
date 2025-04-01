namespace AF
{
    [System.Serializable]
    public class ConsumableProjectileInstance : ItemInstance
    {
        public ConsumableProjectileInstance(string id, ConsumableProjectile item) : base(id, item)
        {
        }

        public new ConsumableProjectileInstance Clone() => new(this.id, this.item as ConsumableProjectile);
    }
}
