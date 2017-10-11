using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Current
        {
            get; private set;
        }

        [SerializeField]
        private Spawner _enemySpawner;

        [SerializeField]
        private GameObject[] _enemyMoveTargets;

        [SerializeField]
        private float _spawnInterval = 1.0f;

        [SerializeField, Tooltip("The time before the first spawn.")]
        private float _waitToSpawn;

        [SerializeField]
        private int _maxEnemyUnitsToSpawn;

        [SerializeField]
        private GameObjectPool _playerProjectilePool;

        [SerializeField]
        private GameObjectPool _enemyProjectilePool;

        private int _enemyCount;

        protected void Awake()
        {
            if (Current == null)
            {
                Current = this;
            }
            else
            {
                Debug.LogError("There are multiple LevelControllers in the scene!");
            }

            if(_enemySpawner == null)
            {
                Debug.Log("No reference to an enemy spawner.");
                _enemySpawner = GetComponentInChildren<Spawner>();

                //Transform childTransform = transform.Find("EnemySpawner");
            }
        }

        protected void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(_waitToSpawn);

            while(_enemyCount < _maxEnemyUnitsToSpawn)
            {
                EnemySpaceShip enemy = SpawnEnemyUnit();
                if (enemy != null)
                {
                    _enemyCount++;
                }
                else
                {
                    Debug.LogError("Could not spawn an enemy!");
                    yield break;
                }

                yield return new WaitForSeconds(_spawnInterval);
            }

            SpawnEnemyUnit();
        }

        private EnemySpaceShip SpawnEnemyUnit()
        {
            GameObject spawnedEnemyObject = _enemySpawner.Spawn();
            EnemySpaceShip enemyShip = spawnedEnemyObject.GetComponent<EnemySpaceShip>();
            if(enemyShip != null)
            {
                enemyShip.SetMovementTargets(_enemyMoveTargets);
            }
            return enemyShip;
        }

        public Projectile GetProjectile(SpaceShipBase.Type type)
        {
            GameObject result = null;

            if(type == SpaceShipBase.Type.Player)
            {
                result = _playerProjectilePool.GetPooledObject();
            }
            else
            {
                result = _enemyProjectilePool.GetPooledObject();
            }

            if(result != null)
            {
                //return result.GetComponent<Projectile>();
                Projectile projectile = result.GetComponent<Projectile>();
                if (projectile == null)
                {
                    Debug.LogError("Projectile component could not be found from the object fetched from the pool.");
                }

                return projectile;
            }

            return null;
        }

        public bool ReturnProjectile(SpaceShipBase.Type type, Projectile projectile)
        {
            if(type == SpaceShipBase.Type.Player)
            {
                return _playerProjectilePool.ReturnObject(projectile.gameObject);
            }
            else
            {
                return _enemyProjectilePool.ReturnObject(projectile.gameObject);
            }
        }
    }
}
