using NUnit.Framework;
using UnityEngine;

namespace AF.Tests
{
    public class FormulasTests
    {
        [TestCase(100, 1, 200, 129)]
        [TestCase(100, 5, 200, 167)]
        [TestCase(100, 10, 200, 198)]
        [TestCase(100, 15, 200, 223)]
        [TestCase(100, 25, 200, 265)]
        [TestCase(100, 50, 200, 350)]
        [TestCase(100, 100, 200, 482)]
        public void CalculateStatForLevel_ValidInputs_Multiplier200_ReturnsExpectedResult(int baseValue, int level, int multiplier, int expectedResult)
        {
            // Act
            int result = Formulas.CalculateStatForLevel(baseValue, level, multiplier);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(100, 1, 2.25f, 102)]
        [TestCase(100, 5, 2.25f, 111)]
        [TestCase(100, 10, 2.25f, 122)]
        [TestCase(100, 15, 2.25f, 133)]
        [TestCase(100, 25, 2.25f, 156)]
        [TestCase(100, 50, 2.25f, 212)]
        [TestCase(100, 100, 2.25f, 325)]
        public void CalculateStatForLevel_ForOldFormula_ReturnsExpectedResult(int baseValue, int level, float multiplier, int expectedResult)
        {
            // Act
            int result = baseValue + (int)(level * multiplier);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(1, 30, 1)]
        [TestCase(2, 30, 12)]
        [TestCase(3, 30, 27)]
        [TestCase(4, 30, 37)]
        [TestCase(5, 30, 45)]
        [TestCase(15, 30, 80)]
        [TestCase(25, 30, 95)]
        [TestCase(50, 30, 117)]
        [TestCase(100, 30, 138)]
        [TestCase(500, 30, 186)]
        [TestCase(1000, 30, 207)]
        public void TestLog(int level, float scaleFactor, int expectedResult)
        {
            // Act
            int result = (int)Formulas.LogarithmicFunctionValue(level, scaleFactor);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        float GetScaling(string weaponScaling)
        {

            float scaling = 0f;
            if (weaponScaling == "D") scaling = Formulas.D;
            if (weaponScaling == "C") scaling = Formulas.C;
            if (weaponScaling == "B") scaling = Formulas.B;
            if (weaponScaling == "A") scaling = Formulas.A;
            if (weaponScaling == "S") scaling = Formulas.S;
            return scaling;
        }

        [TestCase(1, "E", 0)]
        [TestCase(1, "D", 1)]
        [TestCase(1, "C", 1)]
        [TestCase(1, "B", 1)]
        [TestCase(1, "A", 1)]
        [TestCase(1, "S", 1)]
        public void GetBonusFromWeaponForLevels_1(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(5, "E", 0)]
        [TestCase(5, "D", 2)]
        [TestCase(5, "C", 5)]
        [TestCase(5, "B", 9)]
        [TestCase(5, "A", 14)]
        [TestCase(5, "S", 19)]
        public void GetBonusFromWeaponForLevels_5(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(15, "E", 0)]
        [TestCase(15, "D", 6)]
        [TestCase(15, "C", 12)]
        [TestCase(15, "B", 22)]
        [TestCase(15, "A", 34)]
        [TestCase(15, "S", 46)]
        public void GetBonusFromWeaponForLevels_15(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(30, "E", 0)]
        [TestCase(30, "D", 11)]
        [TestCase(30, "C", 21)]
        [TestCase(30, "B", 38)]
        [TestCase(30, "A", 60)]
        [TestCase(30, "S", 80)]
        public void GetBonusFromWeaponForLevels_30(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(60, "E", 0)]
        [TestCase(60, "D", 20)]
        [TestCase(60, "C", 39)]
        [TestCase(60, "B", 71)]
        [TestCase(60, "A", 113)]
        [TestCase(60, "S", 150)]
        public void GetBonusFromWeaponForLevels_60(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(100, "E", 0)]
        [TestCase(100, "D", 34)]
        [TestCase(100, "C", 65)]
        [TestCase(100, "B", 118)]
        [TestCase(100, "A", 187)]
        [TestCase(100, "S", 249)]
        public void GetBonusFromWeaponForLevels_100(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(250, "E", 0)]
        [TestCase(250, "D", 91)]
        [TestCase(250, "C", 172)]
        [TestCase(250, "B", 313)]
        [TestCase(250, "A", 495)]
        [TestCase(250, "S", 657)]
        public void GetBonusFromWeaponForLevels_250(int currentStatLevel, string weaponScaling, int expectedResult)
        {
            // Act
            int result = Formulas.GetBonusFromWeapon(currentStatLevel, GetScaling(weaponScaling));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
