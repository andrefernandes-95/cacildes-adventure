using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Weapon / New Staff Weapon")]
    public class StaffWeapon : Weapon
    {
        [Header("Spells Allowed")]
        public SpellType[] spellType;

        public override void PerformAction(
            CharacterBaseManager character,
            bool isRightHand,
            bool isTriggerAction)
        {
            SpellInstance equippedSpell = character.characterBaseEquipment.GetSpellInstance();
            if (equippedSpell.IsEmpty())
            {
                Debug.Log("Tried to use staff. No spell equipped!");
                return;
            }

            Spell spell = equippedSpell.GetItem<Spell>();

            if (isRightHand)
            {
                if (isTriggerAction)
                {
                    ChooseAction(character, spell.rightTriggerActions);
                }
                else
                {
                    ChooseAction(character, spell.rightBumperActions);
                }
            }
            else
            {
                if (isTriggerAction)
                {
                    ChooseAction(character, spell.leftTriggerActions);
                }
                else
                {
                    ChooseAction(character, spell.leftBumperActions);
                }
            }
        }

    }
}
