using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IPoolable
{
    [SerializeField] protected Transform player; //玩家坐标
    [SerializeField] protected Transform monster;
    protected float dir_x;
    protected float dir_y;
    protected float player_around;
    [SerializeField] protected float move_speed;

    public GameObject ExpPrefab; //经验实体预制体
    private int ExpSpawn; //随机经验生成次数或者数量
    public int ExpNowSpawn;

    public bool is_dead;

    [SerializeField] AudioDefination deadAudio;//死亡音效

    SpriteRenderer color = null;

    protected override void Start()
    {
        base.Start();
        ExpSpawn = UnityEngine.Random.Range(0, 3);
        ExpNowSpawn = 0;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; //查找玩家位置
        }

        //获取父对象（包括自己）和子对象中的Sprite便于修复从对象池捞出来的时候是红色
        
        SpriteRenderer[] colors = GetComponentsInChildren<SpriteRenderer>(true);

        //遍历
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
        //避免说有抽搐现象
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

            //冻结位置和旋转
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            while (ExpNowSpawn < ExpSpawn)
            {
                Instantiate(ExpPrefab, transform.position, Quaternion.identity);
                ExpNowSpawn++;
            }
            deadAudio.PlayAudioClip();
            
        }
    }

    // 当对象从池中获取时调用，可以理解为新的Start()函数
    public void OnObjectReuse()
    {
        // 重置属性
        gameObject.GetComponent<Monster>().is_dead = false;
        gameObject.GetComponent<Monster>().ExpNowSpawn = 0;
        gameObject.GetComponent<Monster>().facing_right = true;

    }


    // 当对象被回收到池中时调用
    public void OnObjectReturn()
    {
        // 清理状态，增强准备下次使用
        gameObject.GetComponent<Character>().current_health = (gameObject.GetComponent<Character>().max_health+=5);
        //清洗颜色
        color.color= Color.white;
        //解除位置冻结保留旋转冻结
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //某些时候可能会调节图层以对其他怪物造成伤害（就是你，boomer）
        gameObject.layer = LayerMask.NameToLayer("Monster");

    }
}
