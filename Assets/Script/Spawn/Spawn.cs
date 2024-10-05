using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //�����������
    [SerializeField] protected GameObject enemyPrefab; // ����Ԥ����
    
    [SerializeField] protected int enemyCount = 5; // ÿ�����ɵĵ�����Ŀ
    static private int MonsterPollNum = 25; //�ڶ�����еĹ���������Ŀ

    protected Transform playerTransform; // ���λ��
    [SerializeField] protected float minDistance = 20f; // ��С���ɾ���
    [SerializeField] protected float maxDistance = 50f; // ������ɾ���

    protected float SpawnCoolDown; //������ȴ
    [SerializeField] protected float SpawnCoolTime = 5; //������ȴʱ��

    public Character monsterChar;

    protected virtual void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //�������λ��
        }
        ObjectPoolManager.SetPoolSize(enemyPrefab, MonsterPollNum); //����س�ʼ��
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
            // ����Ƕ�
            float angle = Random.Range(0f, 360f);
            // �������
            float distance = Random.Range(minDistance, maxDistance);

            // ��������λ��
            Vector3 spawnPosition = playerTransform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);

            // ���ɵ���
            ObjectPoolManager.GetObject(enemyPrefab, spawnPosition, Quaternion.identity);

        }
    }









}
