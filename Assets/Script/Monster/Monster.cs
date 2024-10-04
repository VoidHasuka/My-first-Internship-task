using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [SerializeField] protected Transform player; //�������
    [SerializeField] protected Transform monster;
    protected float dir_x;
    protected float dir_y;
    protected float player_around;
    [SerializeField] protected float move_speed;

    public GameObject ExpPrefab; //����ʵ��Ԥ����
    private int ExpSpawn; //����������ɴ�����������
    private int ExpNowSpawn;

    protected bool is_dead;

    [SerializeField] AudioDefination deadAudio;//������Ч

    protected override void Start()
    {
        base.Start();
        ExpSpawn = UnityEngine.Random.Range(0, 3);
        ExpNowSpawn = 0;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; //�������λ��
        }    
    }

    protected override void Update()
    {
        if ((player != null))
        {
            base.Update();
        }
    }

    
    protected void CheckPlayerPos()
    {
        dir_x = player.position.x - monster.position.x;
        dir_y = player.position.y - monster.position.y;

        player_around = Mathf.Sqrt(dir_x * dir_x + dir_y * dir_y);
    }

    protected override void FlipController()
    {
        //����˵�г鴤����
        if (Mathf.Abs(dir_y) > 1 && Mathf.Abs(dir_x) > 1)
        {
            base.FlipController();
        }
    }

    public void MonsterHurt()
    {
        anim.SetTrigger("be_attacked");
    }

    public void MonsterDead()
    {
        if (!is_dead)
        {
            rb.velocity = Vector2.zero;
            is_dead = true;
           while (ExpNowSpawn < ExpSpawn)
            {
                Instantiate(ExpPrefab, transform.position, Quaternion.identity);
                ExpNowSpawn++;
            }
            deadAudio.PlayAudioClip();
        }
    }
}
