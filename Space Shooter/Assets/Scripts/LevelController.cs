using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private Spawner _enemySpawner;

        [SerializeField]
        private GameObject[] _enemyMoveTargets;

        protected void Awake()
        {
            if(_enemySpawner == null)
            {
                Debug.Log("No reference to an enemy spawner.");
                _enemySpawner = GetComponentInChildren<Spawner>();

                //Transform childTransform = transform.Find("EnemySpawner");
            }

            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(3);
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
    }
}
