using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerSpaceShip : MonoBehaviour
    {
        public float speed = 1;

        public GUIText speedText;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 movementVector = GetMovementVector();

            speedText.text = "SPEED : " + speed;

            transform.Translate(movementVector * speed * Time.deltaTime);
        }

        private Vector3 GetMovementVector()
        {
            Vector3 movementVector = Vector3.zero;

            if(Input.GetKey(KeyCode.LeftArrow))
            {
                movementVector += Vector3.left;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                movementVector += Vector3.right;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                movementVector += Vector3.up;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                movementVector += Vector3.down;
            }

            return movementVector;
        }
    }

}
