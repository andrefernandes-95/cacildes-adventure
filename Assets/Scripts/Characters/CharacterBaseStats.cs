namespace AF
{
    using UnityEngine;

    public abstract class CharacterBaseStats : MonoBehaviour
    {
        public abstract int GetVitality();
        public abstract int GetEndurance();
        public abstract int GetIntelligence();
        public abstract int GetStrength();
        public abstract int GetDexterity();
        public abstract int GetReputation();

        public abstract int GetCurrentLevel();
        public abstract void ResetStats();
    }
}
