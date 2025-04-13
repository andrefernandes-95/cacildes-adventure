using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "Items / Armor / New Helmet")]
    public class Helmet : ArmorBase
    {
        [Header("Graphics")]
        public List<string> helmetPieces = new();
        public Material helmetMaterial;
        public bool hideHair = false;
        public bool hideFace = false;
        public bool hideEyebrows = false;
        public bool hideBeard = false;

        public void OnEquip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.EnableArmorPiece(helmetPieces, helmetMaterial);

            if (hideHair)
            {
                character.syntyCharacterModelManager.DisableHair();
            }
            if (hideFace)
            {
                character.syntyCharacterModelManager.DisableFace();
            }
            if (hideEyebrows)
            {
                character.syntyCharacterModelManager.DisableEyebrows();
            }

            if (hideBeard)
            {
                character.syntyCharacterModelManager.DisableBeard();
            }
        }

        public void OnUnequip(CharacterBaseManager character)
        {
            character.syntyCharacterModelManager.DisablePieces(helmetPieces);

            if (hideHair)
            {
                character.syntyCharacterModelManager.EnableHair();
            }
            if (hideFace)
            {
                character.syntyCharacterModelManager.EnableFace();
            }
            if (hideEyebrows)
            {
                character.syntyCharacterModelManager.EnableEyebrows();
            }

            if (hideBeard)
            {
                character.syntyCharacterModelManager.EnableBeard();
            }
        }
    }

}
