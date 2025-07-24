using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class DamagePopUp : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 1f);
        }

        void FixedUpdate()
        {
            transform.localPosition += new Vector3(0, 0.05f, 0);
            transform.localScale += new Vector3(0.02f, 0.02f, 0);
        }
    }
}