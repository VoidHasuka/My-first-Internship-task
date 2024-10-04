using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ScenesRander : MonoBehaviour
{
    [SerializeField] private Camera cam; // �������
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles; // ���ڴ洢��ͬ��Tile
    [SerializeField] private Transform player; //�������
    [SerializeField] private float tileSize = 1f; // Tile�Ĵ�С

    [SerializeField] private GameObject treePrefab;

    // ��ͼ���ɷ�Χ��С
    [SerializeField] private int buffer_tiles;

    private int screen_width_in_tiles;
    private int screen_height_in_tiles;


    void Start()
    {
        screen_width_in_tiles = Mathf.CeilToInt(cam.orthographicSize * 2 / tileSize)+buffer_tiles;
        screen_height_in_tiles = Mathf.CeilToInt(cam.orthographicSize * cam.aspect)+buffer_tiles;

        // ��ʼ����ͼ
        GenerateInitialMap();

    }

    void GenerateInitialMap()
    {
        // ������ƶ���Χ�����ɳ�ʼ��ͼ
        UpdateMapAroundPlayer();
    }

    void UpdateMapAroundPlayer()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            int playerTileX = Mathf.FloorToInt(playerPosition.x / tileSize);
            int playerTileY = Mathf.FloorToInt(playerPosition.y / tileSize);

            // ���Tile
            for (int x = -screen_width_in_tiles / 2; x < screen_width_in_tiles / 2; x++)
            {
                for (int y = -screen_height_in_tiles / 2; y < screen_height_in_tiles / 2; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(playerTileX + x, playerTileY + y, 0);
                    if (!tilemap.HasTile(tilePosition))
                    {
                        TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                        tilemap.SetTile(tilePosition, randomTile);

                        // ����Ҫ���ŵ�ͼ������ɵĹ��Tree��
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
        // ÿ֡���������Χ�ĵ�ͼ
        UpdateMapAroundPlayer();

    }


    bool ShouldSpawnMonster(Vector3Int tilePosition)
    {
        // ���ɸ���
        float spawnChance = 0.002f; // 0.2%���ɸ��ʣ���ΪÿһƬTile��Ҫִ��һ�Σ�Ȼ��һƬTile�ֺ�С
        return Random.value < spawnChance;
    }

    void SpawnMonster(Vector3Int tilePosition)
    {
        Vector3 worldPosition = tilemap.CellToWorld(tilePosition);
        Instantiate(treePrefab, worldPosition, Quaternion.identity);
    }
}
