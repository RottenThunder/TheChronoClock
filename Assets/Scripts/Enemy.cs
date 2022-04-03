using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP
{
    public class Enemy : MonoBehaviour
    {
        public int health;
        public int attackPower;
        public float moveSpeed;

        private Transform target;
        public float chaseRadius;
        public float attackRadius;

        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }

            if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}