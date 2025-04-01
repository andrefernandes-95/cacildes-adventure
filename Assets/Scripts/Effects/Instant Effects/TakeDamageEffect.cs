namespace AF
{
    using AF.Health;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Character Effects / Instant Effects / Take Damage Effect")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        [HideInInspector] public CharacterBaseManager attacker;
        [HideInInspector] public CharacterBaseManager receiver;

        [Header("Damage")]
        [HideInInspector] public Damage damage;

        [Header("Final Damage Dealt")]
        private int finalDamageDealt = 0; // The damage the character takes after ALL calculations have been made

        [Header("Animation")]
        [HideInInspector] public string damageAnimation;

        [Header("Poise")]
        [HideInInspector] public float poiseDamage = 0;
        [HideInInspector] public bool poiseIsBroken = false; // If a character's poise is broken, they will be "Stunned" and play a damage animation

        [Header("Sound FX")]
        [HideInInspector] public bool willPlayDamageSFX = true;
        [HideInInspector] public AudioClip elementalSoundFX; // Used on top of regular sound effect if there is elemental damage present

        [Header("Direction Damage")]
        [HideInInspector] public float angleHitFrom; // Used to determine what damage animation to play (Move backwards, to the left, to the right...)
        [HideInInspector] public Vector3 contactPoint; // Used to determine where the blood fx instantiates

        public override void ProcessEffect(CharacterBaseManager characterBeingDamaged)
        {
            base.ProcessEffect(characterBeingDamaged);

            if (characterBeingDamaged.health.IsDead())
            {
                return;
            }

            // Check if character is invulnerable

            // Calculate damage
            CalculateDamage();

            // Play damage animation
            PlayDirectionalBasedDamageAnimation();

            // Check for build ups (Poison, Bleed)

            // Play sound effect
            PlayDamageSFX();

            // Play VFX
            PlayDamageVFX();

            // If character is AI, check for new target if character causing damage is present

        }

        private void CalculateDamage()
        {
            // Calculate final damage
            finalDamageDealt = damage.GetTotalDamage();

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            // Calculate poise damage
        }

        private void PlayDamageVFX()
        {
            // If we have fire damage, play fire damage sfx and vfx

            // Lightning...

            receiver.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX()
        {
            receiver.characterSoundManager.PlayPhysicalDamage();

            receiver.characterSoundManager.PlayDamageGrunt();
        }

        private void PlayDirectionalBasedDamageAnimation()
        {
            if (receiver.health.IsDead())
            {
                return;
            }

            if (!receiver.damageReceiver.useDirectionalDamageAnimations)
            {
                return;
            }

            // Calculate if poise is broken
            poiseIsBroken = true;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // Play Front Animation
                damageAnimation = receiver.damageReceiver.hitFromFront.name;
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // Play Front Animation
                damageAnimation = receiver.damageReceiver.hitFromFront.name;
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // Play Back Animation
                damageAnimation = receiver.damageReceiver.hitFromBack.name;
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // Play Left Animation
                damageAnimation = receiver.damageReceiver.hitFromLeft.name;
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // Play Right Animation
                damageAnimation = receiver.damageReceiver.hitFromRight.name;
            }

            if (poiseIsBroken && !string.IsNullOrEmpty(damageAnimation))
            {
                receiver.PlayCrossFadeBusyAnimationWithRootMotion(damageAnimation, 0.2f);
            }
        }
    }
}
