using UnityEngine;

namespace AF
{
    public class UI_Alchemy : MonoBehaviour
    {
        [Header("Components")]
        public UI_Manager uI_Manager;
        public UI_CraftingRecipesList uI_CraftingRecipesList;
        public StarterAssetsInputs starterAssetsInputs;

        [Header("Blacksmith Character")]
        public CharacterBaseManager characterBaseManager;

        [Header("Sounds")]
        public Soundbank soundbank;
        public AudioClip openSfx;
        public AudioClip closeSfx;

        [Header("Debug")]
        public CraftingRecipe[] craftingRecipesToDebug;
        public CraftingMaterial[] craftingMaterialsToDebug;
        public RecipesDatabase recipesDatabase;

        void Awake()
        {
            if (craftingRecipesToDebug != null && craftingRecipesToDebug.Length > 0)
            {
                foreach (CraftingRecipe craftingRecipe in craftingRecipesToDebug)
                {
                    recipesDatabase.AddCraftingRecipe(craftingRecipe);
                }
            }

            if (craftingMaterialsToDebug != null && craftingMaterialsToDebug.Length > 0)
            {
                foreach (CraftingMaterial craftingMaterial in craftingMaterialsToDebug)
                {
                    characterBaseManager.characterBaseInventory.AddCraftingMaterial(craftingMaterial);
                }
            }

            uI_CraftingRecipesList.Refresh();
        }

        void OnEnable()
        {
            starterAssetsInputs.onMenuEvent.AddListener(Close);

            uI_Manager.HidePlayerHUD();
        }

        void OnDisable()
        {
            starterAssetsInputs.onMenuEvent.RemoveListener(Close);
        }

        public void Close()
        {
            soundbank.PlaySound(closeSfx);
            uI_Manager.ShowPlayerHUD();
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (characterBaseManager != null && characterBaseManager.characterBaseBlacksmithManager != null)
            {
                characterBaseManager.characterBaseBlacksmithManager.EndJob();
            }
        }
    }
}
