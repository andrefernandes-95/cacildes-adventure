using System.Collections;
using UnityEngine;

namespace AF
{
    public class CharacterPoise : CharacterAbstractPoise
    {
        public int maxPoiseHits = 3;

        public bool hasHyperArmor = false;

        bool ignorePoiseDamage = false;
        readonly float recoverPoiseCooldown = 3f;

        Coroutine ResetIgnorePoiseDamageCoroutine;

        public override void ResetStates()
        {
            hasHyperArmor = false;
        }

        public override bool CanCallPoiseDamagedEvent()
        {
            if (hasHyperArmor)
            {
                return false;
            }

            if (ignorePoiseDamage)
            {
                return false;
            }

            return true;
        }

        public override int GetMaxPoiseHits()
        {
            return maxPoiseHits;
        }

        public override bool TakePoiseDamage(int poiseDamage)
        {
            bool result = base.TakePoiseDamage(poiseDamage);

            if (result)
            {
                ignorePoiseDamage = true;

                if (ResetIgnorePoiseDamageCoroutine != null)
                {
                    StopCoroutine(ResetIgnorePoiseDamageCoroutine);
                }

                ResetIgnorePoiseDamageCoroutine = StartCoroutine(ResetIgnorePoiseDamage_Coroutine());
            }

            return result;
        }

        IEnumerator ResetIgnorePoiseDamage_Coroutine()
        {
            yield return new WaitForSeconds(recoverPoiseCooldown);
            ignorePoiseDamage = false;
        }
    }
}
