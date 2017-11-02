using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class PlayerSpaceShip : SpaceShipBase
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
            //_collider = GetComponent<CapsuleCollider2D>();
            _renderer = GetComponentInChildren<SpriteRenderer>();

            //speedText = FindObjectOfType<Text>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            //speedText.text = "SPEED : " + localSpeed;

            localSpeed = Mathf.Clamp(localSpeed, minSpeed, maxSpeed);

            if(Input.GetButton(fireButtonName))
            {
                Shoot();
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
    }

}
