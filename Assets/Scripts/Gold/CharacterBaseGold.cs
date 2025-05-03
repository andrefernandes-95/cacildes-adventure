namespace AF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public abstract class CharacterBaseGold : MonoBehaviour
    {
        public abstract int GetCurrentGold();
        public abstract void SetCurrentGold(int value);
        public abstract void AddGold(int value);
        public abstract void RemoveGold(int value);
    }
}
