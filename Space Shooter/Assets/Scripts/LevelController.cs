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
        private Spawner _playerSpawner;

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

        [SerializeField]
        private Transform _playerSpawnPosition;

        [SerializeField, Tooltip("The time between player death and respawn.")]
        private float _waitToRespawn;

        private PlayerSpaceShip _player = null;

        private GameObject _playerObject;

        private int _enemyCount;

        private bool _respawning = false;

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
                GameObject spawnerObject = transform.FindChild("Enemy Spawner").gameObject;
                _enemySpawner = spawnerObject.GetComponent<Spawner>();

                //Transform childTransform = transform.Find("EnemySpawner");
            }

            if (_playerSpawner == null)
            {
                Debug.Log("No reference to a player spawner.");
                GameObject spawnerObject = transform.FindChild("Player Spawner").gameObject;
                _playerSpawner = spawnerObject.GetComponent<Spawner>();
            }

            //_player = _playerSpawner.Object.GetComponent<PlayerSpaceShip>();
        }

        protected void Start()
        {
            //StartCoroutine(SpawnPlayer());

            _playerObject = _playerSpawner.Spawn();


            _player = _playerObject.GetComponent<PlayerSpaceShip>();

            StartCoroutine(SpawnEnemies());
        }

        protected void Update()
        {
            if (!_respawning && _player.Health.IsDead && _player.Lives > 0)
            {
                StartCoroutine(SpawnPlayer());
            }
        }

        private IEnumerator SpawnPlayer()
        {
            _respawning = true;
            yield return new WaitForSeconds(_waitToRespawn);

            _playerObject.transform.position = _playerSpawner.transform.position;
            _playerObject.transform.rotation = _playerSpawner.transform.rotation;
            _playerObject.SetActive(true);
            _respawning = false;
            StartCoroutine(_player.RespawnInvincibility());
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
