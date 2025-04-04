using AF.Health;
using AF.Stats;
using NUnit.Framework;
using UnityEngine;

namespace AF.Tests
{
    public class PlayerDamageResistancesTests
    {
        PlayerDamageResistances playerDamageResistances;
        PlayerManager playerManager;
        CharacterDefenseManager defenseStatManager;

        PlayerStatsDatabase playerStatsDatabase;
        StatsBonusController statsBonusController;

        [SetUp]
        public void Setup()
        {
            playerDamageResistances = new GameObject().AddComponent<PlayerDamageResistances>();
            playerDamageResistances.damageReductionFactor = 2;

            playerManager = new GameObject().AddComponent<PlayerManager>();
            playerDamageResistances.playerManager = playerManager;
            defenseStatManager = new GameObject().AddComponent<CharacterDefenseManager>();
            playerManager.characterBaseDefenseManager = defenseStatManager;

            statsBonusController = new GameObject().AddComponent<StatsBonusController>();
            playerManager.characterBaseDefenseManager.character = playerManager;

        }

        [Test]
        public void Test_NoDamageReduction_WhenDefenseAbsorptionIsZero()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 0;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 50);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs5()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 5;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 48);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs50()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 50;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 32);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs75()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 75;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 26);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs100()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 100;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 22);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs150()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 150;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 16);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs200()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 200;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 12);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionIs300()
        {
            var incomingDamage = new Damage();
            int initialDamage = 50;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 300;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 8);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionBonusIs30()
        {
            var incomingDamage = new Damage();
            int initialDamage = 100;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 0;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 70);
        }

        [Test]
        public void Test_DamageReduction_WhenDefenseAbsorptionBonusIs100()
        {
            var incomingDamage = new Damage();
            int initialDamage = 100;
            incomingDamage.physical = initialDamage;

            playerStatsDatabase.endurance = 0;
            defenseStatManager.damagedAbsorbed.physical = 0;

            var filteredDamage = playerDamageResistances.FilterIncomingDamage(incomingDamage);

            Assert.AreNotEqual(initialDamage, filteredDamage.physical);
            Assert.AreEqual(filteredDamage.physical, 0);
        }

    }
}
