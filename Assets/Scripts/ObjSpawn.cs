using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class ObjSpawn : MonoBehaviour
    {
        [SerializeField] GameObject obje;
        float ihtimal = .3f;
        public void Spawn()
        {
            if (Random.value < ihtimal)
            {
                GameObject ob = Instantiate(obje, transform.position, Quaternion.identity);
            }
        }
    }
}