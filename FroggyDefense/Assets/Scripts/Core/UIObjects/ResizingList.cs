using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.UI
{
    public class ResizingList<T> where T : MonoBehaviour
    {
        public static void Resize(GameObject _prefab, Transform _spawnLoc, List<T> _displayedList, int _neededItemAmount, int _holdAmount)
        {
            // Adjust the size of the crafting materials list to match the needed amount.
            if (_displayedList.Count < _neededItemAmount)
            {
                // Add more items for needed size.
                for (int i = _displayedList.Count; i < _neededItemAmount; i++)
                {
                    _displayedList.Add(GameObject.Instantiate(_prefab, _spawnLoc).GetComponent<T>());
                }
            }
            else if (_displayedList.Count > _neededItemAmount)
            {
                // Shrink list down to needed size.
                for (int i = _displayedList.Count - 1; i >= _neededItemAmount; i--)
                {
                    var obj = _displayedList[i];
                    // If already have the hold amount, delete the extras
                    if (i >= _holdAmount)
                    {
                        _displayedList.Remove(obj);
                        GameObject.Destroy(obj);
                    }
                    else
                    {
                        // If under the hold amount, just disable this to be reused later.
                        obj.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}