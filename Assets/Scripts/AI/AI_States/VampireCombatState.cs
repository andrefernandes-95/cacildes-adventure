using System.Linq;
using AF.Events;
using TigerForge;
using UnityEngine;

namespace AF
{

    public class VampireCombatState : CombatState
    {
        public Accessory garlicAccessory;
        public EquipmentDatabase equipmentDatabase;

        [Header("States")]
        public FleeState fleeState;

        public int maxFleeingAttempts = 3;
        [SerializeField] int fleeingAttempts = 0;

        private void Start()
        {
            EventManager.StartListening(EventMessages.ON_LEAVING_BONFIRE, () =>
            {
                fleeingAttempts = 0;
            });
        }

        public override void OnStateEnter(StateManager stateManager)
        {
            base.OnStateEnter(stateManager);
        }

        public override void OnStateExit(StateManager stateManager)
        {
            base.OnStateExit(stateManager);
        }

        public override State Tick(StateManager stateManager)
        {
            if (equipmentDatabase.IsAccessoryEquiped(garlicAccessory) && fleeingAttempts <= maxFleeingAttempts)
            {
                fleeingAttempts++;

                return fleeState;
            }

            return base.Tick(stateManager);
        }
    }
}
