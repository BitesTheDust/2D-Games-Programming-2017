using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Killbox : MonoBehaviour
    {
        private BoxCollider2D _collider;

        void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            GameObject otherParent = other.gameObject.transform.parent.gameObject;

            if(other.gameObject.CompareTag("Projectile"))
            {
                Destroy(otherParent);
            }

        }
    }
}

