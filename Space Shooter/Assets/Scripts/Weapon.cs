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
        private SpaceShipBase _owner;

        public void Init(SpaceShipBase owner)
        {
            _owner = owner;
        }

        public bool Shoot()
        {
            if(_isInCooldown)
            {
                return false;
            }
            //Instantiate projectile object and Launch() it
            //Projectile projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            //projectile.Launch(transform.up);

            //Get projectile from pool and launch it.
            Projectile projectile = LevelController.Current.GetProjectile(_owner.UnitType);
            if(projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = transform.rotation;
                projectile.Launch(this, transform.up);
            }

            //Projectile is shot so weapon goes on cooldown
            _isInCooldown = true;
            //We just shot a projectile so time is right now
            _timeSinceShot = 0;

            return true;
        }

        public bool DisposeProjectile(Projectile projectile)
        {
            return LevelController.Current.ReturnProjectile(_owner.UnitType, projectile);
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
