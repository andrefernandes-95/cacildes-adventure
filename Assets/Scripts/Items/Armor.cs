using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Armor / New Armor")]
    public class Armor : ArmorBase
    {
        [Header("Graphics")]
        public List<string> armorPieces = new();
        public string malePiece;
        public string femalePiece;
        public Material armorMaterial;

        public void OnEquip(CharacterBaseManager character)
        {
            List<string> finalList = armorPieces;

            if (character.characterBaseAppearance.IsMale())
            {
                finalList.Add(malePiece);
            }
            else
            {
                finalList.Add(femalePiece);
            }

            character.syntyCharacterModelManager.EnableArmorPiece(finalList, armorMaterial);
            character.syntyCharacterModelManager.DisableTorso();
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            List<string> finalList = armorPieces;

            finalList.Add(malePiece);
            finalList.Add(femalePiece);

            character.syntyCharacterModelManager.DisablePieces(finalList);
            character.syntyCharacterModelManager.EnableTorso();
        }
    }
}
