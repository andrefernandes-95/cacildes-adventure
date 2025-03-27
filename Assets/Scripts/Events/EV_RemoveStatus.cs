using System.Collections;

namespace AF
{

    public class EV_RemoveStatus : EventBase
    {
        public StatusEffect statusEffectToRemove;

        PlayerManager _playerManager;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(RemoveStatus());
        }

        IEnumerator RemoveStatus()
        {
            GetPlayerManager()?.statusController?.RemoveStatusEffect(statusEffectToRemove);

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
