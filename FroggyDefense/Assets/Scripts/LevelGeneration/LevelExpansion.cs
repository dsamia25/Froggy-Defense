using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroggyDefense.Core
{
    public class LevelExpansion : MonoBehaviour
    {
        private Tilemap[] _tilemaps;
        public Tilemap[] Tilemaps;

        public int WaveActivate = -1;

        /// <summary>
        /// Activates the level expansion.
        /// </summary>
        public void Activate()
        {

        }
    }
}