using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Description / New Item Description")]
    public class ItemDescription : ScriptableObject
    {
        [TextArea(minLines: 3, maxLines: 10)] public string english;
        [TextArea(minLines: 3, maxLines: 10)] public string portuguese;
    }
}
