using System.Linq;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Chargeable Spell Attack Action", menuName = "Combat / New Chargeable Spell Attack Action", order = 0)]
    public class ChargeableSpell_AttackAction : AttackAction
    {
        [Header("Chargeable Spell FX")]
        [SerializeField] protected GameObject spellWarmupFX;
        [SerializeField] protected GameObject spellCastFX;

        [Header("Sounds")]
        [SerializeField] Soundpack spellWarmupSounds;
        [SerializeField] Soundpack spellCastSounds;


        public override void Execute(CharacterBaseManager attacker)
        {
            if (attackAnimations.Count <= 0)
            {
                return;
            }

            string attackName = attackAnimations.ElementAt(0).Key.name;

            // Allow rotation if we are in the middle of combos
            attacker.EnableCanRotate();

            attacker.animator.speed = animationSpeed;
            attacker.PlayCrossFadeBusyAnimationWithRootMotion(attackName, .2f);
        }

        // This is where we play the "Warm up" animation
        public virtual void WarmupSpell(CharacterBaseManager characterBaseManager)
        {

        }

        // This is where we play the "Throw" or "Cast" animation
        public virtual void CastSpell(CharacterBaseManager characterBaseManager)
        {

        }

        protected virtual void InstantiateWarmupVisualEffects(CharacterBaseManager characterBaseManager)
        {

        }

        protected virtual void InstantiateCastVisualEffects(CharacterBaseManager characterBaseManager)
        {

        }

        public virtual bool CanIUseThisSpell(CharacterBaseManager characterBaseManager)
        {

            return true;
        }
    }
}
