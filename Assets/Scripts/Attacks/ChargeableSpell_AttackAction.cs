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
            attacker.animator.SetBool(AnimatorParametersConstants.IsCharging, true);

            attacker.characterBaseMagicManager.currentChargeableSpellAttackAction = this;
        }

        // This is where we play the "Warm up" animation
        public virtual void WarmupSpell(CharacterBaseManager characterBaseManager)
        {
            GameObject instantiatedSpellWarmup = InstantiateSpellEffect(characterBaseManager, spellWarmupFX);

            characterBaseManager.characterBaseMagicManager.spellWarmupInstance = instantiatedSpellWarmup;
        }

        // This is where we play the "Throw" or "Cast" animation
        public virtual void CastSpell(CharacterBaseManager characterBaseManager)
        {
            characterBaseManager.characterBaseMagicManager.DestroyCurrentWarmupSpellEffects();

            GameObject instantiatedSpellCast = InstantiateSpellEffect(characterBaseManager, spellCastFX);
            instantiatedSpellCast.transform.parent = null;

            if (instantiatedSpellCast.TryGetComponent(out ProjectileSpellManager projectileSpellManager))
            {
                projectileSpellManager.InitializeSpell(characterBaseManager);
            }
        }

        public GameObject InstantiateSpellEffect(CharacterBaseManager characterBaseManager, GameObject fx)
        {
            GameObject instantiatedSpellWarmup = Instantiate(fx);

            WeaponSpellContactPoint weaponSpellContactPoint =
                characterBaseManager.combatManager.currentAttackingMember == AttackingMember.RIGHT_HAND
                ? characterBaseManager.characterWeapons.equippedRightWeaponInstance?.GetComponentInChildren<WeaponSpellContactPoint>()
                : characterBaseManager.characterWeapons.equippedLeftWeaponInstance?.GetComponentInChildren<WeaponSpellContactPoint>();

            if (weaponSpellContactPoint != null)
            {
                instantiatedSpellWarmup.transform.parent = weaponSpellContactPoint.transform;
                instantiatedSpellWarmup.transform.localPosition = Vector3.zero;
                instantiatedSpellWarmup.transform.localRotation = Quaternion.identity;
            }

            return instantiatedSpellWarmup;
        }

        public virtual bool CanIUseThisSpell(CharacterBaseManager characterBaseManager)
        {

            return true;
        }
    }
}
