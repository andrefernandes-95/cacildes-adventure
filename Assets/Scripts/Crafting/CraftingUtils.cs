using System.Collections.Generic;
using System.Linq;
using AF.Inventory;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace AF
{
    public static class CraftingUtils
    {
        public static bool CanCraftItem(InventoryDatabase inventoryDatabase, CraftingRecipe recipe)
        {
            bool hasEnoughMaterial = true;

            foreach (var ingredient in recipe.ingredients)
            {
                var itemEntry = inventoryDatabase.HasItem(ingredient.ingredient)
                    ? inventoryDatabase.ownedItems[ingredient.ingredient]
                    : null;

                var itemCount = inventoryDatabase.GetItemAmount(ingredient.ingredient);

                if (itemEntry == null)
                {
                    hasEnoughMaterial = false;
                    break;
                }

                if (itemCount >= ingredient.amount)
                {
                    hasEnoughMaterial = true;
                }
                else
                {
                    hasEnoughMaterial = false;
                    break;
                }
            }

            return hasEnoughMaterial;
        }

        public static bool CanImproveWeapon(InventoryDatabase inventoryDatabase, WeaponInstance weaponInstance, int ownedGold)
        {
            WeaponUpgradeLevel nextWeaponUpgradeLevel = weaponInstance.GetItem<Weapon>().weaponUpgrades.ElementAtOrDefault(weaponInstance.level - 1);

            if (nextWeaponUpgradeLevel == null)
            {
                return false;
            }

            bool hasAllMaterialsRequired = true;

            foreach (var upgradeMaterial in nextWeaponUpgradeLevel.upgradeMaterials)
            {
                if (!inventoryDatabase.HasItem(upgradeMaterial.Key) || inventoryDatabase.GetItemAmount(upgradeMaterial.Key) < upgradeMaterial.Value)
                {
                    hasAllMaterialsRequired = false;
                    break;
                }
            }

            if (!hasAllMaterialsRequired)
            {
                return false;
            }

            if (ownedGold < nextWeaponUpgradeLevel.goldCostForUpgrade)
            {
                return false;
            }

            return true;
        }

        public static void UpgradeWeapon(
            WeaponInstance weaponInstance,
            UnityAction<int> onUpgrade,
            UnityAction<KeyValuePair<UpgradeMaterial, int>> onUpgradeMaterialUsed
        )
        {
            var currentWeaponLevel = weaponInstance.level;

            WeaponUpgradeLevel weaponUpgradeForNextLevel = weaponInstance.GetItem<Weapon>().weaponUpgrades.ElementAtOrDefault(currentWeaponLevel - 1);

            onUpgrade(weaponUpgradeForNextLevel.goldCostForUpgrade);

            foreach (var upgradeMaterial in weaponUpgradeForNextLevel.upgradeMaterials)
            {
                onUpgradeMaterialUsed(upgradeMaterial);
            }

            weaponInstance.level++;
        }

        public static bool IsItemAnIngredientOfCurrentLearnedRecipes(RecipesDatabase recipesDatabase, Item item)
        {
            if (recipesDatabase.craftingRecipes.Count == 0)
            {
                return false;
            }

            foreach (var recipe in recipesDatabase.craftingRecipes)
            {
                if (recipe.ingredients.Exists(ingredient => ingredient.ingredient == item))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<CraftingRecipe> GetRecipesUsingItem(RecipesDatabase recipesDatabase, Item item)
        {
            List<CraftingRecipe> recipesUsingItem = new List<CraftingRecipe>();

            foreach (var recipe in recipesDatabase.craftingRecipes)
            {
                if (recipe.ingredients.Exists(ingredient => ingredient.ingredient == item))
                {
                    recipesUsingItem.Add(recipe);
                }
            }

            return recipesUsingItem;
        }

        public static string GetFormattedTextForRecipesUsingItem(CraftingRecipe[] resultingRecipes)
        {
            string text = LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Use to prepare:");

            for (int i = 0; i < resultingRecipes.Length; i++)
            {
                text += "- " + resultingRecipes[i].resultingItem.GetName() + $" ({resultingRecipes[i].resultingAmount})";
                if (i < resultingRecipes.Length - 1)
                {
                    text += "\n";
                }
            }

            return text;
        }
    }
}
