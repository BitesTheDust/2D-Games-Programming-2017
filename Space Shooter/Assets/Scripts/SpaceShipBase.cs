using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public abstract class SpaceShipBase : MonoBehaviour
    {
        protected abstract void Move();

        protected virtual void Update()
        {
            try
            {
                Move();
            }
            catch(System.NotImplementedException exception)
            {
                Debug.Log(exception.Message);
            }
            catch(System.Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }

}

