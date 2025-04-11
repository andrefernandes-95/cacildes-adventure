namespace AF
{
    using AF.Health;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Character Effects / Instant Effects / Take Blocked Damage Effect")]
    public class TakeBlockedDamageEffect : InstantCharacterEffect
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

        [Header("Shield Used To Block Damage")]
        [HideInInspector] public Shield shield;

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

            characterBeingDamaged.damageReceiver.angleFromLastHit = angleHitFrom;

            // Check for build ups (Poison, Bleed)

            // Play sound effect
            PlayDamageSFX();

            // Play VFX
            PlayDamageVFX();

            // Save poise damage
            characterBeingDamaged.characterBlockController.receivedPoiseDamage = damage.poiseDamage;

            // If character is AI, check for new target if character causing damage is present
            characterBeingDamaged.characterBaseStateMachine.ChangeToTakeDamageState();
        }

        private void CalculateDamage()
        {
            // Calculate final damage
            Damage negatedDamage = damage.Copy();

            negatedDamage.physical = (int)(negatedDamage.physical * shield.physicalAbsorption);
            negatedDamage.fire = (int)(negatedDamage.fire * shield.fireAbsorption);
            negatedDamage.frost = (int)(negatedDamage.frost * shield.frostAbsorption);
            negatedDamage.water = (int)(negatedDamage.water * shield.waterAbsorption);
            negatedDamage.lightning = (int)(negatedDamage.lightning * shield.lightiningAbsorption);
            negatedDamage.darkness = (int)(negatedDamage.darkness * shield.darknessAbsorption);
            negatedDamage.magic = (int)(negatedDamage.magic * shield.magicAbsorption);

            finalDamageDealt = negatedDamage.GetTotalDamage();

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            // Calculate poise damage
            receiver.health.TakeDamage(finalDamageDealt);

            // Show Damage Popup
            ShowDamagePopups(negatedDamage);
        }

        private void PlayDamageVFX()
        {
            // If we have fire damage, play fire damage sfx and vfx

            // Lightning...

            if (shield.shieldBlockVfx != null)
            {
                Instantiate(shield.shieldBlockVfx, contactPoint, Quaternion.identity);
            }


            //            receiver.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX()
        {
            if (shield.blockingSoundpack != null)
            {
                shield.blockingSoundpack.Play(receiver);
            }

            receiver.characterSoundManager.PlayDamageGrunt();
        }

        private void ShowDamagePopups(Damage damage)
        {
            if (damage.physical > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowPhysicalDamage(damage.physical);
            }
            if (damage.fire > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowFireDamage(damage.fire);
            }
            if (damage.frost > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowFrostDamage(damage.frost);
            }
            if (damage.water > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowWaterDamage(damage.water);
            }
            if (damage.lightning > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowLightningDamage(damage.lightning);
            }
            if (damage.darkness > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowDarknessDamage(damage.darkness);
            }
            if (damage.magic > 0)
            {
                receiver.uI_CharacterDamagePopupManager.ShowMagicDamage(damage.magic);
            }
        }
    }
}
