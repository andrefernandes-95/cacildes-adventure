using System.Collections;

namespace AF
{

    public class EV_InflictStatus : EventBase
    {
        public StatusEffect statusEffectToInflict;

        PlayerManager _playerManager;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(RemoveStatus());
        }

        IEnumerator RemoveStatus()
        {
            GetPlayerManager()?.statusController?.InflictStatusEffect(statusEffectToInflict);

            yield return null;
        }


        PlayerManager GetPlayerManager()
        {
            if (_playerManager == null)
            {
                _playerManager = FindAnyObjectByType<PlayerManager>(UnityEngine.FindObjectsInactive.Include);
            }

            return _playerManager;
        }
    }

}
