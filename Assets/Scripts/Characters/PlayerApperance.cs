using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class PlayerAppearanceManager : CharacterBaseAppearance
    {
        public PlayerCustomization playerCustomization;

        public override List<string> GetHands()
        {
            if (playerCustomization.isMale)
            {
                return playerCustomization.maleHands;
            }

            return playerCustomization.femaleHands;
        }

        public override List<string> GetBeard()
        {
            return playerCustomization.beards;
        }

        public override List<string> GetEyebrows()
        {
            return playerCustomization.eyebrows;
        }

        public override List<string> GetHairs()
        {
            return playerCustomization.hairs;
        }

        public override List<string> GetFace()
        {
            if (playerCustomization.isMale)
            {
                return playerCustomization.maleHead;
            }

            return playerCustomization.femaleHead;
        }

        public override List<string> GetLegs()
        {
            if (playerCustomization.isMale)
            {
                return playerCustomization.maleLegs;
            }

            return playerCustomization.femaleLegs;
        }

        public override List<string> GetTorso()
        {
            if (playerCustomization.isMale)
            {
                return playerCustomization.maleTorso;
            }

            return playerCustomization.femaleTorso;
        }

        public override bool IsMale()
        {
            return playerCustomization.isMale;
        }

        public override Color GetEyeColor()
        {
            return playerCustomization.eyeColor;
        }

        public override Color GetSkinColor()
        {
            return playerCustomization.skinColor;
        }

        public override Color GetHairColor()
        {
            return playerCustomization.hairColor;
        }

        public override Color GetScarColor()
        {
            return playerCustomization.scarColor;
        }
    }
}
