using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ScenesRander : MonoBehaviour
{
    [SerializeField] private Camera cam; // 主摄像机
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles; // 用于存储不同的Tile
    [SerializeField] private Transform player; //玩家坐标
    [SerializeField] private float tileSize = 1f; // Tile的大小

    [SerializeField] private GameObject treePrefab;

    // 地图生成范围大小
    [SerializeField] private int buffer_tiles;

    private int screen_width_in_tiles;
    private int screen_height_in_tiles;


    void Start()
    {
        screen_width_in_tiles = Mathf.CeilToInt(cam.orthographicSize * 2 / tileSize)+buffer_tiles;
        screen_height_in_tiles = Mathf.CeilToInt(cam.orthographicSize * cam.aspect)+buffer_tiles;

        // 初始化地图
        GenerateInitialMap();

    }

    void GenerateInitialMap()
    {
        // 在玩家移动范围内生成初始地图
        UpdateMapAroundPlayer();
    }

    void UpdateMapAroundPlayer()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            int playerTileX = Mathf.FloorToInt(playerPosition.x / tileSize);
            int playerTileY = Mathf.FloorToInt(playerPosition.y / tileSize);

            // 填充Tile
            for (int x = -screen_width_in_tiles / 2; x < screen_width_in_tiles / 2; x++)
            {
                for (int y = -screen_height_in_tiles / 2; y < screen_height_in_tiles / 2; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(playerTileX + x, playerTileY + y, 0);
                    if (!tilemap.HasTile(tilePosition))
                    {
                        TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                        tilemap.SetTile(tilePosition, randomTile);

                        // 生成要跟着地图随机生成的怪物（Tree）
                        if (ShouldSpawnMonster(tilePosition))
                        {
                            SpawnMonster(tilePosition);
                        }

                    }
                }
            }
        }
    }


    void Update()
    {
        // 每帧更新玩家周围的地图
        UpdateMapAroundPlayer();

    }


    bool ShouldSpawnMonster(Vector3Int tilePosition)
    {
        // 生成概率
        float spawnChance = 0.002f; // 0.2%生成概率，因为每一片Tile都要执行一次，然后一片Tile又很小
        return Random.value < spawnChance;
    }

    void SpawnMonster(Vector3Int tilePosition)
    {
        Vector3 worldPosition = tilemap.CellToWorld(tilePosition);
        Instantiate(treePrefab, worldPosition, Quaternion.identity);
    }
}
