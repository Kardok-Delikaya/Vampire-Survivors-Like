using UnityEngine;

namespace VSLike
{
    public class ChestSpawner : MonoBehaviour
    {
        [SerializeField] GameObject chest;
        float possibility = .3f;

        public void Spawn()
        {
            if (Random.value < possibility)
            {
                Instantiate(chest, transform.position, Quaternion.identity);
            }
        }
    }
}