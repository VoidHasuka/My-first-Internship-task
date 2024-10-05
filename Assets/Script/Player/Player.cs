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
    [SerializeField] private float hurt_force; //��������

    public bool is_dead = false;
    public bool is_attacking = false;

    [Header("Weapon info")]
    [SerializeField] private Transform cursor; //����
    [SerializeField] private Animator ReplaceBullet; //���ܶ���
    [SerializeField] private Animator ReplaceBar; //����������
    [SerializeField] private Animator ReplaceKey; //�����ڵ�
    [SerializeField] private GameObject PlayerGun; //�൱��˵��������ǹ������Ҳ�Ǻ�������뷨
    [SerializeField] private float addChance = 0.3f;
    private float gunAngle = 0.0f;

    [Header("Exp info")]
    //����Ҫ���õĶ�������Ķ�
    public float MaxExp;
    public float CurrentExp;
    public UnityEvent<Player> OnChangeExp;
    public int Level;
    [SerializeField] private TextMeshProUGUI ExpText; // �ȼ���ʾ
    [SerializeField] private Player_Heart playerHeart;//HeartUI����
    [SerializeField] private Character playerChar; //��ҵ�Character
    static public int bulletDamage;
    [SerializeField] private Animator LevelUp; //������Ч

    [Header("Sound info")]
    //������Ч
    [SerializeField] private GameObject blood;
    //������Ч
    [SerializeField] private AudioDefination bloodAudio;
    //������Ч
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
        

        //��ת
        FlipController();

        //��������
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


    //�������
    public void PlayerHurt()
    {
        anim.SetTrigger("hurt");
        //���þ�ͷ����
        GameController.camShake.Shake();
        //��Ч
        bloodAudio.PlayAudioClip();
    }

    //������Ч
    public void PlayerRing()
    {    
        ReplaceBullet.SetTrigger("Replace_bullet");
    }

    //����������
    public void PlayerReplaceBar()
    {
        ReplaceBar.SetTrigger("Replace_bullet");
        ReplaceKey.SetTrigger("Replace_bullet");
    }

    public void GetHurt(Transform attacker)
    {
        //����֮�����ִ�е��߼�

        is_hurt= true;
        rb.velocity = Vector2.zero;

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, transform.position.y - attacker.position.y).normalized;

        rb.AddForce(dir*hurt_force,ForceMode2D.Impulse);

        Instantiate(blood,transform.position,Quaternion.identity);
    }

    public void PlayerDead()
    {
        //���ⷴ��ȥ��
        if (!is_dead)
        {
            is_dead = true;
            x_input = 0;
            y_input = 0;
            rb.velocity = new Vector2(0, 0);
            
            //��ͣBGM��ѭ������
            BGMSource.loop = false;
            deadAudio.PlayAudioClip();
            
            Destroy(gameObject);
        }
    }

    //��ҽ�ɫת�����Cursor�����ģ��й�
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
            //����֮��ִ�е��߼�
            CurrentExp = exp.expAmount + CurrentExp - MaxExp;
            MaxExp += 10;
            Level++;
            LevelUp.SetTrigger("LevelUp");


            //��ǹ��
            if (UnityEngine.Random.Range(0f, 1f) < addChance)
            {
                AddWeapon();
                addChance -= 0.05f;
            }

            //��Ѫ����������Ѫ������
            if(playerChar.current_health == playerChar.max_health)
            {
                playerChar.current_health++;
                playerChar.max_health++;
                //����µ�HeartUI
                playerHeart.AddHeart();
                playerHeart.OnHealthChange(playerChar.current_health);
            }
            else
            {
                playerChar.current_health++;
                playerHeart.OnHealthChange(playerChar.current_health);
            }


            //�������1��2�㹥����
            bulletDamage += UnityEngine.Random.Range(1, 3);
        }
        else
        {
            CurrentExp += exp.expAmount;
        }
        exp.DestroyItself();
        UpdateExpText(); // ���¾���ֵ��ʾ
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
