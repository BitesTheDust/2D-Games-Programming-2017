using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Health : MonoBehaviour, IHealth
    {
        //Starting health
        [SerializeField]
        private int _initHealth;

        //Health can't go lower than this
        [SerializeField]
        private int _minHealth;

        //Health can't go higher than this
        [SerializeField]
        private int _maxHealth;

        private int _currentHealth;

        // Get current health value
        public int CurrentHealth
        {
            get
            {
                return _currentHealth;
            }
            private set
            {
                _currentHealth = Mathf.Clamp(value, _minHealth, _maxHealth);
            }
        }

        public bool IsDead
        {
            get { return CurrentHealth == _minHealth; }
        }

        // Deals damage to health
        public void DecreaseHealth(int amount)
        {
            CurrentHealth -= amount;

            ////check if object is alive
            //if(_health > _minHealth)
            //{
            //    //object is alive, so deal damage
            //    //makes sure health doesn't go under minimum value
            //    if((_health - amount) < _minHealth)
            //    {
            //        _health = _minHealth;
            //    }
            //    else
            //    {
            //        _health -= amount;
            //    }
            //}
        }

        // Heals health value
        public void IncreaseHealth(int amount)
        {
            CurrentHealth += amount;

            ////proceed with healing only when health is under maximum
            //if (_health < _maxHealth)
            //{
            //    //health can be increased, so heal
            //    //make sure health doesn't go over maximum value
            //    if ((_health + amount) > _maxHealth)
            //    {
            //        _health = _maxHealth;
            //    }
            //    else
            //    {
            //        _health += amount;
            //    }
            //}
        }

        protected void Awake()
        {
            CurrentHealth = _initHealth;
        }
    }
}
