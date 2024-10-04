using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] protected GameObject enemyPrefab; // 敌人预制体
    
    [SerializeField] protected int enemyCount = 5; // 每次生成的敌人数目

    protected Transform playerTransform; // 玩家位置
    [SerializeField] protected float minDistance = 20f; // 最小生成距离
    [SerializeField] protected float maxDistance = 50f; // 最大生成距离

    protected float SpawnCoolDown; //生成冷却
    [SerializeField] protected float SpawnCoolTime = 5; //生成冷却时间

    protected virtual void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //查找玩家位置
        }
    }

    protected virtual void Update()
    {
        if (SpawnCoolDown < 0 && playerTransform != null)
        {
            SpawnEnemies();
            SpawnCoolDown = SpawnCoolTime;
        }
        SpawnCoolDown -= Time.deltaTime;
    }


    protected virtual void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // 随机角度
            float angle = Random.Range(0f, 360f);
            // 随机距离
            float distance = Random.Range(minDistance, maxDistance);

            // 计算生成位置
            Vector3 spawnPosition = playerTransform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);

            // 生成敌人
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
