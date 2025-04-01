namespace AF
{
    [System.Serializable]
    public class SpellInstance : ItemInstance
    {
        public SpellInstance(string id, Spell spell) : base(id, spell)
        {
        }

        public new SpellInstance Clone() => new(this.id, this.item as Spell);
    }
}
