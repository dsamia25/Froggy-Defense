using System;
using UnityEngine;

namespace FroggyDefense.Interactables
{
    public class DropGems : MonoBehaviour
    {
        [Space]
        [Header("Gems")]
        [Space]
        public Vector2Int GemDropRange = Vector2Int.zero;

        public delegate void DropGemDelegate(DropGemsEventArgs args);
        public static DropGemDelegate DropGemsEvent;

        /// <summary>
        /// Invokes the DropGemsEvent for a manager to spawn in a GemDropper.
        /// </summary>
        public void Drop()
        {
            int amount = UnityEngine.Random.Range(GemDropRange.x, GemDropRange.y);
            DropGemsEvent?.Invoke(new DropGemsEventArgs(transform.position, amount));
        }
    }

    public class DropGemsEventArgs : EventArgs
    {
        public Vector2 pos;     // The position of the event.
        public int gems;        // How many gems should be dropped.

        public DropGemsEventArgs(Vector2 _pos, int _gems)
        {
            pos = _pos;
            gems = _gems;
        }
    }
}