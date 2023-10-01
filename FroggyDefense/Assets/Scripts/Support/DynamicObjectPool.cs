using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectPool: IObjectPool<GameObject>
{   
    private List<GameObject> active;                        // List of currently active objects.
    private List<GameObject> available;                     // List of currently available objects.

    public GameObject Template { get; private set; }        // The gameobject to create.
    public Transform SpawnLoc { get; private set; }         // Where to spawn the new objects.
    public int Count => active.Count + available.Count;     // Total amount of projectiles in the pool.
    public int ActiveCount => active.Count;                 // Amount of currently active projectiles in the pool.
    public int AvailableCount => available.Count;           // Amount of currently available projectiles in the pool.

    // TODO: Make an algorithm to dynamically adjust the hold amount based on how much should be needed.
    public int HoldAmount { get; private set; }             // The amount of objects that should be held.
    public bool DynamicHoldAmount { get; private set; }     // If the hold amount should adjust based on how much should be needed.

    public DynamicObjectPool(GameObject template, Transform spawnLoc, int holdAmount, bool dynamicHoldAmount)
    {
        Template = template;
        SpawnLoc = spawnLoc;
        HoldAmount = holdAmount;
        DynamicHoldAmount = dynamicHoldAmount;

        active = new List<GameObject>();
        available = new List<GameObject>();
    }

    /// <summary>
    /// Gets an item from the pool. Objects received ARE NOT CURRENTLY ACTIVE.
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        GameObject obj = null;

        // If there are no available objects, create a new one.
        if (available.Count <= 0)
        {
            obj = Create();
            active.Add(obj);
            return obj;
        }

        // Get an existing one.
        obj = available[0];
        available.Remove(obj);
        active.Add(obj);
        return obj;
    }

    private GameObject Create()
    {
        return GameObject.Instantiate(Template, SpawnLoc);
    }

    /// <summary>
    /// Returns an object to the pool. If there is already enough held objects, deletes the returned one.
    /// objects received ARE THEN DEACTIVATED.
    /// </summary>
    /// <param name="obj"></param>
    public void Return(GameObject obj)
    {
        try
        {
            if (active.Contains(obj))
            {
                active.Remove(obj);

                if (Count >= HoldAmount)
                {
                    GameObject.Destroy(obj.gameObject);
                    //Debug.Log($"Destroying projectile. Currently {Count} (ac:{ActiveCount})(av:{AvailableCount}) out of wanted size {HoldAmount}");
                    return;
                }

                //Debug.Log($"Adding projectile to available. Currently {Count} (ac:{ActiveCount})(av:{AvailableCount}) out of wanted size {HoldAmount}");
                available.Add(obj);
                obj.SetActive(false);
            }
        }
        catch (System.Exception e)
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
