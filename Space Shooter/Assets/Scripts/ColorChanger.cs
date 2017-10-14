using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    protected SpriteRenderer sr;

    public Color[] availableColors;
    protected Color newSpriteColor;

    protected float r;
    protected float g;
    protected float b;

    public const string colorButtonName = "Color";

    private void Awake() {

        Debug.Log("Awake");

        if(availableColors.Length == 0)
        {
            Debug.LogError("No colors!");
        }
    }

    private void OnEnable() {

        Debug.Log("OnEnable");
    }

    // Use this for initialization
    private void Start () {

        Debug.Log("Start");
        sr = GetComponent<SpriteRenderer>();
       
	}

    // Update is called once per frame
    // LateUpdate is called once per frame, after Update()
    // FixedUpdate is called every physics step (about 50 times per second, can be changed)
    private void Update () {

        Debug.Log("Update");

        if(Input.GetButton(colorButtonName))
        {
            r = Random.Range(0f, 1f);
            g = Random.Range(0f, 1f);
            b = Random.Range(0f, 1f);

            sr.color = new Color(r, g, b, 1);
        }
    }

    private void OnDestroy () {

        Debug.Log("OnDestroy");
    }
}
