using System.Collections;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Armor / New Armor")]
    public class Armor : ArmorBase
    {
        [Header("Graphics")]
        public string[] armorPieces;

        public void OnEquip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in armorPieces)
            {
                character.syntyCharacterModelManager.ShowPiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleTorso(false);
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in armorPieces)
            {
                character.syntyCharacterModelManager.HidePiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleTorso(true);
        }
    }
}
