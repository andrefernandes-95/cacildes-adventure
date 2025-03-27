using AF.Inventory;
using NUnit.Framework;
using UnityEngine;

namespace AF.Tests
{
    public class UIDocumentCraftScreenTests
    {

        UIDocumentCraftScreen uIDocumentCraftScreen;

        InventoryDatabase inventoryDatabase;
        PlayerStatsDatabase playerStatsDatabase;
        PlayerManager playerManager;

        [SetUp]
        public void SetUp()
        {
            inventoryDatabase = ScriptableObject.CreateInstance<InventoryDatabase>();
            playerStatsDatabase = ScriptableObject.CreateInstance<PlayerStatsDatabase>();


            playerManager = new GameObject().AddComponent<PlayerManager>();
            playerManager.playerStatsDatabase = playerStatsDatabase;
            playerManager.playerComponentManager = new GameObject().AddComponent<PlayerComponentManager>();
            playerManager.playerComponentManager.characterController = new GameObject().AddComponent<CharacterController>();

            uIDocumentCraftScreen = new GameObject().AddComponent<UIDocumentCraftScreen>();
            uIDocumentCraftScreen.cursorManager = uIDocumentCraftScreen.gameObject.AddComponent<CursorManager>();
            uIDocumentCraftScreen.playerManager = playerManager;
            uIDocumentCraftScreen.inventoryDatabase = inventoryDatabase;
        }
    }
}
