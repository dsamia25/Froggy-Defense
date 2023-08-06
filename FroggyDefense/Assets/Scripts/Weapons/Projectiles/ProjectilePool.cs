using UnityEngine;

namespace FroggyDefense.Weapons
{
    [System.Serializable]
    public class ProjectilePool
    {
        private GameObject _prefab = null;
        private Transform _projectileParent = null;

        [SerializeField] private Projectile[] _pool = null;   // The pool of objects to grab from.
        int _maxSize = 8;       // The maximum size of the pool.

        private int _poolIndex = 0;  // The current index the item is grabbing from.

        public ProjectilePool(GameObject prefab, Transform parent, int size)
        {
            _prefab = prefab;
            _projectileParent = parent;
            _maxSize = size;

            _pool = new Projectile[_maxSize];
        }

        public ProjectilePool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _projectileParent = parent;

            _pool = new Projectile[_maxSize];
        }

        public Projectile Get()
        {
            if (_poolIndex >= _maxSize)
            {
                return _pool[_poolIndex++ % _maxSize];
            }
            else
            {
                GameObject newProjectile = GameObject.Instantiate(_prefab, _projectileParent);
                newProjectile.SetActive(false);
                _pool[_poolIndex] = newProjectile.GetComponent<Projectile>();
                return _pool[_poolIndex++];
            }
        }

        public void Clear()
        {
            foreach (var projectile in _pool)
            {
                if (projectile != null)
                {
                    GameObject.Destroy(projectile.gameObject);
                }
            }
        }
    }
}