using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            float y = Mathf.Repeat(Time.time * _speed, 1);
            Vector2 offset = new Vector2(0, y);
            _renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }
}
