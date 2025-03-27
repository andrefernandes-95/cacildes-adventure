
using UnityEngine;
using AF;

namespace AF.Conditions
{
    public class BuildDependant : MonoBehaviour
    {
        public BuildManager buildManager;

        public BuildManager.BuildType buildType;

        private void Awake()
        {
            Utils.UpdateTransformChildren(transform, false);
        }

        private void Start()
        {
            Evaluate();
        }

        public void Evaluate()
        {
            bool isActive = false;

            if (buildManager.buildType == buildType)
            {
                isActive = true;
            }

            Utils.UpdateTransformChildren(transform, isActive);
        }
    }
}
