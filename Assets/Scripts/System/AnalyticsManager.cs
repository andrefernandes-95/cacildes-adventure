
namespace AF
{

    public static class AnalyticsUtils
    {
        public static string OnBossKilled(string bossName)
        {
            return $"BossKilled:{bossName}";
        }

        public static string OnUIButtonClick(string buttonName)
        {
            return $"UI:ButtonClick:{buttonName}";
        }

        public static string OnBossWeaponAcquired(string name)
        {
            return $"Inventory:BossWeaponAcquired:{name}";
        }

        public static string OnArmorAcquired(string name)
        {
            return $"Inventory:ArmorAcquired:{name}";
        }

        public static string OnSpellAcquired(string name)
        {
            return $"Inventory:SpellAcquired:{name}";
        }

        public static string OnArenaWon(string mapName)
        {
            return $"ArenaWon:{mapName}";
        }

        public static string OnUnlockBonfire(string name)
        {
            return $"UnlockBonfire:{name}";
        }


        public static string OnQuestProgressed(string questName, string questObjectiveCompleted)
        {
            return $"Quest:{questName}:Started:{questObjectiveCompleted}";
        }
    }
}
