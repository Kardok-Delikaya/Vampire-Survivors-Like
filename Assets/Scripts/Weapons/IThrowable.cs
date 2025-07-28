using UnityEngine;

namespace VSLike
{
    public interface IThrowable
    {
        public void Equalize(int damage, int health, float stayTime, float speed, bool hasEvolved, LayerMask damageableLayer);
    }
}