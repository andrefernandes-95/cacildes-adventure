using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_CraftingRecipesList : MonoBehaviour
    {

        [Header("Footer Components")]
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        [Header("Character")]
        public CharacterBaseManager character;

        [Header("Databases")]
        public RecipesDatabase recipesDatabase; // In the future, we could have a character-recipes database, so we can have different recipes for different characters.

        [Header("Footer Buttons")]
        public GameObject exitBlacksmithButtonPrefab;
        public GameObject confirmUpgradeButtonPrefab;
        GameObject confirmUpgradeButtonInstance;

        [Header("UI Components")]
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] UI_CraftableItemButton uI_CraftableItemButton;
        [SerializeField] UI_ItemToCreatePreview uI_ItemToCreatePreview;

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            uI_ItemToCreatePreview.HideUI();

            SetupFooter();
            DrawRecipesList();
        }

        public void Refresh()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void SetupFooter()
        {
            uI_FooterIndicator.Refresh();

            uI_FooterIndicator.AddFooterActionButton(exitBlacksmithButtonPrefab, Glossary.IsPortuguese() ? "Abandonar mesa de alquimia" : "Leave alchemy table");
            confirmUpgradeButtonInstance = uI_FooterIndicator.AddFooterActionButton(confirmUpgradeButtonPrefab, Glossary.IsPortuguese() ? "Criar item" : "Craft item");
            confirmUpgradeButtonInstance.SetActive(false);
        }

        void DrawRecipesList()
        {
            Utils.ClearScrollRect(scrollRect);

            foreach (CraftingRecipe craftingRecipe in recipesDatabase.craftingRecipes)
            {
                GameObject uI_CraftableItemButtonGameObject = Instantiate(uI_CraftableItemButton.gameObject, scrollRect.content);
                UI_CraftableItemButton uI_CraftableItemButtonInstance = uI_CraftableItemButtonGameObject.GetComponent<UI_CraftableItemButton>();

                uI_CraftableItemButtonInstance.craftingRecipe = craftingRecipe;
                uI_CraftableItemButtonInstance.characterBaseManager = character;

                uI_CraftableItemButtonInstance.UpdateUI();

                uI_CraftableItemButtonInstance.onSelect.AddListener(() =>
                {
                    uI_ItemToCreatePreview.ShowUI(craftingRecipe, character);
                    confirmUpgradeButtonInstance.SetActive(true);
                });

                uI_CraftableItemButtonInstance.onDeselect.AddListener(() =>
                {
                    uI_ItemToCreatePreview.HideUI();
                    confirmUpgradeButtonInstance.SetActive(false);
                });


                uI_CraftableItemButtonInstance.onCreateItem.AddListener(() =>
                {
                    Refresh();
                });
            }

            StartCoroutine(GiveFocusNextFrame());
        }


        IEnumerator GiveFocusNextFrame()
        {
            yield return null; // Wait one frame

            if (
                EventSystem.current.currentSelectedGameObject == null &&
                scrollRect.content.transform.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(scrollRect.content.GetChild(0).gameObject);
            }
        }

    }
}
