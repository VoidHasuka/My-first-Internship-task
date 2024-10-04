using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attack_range;

    public float attack_rate;

    //碰撞之后执行的内容
    //原来我最开始用的是stay啊，难怪是帧伤
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
