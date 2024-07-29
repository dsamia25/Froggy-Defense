using UnityEngine;
using Support;

namespace Core
{
    public class SpawnManager : SingletonMonoBehaviour<SpawnManager>
    {
        public GameObject SpawnSigilPrefab;
        public Material SpawnMaterial;
        public float SpawnDelay = .5f;
        public float SpawnAnimationTime = 1f;
        public Vector2 SpawnPositionOffset = new Vector2(0, .2f);

        public GameObject Spawn(GameObject prefab, Team team, int spawnLevel, Vector2 pos)
        {
            // TODO: Make an object pool for sigils.
            GameObject sigil = Instantiate(SpawnSigilPrefab, pos, Quaternion.identity);
            GameObject spawn = Instantiate(prefab, pos + SpawnPositionOffset, Quaternion.identity);
            Character character;
            if ((character = spawn.GetComponent<Character>()) != null)
            {
                character.SetLevel(spawnLevel);
                character.team = team;
                character.TriggerSummonAnimation();
            }
            return spawn;
        }
    }
}