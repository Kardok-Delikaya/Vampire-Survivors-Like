using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public interface IDamage
    {
        public void TakeDamage(int damage, int power);
    }
}