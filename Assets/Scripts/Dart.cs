using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP
{
    public class Dart : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.gameObject.GetComponent<Enemy>().health -= 3;
                Destroy(gameObject);
            }
            if (collision.collider.CompareTag("Untagged"))
            {
                Destroy(gameObject);
            }
        }
    }
}