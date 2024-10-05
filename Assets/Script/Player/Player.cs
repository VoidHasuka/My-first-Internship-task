using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Events;
using System;
using TMPro;

public class Player : Entity
{

    [Header("Move info")]
    [SerializeField] public float move_speed;
    private float x_input;
    private float y_input;

    public bool is_hurt = false;
    [SerializeField] private float hurt_force; //反弹力度

    public bool is_dead = false;
    public bool is_attacking = false;

    [Header("Weapon info")]
    [SerializeField] private Transform cursor; //靶心
    [SerializeField] private Animator ReplaceBullet; //技能动画
    [SerializeField] private Animator ReplaceBar; //换弹进度条
    [SerializeField] private Animator ReplaceKey; //换弹节点
    [SerializeField] private GameObject PlayerGun; //相当来说升级增加枪的数量也是很神奇的想法
    [SerializeField] private float addChance = 0.3f;
    private float gunAngle = 0.0f;

    [Header("Exp info")]
    //升级要调用的东西是真的多
    public float MaxExp;
    public float CurrentExp;
    public UnityEvent<Player> OnChangeExp;
    public int Level;
    [SerializeField] private TextMeshProUGUI ExpText; // 等级显示
    [SerializeField] private Player_Heart playerHeart;//HeartUI调用
    [SerializeField] private Character playerChar; //玩家的Character
    static public int bulletDamage;
    [SerializeField] private Animator LevelUp; //升级特效

    [Header("Sound info")]
    //受伤特效
    [SerializeField] private GameObject blood;
    //受伤音效
    [SerializeField] private AudioDefination bloodAudio;
    //死亡音效
    [SerializeField] private AudioDefination deadAudio;
    [SerializeField] private AudioSource BGMSource;

    protected override void Start()
    {
        base.Start();
        bulletDamage = 10;
    }

    protected override void Update()
    {
        base.Update();
        
        if (!is_dead)
        {
            CheckInput();
        }

        if (!is_hurt)
        {
            Movement();
        }
        

        //反转
        FlipController();

        //动画更新
        AnimationControllers();
    }

    private void Movement()
    {
        if (x_input != 0 && y_input != 0)
        {
            rb.velocity = new Vector2(x_input * move_speed / sqrt, y_input * move_speed / sqrt);
        }
        else rb.velocity = new Vector2(x_input * move_speed, y_input * move_speed);
    }

    private void CheckInput()
    {
        x_input = UnityEngine.Input.GetAxisRaw("Horizontal");
        y_input = UnityEngine.Input.GetAxisRaw("Vertical");
        if (UnityEngine.Input.GetMouseButton(0))
        {
            move_speed = 4;
            is_attacking = true;
        }
        else
        {
            move_speed = 8;
            is_attacking= false;
        }
    }

    private void AnimationControllers()
    {
        bool is_moving = (rb.velocity.x != 0||rb.velocity.y!=0);
        anim.SetBool("is_moving", is_moving);
        anim.SetBool("is_dead", is_dead);
        anim.SetBool("is_attacking", is_attacking);
    }


    //玩家受伤
    public void PlayerHurt()
    {
        anim.SetTrigger("hurt");
        //调用镜头抖动
        GameController.camShake.Shake();
        //音效
        bloodAudio.PlayAudioClip();
    }

    //换弹特效
    public void PlayerRing()
    {    
        ReplaceBullet.SetTrigger("Replace_bullet");
    }

    //换弹进度条
    public void PlayerReplaceBar()
    {
        ReplaceBar.SetTrigger("Replace_bullet");
        ReplaceKey.SetTrigger("Replace_bullet");
    }

    public void GetHurt(Transform attacker)
    {
        //受伤之后玩家执行的逻辑

        is_hurt= true;
        rb.velocity = Vector2.zero;

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, transform.position.y - attacker.position.y).normalized;

        rb.AddForce(dir*hurt_force,ForceMode2D.Impulse);

        Instantiate(blood,transform.position,Quaternion.identity);
    }

    public void PlayerDead()
    {
        //避免反复去世
        if (!is_dead)
        {
            is_dead = true;
            x_input = 0;
            y_input = 0;
            rb.velocity = new Vector2(0, 0);
            
            //关停BGM的循环功能
            BGMSource.loop = false;
            deadAudio.PlayAudioClip();
            
            Destroy(gameObject);
        }
    }

    //玩家角色转向仅与Cursor（靶心）有关
    protected override void FlipController()
    {
        float CursorToPlayer = cursor.position.x - transform.position.x;
        if (CursorToPlayer > 0 && !facing_right)
        {
            Flip();
        }
        else if (CursorToPlayer < 0 && facing_right)
        {
            Flip();
        }
    }


    public void TakeExp(ExpBody exp)
    {
        if (exp.expAmount + CurrentExp >= MaxExp)
        {
            //升级之后执行的逻辑
            CurrentExp = exp.expAmount + CurrentExp - MaxExp;
            MaxExp += 10;
            Level++;
            LevelUp.SetTrigger("LevelUp");


            //加枪！
            if (UnityEngine.Random.Range(0f, 1f) < addChance)
            {
                AddWeapon();
                addChance -= 0.05f;
            }

            //满血升级则增加血量上限
            if(playerChar.current_health == playerChar.max_health)
            {
                playerChar.current_health++;
                playerChar.max_health++;
                //添加新的HeartUI
                playerHeart.AddHeart();
                playerHeart.OnHealthChange(playerChar.current_health);
            }
            else
            {
                playerChar.current_health++;
                playerHeart.OnHealthChange(playerChar.current_health);
            }


            //随机增加1到2点攻击力
            bulletDamage += UnityEngine.Random.Range(1, 3);
        }
        else
        {
            CurrentExp += exp.expAmount;
        }
        exp.DestroyItself();
        UpdateExpText(); // 更新经验值显示
        OnChangeExp?.Invoke(this);
    }

    private void UpdateExpText()
    {
        ExpText.text = "Level     " + Level.ToString();
    }

    private void AddWeapon()
    {
        gunAngle += (52.0f/180)*3.1415f;
        GameObject newGun = Instantiate(PlayerGun, transform.position, transform.rotation);
        newGun.transform.SetParent(transform);
        newGun.GetComponent<Player_Arrow>().anglePos = gunAngle;
    }

}
