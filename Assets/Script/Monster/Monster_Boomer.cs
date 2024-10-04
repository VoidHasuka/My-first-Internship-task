using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boomer : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (anim != null)
        {
            base.Update();

            if (!is_dead&&player!=null)
            {
                CheckPlayerPos();
                UpdateMove();
            }

            if (player_around < 3&&!is_dead)
            {
                MonsterDead();
            }


            AnimationControllers();
        }
    }

    protected void UpdateMove()
    {
        float now_dir_x = dir_x / Mathf.Abs(dir_x);
        float now_dir_y = dir_y / Mathf.Abs(dir_y);
        if (Mathf.Abs(dir_y) > 1 && Mathf.Abs(dir_x)>1)
                    {
            if (dir_x != 0 && dir_y != 0)
            {
                rb.velocity = new Vector2(now_dir_x * move_speed * sqrt, now_dir_y * move_speed * sqrt);
            }
            else
            {
                rb.velocity = new Vector2(now_dir_x * move_speed, now_dir_y * move_speed);
            }
        }
    }

    protected void AnimationControllers()
    {
        anim.SetBool("is_dead",is_dead);
    }

}
