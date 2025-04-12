using System;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Action Item / New Action Item")]
    public class ActionItem : Item
    {
        [Header("⚔️ Attack Actions")]
        public List<AttackAction> rightBumperActions = new();
        public List<AttackAction> rightTriggerActions = new();
        public List<AttackAction> leftBumperActions = new();
        public List<AttackAction> leftTriggerActions = new();

        [Header("⚔️ Two Handing Actions")]
        public List<AttackAction> two_hand_rightBumperActions = new();
        public List<AttackAction> two_hand_rightTriggerActions = new();

        public virtual void PerformAction(
            CharacterBaseManager character,
            bool isRightHand,
            bool isTriggerAction)
        {

            if (isRightHand)
            {
                if (isTriggerAction)
                {
                    ChooseAction(character, isRightHand && character.characterBaseTwoHandingManager.isTwoHanding
                        ? two_hand_rightTriggerActions : rightTriggerActions);
                }
                else
                {
                    ChooseAction(character, isRightHand && character.characterBaseTwoHandingManager.isTwoHanding
                        ? two_hand_rightBumperActions : rightBumperActions);
                }
            }
            else
            {
                if (isTriggerAction)
                {
                    ChooseAction(character, leftTriggerActions);
                }
                else
                {
                    ChooseAction(character, leftBumperActions);
                }
            }
        }

        protected void ChooseAction(CharacterBaseManager character, List<AttackAction> actions)
        {
            if (actions == null || actions.Count == 0)
            {
                Debug.LogWarning($"{this.name}: No actions assigned for this input.");
                return;
            }
            AttackAction previousAction = character.combatManager.lastAttackAction;

            AttackAction chosenAttackAction = actions[0];

            if (previousAction != null && actions.Contains(previousAction))
            {
                int nextIndex = actions.IndexOf(previousAction) + 1;
                if (nextIndex >= actions.Count)
                    nextIndex = 0;

                chosenAttackAction = actions[nextIndex];
            }

            if (chosenAttackAction != null)
            {
                character.combatManager.lastAttackAction = chosenAttackAction;
                chosenAttackAction.Execute(character);
            }
        }
    }
}
