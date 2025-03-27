using AF.Events;
using TigerForge;

namespace AF
{
    public class CharacterPosture : CharacterAbstractPosture
    {
        public int maxPostureDamage = 100;
        int defaultMaxPostureDamage;

        public GameSession gameSession;

        private void Awake()
        {
            defaultMaxPostureDamage = maxPostureDamage;
        }

        private void Start()
        {
            EventManager.StartListening(EventMessages.ON_LEAVING_BONFIRE, () =>
            {
                maxPostureDamage = defaultMaxPostureDamage;
            });
        }

        public override bool CanPlayPostureDamagedEvent()
        {
            return true;
        }

        public override int GetMaxPostureDamage()
        {
            return Utils.ScaleWithCurrentNewGameIteration(maxPostureDamage, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
        }

        public override float GetPostureDecreateRate()
        {
            return 1f;
        }

        public void ResetPosture()
        {
            this.currentPostureDamage = 0;
        }
        public override bool TakePostureDamage(int extraPostureDamage)
        {
            bool hasBrokenPosture = base.TakePostureDamage(extraPostureDamage);

            // If taking posture damage, increase the max posture
            if (hasBrokenPosture)
            {
                maxPostureDamage += (int)(maxPostureDamage / 2);
            }

            return hasBrokenPosture;
        }
    }
}
