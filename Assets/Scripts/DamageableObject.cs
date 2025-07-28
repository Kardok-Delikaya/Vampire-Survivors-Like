using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class DamageableObject : MonoBehaviour, IDamage
    {
        Transform player;
        [SerializeField] GameObject health;
        [SerializeField] GameObject gold;

        private void Start()
        {
            player = FindAnyObjectByType<PlayerManager>().transform;
        }

        void FixedUpdate()
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > 50)
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(int damage, int power)
        {
            int randomNumber = Random.Range(0, 100);

            if (randomNumber < 30)
            {
                Instantiate(health, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(gold, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}