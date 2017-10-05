using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class PlayerSpaceShip : SpaceShipBase
    {
        public Weapon playerWeapon;

        public float speed = 8;
        public float minSpeed = 8;
        public float maxSpeed = 20;

        public const string horizontalAxis = "Horizontal";
        public const string verticalAxis = "Vertical";
        public const string fireButtonName = "Fire1";
        public const string adjustHPAxis = "AdjustHP";

        public override Type UnitType
        {
            get { return Type.Player; }
        }

        public Text speedText;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            speedText.text = "SPEED : " + speed;

            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            if(Input.GetButton(fireButtonName))
            {
                Shoot();
            }

            //float adjustHpInput = Input.GetAxis(adjustHPAxis);

            //if (adjustHpInput > 0.0f)
            //{
            //    playerHP.IncreaseHealth(1);
            //}
            //else if (adjustHpInput < 0.0f)
            //{
            //    playerHP.DecreaseHealth(1);
            //}
        }

        private Vector3 GetInputVector()
        {
            //Vector3 movementVector = Vector3.zero;

            float horizontalInput = Input.GetAxis(horizontalAxis);
            float verticalInput = Input.GetAxis(verticalAxis);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                speed += 0.5f;
            }
            else
            {
                speed -= 1f;
            }
            //if(Input.GetKey(KeyCode.LeftArrow))
            //{
            //    movementVector += Vector3.left;
            //}

            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    movementVector += Vector3.right;
            //}

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    movementVector += Vector3.up;
            //}

            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    movementVector += Vector3.down;
            //}

            return new Vector3(horizontalInput, verticalInput);
        }

        protected override void Move()
        {
            Vector3 movementVector = GetInputVector();

            transform.Translate(movementVector * speed * Time.deltaTime);
        }
    }

}
