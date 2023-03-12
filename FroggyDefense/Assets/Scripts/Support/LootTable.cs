using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable<T> where T : class
{
    private Dictionary<T, int> _table = null;   // Table linking each item to its roll weight. The roll weight is how much of a chance that item has to be rolled as its percent contribution to the total weight of all items.
    private int _totalWeight = 0;    // The total weight of all objects in the pool.
    int tests = 0;  // Which round of tests the table is on. For differentiating debug outputs.

    public LootTable()
    {
        _table = new Dictionary<T, int>();
    }

    /// <summary>
    /// Adds the input item to the list with the given weight. If the item is already in the list,
    /// replace the existing weight with the new one.
    /// Replacing works the same as the ChangeWeight method but does not return the new weight.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="weight"></param>
    public void Add(T key, int weight)
    {
        if (_table.ContainsKey(key))
        {
            _totalWeight -= _table[key];    // Subtract current weight from total.
            _table[key] = weight;
            _totalWeight += weight;         // Add the new weight to the total.
        }
        else
        {
            if (weight < 0)
            {
                weight = 0;
            }
            _table.Add(key, weight);
            _totalWeight += weight;
        }
    }

    /// <summary>
    /// Removes the given item from the list and removes its weight from the
    /// total weight of the table. Returns true if successful, false if not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Remove(T key)
    {
        if (_table.ContainsKey(key))
        {
            _totalWeight -= _table[key];
            return _table.Remove(key);
        }
        return false;
    }

    /// <summary>
    /// Returns true if the given item is in the list.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(T key)
    {
        return _table.ContainsKey(key);
    }

    /// <summary>
    /// Returns the weight of the given item if it is in the list.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetWeight(T key)
    {
        if (_table.ContainsKey(key))
        {
            return _table[key];
        }
        return -1;
    }

    /// <summary>
    /// Changes the weight of the given item if it is in the list and returns its
    /// newest weight. If the item is not in the list, returns -1.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public int ChangeWeight(T key, int weight)
    {
        if (_table.ContainsKey(key))
        {
            _totalWeight -= _table[key];    // Subtract current weight from total.
            _table[key] = weight;
            _totalWeight += weight;         // Add the new weight to the total.
            return _table[key];
        }
        return -1;
    }

    /// <summary>
    /// Returns a random item from the list based on their roll weight. An object with a weight of 5
    /// out of a total weight of 100 will have a 5% chance to be returned, 25 out of 50 will have a
    /// 50% chance, and so on.
    /// If there are no items or no items with weights greater than 0, returns null.
    /// </summary>
    /// <returns></returns>
    public T Roll()
    {
        if (_table.Count > 0 && _totalWeight > 0)
        {
            int roll = UnityEngine.Random.Range(1, _totalWeight);
            int weight = 0;

            var enumerator = _table.GetEnumerator();
            do
            {
                weight += enumerator.Current.Value;
                if (weight >= roll)
                {
                    return enumerator.Current.Key;
                }
            } while (enumerator.MoveNext());
            return enumerator.Current.Key;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Tests the percents of each item in the list by rolling a set amount of time and
    /// recording how many times each item was returned. Displays the output % as well as
    /// the expected %.
    /// </summary>
    /// <param name="rolls"></param>
    public void LootTableTest(int rolls)
    {
        Dictionary<T, int> results = new Dictionary<T, int>();
        for (int i = 0; i < rolls; i++)
        {
            var result = Roll();
            if (result == null)
            {
                break;
            }
            if (results.ContainsKey(result))
            {
                results[result]++;
            }
            else
            {
                results.Add(result, 1);
            }
        }
        string review = "";
        foreach (var result in results)
        {
            review += "item: " + result.Key.ToString() + ", weight: " + _table[result.Key] + "(" + (float)(100.0f * _table[result.Key] / _totalWeight) + "%), rolled " + result.Value + " times (" + (float)(100.0f * result.Value / rolls) + "%)\n";
        }

        Debug.Log("LootTable Test " + ++tests + " Results\n{\n" + review + "\n}");
    }
}
