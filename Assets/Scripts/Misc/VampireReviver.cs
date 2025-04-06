using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using AF.Combat;
using System;
using TigerForge;
using AF.Events;

namespace AF
{
    public class VampireReviver : MonoBehaviour, IDamageable
    {
        public bool allowDamage = false;
        public bool canRevive = true;

        public float timeBeforeReviving = 8f;

        public CharacterManager characterManager;

        [Header("Components")]
        public PlayerManager playerManager;

        public UnityEvent onRevival;
        public UnityEvent onStoppingRevival;

        private void Awake()
        {
            EventManager.StartListening(EventMessages.ON_LEAVING_BONFIRE, () =>
            {
                allowDamage = false;
                canRevive = true;

                this.gameObject.SetActive(false);
            });
        }

        public void OnEnable()
        {
            // TODO Handle this code later
            if (playerManager == null)
            {
                playerManager = FindAnyObjectByType<PlayerManager>(FindObjectsInactive.Include);
            }

            if (playerManager == null)
            {
                return;
            }

            WeaponInstance playerWeapon = playerManager.characterBaseEquipment.GetRightHandWeapon();

            if (playerWeapon.Exists() && playerWeapon.GetItem<Weapon>().isHolyWeapon)
            {
                this.gameObject.SetActive(false);
                return;
            }

            allowDamage = false;

            StartCoroutine(AllowDamage());

            if (!canRevive)
            {
                this.gameObject.SetActive(false);
                return;
            }

            StartCoroutine(EvaluateIfCanRevive());
        }

        IEnumerator AllowDamage()
        {
            yield return new WaitForSeconds(0.5f);
            allowDamage = true;
        }

        IEnumerator EvaluateIfCanRevive()
        {
            yield return new WaitForSeconds(timeBeforeReviving);

            if (canRevive)
            {
                characterManager.shouldReturnToInitialPositionOnRevive = false;
                characterManager.Revive();
                onRevival?.Invoke();
                characterManager.shouldReturnToInitialPositionOnRevive = true;
            }

            this.gameObject.SetActive(false);
            canRevive = false;
        }

        public void SetCanRevive(bool value)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            this.canRevive = value;

            if (value == false)
            {
                onStoppingRevival?.Invoke();
                this.gameObject.SetActive(false);
            }
        }

        public void OnDamage(CharacterBaseManager attacker, Action onDamageInflicted)
        {
            if (!allowDamage)
            {
                return;
            }

            SetCanRevive(false);
        }
    }
}
