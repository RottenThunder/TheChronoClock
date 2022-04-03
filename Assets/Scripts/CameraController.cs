using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float smoothing;

        private void LateUpdate()
        {
            if (transform.position != target.position)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
            }
        }
    }
}