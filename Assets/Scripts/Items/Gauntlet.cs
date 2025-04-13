using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Gauntlet")]
    public class Gauntlet : ArmorBase
    {
        [Header("Graphics")]
        public List<string> gauntletPieces = new();
        public Material gauntletsMaterial;

        public void OnEquip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.EnableArmorPiece(gauntletPieces, gauntletsMaterial);
            character.syntyCharacterModelManager.DisableHands();
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.DisablePieces(gauntletPieces);
            character.syntyCharacterModelManager.EnableHands();
        }
    }
}
