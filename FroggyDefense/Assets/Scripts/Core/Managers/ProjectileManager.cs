using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Weapons;

namespace FroggyDefense.Core {
    public class ProjectileManager : MonoBehaviour
    {
        public static ProjectileManager instance;

        [Header("Projectile Spawning")]
        [Space]
        [SerializeField]
        private GameObject projectilePrefab;
        private Vector3 projectileSpawnLoc;

        public GameObject ProjectilePrefab { get => projectilePrefab; }
        public Vector3 ProjectileSpawnLoc { get => projectileSpawnLoc; }

        private Dictionary<ProjectileInfo, DynamicProjectilePool> projectileIndex;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Debug.LogWarning($"Already an instance of ProjectileManager. Destroying new manager.");
                Destroy(this);
            }

            projectileIndex = new Dictionary<ProjectileInfo, DynamicProjectilePool>();
        }

        /// <summary>
        /// Gets a projectile from the pool.
        /// Returns null on error.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        public Projectile GetProjectile(ProjectileInfo projectile)
        {
            // Check if the pool already has this projectile type.
            if (!projectileIndex.ContainsKey(projectile))
            {
                // If not, create a new entry.
                projectileIndex.Add(projectile, new DynamicProjectilePool(projectile));
            }

            return Grab(projectile);
        }

        /// <summary>
        /// Grabs a projectile from the appropriate pool.
        /// Returns null on error.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        private Projectile Grab(ProjectileInfo projectile)
        {
            try
            {
                return projectileIndex[projectile].Get();
            } catch (Exception e)
            {
                Debug.LogWarning($"Error grabbing projectile: {e}");
                return null;
            }
        }

        /// <summary>
        /// Returns the projectile to the projectile pool. Returns true if successful
        /// and false if not.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        public bool Return(Projectile projectile)
        {
            if (projectileIndex.ContainsKey(projectile.Template))
            {
                projectileIndex[projectile.Template].Return(projectile);
            }
            return false;
        }
    }
}