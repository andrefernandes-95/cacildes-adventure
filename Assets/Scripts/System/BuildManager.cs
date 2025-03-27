using System;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "BuildManager", menuName = "System/New Build Manager", order = 0)]
    public class BuildManager : ScriptableObject
    {
        public enum BuildType
        {
            PRODUCTION,
            DEMO,
        }

        public BuildType buildType = BuildType.PRODUCTION;
    }

}
