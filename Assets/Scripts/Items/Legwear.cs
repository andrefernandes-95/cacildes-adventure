using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Legwear")]
    public class Legwear : ArmorBase
    {

        [Header("Graphics")]
        public List<string> bootPieces = new();
        public Material bootsMaterial;

        public void OnEquip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.EnableArmorPiece(bootPieces, bootsMaterial);
            character.syntyCharacterModelManager.DisableLegs();
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.DisablePieces(bootPieces);
            character.syntyCharacterModelManager.EnableLegs();
        }
    }
}
