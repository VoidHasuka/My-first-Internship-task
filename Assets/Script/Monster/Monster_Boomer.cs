using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boomer : Monster
{
    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }

    protected override void Update()
    {
        if (!is_dead && anim != null)
        {
            base.Update();

            if (player!=null)
            {
                CheckPlayerPos();
                UpdateMove();
            }

            if (player_around < 3&&!is_dead)
            {
                MonsterDead();
            }   
        }
        AnimationControllers();
    }

    protected void UpdateMove()
    {
        // ȷ�� dir_x �� dir_y ��Ϊ 0
        float now_dir_x = (dir_x != 0) ? dir_x / Mathf.Abs(dir_x) : 0;
        float now_dir_y = (dir_y != 0) ? dir_y / Mathf.Abs(dir_y) : 0;

        if (dir_x != 0 && dir_y != 0)
            {
                rb.velocity = new Vector2(now_dir_x * move_speed * sqrt, now_dir_y * move_speed * sqrt);
            }
            else
            {
                rb.velocity = new Vector2(now_dir_x * move_speed, now_dir_y * move_speed);
            }
    }

    protected void AnimationControllers()
    {
        anim.SetBool("is_dead",is_dead);
    }

    //����ʱ����Ϊ�µ�ͼ�㣬�����˺�����������
    public void SetLayerDead()
    {
        gameObject.layer = LayerMask.NameToLayer("Boomer");
    }


}
