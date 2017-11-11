using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PowerupBase : MonoBehaviour
    {
        // Type of powerup for discerning between prefabs
        public enum Type
        {
            Health,
            Weapon
        }

        [SerializeField]
        private Type _powerupType;

        public Type PowerType
        {
            get { return _powerupType; }
        }

        // Determinees how long should the poewrup stay once activated
        [SerializeField, Tooltip("Time for powerup to stay visible before disappearing.")]
        private float _lifeTime;

        private float _timeAlive;

        [SerializeField, Tooltip("Powerup effects on the player.")]
        private PowerUpProperties _properties;

        public PowerUpProperties GetPowerUp()
        {
            return _properties;
        }

        protected virtual void Awake()
        {
            _timeAlive = 0;
        }

        protected void FixedUpdate()
        {
            _timeAlive += Time.fixedDeltaTime;

            // If powerup was not picked up in time, send back to pool
            if(_timeAlive >= _lifeTime)
            {
                if (!DisposePowerup())
                {
                    Debug.LogError("Could not return the projectile back to the pool!");
                    Destroy(gameObject);
                }
            }
        }

        // Sends powerup properties to colliding owner of a IPowerupReceiver
        protected void OnTriggerEnter2D(Collider2D other)
        {
            IPowerupReceiver powerupReceiver = other.GetComponent<IPowerupReceiver>();
            if (powerupReceiver != null)
            {
                Debug.Log("Hit a powerup receiver.");
                powerupReceiver.TakePowerup(GetPowerUp());
            }

            if (!DisposePowerup())
            {
                Debug.LogError("Could not return the projectile back to the pool!");
                Destroy(gameObject);
            }
        }

        // Returns itself to a pool upon expiration
        protected bool DisposePowerup()
        {
            return LevelController.Current.ReturnPowerUp(this);
        }

        // Contains effects to boost the player ship with
        [Serializable]
        public class PowerUpProperties
        {
            // Restores health
            [SerializeField]
            private int _healthToProvide = 0;

            public int HealthToProvide
            {
                get { return _healthToProvide; }
            }

            // Adds to bonus weapon usage timer
            [SerializeField]
            private float _bonusWeaponTime = 0;

            public float BonusWeaponTime
            {
                get { return _bonusWeaponTime; }
            }
        }
    }
}
