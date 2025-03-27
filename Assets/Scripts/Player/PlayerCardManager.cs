using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AF.Health;
using AYellowpaper.SerializedCollections;
using UnityEngine.Localization.Settings;

namespace AF
{
    public class PlayerCardManager : MonoBehaviour
    {

        public PlayerManager playerManager;

        public Card currentCard;
        public NotificationManager notificationManager;


        [Header("Effect Instances")]
        [SerializedDictionary("Card", "World Instance")]
        public SerializedDictionary<Card, CardEffectInstance> cardEffectInstances;


        public bool StartCardUse(Card card)
        {
            if (!CanUseCard(card))
            {
                return false;
            }

            SetCurrentCard(card);

            if (card.staminaRequired > 0)
            {
                playerManager.staminaStatManager.DecreaseStamina(card.staminaRequired);
            }

            if (card.manaRequired > 0)
            {
                playerManager.manaManager.DecreaseMana(card.manaRequired);
            }


            foreach (StatusEffect statusEffect in card.statusEffectsWhenConsumed)
            {
                playerManager.statusController.statusEffectInstances.FirstOrDefault(x => x.Key == statusEffect).Value?.onConsumeStart?.Invoke();
            }

            CardEffectInstance cardEffectInstance = GetCardEffectInstance(card);
            if (cardEffectInstance != null)
            {
                cardEffectInstance.onCardStart?.Invoke();
            }

            // Drop IK Helper
            playerManager.playerAnimationEventListener.DropIKHelper();

            if (card != null && card.clip != null)
            {
                playerManager.UpdateAnimatorOverrideControllerClip("Cacildes - Special Attack - 1", card.clip);
                playerManager.RefreshAnimationOverrideState();
                BeginSpecialAttack();
            }

            return true;
        }

        public void EndCurrentCardUse()
        {
            if (currentCard == null)
            {
                return;
            }

            CardEffectInstance cardEffectInstance = GetCardEffectInstance(currentCard);
            if (cardEffectInstance != null)
            {
                cardEffectInstance.onCardEnd?.Invoke();
            }

            SetCurrentCard(null);
        }

        void SetCurrentCard(Card card)
        {
            this.currentCard = card;
        }

        public void UseCurrentCard()
        {
            if (currentCard == null)
            {
                return;
            }

            CardEffectInstance cardEffectInstance = GetCardEffectInstance(currentCard);
            if (cardEffectInstance == null)
            {
                return;
            }

            cardEffectInstance.onCardUse?.Invoke();
        }

        void BeginSpecialAttack()
        {
            playerManager.playerCombatController.HandleHeavyAttack(true);
        }

        public bool HasCard()
        {
            return this.currentCard != null;
        }

        public bool CanUseCard(Card card)
        {
            if (card == null)
            {
                return false;
            }

            if (card.staminaRequired > 0 && !playerManager.staminaStatManager.HasEnoughStaminaForAction(card.staminaRequired))
            {
                return false;
            }
            if (card.manaRequired > 0 && !playerManager.manaManager.HasEnoughManaForAction(card.manaRequired))
            {
                return false;
            }

            /*            if (playerManager.equipmentDatabase.GetCurrentWeapon() != null)
                        {
                            notificationManager.ShowNotification(
                                LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Can not use current card with this weapon")
                            );

                            return currentCard.weaponCategory == playerManager.equipmentDatabase.GetCurrentWeapon().weaponCategory;
                        }*/

            return true;
        }


        CardEffectInstance GetCardEffectInstance(Card card)
        {
            if (cardEffectInstances != null && cardEffectInstances.ContainsKey(card))
            {
                return cardEffectInstances[card];
            }

            return null;
        }

        public Damage CombineDamageWithCard(Damage baseDamage)
        {
            if (baseDamage.physical > 0)
            {
                baseDamage.physical += currentCard.cardDamage.physical;
            }

            if (baseDamage.fire > 0)
            {
                baseDamage.fire += currentCard.cardDamage.fire;
            }

            if (baseDamage.frost > 0)
            {
                baseDamage.frost += currentCard.cardDamage.frost;
            }

            if (baseDamage.magic > 0)
            {
                baseDamage.magic += currentCard.cardDamage.magic;
            }

            if (baseDamage.lightning > 0)
            {
                baseDamage.lightning += currentCard.cardDamage.lightning;
            }

            if (baseDamage.darkness > 0)
            {
                baseDamage.darkness += currentCard.cardDamage.darkness;
            }

            if (baseDamage.water > 0)
            {
                baseDamage.water += currentCard.cardDamage.water;
            }

            if (currentCard.forcePushForceToZero)
            {
                baseDamage.pushForce = 0;
            }
            else if (baseDamage.pushForce > 0)
            {
                baseDamage.pushForce += currentCard.cardDamage.pushForce;
            }

            if (baseDamage.statusEffects != null && baseDamage.statusEffects.Length > 0)
            {
                List<StatusEffectEntry> thisDamageStatusEffects = baseDamage.statusEffects.ToList();

                foreach (StatusEffectEntry cardStatusEffectEntry in currentCard.cardDamage.statusEffects)
                {
                    int idx = thisDamageStatusEffects.FindIndex(x => x == cardStatusEffectEntry);

                    if (idx != -1)
                    {
                        thisDamageStatusEffects[idx].amountPerHit += cardStatusEffectEntry.amountPerHit;
                    }
                    else
                    {
                        thisDamageStatusEffects.Add(cardStatusEffectEntry);
                    }
                }

                baseDamage.statusEffects = thisDamageStatusEffects.ToArray();
            }

            return baseDamage;
        }
    }
}
