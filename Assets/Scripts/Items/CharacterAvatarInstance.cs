namespace AF
{
    [System.Serializable]
    public class CharacterAvatarInstance : ItemInstance
    {
        public CharacterAvatarInstance(string id, CharacterAvatar item) : base(id, item)
        {
        }

        public new CharacterAvatarInstance Clone() => new(this.id, this.item as CharacterAvatar);
    }
}
