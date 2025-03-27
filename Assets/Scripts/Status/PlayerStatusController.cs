using System.Linq;
using AF.Stats;
using UnityEngine;

namespace AF.StatusEffects
{

    public class PlayerStatusController : StatusController
    {

        public TeleportManager teleportManager;
        public StatusDatabase statusDatabase;
        public StatsBonusController statsBonusController;

        private void Start()
        {
            foreach (var appliedStatus in statusDatabase.appliedStatus)
            {
                InflictStatusEffect(appliedStatus.statusEffect, appliedStatus.currentAmount, appliedStatus.hasReachedTotalAmount);
            }

            statusDatabase.appliedStatus.Clear();
        }

        private void OnEnable()
        {
            teleportManager.onChangingScene += SaveAppliedStatuses;
        }

        private void OnDisable()
        {
            teleportManager.onChangingScene -= SaveAppliedStatuses;
        }

        private void SaveAppliedStatuses()
        {
            statusDatabase.appliedStatus.Clear();
            foreach (var appliedStatus in this.appliedStatusEffects)
            {
                statusDatabase.appliedStatus.Add(appliedStatus);
            }
        }

        public override float GetFinalAmountBasedOnCancellationRate(StatusEffect statusEffect, float amount)
        {
            if (statsBonusController.statusEffectCancellationRates.ContainsKey(statusEffect))
            {
                return Mathf.Clamp(amount - statsBonusController.statusEffectCancellationRates[statusEffect], 0, Mathf.Infinity);
            }

            return amount;
        }

        public int GetResistanceForStatusEffect(StatusEffect statusEffect)
        {
            if (statusEffectResistances.ContainsKey(statusEffect))
            {
                int bonusMatch = (int)(statusEffectResistanceBonuses.ContainsKey(statusEffect) ? statusEffectResistanceBonuses[statusEffect] : 0);

                return (int)statusEffectResistances[statusEffect] + bonusMatch;
            }

            return 0;
        }
    }
}
