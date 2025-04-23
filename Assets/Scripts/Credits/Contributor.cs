using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Misc / Credits / New Contributor")]
    public class Contributor : ScriptableObject
    {
        public ContributionType contributionType;
        public string author;
        [TextArea(minLines: 1, maxLines: 5)] public string enContribution;
        [TextArea(minLines: 1, maxLines: 5)] public string ptContribution;
        public string authorUrl;
        [ShowAssetPreview]
        public Sprite urlSprite;
    }
}
