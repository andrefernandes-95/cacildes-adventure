using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AF.Misc
{
    public class ActivateLostCoinsPickup : MonoBehaviour
    {
        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;

        [Header("Components")]
        public UIDocumentPlayerGold uIDocumentPlayerGold;

        [Header("Events")]
        public UnityEvent onPickup;

        private void Awake()
        {
            Evaluate();
        }

        void Evaluate()
        {
            bool shouldActivate = playerStatsDatabase.HasLostGoldToRecover() && SceneManager.GetActiveScene().name == playerStatsDatabase.sceneWhereGoldWasLost;

            this.gameObject.SetActive(shouldActivate);

            if (shouldActivate)
            {
                transform.position = playerStatsDatabase.positionWhereGoldWasLost;
            }
        }

        public void PickupLostCoins()
        {
            var amountToCollect = playerStatsDatabase.lostGold;
            playerStatsDatabase.ClearLostGold();
            uIDocumentPlayerGold.AddGold(amountToCollect);
            gameObject.SetActive(false);
        }
    }
}
