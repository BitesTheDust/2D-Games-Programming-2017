using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private float _cooldownTime = 0.5f;
        [SerializeField]
        private Projectile _projectilePrefab;

        private float _timeSinceShot = 0;
        private bool _isInCooldown = false;

        public bool Shoot()
        {
            if(_isInCooldown)
            {
                return false;
            }
            //Instantiate projectile object and Launch() it
            Projectile projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            projectile.Launch(transform.up);

            //Projectile is shot so weapon goes on cooldown
            _isInCooldown = true;
            //We just shot a projectile so time is right now
            _timeSinceShot = 0;

            return true;
        }

        // Update is called once per frame
        void Update()
        {
            if(_isInCooldown)
            {
                _timeSinceShot += Time.deltaTime;
                if(_timeSinceShot >= _cooldownTime)
                {
                    _isInCooldown = false;
                }
            }
        }
    }

}
