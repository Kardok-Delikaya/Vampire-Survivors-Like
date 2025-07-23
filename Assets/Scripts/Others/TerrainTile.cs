using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class TerrainTile : MonoBehaviour
    {
        [SerializeField] Vector2Int tilePos;
        [SerializeField] List<ObjSpawn> spawnPoints;
        void Start()
        {
            GetComponentInParent<WorldScrolling>().Add(gameObject, tilePos);

            transform.position = new Vector3(-100, -100, 0);
        }

        public void Spawn()
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                spawnPoints[i].Spawn();
            }
        }
    }
}