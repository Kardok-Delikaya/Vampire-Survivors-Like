using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class WorldScrolling : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;
        Vector2Int currentTilePosition = new Vector2Int(0, 0);
        [SerializeField] Vector2Int playerTilePosition;
        Vector2Int onTileGridPosition;
        GameObject[,] terrainTiles;
        [SerializeField] int terrainTileHorizontalCount;
        [SerializeField] int terrainTileVerticalCount;
        int fieldOfVisionHeight = 3;
        int fieldOfVisionWidth = 3;
        void Awake()
        {
            terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
        }
        void Start()
        {
            UpdateTilesOnScreen();
        }
        void Update()
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
            for (int pov_x = -fieldOfVisionWidth / 2; pov_x <= fieldOfVisionWidth / 2; pov_x++)
            {
                for (int pov_y = -fieldOfVisionHeight / 2; pov_y <= fieldOfVisionHeight / 2; pov_y++)
                {
                    int tileToUpdate_x = CalculatePositionOnAxisWithWrap(playerTilePosition.x + pov_x, true);
                    int tileToUpdate_y = CalculatePositionOnAxisWithWrap(playerTilePosition.y + pov_y, false);

                    GameObject tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];
                    Vector3 newPosition = CalculateTilePosition(playerTilePosition.x + pov_x, playerTilePosition.y + pov_y);
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