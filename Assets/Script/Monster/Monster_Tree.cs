using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Tree : Monster
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (player != null)
        {
            CheckPlayerPos();
            AnimationControllers();
        }
    }



    protected void AnimationControllers()
    {
        anim.SetFloat("player_around", player_around);
    }

    public void KeepHealth()
    {
        //保持树的无敌
        Character ch = GetComponent<Character>();
        if(ch != null)
        {
            ch.current_health=ch.max_health;
        }
    }

}
