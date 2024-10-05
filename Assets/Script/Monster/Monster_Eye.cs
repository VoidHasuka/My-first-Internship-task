using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Eye : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!is_dead && player != null)
        {
            CheckPlayerPos();
            FlipController();
            UpdateMove();
        }   

        AnimationControllers();
    }

    protected void UpdateMove()
    {
        if (player_around > 5)
        {
            // 确保 dir_x 和 dir_y 不为 0
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
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    protected void AnimationControllers()
    {
        anim.SetBool("is_dead", is_dead);
    }
}
