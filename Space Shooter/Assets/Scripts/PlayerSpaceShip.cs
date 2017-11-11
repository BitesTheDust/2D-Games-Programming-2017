using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class PlayerSpaceShip : SpaceShipBase, IPowerupReceiver
    {
        //private CapsuleCollider2D _collider;
        private SpriteRenderer _renderer;

        // Normal opacity player sprite
        [SerializeField, Tooltip("Normal color for the player sprite.")]
        private Color _normalColor;

        // Blinking respawn colors
        [SerializeField, Tooltip("Colors to cycle through after respawning.")]
        private Color[] _respawnColors;

        [SerializeField, Tooltip("Amount of time a single invincibility blink lasts.")]
        private float _blinkTime;

        [SerializeField, Tooltip("Amount of time immortality lasts.")]
        private float _immortalTime;

        // Are the bonus weapons active
        private bool _bonusWeapons;

        // Amount of time bonus weapons are active
        private float _bonusTime;

        // LevelController can access _bonusTime
        public float BonusTime
        {
            get { return _bonusTime; }
        }

        // How often should bonustime be decreased
        [SerializeField, Tooltip("Time for each tick on Bonus counter.")]
        private float _bonusDecreaseInterval = 1.0f;

        [SerializeField]
        private float localSpeed;
        [SerializeField]
        private float minSpeed;
        [SerializeField]
        private float maxSpeed;

        public const string horizontalAxis = "Horizontal";
        public const string verticalAxis = "Vertical";
        public const string fireButtonName = "Fire1";

        public override Type UnitType
        {
            get { return Type.Player; }
        }

        // Use this for initialization
        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void Awake()
        {
            base.Awake();

            // Set bonus values off
            _bonusTime = 0;
            _bonusWeapons = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            localSpeed = Mathf.Clamp(localSpeed, minSpeed, maxSpeed);

            if(Input.GetButton(fireButtonName))
            {
                Shoot();
            }
        }

        protected void LateUpdate()
        {
            // If bonus weapons are active and time has run out
            if(_bonusTime <= 0 && _bonusWeapons)
            {
                // Set bonus off and swap back to normal weapon
                _bonusWeapons = false;
                SwapWeapons();
            }
        }

        private Vector3 GetInputVector()
        {
            float horizontalInput = Input.GetAxis(horizontalAxis);
            float verticalInput = Input.GetAxis(verticalAxis);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                localSpeed += 0.5f;
            }
            else
            {
                localSpeed -= 1f;
            }

            return new Vector3(horizontalInput, verticalInput);
        }

        protected override void Move()
        {
            Vector3 movementVector = GetInputVector();

            transform.Translate(movementVector * (Speed * localSpeed) * Time.deltaTime);
        }

        protected override void Die()
        {
            base.Die();
            // Decrease player lives by one
            GameManager.Instance.CurrentLives--;
        }

        public void BecomeInvincible(float time = 0.0f)
        {
            if(time <= _immortalTime)
            {
                StartCoroutine(Invincibility(_immortalTime));
            }
            else
            {
                StartCoroutine(Invincibility(time));
            }
        }

        // Gives blink effect and disables hitbox for a while after respawn
        private IEnumerator Invincibility(float time)
        {
            float timer = 0.0f;

            Health.SetImmortal(true);

            while(timer < time)
            {
                for (int a = 0; a < _respawnColors.Length; a++)
                {
                    timer += _blinkTime;
                    //color.a = color.a == 1 ? 0 : 1;
                    _renderer.color = _respawnColors[a];
                    yield return new WaitForSeconds(_blinkTime);
                }
            }

            // Resets normal sprite renderer & collider states
            Health.SetImmortal(false);
            _renderer.color = _normalColor;
        }

        // Counts time down on bonus weapon timer for as long as they are active
        private IEnumerator BonusTimer()
        {
            while(_bonusWeapons)
            {
                yield return new WaitForSeconds(_bonusDecreaseInterval);

                _bonusTime--;
            }
        }

        // Adds more time to bonus counter
        public void AddBonusTime(float time)
        {
            // If bonus time was 0, turn on bonus weapons
            if (_bonusTime <= 0)
            {
                _bonusWeapons = true;
                SwapWeapons();
                StartCoroutine(BonusTimer());
            }

            // Add time to counter
            _bonusTime += time;
        }

        // Swap between normal and bonus weapons
        private void SwapWeapons()
        {
            foreach(Weapon weapon in Weapons)
            {
                if(weapon.enabled)
                {
                    weapon.enabled = false;
                }
                else
                {
                    weapon.enabled = true;
                }
            }
        }

        // Receives powerup
        public void TakePowerup(PowerupBase.PowerUpProperties pUProperties)
        {
            // Receive health powerup value
            if(pUProperties.HealthToProvide > 0)
            {
                Health.IncreaseHealth(pUProperties.HealthToProvide);
            }

            // Receive bonus weapon time
            if(pUProperties.BonusWeaponTime > 0)
            {
                AddBonusTime(pUProperties.BonusWeaponTime);
            }
        }
    }

}
