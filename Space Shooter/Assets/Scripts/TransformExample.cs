using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformExample : MonoBehaviour {

    public float speed = 8;
    public float minSpeed = 8;
    public float maxSpeed = 20;

    //public float

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            speed += 0.5f;
        } else
        {
            speed -= 1f;
        }
        //transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
