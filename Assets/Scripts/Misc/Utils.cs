using System;
using System.Linq;
using AF.Characters;
using AF.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{

    public static class Utils
    {
        public static int ScaleWithCurrentNewGameIteration(int baseValue, int newGameCurrentIteration, float scalingFactor)
        {
            if (newGameCurrentIteration == 0)
            {
                return baseValue;
            }

            return (int)(baseValue * Math.Pow(scalingFactor, newGameCurrentIteration));
        }

        public static Vector3 GetNearestNavMeshPoint(Vector3 reference)
        {
            return Utils.GetNearestNavMeshPoint(reference, NavMesh.AllAreas);
        }

        public static Vector3 GetNearestNavMeshPoint(Vector3 reference, int area)
        {
            // Teleport near player
            NavMesh.SamplePosition(reference, out NavMeshHit hit, Mathf.Infinity, area);

            if (hit.position == null)
            {
                return reference;
            }

            return hit.position;
        }

        public static void AvoidInvalidPaths(NavMeshAgent agent)
        {
            var path = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, agent.transform.position, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathInvalid)
            {

                NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);

                if (!float.IsNaN(hit.position.x) && !float.IsInfinity(hit.position.x) &&
                    !float.IsNaN(hit.position.y) && !float.IsInfinity(hit.position.y) &&
                    !float.IsNaN(hit.position.z) && !float.IsInfinity(hit.position.z))
                {
                    // It's a valid position, so assign it to nextPosition
                    agent.nextPosition = hit.position != null ? hit.position : agent.transform.position;
                    agent.updatePosition = true;
                }
                else
                {
                    // Handle the case where the position is invalid
                    Debug.LogError("Invalid positionWithLocalOffset: " + hit.position);
                }

            }
        }

        public static void UpdateTransformChildren(Transform transformTarget, bool isActive)
        {
            if (transformTarget.childCount <= 0)
            {
                return;
            }

            foreach (Transform transformChild in transformTarget)
            {
                transformChild.gameObject.SetActive(isActive);
            }
        }

        public static void UpdateTransformChildrenWhere(Transform transformTarget, Func<GameObject, bool> condition)
        {
            if (transformTarget.childCount <= 0)
            {
                return;
            }

            foreach (Transform transformChild in transformTarget)
            {
                transformChild.gameObject.SetActive(condition(transformChild.gameObject));
            }
        }

        public static string GetItemPath(Item item)
        {
            var prefix = "Items/";

            var subFolder = "";

            if (item is Accessory)
            {
                subFolder = "Accessories/";
            }
            else if (item is UpgradeMaterial)
            {
                subFolder = "Upgrade Materials/";
            }
            else if (item is CraftingMaterial)
            {
                subFolder = "Alchemy/";
            }
            else if (item is Arrow)
            {
                subFolder = "Arrows/";
            }
            else if (item is Card)
            {
                subFolder = "Cards/";
            }
            else if (item is Consumable)
            {
                subFolder = "Consumables/";
            }
            else if (item is ConsumableProjectile)
            {
                subFolder = "Consumables/";
            }
            else if (item is Helmet)
            {
                subFolder = "Helmets/";
            }
            else if (item is Armor)
            {
                subFolder = "Armors/";
            }
            else if (item is Gauntlet)
            {
                subFolder = "Gauntlets/";
            }
            else if (item is Legwear)
            {
                subFolder = "Legwears/";
            }
            else if (item is Shield)
            {
                subFolder = "Shields/";
            }
            else if (item is Spell)
            {
                subFolder = "Spells/";
            }
            else if (item is Weapon)
            {
                subFolder = "Weapons/";
            }
            else if (item is KeyItem || item is Item)
            {
                subFolder = "Key Items/";
            }

            return prefix + subFolder + item.name;
        }


        public static CharacterManager GetClosestEnemy(PlayerManager playerManager, CharacterFaction playerFaction)
        {
            CharacterManager target = playerManager.lockOnManager.nearestLockOnTarget?.characterManager;
            if (target == null)
            {
                // Get all characters in the scene
                var allCharacters = MonoBehaviour.FindObjectsByType<CharacterManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                // Filter characters by tag "Enemy"
                var enemyCharacters = allCharacters.Where(character => character.CompareTag("Enemy"));

                // Exclude the character that is the same as this character
                var filteredCharacters = enemyCharacters.Where(_character => !_character.characterFactions.Contains(playerFaction) && _character.health.GetCurrentHealth() > 0);

                // Sort characters by distance to the player
                var closestCharacter = filteredCharacters.OrderBy(
                    character => Vector3.Distance(playerManager.transform.position, character.transform.position))?.FirstOrDefault();

                if (closestCharacter != null)
                {
                    target = closestCharacter;
                }
            }

            return target;
        }

        public static bool HasEnemyFighting()
        {
            return MonoBehaviour.FindObjectsByType<TargetManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Any(
                x => x.currentTarget != null
                && x.characterManager.health.GetCurrentHealth() > 0);
        }
    }
}
