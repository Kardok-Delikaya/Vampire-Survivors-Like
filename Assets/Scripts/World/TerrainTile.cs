using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    private ChestSpawner chestSpawner;
    [SerializeField] private Vector2Int tilePos;

    private void Start()
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