namespace AF
{
    using System.Collections.Generic;
    using UnityEngine;
    using AYellowpaper.SerializedCollections;
    using UnityEngine.UI;
    using System.Linq;

    public class SyntyCharacterModelManager : MonoBehaviour
    {
        public SerializedDictionary<string, GameObject> syntyCharacterBodyParts = new();

        public CharacterBaseManager character;

        public void Initialize()
        {
            CacheCharacterParts();
            InitializeDefaultBodyParts();
        }

        void InitializeDefaultBodyParts()
        {
            ToggleHair(true);
            ToggleBeard(true);
            ToggleEyebrows(true);
            ToggleFace(true);
            ToggleTorso(true);
            ToggleHands(true);
            ToggleLegs(true);
        }

        void CacheCharacterParts()
        {
            syntyCharacterBodyParts.Clear();

            var syntyPieces = transform.GetChild(0).GetComponentsInChildren<Transform>(true);

            foreach (Transform t in syntyPieces)
            {
                if (!syntyCharacterBodyParts.ContainsKey(t.gameObject.name))
                {
                    syntyCharacterBodyParts.Add(t.gameObject.name, t.gameObject);

                    // Hide All Pieces By Default
                    t.gameObject.SetActive(false);
                }
            }

            if (syntyCharacterBodyParts.ContainsKey("Armature"))
            {
                // TODO: Improve this logic

                syntyCharacterBodyParts["Armature"].SetActive(true);
                foreach (Transform child in syntyCharacterBodyParts["Armature"].transform)
                {
                    SetActiveRecursively(child.gameObject, true);
                }
            }

            if (syntyCharacterBodyParts.ContainsKey("Exported Synty Character"))
            {
                syntyCharacterBodyParts["Exported Synty Character"].SetActive(true);
            }
        }
        void SetActiveRecursively(GameObject obj, bool isActive)
        {
            obj.SetActive(isActive);

            foreach (Transform child in obj.transform)
            {
                SetActiveRecursively(child.gameObject, isActive);
            }
        }

        public void ShowPiece(string gameObjectName)
        {
            if (syntyCharacterBodyParts.ContainsKey(gameObjectName))
            {
                syntyCharacterBodyParts[gameObjectName].SetActive(true);
            }
        }

        public void HidePiece(string gameObjectName)
        {
            if (syntyCharacterBodyParts.ContainsKey(gameObjectName))
            {
                syntyCharacterBodyParts[gameObjectName].SetActive(false);
            }
        }

        void TogglePiece(List<string> pieces, bool show)
        {
            foreach (string gameObjectName in pieces)
            {
                if (show)
                {
                    ShowPiece(gameObjectName);
                }
                else
                {
                    HidePiece(gameObjectName);
                }
            }
        }

