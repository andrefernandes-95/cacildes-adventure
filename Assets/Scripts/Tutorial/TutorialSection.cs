namespace AF.Tutorial
{
    using UnityEngine;
    using UnityEngine.Localization;

    public class TutorialSection : MonoBehaviour
    {
        public UIDocumentPlayerHUDV2 uIDocumentPlayerHUDV2;

        public UIDocumentPlayerHUDV2.ControlKey controlKeyToHighlight = UIDocumentPlayerHUDV2.ControlKey.None;

        public PlayerManager playerManager;

        public TutorialSpawnRef tutorialSpawnRef;

        public LocalizedString tutorialName;
        public SceneSettings sceneSettings;


        private void Awake()
        {
            if (tutorialSpawnRef == null)
            {
                Debug.LogError("Tutorial spawn ref not assigned!");
            }
        }

        public void Activate()
        {
            playerManager.playerComponentManager.TeleportPlayer(tutorialSpawnRef.transform);

            if (controlKeyToHighlight != UIDocumentPlayerHUDV2.ControlKey.None)
            {
                uIDocumentPlayerHUDV2.HighlightKey(controlKeyToHighlight);
            }
            else
            {
                uIDocumentPlayerHUDV2.DisableHighlights();
            }

            if (tutorialName.IsEmpty == false)
            {
                StartCoroutine(sceneSettings.DisplaySceneNameCoroutine(tutorialName.GetLocalizedString()));
            }
        }
    }
}
