using System.Collections;
using AF.Events;
using AF.Health;
using AF.Stats;
using TigerForge;
using UnityEngine;

namespace AF
{
    public class CharacterBaseMagicManager : MonoBehaviour
    {

        [Header("Charging Spell Modifier")]
        public float currentChargingAttackMultiplier = 0f;

        [Header("Components")]
        public CharacterBaseManager character;

        [Header("Current Damage")]
        public Damage currentSpellDamage;


        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;
        public EquipmentDatabase equipmentDatabase;

        [Header("Components")]
        public StatsBonusController playerStatsBonusController;

        public StarterAssetsInputs inputs;

        public SyntyCharacterModelManager equipmentGraphicsHandler;

        public PlayerManager playerManager;

        [Header("Regeneration Settings")]
        public float MANA_REGENERATION_RATE = 20f;

        private void Start()
        {
            // Initialize Mana
            if (playerStatsDatabase.currentMana == -1)
            {
                playerStatsDatabase.currentMana = GetMaxMana();
            }


            character.characterBaseEquipment.onSwitchingRightWeapon.AddListener(RecalculateDamages);
            character.characterBaseEquipment.onSwitchingLeftWeapon.AddListener(RecalculateDamages);
            character.characterBaseEquipment.onSwitchingSpell.AddListener(RecalculateDamages);


            character.characterBaseEquipment.onSwitchingSpell.AddListener(UpdateSpellAnimations);
        }

        void UpdateSpellAnimations()
        {
            SpellInstance currentSpell = character.characterBaseEquipment.GetSpellInstance();
            if (currentSpell.Exists())
            {
                Spell spell = currentSpell.GetItem<Spell>();
                character.UpdateAttackAnimations(spell.rightBumperActions.ToArray());
                character.UpdateAttackAnimations(spell.leftBumperActions.ToArray());
                character.UpdateAttackAnimations(spell.rightTriggerActions.ToArray());
            }
        }

        private void Update()
        {
            if (playerStatsBonusController.shouldRegenerateMana && playerStatsDatabase.currentMana < playerStatsDatabase.maxMana)
            {
                HandleManaRegen();
            }
        }

        // Call this every time equipment changes or stats are updated
        public void RecalculateDamages()
        {
            CalculateCurrentSpellDamage();
        }




        public void CalculateCurrentSpellDamage()
        {
            SpellInstance spellInstance = character.characterBaseEquipment.GetSpellInstance();

            Damage initialDamage = new();

            if (spellInstance.Exists())
            {
                initialDamage = CalculateDamageOutput(spellInstance);
            }

            this.currentSpellDamage = initialDamage;
        }

        public Damage CalculateDamageOutput(SpellInstance spellInstance)
        {
            if (spellInstance == null || spellInstance.IsEmpty())
            {
                return null;
            }

            Spell spell = spellInstance.GetItem<Spell>();

            Damage clonedDamage = spell.damage;

            // Scale damage
            ScaleDamage(clonedDamage);

            // Apply attributes
            ApplyModifiers(clonedDamage);

            return clonedDamage;
        }

        void ApplyModifiers(Damage damage)
        {

        }

        void ApplyModifier(Damage damage, float modifier)
        {
            damage.physical = (int)(damage.physical * modifier);
            damage.fire = (int)(damage.fire * modifier);
            damage.frost = (int)(damage.frost * modifier);
            damage.magic = (int)(damage.magic * modifier);
            damage.lightning = (int)(damage.lightning * modifier);
            damage.darkness = (int)(damage.darkness * modifier);
            damage.water = (int)(damage.water * modifier);
        }

        void AddToDamage(Damage damage, int value)
        {
            if (damage.physical > 0)
            {
                damage.physical += value;
            }
            if (damage.fire > 0)
            {
                damage.fire += value;
            }
            if (damage.frost > 0)
            {
                damage.frost += value;
            }
            if (damage.magic > 0)
            {
                damage.magic += value;
            }
            if (damage.lightning > 0)
            {
                damage.lightning += value;
            }
            if (damage.darkness > 0)
            {
                damage.darkness += value;
            }
            if (damage.water > 0)
            {
                damage.water += value;
            }
        }


        void ScaleDamage(Damage incomingDamage)
        {
            incomingDamage.physical += AttackUtils.GetScalingBonus(character.characterBaseStats.GetStrength(), incomingDamage.strengthScaling);
            incomingDamage.physical += AttackUtils.GetScalingBonus(character.characterBaseStats.GetDexterity(), incomingDamage.dexterityScaling);

            incomingDamage.fire += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.frost += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.magic += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.water += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);

            int characterReputation = character.characterBaseStats.GetReputation();

            if (characterReputation >= 0)
            {
                incomingDamage.lightning += AttackUtils.GetScalingBonus(character.characterBaseStats.GetReputation(), incomingDamage.faithScaling);
            }
            else
            {
                incomingDamage.darkness += AttackUtils.GetScalingBonus(character.characterBaseStats.GetReputation(), incomingDamage.hexScaling);
            }
        }



        void HandleManaRegen()
        {
            var finalRegenerationRate = MANA_REGENERATION_RATE + playerStatsBonusController.staminaRegenerationBonus;

            playerStatsDatabase.currentMana = Mathf.Clamp(playerStatsDatabase.currentMana + finalRegenerationRate * Time.deltaTime, 0f, GetMaxMana());
        }

        public int GetMaxMana()
        {
            return Formulas.CalculateStatForLevel(
                playerStatsDatabase.maxMana + playerStatsBonusController.magicBonus,
                playerManager.characterBaseStats.GetIntelligence(),
                playerStatsDatabase.levelMultiplierForMana);
        }

        public void DecreaseMana(float amount)
        {
            playerStatsDatabase.currentMana = Mathf.Clamp(playerStatsDatabase.currentMana - amount, 0, GetMaxMana());
        }

        public bool HasEnoughManaForSpell(Spell spell)
        {
            if (spell == null)
            {
                return false;
            }

            return HasEnoughManaForAction((int)spell.manaCostPerCast);
        }

        public bool HasEnoughManaForAction(int actionCost)
        {
            bool canPerform = playerStatsDatabase.currentMana - actionCost > 0;
            if (!canPerform)
            {
                playerManager.uIDocumentPlayerHUDV2.DisplayInsufficientMana();
            }

            return canPerform;
        }

        public void RestoreFullMana()
        {
            playerStatsDatabase.currentMana = GetMaxMana();
        }

        public void RestoreManaPercentage(float amount)
        {
            var percentage = this.GetMaxMana() * amount / 100;
            var nextValue = Mathf.Clamp(playerStatsDatabase.currentMana + percentage, 0, this.GetMaxMana());

            playerStatsDatabase.currentMana = nextValue;
        }

        public void RestoreManaPoints(float amount)
        {
            var nextValue = Mathf.Clamp(playerStatsDatabase.currentMana + amount, 0, this.GetMaxMana());

            playerStatsDatabase.currentMana = nextValue;
        }

        public float GetManaPointsForGivenIntelligence(int intelligence)
        {
            return playerStatsDatabase.maxMana + (int)Mathf.Ceil(intelligence * playerStatsDatabase.levelMultiplierForMana);
        }


        public float GetCurrentManaPercentage()
        {
            return playerStatsDatabase.currentMana * 100 / GetMaxMana();
        }
    }
}
