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
        private bool _isImmortal = false;

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
            if(!_isImmortal)
            {
                CurrentHealth -= amount;
            }

        }

        // Heals health value
        public void IncreaseHealth(int amount)
        {
            CurrentHealth += amount;

        }

        public void Restore()
        {
            CurrentHealth = _initHealth;
        }

        public void SetImmortal(bool isImmortal)
        {
            _isImmortal = isImmortal;
        }

        protected void Awake()
        {
            CurrentHealth = _initHealth;
        }
    }
}
