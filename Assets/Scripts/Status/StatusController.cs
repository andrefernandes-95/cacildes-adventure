using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

namespace AF.StatusEffects
{
    public class StatusController : MonoBehaviour
    {
        [Header("Character")]
        public CharacterBaseManager characterBaseManager;

        // TODO: Refactor to be in database when we have cross scene teleport to test

        [Header("Resistances")]


        // These two make the resistance bar to the given status effect larger, taking longer to be inflicted
        [SerializedDictionary("Status Resistances", "Duration (seconds)")]
        public SerializedDictionary<StatusEffect, float> statusEffectResistances = new();

        public Dictionary<StatusEffect, float> statusEffectResistanceBonuses = new();

        // This one removes the amount of the given status effect, delaying or cancelling it somewhat
        public Dictionary<StatusEffect, float> statusEffectCancellationBonuses = new();

        public List<AppliedStatusEffect> appliedStatusEffects = new();

        // END TODO

        [Header("Effect Instances")]
        [SerializedDictionary("Status Effect", "World Instance")]
        public SerializedDictionary<StatusEffect, StatusEffectInstance> statusEffectInstances;

        [Header("UI")]
        public InterfaceReference<IStatusEffectUI, MonoBehaviour> statusEffectUI;

        [Header("Unity Events")]
        public UnityEvent onAwake;

        public GameSession gameSession;

        [Header("Optional Components")]
        public UIDocumentStatusEffectApplied uIDocumentStatusEffectApplied;

        private void Awake()
        {
            onAwake?.Invoke();

            if (gameSession.currentGameIteration > 0)
            {
                ScaleResistancesToNewGamePlus();
            }
        }

        void ScaleResistancesToNewGamePlus()
        {
            // Create a list of keys to avoid modifying the dictionary while iterating
            List<StatusEffect> keys = new List<StatusEffect>(statusEffectResistances.Keys);

            foreach (var key in keys)
            {
                statusEffectResistances[key] = Utils.ScaleWithCurrentNewGameIteration(
                    (int)statusEffectResistances[key], gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            }
        }

        private void Update()
        {
            if (appliedStatusEffects.Count <= 0)
            {
                return;
            }

            HandleStatusEffects();
        }


        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="statusEffect"></param>
        public void InflictStatusEffect(StatusEffect statusEffect)
        {
            InflictStatusEffect(statusEffect, GetMaximumStatusResistanceBeforeSufferingStatusEffect(statusEffect, true), true);
        }

        public void InflictStatusEffect(StatusEffect statusEffect, float amount, bool hasReachedFullAmount)
        {
            var existingStatusEffectIndex = appliedStatusEffects.FindIndex(x => x.statusEffect == statusEffect);

            // If inflicted amount is greater than the character's maximum resistance, make him suffer the status effect
            float maximumResistanceToStatusEffect = GetMaximumStatusResistanceBeforeSufferingStatusEffect(statusEffect, true);
            if (amount >= maximumResistanceToStatusEffect)
            {
                amount = maximumResistanceToStatusEffect;
                hasReachedFullAmount = true;
            }

            amount = GetAmountAfterCalculatingCancellationBonuses(statusEffect, amount);
            amount = GetFinalAmountBasedOnCancellationRate(statusEffect, amount);

            if (existingStatusEffectIndex == -1)
            {
                AddStatusEffect(statusEffect, amount, hasReachedFullAmount, maximumResistanceToStatusEffect);
            }
            // Don't inflict status effect on an already fully-inflicted status effect
            else if (!appliedStatusEffects[existingStatusEffectIndex].hasReachedTotalAmount)
            {
                appliedStatusEffects[existingStatusEffectIndex].currentAmount += amount;
                HandleReachedAmountCalculation(statusEffect, existingStatusEffectIndex);
            }
        }

        void HandleReachedAmountCalculation(StatusEffect statusEffect, int appliedStatusEffectIndex)
        {
            float maxAmountBeforeSuffering = GetMaximumStatusResistanceBeforeSufferingStatusEffect(statusEffect, true);
            if (appliedStatusEffects[appliedStatusEffectIndex].currentAmount < maxAmountBeforeSuffering)
            {
                return;
            }

            appliedStatusEffects[appliedStatusEffectIndex].currentAmount = GetMaximumStatusResistanceBeforeSufferingStatusEffect(statusEffect, false);
            appliedStatusEffects[appliedStatusEffectIndex].hasReachedTotalAmount = true;

            StatusEffectInstance statusEffectInstance = GetStatusEffectInstance(statusEffect);
            if (statusEffectInstance != null)
            {
                statusEffectInstance.onApplied_Start?.Invoke();

                if (uIDocumentStatusEffectApplied != null)
                {
                    uIDocumentStatusEffectApplied.Display(statusEffect);
                }
            }
        }

        StatusEffectInstance GetStatusEffectInstance(StatusEffect statusEffect)
        {
            if (statusEffectInstances.ContainsKey(statusEffect))
            {
                return statusEffectInstances[statusEffect];
            }

            return null;
        }

        public float GetMaximumStatusResistanceBeforeSufferingStatusEffect(StatusEffect statusEffect, bool useResistanceBonuses)
        {
            float resistance = 0;

            if (statusEffectResistances.ContainsKey(statusEffect))
            {
                resistance += statusEffectResistances[statusEffect];
            }

            if (useResistanceBonuses && statusEffectResistanceBonuses.ContainsKey(statusEffect))
            {
                resistance += statusEffectResistanceBonuses[statusEffect];
            }

            return resistance;
        }


        void AddStatusEffect(StatusEffect statusEffect, float amount, bool hasReachedFullAmount, float maximumResistanceToStatusEffect)
        {
            if (appliedStatusEffects.Exists(x => x.statusEffect == statusEffect))
            {
                return;
            }

            AppliedStatusEffect appliedStatus = new()
            {
                statusEffect = statusEffect,
                currentAmount = amount,
                hasReachedTotalAmount = hasReachedFullAmount
            };

            appliedStatusEffects.Add(appliedStatus);

            if (statusEffectUI != null && statusEffectUI.Value != null)
            {
                statusEffectUI?.Value?.AddEntry(appliedStatus, maximumResistanceToStatusEffect);
            }

            StatusEffectInstance statusEffectInstance = GetStatusEffectInstance(statusEffect);
            if (statusEffectInstance != null && appliedStatus.hasReachedTotalAmount)
            {
                statusEffectInstance.onApplied_Start?.Invoke();
            }
        }

        private void HandleStatusEffects()
        {
            List<AppliedStatusEffect> statusToDelete = new();

            foreach (var entry in appliedStatusEffects.ToList())
            {

                entry.currentAmount -= (entry.hasReachedTotalAmount
                    ? entry.statusEffect.decreaseRateWithDamage
                    : entry.statusEffect.decreaseRateWithoutDamage) * Time.deltaTime;

                if (statusEffectUI != null && statusEffectUI.Value != null)
                {
                    statusEffectUI.Value.UpdateEntry(entry, GetMaximumStatusResistanceBeforeSufferingStatusEffect(entry.statusEffect, true));
                }

                if (ShouldRemove(entry))
                {
                    statusToDelete.Add(entry);
                }

                StatusEffectInstance statusEffectInstance = GetStatusEffectInstance(entry.statusEffect);
                if (entry.hasReachedTotalAmount && statusEffectInstance != null)
                {
                    statusEffectInstance.onApplied_Update?.Invoke();
                }
            }

            foreach (var status in statusToDelete)
            {
                RemoveAppliedStatus(status);
            }
        }

        bool ShouldRemove(AppliedStatusEffect appliedStatusEffect)
        {
            if (characterBaseManager?.health?.GetCurrentHealth() <= 0)
            {
                return true;
            }

            if (appliedStatusEffect.hasReachedTotalAmount && appliedStatusEffect.statusEffect.isAppliedImmediately)
            {
                return true;
            }

            if (appliedStatusEffect.currentAmount > 0)
            {
                return false;
            }

            return true;
        }

        public void RemoveAppliedStatus(AppliedStatusEffect appliedStatus)
        {
            if (appliedStatus == null || appliedStatusEffects.Contains(appliedStatus) == false)
            {
                return;
            }

            StatusEffectInstance statusEffectInstance = GetStatusEffectInstance(appliedStatus.statusEffect);
            if (statusEffectInstance != null)
            {
                statusEffectInstance.onApplied_End?.Invoke();
            }

            if (statusEffectUI != null && statusEffectUI.Value != null)
            {
                statusEffectUI.Value.RemoveEntry(appliedStatus);
            }

            appliedStatusEffects.Remove(appliedStatus);
        }

        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            if (appliedStatusEffects != null && appliedStatusEffects.Count > 0)
            {
                var existingStatusEffectIndex = appliedStatusEffects.FindIndex(x => x.statusEffect == statusEffect);
                if (existingStatusEffectIndex != -1)
                {
                    RemoveAppliedStatus(appliedStatusEffects[existingStatusEffectIndex]);
                }
            }
        }

