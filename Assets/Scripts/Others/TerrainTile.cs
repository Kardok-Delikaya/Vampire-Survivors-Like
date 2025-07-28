using UnityEngine;

namespace VSLike
{
    public class TerrainTile : MonoBehaviour
    {
        ChestSpawner chestSpawner;
        [SerializeField] Vector2Int tilePos;

        void Start()
        {
            chestSpawner = GetComponentInChildren<ChestSpawner>();

            GetComponentInParent<WorldScrolling>().Add(gameObject, tilePos);

            transform.position = new Vector3(-100, -100, 0);
        }

        public void Spawn()
        {
            chestSpawner.Spawn();
        }
    }
}