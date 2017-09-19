using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class EnemySpaceShip : SpaceShipBase
    {
        [SerializeField]
        private float _reachDistance = 0.5f;

        private GameObject[] _movementTargets;
        private int _currentMoveTarget = 0;

        public Transform CurrentMoveTarget
        {
            get
            {
                return _movementTargets [_currentMoveTarget].transform;
            }
        }

        public float speed = 8;
        public float minSpeed = 8;
        public float maxSpeed = 20;

        protected override void Move()
        {
            if(_movementTargets == null || _movementTargets.Length == 0)
            {
                return;
            }

            UpdateMoveTarget();
            Vector3 direction = (CurrentMoveTarget.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        }

        public void SetMovementTargets(GameObject[] movementTargets)
        {
            _movementTargets = movementTargets;
            _currentMoveTarget = 0;
        }

        private void UpdateMoveTarget()
        {
            if(Vector3.Distance(transform.position, CurrentMoveTarget.position) < _reachDistance)
            {
                if (_currentMoveTarget >= _movementTargets.Length - 1)
                    _currentMoveTarget = 0;
                else
                    _currentMoveTarget++;
            }
        }
    }
}