        public void ToggleHair(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetHairs(), show);
        }
        public void ToggleEyebrows(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetEyebrows(), show);
        }
        public void ToggleBeard(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetBeard(), show);
        }
        public void ToggleFace(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetFace(), show);
        }
        public void ToggleTorso(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetTorso(), show);
        }
        public void ToggleHands(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetHands(), show);
        }
        public void ToggleLegs(bool show)
        {
            TogglePiece(character.characterBaseAppearance.GetLegs(), show);
        }




        public void InitializeEquipment()
        {
            /*
            DrawCharacterGraphics();

            if (equipmentDatabase.helmet.Exists())
            {
                EquipHelmet(equipmentDatabase.helmet);
            }

            if (equipmentDatabase.armor.Exists())
            {
                EquipArmor(equipmentDatabase.armor);
            }

            if (equipmentDatabase.legwear.Exists())
            {
                EquipLegwear(equipmentDatabase.legwear);
            }

            if (equipmentDatabase.gauntlet.Exists())
            {
                EquipGauntlet(equipmentDatabase.gauntlet);
            }

            for (int i = 0; i < equipmentDatabase.accessories.Length; i++)
            {
                EquipAccessory(equipmentDatabase.accessories[i], i);
            }*/
        }

        #region Helmet
        public void EquipHelmet(HelmetInstance helmetToEquip)
        {/*
            HelmetInstance helmetToEquipClone = helmetToEquip.Clone();

            if (helmetToEquip.IsEmpty())
            {
                return;
            }

            UnequipHelmet();

            if (helmetToEquipClone != equipmentDatabase.helmet)
            {
                equipmentDatabase.EquipHelmet(helmetToEquipClone);
            }

            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();*/
        }

        public void UnequipHelmet()
        {/*
            foreach (Transform t in playerEquipmentRoot.GetComponentsInChildren<Transform>(true))
            {
                if (equipmentDatabase.helmet.Exists())
                {
                    if (equipmentDatabase.helmet.GetItem<Helmet>()?.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }

                    if (equipmentDatabase.helmet.GetItem<Helmet>()?.graphicNamesToHide?.Contains(t.gameObject.name) ?? false)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }

            equipmentDatabase.UnequipHelmet();

            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();*/
        }
        #endregion

        #region Armor
        public void EquipArmor(ArmorInstance armorToEquip)
        {/*
            ArmorInstance armorToEquipClone = armorToEquip.Clone();

            if (armorToEquip.IsEmpty())
            {
                return;
            }

            UnequipArmor();

            if (armorToEquipClone != equipmentDatabase.armor)
            {
                equipmentDatabase.EquipArmor(armorToEquipClone);
            }

            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();

            EventManager.EmitEvent(EventMessages.ON_BODY_TYPE_CHANGED);*/
        }

        public void UnequipArmor()
        {/*
            foreach (Transform t in playerEquipmentRoot.GetComponentsInChildren<Transform>(true))
            {
                if (equipmentDatabase.armor.Exists())
                {
                    if (equipmentDatabase.armor.GetItem<Armor>()?.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }

                    if (equipmentDatabase.armor.GetItem<Armor>()?.graphicNamesToHide?.Contains(t.gameObject.name) ?? false)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }

            equipmentDatabase.UnequipArmor();

            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();
            */
        }
        #endregion

        #region Gauntlets
        public void EquipGauntlet(GauntletInstance gauntletToEquip)
        {
            /*
            GauntletInstance gauntletToEquipClone = gauntletToEquip.Clone();
            if (gauntletToEquip.IsEmpty())
            {
                return;
            }

            UnequipGauntlet();

            if (gauntletToEquipClone != equipmentDatabase.gauntlet)
            {
                equipmentDatabase.EquipGauntlet(gauntletToEquipClone);
            }

            DrawCharacterGraphics();
            statsBonusController.RecalculateEquipmentBonus();*/
        }

        public void UnequipGauntlet()
        {
            /*
            foreach (Transform t in playerEquipmentRoot.GetComponentsInChildren<Transform>(true))
            {
                if (equipmentDatabase.gauntlet.Exists())
                {
                    if (equipmentDatabase.gauntlet.GetItem<Gauntlet>()?.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }

                    if (equipmentDatabase.gauntlet.GetItem<Gauntlet>()?.graphicNamesToHide?.Contains(t.gameObject.name) ?? false)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }

            equipmentDatabase.UnequipGauntlet();
            DrawCharacterGraphics();
            statsBonusController.RecalculateEquipmentBonus();*/
        }
        #endregion

        #region Legwear
        public void EquipLegwear(LegwearInstance legwearToEquip)
        {/*
            LegwearInstance legwearToEquipClone = legwearToEquip.Clone();

            if (legwearToEquip.IsEmpty())
            {
                return;
            }

            UnequipLegwear();

            if (legwearToEquipClone != equipmentDatabase.legwear)
            {
                equipmentDatabase.EquipLegwear(legwearToEquipClone);
            }

            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();*/
        }

        public void UnequipLegwear()
        {/*
            foreach (Transform t in playerEquipmentRoot.GetComponentsInChildren<Transform>(true))
            {
                if (equipmentDatabase.legwear.Exists())
                {
                    if (equipmentDatabase.legwear.GetItem<Legwear>()?.graphicNameToShow == t.gameObject.name)
                    {
                        t.gameObject.SetActive(false);
                    }

                    if (equipmentDatabase.legwear.GetItem<Legwear>()?.graphicNamesToHide?.Contains(t.gameObject.name) ?? false)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }

            equipmentDatabase.UnequipLegwear();
            DrawCharacterGraphics();

            statsBonusController.RecalculateEquipmentBonus();
            */
        }
        #endregion

        #region Accessories


        public void EquipAccessory(AccessoryInstance accessoryToEquip, int slotIndex)
        {/*
            if (accessoryToEquip.IsEmpty())
            {
                return;
            }

            equipmentDatabase.EquipAccessory(accessoryToEquip, slotIndex);

            statsBonusController.RecalculateEquipmentBonus();*/
        }
        #endregion

        public void UnequipAccessory(int slotIndex)
        {/*
            equipmentDatabase.UnequipAccessory(slotIndex);

            statsBonusController.RecalculateEquipmentBonus();*/
        }

        void DrawCharacterGraphics()
        {/*

            // Cache the list of all transforms in the player's equipment root
            var allTransforms = playerEquipmentRoot.GetComponentsInChildren<Transform>(true);

            // Cache equipment items and naked parts
            var helmet = equipmentDatabase.helmet;
            var helmetItem = helmet.GetItem<Helmet>();
            var helmetGraphicsToShow = helmetItem?.graphicNameToShow;
            var helmetGraphicsToHide = helmetItem?.graphicNamesToHide;

            var armor = equipmentDatabase.armor;
            var armorItem = armor.GetItem<Armor>();
            var armorGraphicsToShow = armorItem?.graphicNameToShow;
            var armorGraphicsToHide = armorItem?.graphicNamesToHide;

            var gauntlet = equipmentDatabase.gauntlet;
            var gauntletItem = gauntlet.GetItem<Gauntlet>();
            var gauntletGraphicsToShow = gauntletItem?.graphicNameToShow;
            var gauntletGraphicsToHide = gauntletItem?.graphicNamesToHide;

            var legwear = equipmentDatabase.legwear;
            var legwearItem = legwear.GetItem<Legwear>();
            var legwearGraphicsToShow = legwearItem?.graphicNameToShow;
            var legwearGraphicsToHide = legwearItem?.graphicNamesToHide;

            foreach (Transform t in allTransforms)
            {
                /*
                HandleGraphics(t, helmet, _helmetNakedParts, helmetGraphicsToShow, helmetGraphicsToHide);
                HandleGraphics(t, armor, _armorNakedParts, armorGraphicsToShow, armorGraphicsToHide);
                HandleGraphics(t, gauntlet, _gauntletsNakedParts, gauntletGraphicsToShow, gauntletGraphicsToHide);
                HandleGraphics(t, legwear, _legwearNakedParts, legwearGraphicsToShow, legwearGraphicsToHide);
        }*/
        }

        void HandleGraphics(Transform t, ArmorBaseInstance equipment, List<string> nakedParts, string graphicToShow, IEnumerable<string> graphicsToHide)
        {/*
            if (equipment.IsEmpty())
            {
                // Show naked parts if no equipment is equipped
                if (nakedParts.Contains(t.gameObject.name))
                {
                    t.gameObject.SetActive(true);
                }
            }
            else
            {
                // Show the graphic if it matches the item's graphic to show
                if (graphicToShow == t.gameObject.name)
                {
                    t.gameObject.SetActive(true);
                }

                // Hide graphics if they match the item's graphics to hide
                foreach (string graphicNameToHide in graphicsToHide)
                {
                    if (t.gameObject.name == graphicNameToHide)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }*/
        }

        /*

        public float GetHeavyWeightThreshold()
        {

            return 0.135f + GetStrengthWeightLoadBonus();
        }

        public float GetMidWeightThreshold()
        {

            return 0.05f + GetStrengthWeightLoadBonus();
        }

        float GetStrengthWeightLoadBonus()
        {
            float bonus = playerStatsDatabase.strength + statsBonusController.strengthBonus;

            bonus *= 0.0025f;

            if (bonus > 0f)
            {
                return bonus;
            }

            return 0f;
        }

        public bool IsLightWeight()
        {
            return statsBonusController.weightPenalty <= GetMidWeightThreshold();
        }

        public bool IsMidWeight()
        {
            return statsBonusController.weightPenalty < GetHeavyWeightThreshold() && statsBonusController.weightPenalty > GetMidWeightThreshold();
        }

        public bool IsHeavyWeight()
        {
            return statsBonusController.weightPenalty >= GetHeavyWeightThreshold();
        }

        public bool IsLightWeightForGivenValue(float givenWeightPenalty)
        {
            return givenWeightPenalty <= GetMidWeightThreshold();
        }

        public bool IsMidWeightForGivenValue(float givenWeightPenalty)
        {
            return givenWeightPenalty < GetHeavyWeightThreshold() && givenWeightPenalty > GetMidWeightThreshold();
        }

        public bool IsHeavyWeightForGivenValue(float givenWeightPenalty)
        {
            return givenWeightPenalty >= GetHeavyWeightThreshold();
        }

        public float GetEquipLoad()
        {
            return statsBonusController.weightPenalty;
        }

        public string GetWeightLoadLabel(float givenWeightLoad)
        {
            if (IsLightWeightForGivenValue(givenWeightLoad))
            {
                return LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Light Load" : "Leve";
            }
            if (IsMidWeightForGivenValue(givenWeightLoad))
            {
                return LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Medium Load" : "Médio";
            }
            if (IsHeavyWeightForGivenValue(givenWeightLoad))
            {
                return LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? "Heavy Load" : "Pesado";
            }

            return "";
        }


        public Damage OnDamageEvent(CharacterBaseManager attacker, CharacterBaseManager receiver, Damage damage)
        {
            if (receiver is PlayerManager)
            {

                if (playerManager.equipmentDatabase.helmet.Exists() && (playerManager.equipmentDatabase.helmet.GetItem<Helmet>()?.canDamageEnemiesUponAttack ?? false))
                {
                    playerManager.equipmentDatabase.helmet.GetItem<Helmet>()?.AttackEnemy(attacker as CharacterManager);
                }
                if (playerManager.equipmentDatabase.armor.Exists() && (playerManager.equipmentDatabase.armor.GetItem<Armor>()?.canDamageEnemiesUponAttack ?? false))
                {
                    playerManager.equipmentDatabase.armor.GetItem<Armor>()?.AttackEnemy(attacker as CharacterManager);
                }
                if (playerManager.equipmentDatabase.gauntlet.Exists() && (playerManager.equipmentDatabase.gauntlet.GetItem<Gauntlet>()?.canDamageEnemiesUponAttack ?? false))
                {
                    playerManager.equipmentDatabase.gauntlet.GetItem<Gauntlet>()?.AttackEnemy(attacker as CharacterManager);
                }
                if (playerManager.equipmentDatabase.legwear.Exists() && (playerManager.equipmentDatabase.legwear.GetItem<Legwear>()?.canDamageEnemiesUponAttack ?? false))
                {
                    playerManager.equipmentDatabase.legwear.GetItem<Legwear>()?.AttackEnemy(attacker as CharacterManager);
                }
            }

            return damage;
        }*/
    }
}
