using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Attack Action", menuName = "Combat / New Attack Action", order = 0)]
    public class AttackAction : ScriptableObject
    {
        [Header("Animations")]
        public SerializedDictionary<AnimationEnum, AnimationClip> attackAnimations = new();
        [SerializeField] float crossFade = .2f;

        [Header("Modifiers")]
        public float damageMultiplier = 1f;

        [Header("Conditions")]
        [Range(0, 100)] public int attackWeight = 50;
        public float cooldown = 3f;
        public float minimumAttackAngle = -35f;
        public float maximumAttackAngle = 35f;
        public float minimumAttackDistance = 0f;
        public float maximumAttackDistance = 5f;

        public virtual void Execute(CharacterBaseManager attacker)
        {
            if (attackAnimations.Count <= 0)
            {
                return;
            }

            string attackName = attackAnimations.ElementAt(0).Key.name;

            attacker.PlayCrossFadeBusyAnimationWithRootMotion(attackName, crossFade);
        }
    }
}
