﻿using System.Collections;
using AF.Music;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentGameOver : MonoBehaviour
    {
        [Header("Components")]
        public BGMManager bgmManager;
        public Soundbank soundbank;
        public PlayerManager playerManager;
        public SaveManager saveManager;
        public UIDocumentPlayerGold uIDocumentPlayerGold;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;

        [Header("Settings")]
        public float gameOverDuration = 2.5f;

        [Header("You Died Texts")]
        public LocalizedString[] youDiedText;

        private void Awake()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void DisplayGameOver()
        {
            this.gameObject.SetActive(true);
            StartCoroutine(GameOver_Coroutine());
        }

        IEnumerator GameOver_Coroutine()
        {
            Label youDiedLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("YouDiedText");
            youDiedLabel.text = youDiedText[Random.Range(0, youDiedText.Length)].GetLocalizedString();
            UIUtils.PlayPopAnimation(youDiedLabel);

            bgmManager.StopMusic();
            soundbank.PlaySound(soundbank.gameOverFanfare);

            playerManager.playerComponentManager.DisableCharacterController();
            playerManager.playerComponentManager.DisableComponents();

            if (playerStatsDatabase.HasLostGoldToRecover())
            {
                playerStatsDatabase.ClearLostGold();
            }
            else
            {
                playerStatsDatabase.SetLostGold(playerManager.thirdPersonController.lastGroundedPosition);
            }

            uIDocumentPlayerGold.LoseGold(playerStatsDatabase.gold);

            yield return new WaitForSeconds(gameOverDuration);

            saveManager.LoadLastSavedGame(true);
        }
    }
}
