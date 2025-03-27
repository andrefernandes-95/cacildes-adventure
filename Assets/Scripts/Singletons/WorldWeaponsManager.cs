using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    /// <summary>
    /// Manages the weapon system for both player and AI characters.
    /// 
    /// This script maintains a centralized list of weapons, which can be attached to character hands.
    /// Each weapon supports left-hand, right-hand, dual-wielding, two-handed, and heavy attacks.
    /// 
    /// The goal is to provide a unified weapon system shared between the player and AI,
    /// ensuring consistency and reducing maintenance overhead. Weapons can be instantiated
    /// as needed by creating copies from the centralized list.
    /// </summary>
    public class WorldWeaponsManager : MonoBehaviour
    {
        public static WorldWeaponsManager instance;

        public SerializedDictionary<Weapon, WorldWeapon> weapons;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
