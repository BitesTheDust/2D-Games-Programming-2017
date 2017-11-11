using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.States;
using TMPro;

namespace SpaceShooter
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Current
        {
            get; private set;
        }

        [SerializeField]
        private GameStateType _nextState;

        private Spawner _playerSpawner;
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
        private GameObjectPool _healthPowerupPool;

        [SerializeField]
        private GameObjectPool _weaponPowerupPool;

        [SerializeField, Tooltip("Chance to spawn powerup on enemy death. 0.0 to 1.0")]
        private float _powerupChance;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _healthText;

        [SerializeField]
        private TextMeshProUGUI _bonusText;

        [SerializeField]
        private TextMeshProUGUI _bonusHeader;

        [SerializeField, Tooltip("The time between player death and respawn.")]
        private float _waitToRespawn;

        private PlayerSpaceShip _player = null;

        private GameObject _playerObject;

        private int _enemyCount;

        private int _enemiesKilled = 0;

        private bool _respawning = false;

        [SerializeField]
        private bool _isLastLevel = false;

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
            }

            if (_playerSpawner == null)
            {
                Debug.Log("No reference to a player spawner.");
                GameObject spawnerObject = transform.FindChild("Player Spawner").gameObject;
                _playerSpawner = spawnerObject.GetComponent<Spawner>();
            }

        }

        protected void Start()
        {
            _playerObject = _playerSpawner.Spawn();

            _player = _playerObject.GetComponent<PlayerSpaceShip>();

            StartCoroutine(SpawnEnemies());

            int moveTargetCount = transform.Find("MoveTargets").transform.childCount;
            _enemyMoveTargets = new GameObject[moveTargetCount];

            for (int i = 0; i < _enemyMoveTargets.Length; i++)
            {
                _enemyMoveTargets[i] = transform.Find("MoveTargets").transform.GetChild(i).gameObject;
            }

            _scoreText.text = "" + GameManager.Instance.CurrentScore;
        }

        protected void Update()
        {
            // Set player health to UI component
            _healthText.text = "" + _player.Health.CurrentHealth;

            // Set player bonus time to UI component if active
            if(_player.BonusTime > 0)
            {
                _bonusText.text = "" + _player.BonusTime;
                _bonusHeader.text = "Bonus";
            }
            // Otherwise hide UI component
            else
            {
                _bonusText.text = "";
                _bonusHeader.text = "";
            }
        }

        private IEnumerator SpawnPlayer()
        {
            _respawning = true;
            yield return new WaitForSeconds(_waitToRespawn);

            _playerObject.transform.position = _playerSpawner.transform.position;
            _playerObject.transform.rotation = _playerSpawner.transform.rotation;
            _playerObject.SetActive(true);
            _player.BecomeInvincible();
            _respawning = false;
        }

        public void LivesLost()
        {
            if(GameManager.Instance.CurrentLives <= 0)
            {
                GameStateController.PerformTransition(GameStateType.GameOver);
            }
            else
            {
                StartCoroutine(SpawnPlayer());
            }
        }

        public void EnemyDestroyed()
		{
            _scoreText.text = "" + GameManager.Instance.CurrentScore;

			_enemiesKilled++;

			if(_enemiesKilled >= _maxEnemyUnitsToSpawn)
			{
				if(_isLastLevel)
				{
					GameManager.Instance.PlayerWins = true;
				}

				if( GameStateController.PerformTransition(_nextState) == false)
				{
					Debug.LogError("Could not change state to " + _nextState);
				}
			}
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

        public PowerupBase GetPowerUp()
        {
            // Attempt powerup spawn
            float powerupSpawn = Random.value;

            if (powerupSpawn > _powerupChance)
            {
                return null;
            }

            GameObject result = null;

            // Randomize powerup type
            float type = Random.value;

            if(type < 0.5f)
            {
                result = _healthPowerupPool.GetPooledObject();
            } 
            else
            {
                result = _weaponPowerupPool.GetPooledObject();
            }

            if (result != null)
            {
                PowerupBase powerup = result.GetComponent<PowerupBase>();
                if (powerup == null)
                {
                    Debug.LogError("Powerup component could not be found from the object fetched from the pool.");
                }

                return powerup;
            }

            return null;
        }

        // Return powerup to a pool based on its Type
        public bool ReturnPowerUp(PowerupBase powerup)
        {
            if (powerup.PowerType == PowerupBase.Type.Health)
            {
                return _healthPowerupPool.ReturnObject(powerup.gameObject);
            }
            else
            {
                return _weaponPowerupPool.ReturnObject(powerup.gameObject);
            }
        }
    }
}
