using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(IHealth))]
    public abstract class SpaceShipBase : MonoBehaviour, IDamageReceiver
    {
        public enum Type
        {
            Player,
            Enemy
        }

        [SerializeField]
        private float _speed = 1.5f;

        private Weapon[] _weapons;

        public float Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        public Weapon[] Weapons
        {
            get { return _weapons; }
        }

        public IHealth Health { get; protected set; }

        public abstract Type UnitType { get; }

        protected virtual void Awake()
        {
            _weapons = GetComponentsInChildren<Weapon>(includeInactive: true);
            foreach(Weapon weapon in _weapons)
            {
                weapon.Init(this);
            }
            Health = GetComponent<IHealth>();
        }

        protected void Shoot()
        {
            foreach(Weapon weapon in Weapons)
            {
                weapon.Shoot();
            }
        }

        protected abstract void Move();

        protected virtual void Update()
        {
            try
            {
                Move();
            }
            catch (System.NotImplementedException exception)
            {
                Debug.Log(exception.Message);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public void TakeDamage(int amount)
        {
            Health.DecreaseHealth(amount);

            if (Health.IsDead)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        protected Projectile GetPooledProjectile()
        {
            return LevelController.Current.GetProjectile(UnitType);
        }

        protected bool ReturnPooledProjectile(Projectile projectile)
        {
            return LevelController.Current.ReturnProjectile(UnitType, projectile);
        }
    }

}

