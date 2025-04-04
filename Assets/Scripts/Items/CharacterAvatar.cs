using AF.Health;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Item / New Character Avatar")]
    public class CharacterAvatar : Item
    {
        [Header("Damage Negation")]
        public Damage damageNegation;


        [Header("Graphics Settings")]
        public GameObject characterPrefab;

        [Header(" Weapon Settings")]
        public string rightWeaponBoneName = "Hand_R";
        public Vector3 rightHandWeaponPivot;
        public Vector3 rightHandWeaponRotation;
        [Header(" Weapon Settings")]
        public string leftWeaponBoneName = "Hand_L";
        public Vector3 leftHandWeaponPivot;
        public Vector3 leftHandWeaponRotation;


    }
}
