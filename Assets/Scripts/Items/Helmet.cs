using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Helmet")]
    public class Helmet : ArmorBase
    {

        [Header("Graphics")]
        public string[] helmetPieces;
        public bool hideHair = false;
        public bool hideFace = false;
        public bool hideEyebrows = false;
        public bool hideBeard = false;

        public void OnEquip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in helmetPieces)
            {
                character.syntyCharacterModelManager.ShowPiece(gameObjectName);
            }

            if (hideHair)
            {
                character.syntyCharacterModelManager.ToggleHair(false);
            }
            if (hideFace)
            {
                character.syntyCharacterModelManager.ToggleFace(false);
            }
            if (hideEyebrows)
            {
                character.syntyCharacterModelManager.ToggleEyebrows(false);
            }

            if (hideBeard)
            {
                character.syntyCharacterModelManager.ToggleBeard(false);
            }
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            foreach (string gameObjectName in helmetPieces)
            {
                character.syntyCharacterModelManager.HidePiece(gameObjectName);
            }

            character.syntyCharacterModelManager.ToggleHair(true);
            character.syntyCharacterModelManager.ToggleFace(true);
            character.syntyCharacterModelManager.ToggleEyebrows(true);
            character.syntyCharacterModelManager.ToggleBeard(true);
        }
    }

}
