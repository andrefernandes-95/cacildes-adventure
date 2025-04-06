using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Gauntlet")]
    public class Gauntlet : ArmorBase
    {
        [Header("Graphics")]
        public string[] gauntletPieces;

        public void OnEquip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in gauntletPieces)
            {
                character.syntyCharacterModelManager.ShowPiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleHands(false);
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in gauntletPieces)
            {
                character.syntyCharacterModelManager.HidePiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleHands(true);
        }
    }

}
