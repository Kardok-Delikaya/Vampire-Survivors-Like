﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class LootableObject : MonoBehaviour
    {
        Transform player;

        public int id;
        [HideInInspector] public int count;

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

                if (id == 0)
                {
                    FindAnyObjectByType<PlayerManager>().xpCount--;
                }
            }
        }
    }
}