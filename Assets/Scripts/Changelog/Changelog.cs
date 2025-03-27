using System;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Misc / Changelog / New Changelog")]
    public class Changelog : ScriptableObject
    {
        public string date = "13/08/2024";
        public Sprite changelogThumbnail;
        public LocalizedString smallDescription;

        public LocalizedString[] additions;
        public LocalizedString[] improvements;
        public LocalizedString[] bugfixes;

        public UpdateType updateType = UpdateType.SMALL_UPDATE;

        public enum UpdateType
        {
            SMALL_UPDATE,
            BIG_UPDATE,
            EXPANSION
        }
    }

}
