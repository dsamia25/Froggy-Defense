using UnityEngine;

namespace FroggyDefense.Core
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager instance;

        public GameObject SpawnSigilPrefab;
        public Material SpawnMaterial;
        public float SpawnDelay = .5f;
        public float SpawnAnimationTime = 1f;
        public Vector2 SpawnPositionOffset = new Vector2(0, .2f);

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning($"Error: Already an instance of SpawnManager.");
                Destroy(this);
            }
        }

        public GameObject Spawn(GameObject prefab, Vector2 pos)
        {
            // TODO: Make an object pool for sigils.
            GameObject sigil = Instantiate(SpawnSigilPrefab, pos, Quaternion.identity);
            GameObject spawn = Instantiate(prefab, pos + SpawnPositionOffset, Quaternion.identity);
            Character character;
            if ((character = spawn.GetComponent<Character>()) != null)
            {
                character.visualsAnimator.SetTrigger("SummonAnimation");
            }
            return spawn;
        }
    }
}