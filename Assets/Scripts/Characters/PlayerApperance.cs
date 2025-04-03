using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class PlayerAppearanceManager : CharacterBaseAppearance
    {
        [Header("Options")]
        public bool isMale = true;

        [Header("Common Parts")]
        [SerializeField] List<string> hairs = new();
        [SerializeField] List<string> eyebrows = new();
        [SerializeField] List<string> beards = new();

        [Header("Male Parts")]
        [SerializeField] List<string> maleHead = new();
        [SerializeField] List<string> maleTorso = new();
        [SerializeField] List<string> maleHands = new();
        [SerializeField] List<string> maleLegs = new();

        [Header("Female Parts")]
        [SerializeField] List<string> femaleHead = new();
        [SerializeField] List<string> femaleTorso = new();
        [SerializeField] List<string> femaleHands = new();
        [SerializeField] List<string> femaleLegs = new();

        public override List<string> GetHands()
        {
            if (isMale)
            {
                return maleHands;
            }

            return femaleHands;
        }

        public override List<string> GetBeard()
        {
            return beards;
        }

        public override List<string> GetEyebrows()
        {
            return eyebrows;
        }

        public override List<string> GetHairs()
        {
            return hairs;
        }

        public override List<string> GetFace()
        {
            if (isMale)
            {
                return maleHead;
            }

            return femaleHead;
        }

        public override List<string> GetLegs()
        {
            if (isMale)
            {
                return maleLegs;
            }

            return femaleLegs;
        }

        public override List<string> GetTorso()
        {
            if (isMale)
            {
                return maleTorso;
            }

            return femaleTorso;
        }

        public override bool IsMale()
        {
            return isMale;
        }

    }
}