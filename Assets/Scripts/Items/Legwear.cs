using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Legwear")]
    public class Legwear : ArmorBase
    {

        [Header("Graphics")]
        public string[] bootPieces;

        public void OnEquip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in bootPieces)
            {
                character.syntyCharacterModelManager.ShowPiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleLegs(false);
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in bootPieces)
            {
                character.syntyCharacterModelManager.HidePiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleLegs(true);
        }
    }

}
