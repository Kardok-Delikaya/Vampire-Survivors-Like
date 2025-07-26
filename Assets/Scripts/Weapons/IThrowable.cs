using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public interface IThrowable
    {
        public void Equalize(int damage, int health, float stayTime, float speed, bool evoltion, LayerMask damageableLayer);
    }
}