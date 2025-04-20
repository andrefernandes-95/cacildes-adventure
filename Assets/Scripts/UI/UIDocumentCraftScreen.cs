using System.Collections.Generic;
using System.Linq;
using AF.Health;
using AF.Inventory;
using AF.Music;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace AF
{
    public class UIDocumentCraftScreen : MonoBehaviour
    {
        public enum CraftActivity
        {
            ALCHEMY,
            COOKING,
            BLACKSMITH,
        }

        public CraftActivity craftActivity;

        [Header("UI")]
        public VisualTreeAsset recipeItem;
        public VisualTreeAsset ingredientItem;
        public Sprite alchemyBackgroundImage;
        public Sprite cookingBackgroundImage;
        public Sprite blacksmithBackgroundImage;

        public Sprite goldSprite;

        [Header("SFX")]
        public AudioClip sfxOnEnterMenu;

        [Header("UI Components")]
        public UIDocument uIDocument;
        [HideInInspector] public VisualElement root;
        public UIDocumentBonfireMenu uIDocumentBonfireMenu;
        public UIDocumentPlayerGold uIDocumentPlayerGold;

        [Header("Components")]
        public NotificationManager notificationManager;
        public UIManager uiManager;
        public PlayerManager playerManager;
        public BGMManager bgmManager;
        public Soundbank soundbank;

        [HideInInspector] public bool returnToBonfire = false;

        [Header("Databases")]
        public RecipesDatabase recipesDatabase;
        public InventoryDatabase inventoryDatabase;
        public PlayerStatsDatabase playerStatsDatabase;

        // Last scroll position
        int lastScrollElementIndex = -1;

        [Header("Localization")]
        // "Crafting Table"
        public LocalizedString CraftingTable_LocalizedString;

        // "Weapon Upgrades"
        public LocalizedString WeaponUpgrades_LocalizedString;
        // "Return"
        public LocalizedString Return_LocalizedString;
        // "Craft"
        public LocalizedString Craft_LocalizedString;
        // "Cook"
        public LocalizedString Cook_LocalizedString;
        // "Upgrade"
        public LocalizedString Upgrade_LocalizedString;
        // "Next Physical Damage: "
        public LocalizedString NextPhysicalDamage_LocalizedString;
        // "Next Fire Bonus: "
        public LocalizedString NextFireBonus_LocalizedString;
        // "Next Frost Bonus: "
        public LocalizedString NextFrostBonus_LocalizedString;
        // "Next Lightning Bonus: "
        public LocalizedString NextLightningBonus_LocalizedString;
        // "Next Magic Bonus: "
        public LocalizedString NextMagicBonus_LocalizedString;
        // "Next Darkness Bonus: "
        public LocalizedString NextDarknessBonus_LocalizedString;
        // "Gold"
        public LocalizedString Gold_LocalizedString;


        private void Awake()
        {
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            this.root = uIDocument.rootVisualElement;

            bgmManager.PlaySound(sfxOnEnterMenu, null);

            DrawUI();
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OpenBlacksmithMenu()
        {
            LogAnalytic(AnalyticsUtils.OnUIButtonClick("Blacksmith"));

            this.craftActivity = CraftActivity.BLACKSMITH;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OpenAlchemyMenu()
        {
            LogAnalytic(AnalyticsUtils.OnUIButtonClick("Alchemy"));

            this.craftActivity = CraftActivity.ALCHEMY;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnClose()
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            Close();
        }

        public void Close()
        {
            if (returnToBonfire)
            {
                returnToBonfire = false;

                uIDocumentBonfireMenu.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                return;
            }

            playerManager.playerComponentManager.EnableComponents();
            playerManager.playerComponentManager.EnableCharacterController();

            this.gameObject.SetActive(false);
        }

        void ClearPreviews()
        {
            root.Q<VisualElement>("IngredientsListPreview").style.opacity = 0;

            root.Q<Label>("WeaponLevelPreview").text = "";
            root.Q<Label>("WeaponLevelPreview").style.display = DisplayStyle.None;
            root.Q<Label>("PhysicalAttack").style.display = DisplayStyle.None;
            root.Q<Label>("FireAttack").style.display = DisplayStyle.None;
            root.Q<Label>("FrostAttack").style.display = DisplayStyle.None;
            root.Q<Label>("LightningAttack").style.display = DisplayStyle.None;
            root.Q<Label>("MagicAttack").style.display = DisplayStyle.None;
            root.Q<Label>("DarknessAttack").style.display = DisplayStyle.None;
        }

        void SetupActivity()
        {
            string targetActivityTitleText = "";
            StyleBackground targetBackground = null;
            if (craftActivity == CraftActivity.ALCHEMY)
            {
                targetActivityTitleText = CraftingTable_LocalizedString.GetLocalizedString();
                targetBackground = new StyleBackground(alchemyBackgroundImage);
            }
            else if (craftActivity == CraftActivity.BLACKSMITH)
            {
                targetActivityTitleText = WeaponUpgrades_LocalizedString.GetLocalizedString();
                targetBackground = new StyleBackground(blacksmithBackgroundImage);
            }
            root.Q<VisualElement>("ImageBack").style.backgroundImage = targetBackground;
            root.Q<Label>("CraftActivityTitle").text = targetActivityTitleText;
        }

        void DrawUI()
        {
            ClearPreviews();

            SetupActivity();

            PopulateScrollView(recipesDatabase.craftingRecipes.ToArray());
        }

        void PopulateScrollView(CraftingRecipe[] ownedCraftingRecipes)
        {
            root.Q<VisualElement>("WeaponNextUpgradeDescription").style.display = DisplayStyle.None;

            var scrollView = this.root.Q<ScrollView>();
            scrollView.Clear();

            Button exitButton = new()
            {
                text = Return_LocalizedString.GetLocalizedString()
            };
            exitButton.AddToClassList("primary-button");
            UIUtils.SetupButton(exitButton, () =>
            {
                Close();
            }, soundbank);

            scrollView.Add(exitButton);


            if (craftActivity == CraftActivity.BLACKSMITH)
            {
                PopulateWeaponsScrollView();
            }
            else
            {
                PopulateCraftingScroll(scrollView, ownedCraftingRecipes);
            }

            if (lastScrollElementIndex == -1)
            {
                scrollView.ScrollTo(exitButton);
                exitButton.Focus();
            }
            else
            {
                Invoke(nameof(GiveFocus), 0f);
            }

        }

        void GiveFocus()
        {
            UIUtils.ScrollToLastPosition(
                lastScrollElementIndex,
                root.Q<ScrollView>(),
                () =>
                {
                    lastScrollElementIndex = -1;
                }
            );
        }


        public string GetItemDescription(CraftingRecipe recipe)
        {
            if (recipe.resultingItem == null)
            {
                return "";
            }

            string itemDescription = recipe.resultingItem.GetShortDescription()?.Length > 0 ?
                                     recipe.resultingItem.GetShortDescription().Substring(
                                        0, System.Math.Min(60, recipe.resultingItem.GetShortDescription().Length)) : "";
            return itemDescription + (recipe.resultingItem.GetShortDescription()?.Length > 60 ? "..." : "");
        }

        void PopulateCraftingScroll(ScrollView scrollView, CraftingRecipe[] ownedCraftingRecipes)
        {
            if (ownedCraftingRecipes.Length <= 0)
            {
                return;
            }

            int i = 0;
            foreach (var recipe in ownedCraftingRecipes)
            {
                int currentIndex = i;
                var scrollItem = this.recipeItem.CloneTree();

                scrollItem.Q<IMGUIContainer>("ItemIcon").style.backgroundImage = new StyleBackground(recipe.resultingItem?.sprite);
                scrollItem.Q<Label>("ItemName").text = recipe.resultingItem?.GetName() + $" ({recipe.resultingAmount})";

                scrollItem.Q<Label>("ItemDescription").text = GetItemDescription(recipe);
                scrollItem.Q<Label>("ItemDescription").style.display = DisplayStyle.Flex;

                var craftBtn = scrollItem.Q<Button>("CraftButtonItem");
                var craftLabel = scrollItem.Q<Label>("CraftLabel");
                craftLabel.text = GetCraftLabel();

                craftBtn.style.opacity = CraftingUtils.CanCraftItem(playerManager.characterBaseInventory, recipe) ? 1f : 0.25f;

                UIUtils.SetupButton(craftBtn,
                () =>
                {
                    lastScrollElementIndex = currentIndex;

                    if (!CraftingUtils.CanCraftItem(playerManager.characterBaseInventory, recipe))
                    {
                        HandleCraftError(LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Missing ingredients!"));
                        return;
                    }

                    if (ShouldRuinMixture(recipe))
                    {
                        HandleCraftError(LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Crafting failed! Try again..."));
                        return;
                    }

                    HandleCraftSuccess(recipe);

                    DrawUI();
                },
                () =>
                {
                    ShowRequiredIngredients(recipe);
                    scrollView.ScrollTo(craftBtn);
                },
                () =>
                {

                },
                true,
                soundbank);

                scrollView.Add(craftBtn);

                i++;
            }

        }

        void PopulateWeaponsScrollView()
        {
            var scrollView = this.root.Q<ScrollView>();

            int i = 0;

            foreach (WeaponInstance weaponInstance in GetUpgradeableWeapons())
            {
                int currentIndex = i;

                Weapon wp = weaponInstance.GetItem<Weapon>();

                if (ShouldSkipUpgrade(wp, wp.level))
                {
                    continue;
                }

                var scrollItem = this.recipeItem.CloneTree();

                scrollItem.Q<IMGUIContainer>("ItemIcon").style.backgroundImage = new StyleBackground(wp.sprite);
                scrollItem.Q<Label>("ItemName").text = GetWeaponName(wp);
                scrollItem.Q<Label>("ItemDescription").style.display = DisplayStyle.None;

                var craftBtn = scrollItem.Q<Button>("CraftButtonItem");
                var craftLabel = scrollItem.Q<Label>("CraftLabel");
                craftLabel.text = GetCraftLabel();

                craftBtn.style.opacity = CraftingUtils.CanImproveWeapon(playerManager.characterBaseInventory, weaponInstance, playerStatsDatabase.gold) ? 1f : 0.25f;

                UIUtils.SetupButton(craftBtn, () =>
                {
                    lastScrollElementIndex = currentIndex;

                    if (!CraftingUtils.CanImproveWeapon(playerManager.characterBaseInventory, weaponInstance, playerStatsDatabase.gold))
                    {
                        HandleCraftError(LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Missing ingredients!"));
                        return;
                    }

                    HandleWeaponUpgrade(weaponInstance);

                    DrawUI();
                },
                () =>
                {
                    ShowRequirements(weaponInstance);
                    scrollView.ScrollTo(craftBtn);
                },
                () => { },
                true,
                soundbank);

                scrollView.Add(craftBtn);

                i++;
            }
        }

        // Helper methods
        string GetCraftLabel()
        {
            return craftActivity switch
            {
                CraftActivity.ALCHEMY => Craft_LocalizedString.GetLocalizedString(),
                CraftActivity.COOKING => Cook_LocalizedString.GetLocalizedString(),
                CraftActivity.BLACKSMITH => Upgrade_LocalizedString.GetLocalizedString(),
                _ => "",
            };
        }

        void HandleCraftError(string errorMessage)
        {
            soundbank.PlaySound(soundbank.craftError);
            notificationManager.ShowNotification(errorMessage, notificationManager.alchemyLackOfIngredients);
        }

        bool ShouldRuinMixture(CraftingRecipe recipe)
        {
            var ingredientThatCanRuinMixture = recipe.ingredients.FirstOrDefault(x => x.ingredient.chanceToRuinMixture > 0);
            return ingredientThatCanRuinMixture != null && Random.Range(0, 100) < ingredientThatCanRuinMixture.ingredient.chanceToRuinMixture;
        }

        void HandleCraftSuccess(CraftingRecipe recipe)
        {
            if (recipe.resultingItem == null)
            {
                return;
            }

            if (craftActivity == CraftActivity.COOKING)
            {
                playerManager.playerAchievementsManager.achievementForCookingFirstMeal.AwardAchievement();
            }
            else if (craftActivity == CraftActivity.ALCHEMY)
            {
                playerManager.playerAchievementsManager.achievementForBrewingFirstPotion.AwardAchievement();
            }

            LogAnalytic(AnalyticsUtils.OnUIButtonClick("CraftItem"), new() {
                { "item_created", recipe.resultingItem.name }
            });

            soundbank.PlaySound(soundbank.craftSuccess);

            // Must be consumable?
            for (int i = 0; i < recipe.resultingAmount; i++)
            {
                if (recipe.resultingItem is Consumable createdConsumable)
                {
                    playerManager.playerInventory.AddConsumable(createdConsumable);
                }
            }

            notificationManager.ShowNotification(LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Received") + $" x{recipe.resultingAmount} " + recipe.resultingItem?.GetName(), recipe.resultingItem?.sprite);

            foreach (var ingredient in recipe.ingredients)
            {
                playerManager.playerInventory.RemoveItem(ingredient.ingredient);
            }
        }

        List<ItemInstance> GetUpgradeableWeapons()
        {
            return inventoryDatabase.ownedItems
                .Where(itemEntry => itemEntry.Key is Weapon wp && wp.canBeUpgraded)
                .SelectMany(itemEntry => itemEntry.Value).ToList();
        }

        bool ShouldSkipUpgrade(Weapon wp, int nextLevel)
        {
            return wp.canBeUpgraded == false || nextLevel >= wp.weaponUpgrades.Count();
        }

        string GetWeaponName(Weapon wp)
        {
            return $"{wp.GetName()} +{wp.level} > {wp.GetName()} +{wp.level + 1}";
        }

        void HandleWeaponUpgrade(WeaponInstance wp)
        {
            playerManager.playerAchievementsManager.achievementForUpgradingFirstWeapon.AwardAchievement();
            soundbank.PlaySound(soundbank.craftSuccess);
            notificationManager.ShowNotification(LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Weapon improved!"), wp.GetItem<Weapon>().sprite);


            LogAnalytic(AnalyticsUtils.OnUIButtonClick("UpgradeWeapon"), new() {
                { "weapon_upgraded", wp.GetItem<Weapon>().name }
            });

            CraftingUtils.UpgradeWeapon(
                wp,
                (goldUsed) => uIDocumentPlayerGold.LoseGold(goldUsed),
                (upgradeMaterialUsed) => playerManager.playerInventory.RemoveItem(upgradeMaterialUsed.Key)
            );
        }

        void ShowRequiredIngredients(CraftingRecipe recipe)
        {
            root.Q<VisualElement>("ItemInfo").Clear();

            foreach (var ingredient in recipe.ingredients)
            {
                var ingredientItemEntry = ingredientItem.CloneTree();
                ingredientItemEntry.Q<IMGUIContainer>("ItemIcon").style.backgroundImage = new StyleBackground(ingredient.ingredient.sprite);
                ingredientItemEntry.Q<Label>("Title").text = ingredient.ingredient.GetName();

                var playerOwnedIngredientAmount = 0;

                var playerOwnedIngredient = playerManager.characterBaseInventory.HasItem(ingredient.ingredient)
                    ? inventoryDatabase.ownedItems[ingredient.ingredient]
                    : null;

                if (playerOwnedIngredient != null)
                {
                    playerOwnedIngredientAmount = playerManager.characterBaseInventory.GetItemQuantity(ingredient.ingredient);
                }

                ingredientItemEntry.Q<Label>("Amount").text = playerOwnedIngredientAmount + " / " + ingredient.amount;
                ingredientItemEntry.Q<Label>("Amount").style.opacity = playerOwnedIngredientAmount >= ingredient.amount ? 1 : 0.25f;

                root.Q<VisualElement>("ItemInfo").Add(ingredientItemEntry);
            }

            root.Q<VisualElement>("IngredientsListPreview").style.opacity = 1;
        }
        void ShowRequirements(WeaponInstance weaponInstance)
        {
            Weapon weapon = weaponInstance.GetItem<Weapon>();
            WeaponUpgradeLevel weaponUpgradeLevel = weapon.weaponUpgrades.ElementAtOrDefault(weapon.level - 1);

            if (weaponUpgradeLevel == null)
            {
                return;
            }

            var nextLevel = weaponInstance.level + 1;
            root.Q<VisualElement>("WeaponNextUpgradeDescription").style.display = DisplayStyle.Flex;

            // Weapon preview
            root.Q<Label>("WeaponLevelPreview").text = weapon.GetName() + " +" + nextLevel;
            root.Q<Label>("PhysicalAttack").style.display = DisplayStyle.None;
            root.Q<Label>("FireAttack").style.display = DisplayStyle.None;
            root.Q<Label>("FrostAttack").style.display = DisplayStyle.None;
            root.Q<Label>("LightningAttack").style.display = DisplayStyle.None;
            root.Q<Label>("MagicAttack").style.display = DisplayStyle.None;
            root.Q<Label>("DarknessAttack").style.display = DisplayStyle.None;

            Damage currentWeaponDamage = playerManager.characterBaseAttackManager.currentRightWeaponDamage;
            Damage nextLevelWeaponDamage = weapon.GetDamageForLevel(weaponInstance.level);

            if (nextLevelWeaponDamage.physical > 0)
            {
                root.Q<Label>("PhysicalAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("PhysicalAttack").text = NextPhysicalDamage_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.physical + " > " + nextLevelWeaponDamage.physical;
            }
            if (nextLevelWeaponDamage.fire > 0)
            {
                root.Q<Label>("FireAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("FireAttack").text = NextFireBonus_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.fire + " > " + nextLevelWeaponDamage.fire;
            }
            if (nextLevelWeaponDamage.frost > 0)
            {
                root.Q<Label>("FrostAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("FrostAttack").text = NextFrostBonus_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.frost + " > " + nextLevelWeaponDamage.frost;
            }

            if (nextLevelWeaponDamage.lightning > 0)
            {
                root.Q<Label>("LightningAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("LightningAttack").text = NextLightningBonus_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.lightning + " > " + nextLevelWeaponDamage.lightning;
            }
            if (nextLevelWeaponDamage.magic > 0)
            {
                root.Q<Label>("MagicAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("MagicAttack").text = NextMagicBonus_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.magic + " > " + nextLevelWeaponDamage.magic;
            }
            if (nextLevelWeaponDamage.darkness > 0)
            {
                root.Q<Label>("DarknessAttack").style.display = DisplayStyle.Flex;
                root.Q<Label>("DarknessAttack").text = NextDarknessBonus_LocalizedString.GetLocalizedString() + " "
                    + currentWeaponDamage.darkness + " > " + nextLevelWeaponDamage.darkness;
            }

            // TODO: Add Water DMG

            // Requirements

            root.Q<VisualElement>("ItemInfo").Clear();

            foreach (var upgradeMaterial in weaponUpgradeLevel.upgradeMaterials)
            {
                UpgradeMaterial upgradeMaterialItem = upgradeMaterial.Key;
                int amountRequiredFoUpgrade = upgradeMaterial.Value;

                var ingredientItemEntry = ingredientItem.CloneTree();
                ingredientItemEntry.Q<IMGUIContainer>("ItemIcon").style.backgroundImage = new StyleBackground(upgradeMaterialItem.sprite);
                ingredientItemEntry.Q<Label>("Title").text = upgradeMaterialItem.GetName();

                var playerOwnedIngredient = playerManager.characterBaseInventory.HasItem(upgradeMaterialItem)
                    ? inventoryDatabase.ownedItems[upgradeMaterialItem]
                    : null;

                var playerOwnedIngredientAmount = 0;
                if (playerOwnedIngredient != null)
                {

                    playerOwnedIngredientAmount = playerManager.characterBaseInventory.GetItemQuantity(upgradeMaterialItem);
                }
                ingredientItemEntry.Q<Label>("Amount").text = playerOwnedIngredientAmount + " / " + amountRequiredFoUpgrade;
                ingredientItemEntry.Q<Label>("Amount").style.opacity =
                    playerOwnedIngredient != null && playerOwnedIngredientAmount >= amountRequiredFoUpgrade ? 1 : 0.25f;

                root.Q<VisualElement>("ItemInfo").Add(ingredientItemEntry);
            }

            // Add Gold

            var goldItemEntry = ingredientItem.CloneTree();
            goldItemEntry.Q<IMGUIContainer>("ItemIcon").style.backgroundImage = new StyleBackground(goldSprite);
            goldItemEntry.Q<Label>("Title").text = LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Gold");

            goldItemEntry.Q<Label>("Amount").text = playerStatsDatabase.gold + " / " + weaponUpgradeLevel.goldCostForUpgrade;
            goldItemEntry.Q<Label>("Amount").style.opacity = playerStatsDatabase.gold >= weaponUpgradeLevel.goldCostForUpgrade ? 1 : 0.25f;

            root.Q<VisualElement>("ItemInfo").Add(goldItemEntry);
            root.Q<VisualElement>("IngredientsListPreview").style.opacity = 1;
        }

        void LogAnalytic(string eventName)
        {
            if (!GameAnalytics.Initialized)
            {
                GameAnalytics.Initialize();
            }

            GameAnalytics.NewDesignEvent(eventName);
        }
        void LogAnalytic(string eventName, Dictionary<string, object> values)
        {
            if (!GameAnalytics.Initialized)
            {
                GameAnalytics.Initialize();
            }

            GameAnalytics.NewDesignEvent(eventName, values);
        }
    }
}
