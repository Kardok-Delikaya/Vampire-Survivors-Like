using UnityEngine;

namespace Others
{
    public class WorldScrolling : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private Vector2Int currentTilePosition = new Vector2Int(0, 0);
        [SerializeField] private Vector2Int playerTilePosition;
        private Vector2Int onTileGridPosition;
        private GameObject[,] terrainTiles;
        [SerializeField] private int terrainTileHorizontalCount;
        [SerializeField] private int terrainTileVerticalCount;
        private int fieldOfVisionHeight = 3;
        private int fieldOfVisionWidth = 3;

        private void Awake()
        {
            terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
        }

        private void Start()
        {
            UpdateTilesOnScreen();
        }

        private void Update()
        {
            playerTilePosition.x = (int)(playerTransform.position.x / 20);
            playerTilePosition.y = (int)(playerTransform.position.y / 20);

            playerTilePosition.x -= playerTransform.position.x < 0 ? 1 : 0;
            playerTilePosition.y -= playerTransform.position.y < 0 ? 1 : 0;

            if (currentTilePosition != playerTilePosition)
            {
                currentTilePosition = playerTilePosition;

                onTileGridPosition.x = CalculatePositionOnAxisWithWrap(onTileGridPosition.x, true);
                onTileGridPosition.y = CalculatePositionOnAxisWithWrap(onTileGridPosition.y, false);
                UpdateTilesOnScreen();
            }
        }

        private void UpdateTilesOnScreen()
        {
            for (var pov_x = -fieldOfVisionWidth / 2; pov_x <= fieldOfVisionWidth / 2; pov_x++)
            {
                for (var pov_y = -fieldOfVisionHeight / 2; pov_y <= fieldOfVisionHeight / 2; pov_y++)
                {
                    var tileToUpdate_x = CalculatePositionOnAxisWithWrap(playerTilePosition.x + pov_x, true);
                    var tileToUpdate_y = CalculatePositionOnAxisWithWrap(playerTilePosition.y + pov_y, false);

                    var tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];
                    var newPosition = CalculateTilePosition(playerTilePosition.x + pov_x, playerTilePosition.y + pov_y);
                    if (newPosition != tile.transform.position)
                    {
                        tile.transform.position = newPosition;
                        terrainTiles[tileToUpdate_x, tileToUpdate_y].GetComponent<TerrainTile>().Spawn();
                    }
                }
            }
        }
        private Vector3 CalculateTilePosition(int x, int y)
        {
            return new Vector3(x * 20, y * 20, 0f);
        }
        private int CalculatePositionOnAxisWithWrap(float currentValue, bool horizontal)
        {
            if (horizontal)
            {
                if (currentValue >= 0)
                {
                    currentValue = currentValue % terrainTileHorizontalCount;
                }
                else
                {
                    currentValue += 1;
                    currentValue = terrainTileHorizontalCount - 1 + currentValue % terrainTileHorizontalCount;
                }
            }
            else
            {
                if (currentValue >= 0)
                {
                    currentValue = currentValue % terrainTileVerticalCount;
                }
                else
                {
                    currentValue += 1;
                    currentValue = terrainTileVerticalCount - 1 + currentValue % terrainTileVerticalCount;
                }
            }
            return (int)currentValue;
        }

        internal void Add(GameObject tileGameObject, Vector2Int tilePoz)
        {
            terrainTiles[tilePoz.x, tilePoz.y] = tileGameObject;
        }
    }
}