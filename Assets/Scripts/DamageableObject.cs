using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class DamageableObject : MonoBehaviour, IDamage
    {
        Transform player;
        [SerializeField] GameObject can;
        [SerializeField] GameObject altin;
        private void Start()
        {
            player = FindObjectOfType<Player>().transform;
        }

        void FixedUpdate()
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > 50)
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(int Hasar, int güç)
        {
            int randomNumber;
            randomNumber = Random.Range(0, 100);
            if (randomNumber < 30)
            {
                Instantiate(can, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(altin, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}