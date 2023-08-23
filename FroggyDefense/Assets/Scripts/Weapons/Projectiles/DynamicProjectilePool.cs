using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public class DynamicProjectilePool
    {
        private List<Projectile> active;                        // List of currently active projectiles.
        private List<Projectile> available;                     // List of currently available projectiles.

        public ProjectileObject Template { get; private set; }  // The projectile to create.
        public int Count => active.Count + available.Count;     // Total amount of projectiles in the pool.
        public int ActiveCount => active.Count;                 // Amount of currently active projectiles in the pool.
        public int AvailableCount => available.Count;           // Amount of currently available projectiles in the pool.

        private ProjectileManager manager;

        public DynamicProjectilePool(ProjectileObject template, ProjectileManager manager)
        {
            Template = template;
            active = new List<Projectile>();
            available = new List<Projectile>();
            this.manager = manager;
        }

        /// <summary>
        /// Gets a projectile. If there are no currently available projectiles,
        /// returns a newly created one.
        /// </summary>
        /// <returns></returns>
        public Projectile Get()
        {
            Projectile proj = null;

            // If there are no available projectiles, create a new one.
            if (available.Count <= 0)
            {
                proj = Create();
                active.Add(proj);
                return proj;
            }

            // Get an existing one.
            proj = available[0];
            available.Remove(proj);
            active.Add(proj);
            return proj;
        }

        /// <summary>
        /// Creates a new projectile for the pool.
        /// </summary>
        /// <returns></returns>
        private Projectile Create()
        {
            Projectile proj = GameObject.Instantiate(Template.Vfx, manager.ProjectileSpawnLoc, Quaternion.identity).GetComponent<Projectile>();
            proj.Init(Template);
            return proj;
        }

        /// <summary>
        /// Returns a Projectile to the available list. If the list is oversaturated, destroy
        /// the projectile instead.
        /// </summary>
        /// <param name="projectile"></param>
        public void Return(Projectile projectile)
        {
            try
            {
                if (active.Contains(projectile))
                {
                    active.Remove(projectile);

                    if (Count >= Template.ProjectilePoolSize)
                    {
                        GameObject.Destroy(projectile.gameObject);
                        Debug.Log($"Destroying projectile. Currently {Count} (ac:{ActiveCount})(av:{AvailableCount}) out of wanted size {Template.ProjectilePoolSize}");
                        return;
                    }

                    Debug.Log($"Adding projectile to available. Currently {Count} (ac:{ActiveCount})(av:{AvailableCount}) out of wanted size {Template.ProjectilePoolSize}");
                    available.Add(projectile);
                }
            } catch (Exception e)
            {
                Debug.Log($"Error returning projectile to pool: {e}");
            }
        }

        /// <summary>
        /// Destroy everything in pool.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < active.Count; i++)
            {
                var temp = active[i];
                active.Remove(temp);
                GameObject.Destroy(temp);
            }
            for (int i = 0; i < available.Count; i++)
            {
                var temp = available[i];
                available.Remove(temp);
                GameObject.Destroy(temp);
            }
        }
    }
}