        public void RemoveAllStatuses()
        {
            AppliedStatusEffect[] appliedStatusEffectsClone = appliedStatusEffects.ToArray();
            foreach (var appliedStatusEffect in appliedStatusEffectsClone)
            {
                RemoveAppliedStatus(appliedStatusEffect);
            }
        }

        float GetAmountAfterCalculatingCancellationBonuses(StatusEffect statusEffect, float amount)
        {
            if (statusEffectCancellationBonuses != null && statusEffectCancellationBonuses.Count > 0 && statusEffectCancellationBonuses.ContainsKey(statusEffect))
            {
                amount = Mathf.Clamp(amount - statusEffectCancellationBonuses[statusEffect], 0, amount);
            }

            return amount;
        }

        public void Add_05_CancellationBonuses(StatusEffect statusEffect)
        {
            AddCancellationBonuses(statusEffect, .5f);
        }

        public void Add_1_CancellationBonuses(StatusEffect statusEffect)
        {
            AddCancellationBonuses(statusEffect, 1f);
        }

        void AddCancellationBonuses(StatusEffect statusEffect, float amount)
        {

            if (statusEffectCancellationBonuses.ContainsKey(statusEffect))
            {
                statusEffectCancellationBonuses[statusEffect] = amount;
            }
            else
            {
                statusEffectCancellationBonuses.Add(statusEffect, amount);
            }
        }

        public void RemoveCancellationBonuses(StatusEffect statusEffect)
        {
            if (statusEffectCancellationBonuses != null && statusEffectCancellationBonuses.Count > 0 && statusEffectCancellationBonuses.ContainsKey(statusEffect))
            {
                statusEffectCancellationBonuses.Remove(statusEffect);
            }
        }

        public virtual float GetFinalAmountBasedOnCancellationRate(StatusEffect statusEffect, float amount)
        {

            return amount;
        }

    }
}
