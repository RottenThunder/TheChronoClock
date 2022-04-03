using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IP
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 moveInput;
        private Vector2 mousePos;

        public Transform firePoint;
        public float dartSpeed;

        private Rigidbody2D rb;
        private Vector3 transformedPos;

        [SerializeField]
        private float speed;
        [SerializeField]
        private GameObject dart;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 LookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90.0f;
            rb.rotation = angle;

            transformedPos = Vector3.zero;
            transformedPos.x = moveInput.x;
            transformedPos.y = moveInput.y;

            if (transformedPos != Vector3.zero)
            {
                rb.MovePosition(transform.position + transformedPos * speed * Time.deltaTime);
            }
        }

        #region Input

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameObject newDart = Instantiate(dart, firePoint.position, firePoint.rotation);
                newDart.GetComponent<Rigidbody2D>().AddForce(firePoint.up * dartSpeed, ForceMode2D.Impulse);
            }
        }

        public void MousePosTracking(InputAction.CallbackContext context)
        {
            mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        }

        #endregion
    }
}