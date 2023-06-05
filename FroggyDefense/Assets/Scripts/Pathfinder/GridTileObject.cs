using UnityEngine;

namespace Pathfinder
{
    /// <summary>
    /// ScriptableObject containing info about a GridTile.
    /// </summary>
    [CreateAssetMenu(fileName = "New GridTile", menuName = "ScriptableObjects/Pathfinder/GridTile")]
    public class GridTileObject : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _isWater;
        [SerializeField] private bool _isWall;
        [SerializeField] private bool _isImpassable;
        [SerializeField] private float _toDistance;
        [SerializeField] private float _fromDistance;

        public string Name => _name;                    // The kind of tile.
        public bool isWater => _isWater;                // Water tile that most things cannot pass.
        public bool isWall => _isWall;                  // Wall tile that most things cannot pass.
        public bool isImpassable => _isImpassable;      // Impassable tile that nothing can pass.
        public float ToDistance => _toDistance;         // The cost of moving from this tile.
        public float FromDistance => _fromDistance;     // The cost of moving to this tile.
    }
}