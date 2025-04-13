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

        [Header("Contextual Attacks For Right Hand")]
        public List<AttackAction> runAttackActions = new();
        public List<AttackAction> dodgeAttackActions = new();
        public List<AttackAction> backstepAttackActions = new();
        public List<AttackAction> pokingBehindShieldAttackActions = new();
        public List<AttackAction> powerStanceAttackActions = new();
        public List<AttackAction> jumpAttacks = new();

        public virtual void UpdateRightAttackAnimations(CharacterBaseManager character)
        {
            // If is two handing and has right bumper actions for two handing, equip them
            if (character.characterBaseTwoHandingManager.isTwoHanding)
            {
                if (two_hand_rightBumperActions.Count > 0)
                {
                    character.UpdateAttackAnimations(two_hand_rightBumperActions.ToArray());
                }
                if (two_hand_rightTriggerActions.Count > 0)
                {
                    character.UpdateAttackAnimations(two_hand_rightTriggerActions.ToArray());
                }
            }
            else
            {
                character.UpdateAttackAnimations(rightBumperActions.ToArray());
                character.UpdateAttackAnimations(rightTriggerActions.ToArray());
            }

            if (runAttackActions.Count > 0)
            {
                character.UpdateAttackAnimations(runAttackActions.ToArray());
            }
            if (jumpAttacks.Count > 0)
            {
                character.UpdateAttackAnimations(jumpAttacks.ToArray());
            }
        }

        public virtual void UpdateLeftAttackAnimations(CharacterBaseManager character)
        {
            character.UpdateAttackAnimations(leftBumperActions.ToArray());
            character.UpdateAttackAnimations(leftTriggerActions.ToArray());
        }

        public virtual void PerformAction(
            CharacterBaseManager character,
            bool isRightHand,
            bool isTriggerAction)
        {

            if (isRightHand)
            {
                if (isTriggerAction)
                {
                    ChooseAction(character, character.characterBaseTwoHandingManager.isTwoHanding
                        ? two_hand_rightTriggerActions : rightTriggerActions);
                }
                else
                {

                    List<AttackAction> attackActions = new();

                    if (character.combatManager.wantsToRunAttack)
                    {
                        attackActions = runAttackActions;
                    }
                    else if (character.combatManager.wantsToJumpAttack)
                    {
                        attackActions = jumpAttacks;
                    }
                    else if (character.characterBaseTwoHandingManager.isTwoHanding)
                    {
                        attackActions = two_hand_rightBumperActions;
                    }
                    else
                    {
                        attackActions = rightBumperActions;
                    }

                    ChooseAction(character, attackActions);
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
