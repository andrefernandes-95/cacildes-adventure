using System.Collections.Generic;
using AF.Companions;
using AF.Inventory;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace AF
{

#if UNITY_EDITOR

    [CustomEditor(typeof(PlayerPreset), editorForChildClasses: true)]
    public class EventEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            // Ensure GUI is enabled for the button
            GUI.enabled = true;
            PlayerPreset playerPreset = target as PlayerPreset;

            if (GUILayout.Button("Start Playtest"))
            {
                if (!Application.isPlaying)
                {
                    // Start play mode
                    EditorApplication.isPlaying = true;

                    playerPreset.LoadPlayerPreset();
                }
            }

            base.OnInspectorGUI();
        }
    }

#endif



    [CreateAssetMenu(fileName = "New Player Preset", menuName = "System/New Player Preset", order = 0)]
    public class PlayerPreset : ScriptableObject
    {
        [Header("Inventory")]
        public SerializedDictionary<Item, ItemAmount> ownedItems = new();
        public bool loadAllItems = false;

        [Header("Equipped Items")]
        public Weapon[] weapons = new Weapon[3]; // Fixed size array for weapons

        public Shield[] shields = new Shield[3]; // Fixed size array for shields

        public Arrow[] arrows = new Arrow[2];

        public Spell[] spells = new Spell[5];

        public Consumable[] consumables = new Consumable[10];
        public Accessory[] accessories = new Accessory[4];

        public Helmet helmet;
        public Armor armor;
        public Gauntlet gauntlet;
        public Legwear legwear;

        [Header("Stats")]

        public int vitality = 1;
        public int endurance = 1;
        public int strength = 1;
        public int dexterity = 1;
        public int intelligence = 1;
        public int reputation = 1;
        public int gold = 0;


        [Header("Quests")]
        public List<QuestParent> completedQuests = new();
        public QuestParent currentQuest;
        public int currentQuestProgress;

        [Header("Components")]
        public PlayerStatsDatabase playerStatsDatabase;
        public EquipmentDatabase equipmentDatabase;

        public InventoryDatabase inventoryDatabase;

        [Header("Companions")]
        public CompanionID[] companions;
        public CompanionsDatabase companionsDatabase;

        public void LoadPlayerPreset()
        {
            LoadStats();
            LoadInventory();
            LoadEquipment();
            LoadQuests();
            LoadCompanions();
        }

        void LoadStats()
        {
            playerStatsDatabase.vitality = vitality;
            playerStatsDatabase.endurance = endurance;
            playerStatsDatabase.strength = strength;
            playerStatsDatabase.dexterity = dexterity;
            playerStatsDatabase.intelligence = intelligence;
            playerStatsDatabase.reputation = reputation;
            playerStatsDatabase.gold = gold;
        }

        void LoadInventory()
        {
            if (loadAllItems)
            {
                Item[] items = Resources.LoadAll<Item>("Items");
                foreach (var item in items)
                {
                    inventoryDatabase.AddItem(item);
                }
                return;
            }

            foreach (var ownedItem in ownedItems)
            {
                inventoryDatabase.AddItem(ownedItem.Key, ownedItem.Value.amount);
            }
        }

        void LoadEquipment()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    equipmentDatabase.EquipWeapon(weapons[i], i);
                }
            }
            for (int i = 0; i < shields.Length; i++)
            {
                if (shields[i] != null)
                {
                    equipmentDatabase.EquipShield(shields[i], i);
                }
            }
            for (int i = 0; i < arrows.Length; i++)
            {
                if (arrows[i] != null)
                {
                    equipmentDatabase.EquipArrow(arrows[i], i);
                }
            }
            for (int i = 0; i < spells.Length; i++)
            {
                if (spells[i] != null)
                {
                    equipmentDatabase.EquipSpell(spells[i], i);
                }
            }
            for (int i = 0; i < accessories.Length; i++)
            {
                if (accessories[i] != null)
                {
                    equipmentDatabase.EquipAccessory(accessories[i], i);
                }
            }
            for (int i = 0; i < consumables.Length; i++)
            {
                if (consumables[i] != null)
                {
                    equipmentDatabase.EquipConsumable(consumables[i], i);
                }
            }

            equipmentDatabase.EquipHelmet(helmet);
            equipmentDatabase.EquipArmor(armor);
            equipmentDatabase.EquipLegwear(legwear);
            equipmentDatabase.EquipGauntlet(gauntlet);
        }

        void LoadQuests()
        {
            foreach (var completedQuest in completedQuests)
            {
                int questObjectiveCount = completedQuest.questObjectives.Length;

                completedQuest.SetProgress(questObjectiveCount);
            }

            if (currentQuest != null)
            {
                currentQuest.SetProgress(currentQuestProgress);
                currentQuest.Track();
            }
        }

        void LoadCompanions()
        {
            if (companionsDatabase != null && companions.Length > 0)
            {
                foreach (var c in companions)
                {
                    companionsDatabase.AddToParty(c.GetCompanionID());
                }
            }
        }
    }

}
