using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_bullet : MonoBehaviour
{
    [SerializeField] private Vector3 move_dir;
    [SerializeField] private float move_speed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform monster;
    [SerializeField] private GameObject bulletPrefab; //�ӵ�Ԥ����
    public Transform firePoint; // �ӵ��ķ���λ�ã��󶨵�������
    [SerializeField] private float fireCoolTime;
    private float fireCoolCounter;
    private float player_around;

    [SerializeField] AudioDefination audioShoot;//�ӵ������Ч

    private void Awake()
    {
    }
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; //�������λ��
        }
    }

    void Update()
    {
        if(player != null)
        {
            CheckPlayerPos();
            if (player_around<=5&&fireCoolCounter<0)
            {
                fireCoolCounter = fireCoolTime;
                Shoot();
            }
        }
        fireCoolCounter-=Time.deltaTime;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        audioShoot.PlayAudioClip();
    }

    protected void CheckPlayerPos()
    {
        float dir_x = player.position.x - monster.position.x;
        float dir_y = player.position.y - monster.position.y;

        player_around = Mathf.Sqrt(dir_x * dir_x + dir_y * dir_y);
    }
}
