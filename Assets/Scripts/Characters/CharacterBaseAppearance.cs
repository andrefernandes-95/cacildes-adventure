namespace AF
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class CharacterBaseAppearance : MonoBehaviour
    {
        public abstract bool IsMale();

        public abstract List<string> GetHairs();
        public abstract List<string> GetEyebrows();
        public abstract List<string> GetBeard();
        public abstract List<string> GetFace();
        public abstract List<string> GetTorso();
        public abstract List<string> GetHands();
        public abstract List<string> GetLegs();

        public abstract Color GetEyeColor();
        public abstract Color GetSkinColor();
        public abstract Color GetHairColor();
        public abstract Color GetScarColor();

    }
}
