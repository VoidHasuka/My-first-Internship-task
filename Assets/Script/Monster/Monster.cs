using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IPoolable
{
    [SerializeField] protected Transform player; //�������
    [SerializeField] protected Transform monster;
    protected float dir_x;
    protected float dir_y;
    protected float player_around;
    [SerializeField] protected float move_speed;

    public GameObject ExpPrefab; //����ʵ��Ԥ����
    private int ExpSpawn; //����������ɴ�����������
    public int ExpNowSpawn;

    public bool is_dead;

    [SerializeField] AudioDefination deadAudio;//������Ч

    SpriteRenderer color = null;

    protected override void Start()
    {
        base.Start();
        ExpSpawn = UnityEngine.Random.Range(0, 3);
        ExpNowSpawn = 0;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; //�������λ��
        }

        //��ȡ�����󣨰����Լ������Ӷ����е�Sprite�����޸��Ӷ�����̳�����ʱ���Ǻ�ɫ
        
        SpriteRenderer[] colors = GetComponentsInChildren<SpriteRenderer>(true);

        //����
        foreach (SpriteRenderer component in colors)
        {
            if (component.gameObject != gameObject)
            {
                color = component;
                break;
            }
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

            //����λ�ú���ת
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            while (ExpNowSpawn < ExpSpawn)
            {
                Instantiate(ExpPrefab, transform.position, Quaternion.identity);
                ExpNowSpawn++;
            }
            deadAudio.PlayAudioClip();
            
        }
    }

    // ������ӳ��л�ȡʱ���ã��������Ϊ�µ�Start()����
    public void OnObjectReuse()
    {
        // ��������
        gameObject.GetComponent<Monster>().is_dead = false;
        gameObject.GetComponent<Monster>().ExpNowSpawn = 0;
        gameObject.GetComponent<Monster>().facing_right = true;

    }


    // �����󱻻��յ�����ʱ����
    public void OnObjectReturn()
    {
        // ����״̬����ǿ׼���´�ʹ��
        gameObject.GetComponent<Character>().current_health = (gameObject.GetComponent<Character>().max_health+=5);
        //��ϴ��ɫ
        color.color= Color.white;
        //���λ�ö��ᱣ����ת����
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //ĳЩʱ����ܻ����ͼ���Զ�������������˺��������㣬boomer��
        gameObject.layer = LayerMask.NameToLayer("Monster");

    }
}
