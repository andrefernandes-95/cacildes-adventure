using System;
using Steamworks;
using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;

namespace AF
{
    [CreateAssetMenu(menuName = "Data / New Int Stat")]

    public class IntStat : ScriptableObject
    {
        public void UpdateStat()
        {
            try
            {
                if (!GameAnalytics.Initialized)
                {
                    GameAnalytics.Initialize();
                }

                GameAnalytics.NewDesignEvent(
                    $"Achievement:{name}");
            }
            catch (Exception e)
            {
                Debug.Log("An error occurred while sending stat '" + name + "': " + e.Message);
            }
        }
    }
}
