using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroggyDefense.LevelGeneration
{
    public class LevelGenerator
    {
        public Tilemap tilemap;

        public static int[,] GenerateArray(int _width, int _height, bool _empty)
        {
            int[,] map = new int[_width, _height];
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    if (_empty)
                    {
                        map[x, y] = 0;
                    }
                    else
                    {
                        map[x, y] = 1;
                    }
                }
            }
            return map;
        }

        public static float[,] SetArea(float[,] _map, Vector2Int xRange, Vector2Int yRange, float _value)
        {
            for (int x = xRange.x; x < xRange.y; x++)
            {
                for (int y = yRange.x; y < yRange.y; y++)
                {
                    _map[x, y] = _value;
                }
            }
            return _map;
        }

        public static float[,] GenerateNoise(int _width, float _seed, float _scale)
        {
            return GenerateNoise(_width, _width, _seed, _scale);
        }

        public static float[,] GenerateNoise(int _width, int _height, float _seed, float _scale)
        {
            float[,] noiseMap = new float[_width, _height];
            float xOffset = 13.1f * _seed;
            float yOffset = 7.6f * _seed;
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float noiseValue = Mathf.PerlinNoise(1.0f * ((float)x * _scale + xOffset), 1.0f * ((float)y * _scale + yOffset));
                    noiseMap[x, y] = noiseValue;
                }
            }
            return noiseMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_map"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static int[,] InterpretNoise(float[,] _map, float[] _key)
        {
            int[,] valueMap = new int[_map.GetUpperBound(0), _map.GetUpperBound(1)];
            for (int x = 0; x < valueMap.GetUpperBound(0); x++)
            {
                for (int y = 0; y < valueMap.GetUpperBound(1); y++)
                {
                    for (int k = 0; k < _key.Length; k++)
                    {
                        if (k == _key.Length - 1)
                        {
                            valueMap[x, y] = k;
                        }
                        if (_key[k] >= _map[x, y])
                        {
                            valueMap[x, y] = k;
                            break;
                        }
                    }
                }
            }
            return valueMap;
        }

        public static void RenderMap(int[,] _map, Tilemap _tilemap, TileBase[] _tileSet)
        {
            _tilemap.ClearAllTiles();

            for (int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), _tileSet[_map[x, y]]);
                }
            }
        }

        public static void RenderMap(int[,] _map, Tilemap[] _tilemapSet, TileBase[] _tileSet)
        {
            foreach (Tilemap map in _tilemapSet)
            {
                map.ClearAllTiles();
            }

            for (int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    _tilemapSet[_map[x, y]].SetTile(new Vector3Int(x, y, 0), _tileSet[_map[x, y]]);
                }
            }
        }

        public static void UpdateMap(int[,] map, Tilemap tilemap)
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    if (map[x, y] == 0)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }

        // TODO: Not being used.
        /// <summary>
        /// Merges two tilemaps together.
        /// </summary>
        /// <param name="main"></param>
        /// <param name="addition"></param>
        public static void MergeTilemaps(Tilemap main, Tilemap addition)
        {
            //var tiles = addition.GetTilesBlock(addition.cellBounds);
            for (int x = addition.cellBounds.xMin; x < addition.cellBounds.xMax; x++)
            {
                for (int y = addition.cellBounds.yMin; x < addition.cellBounds.yMax; x++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    main.SetTile(pos, addition.GetTile(pos));
                }
            }
        }
    }
}


