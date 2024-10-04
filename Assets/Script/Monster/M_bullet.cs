using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_bullet : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float angle;
    [SerializeField] private Vector2 move_dir;
    [SerializeField] private float move_speed;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        //保护用
        target =FindNearestTarget();
        if (target != null)
        {
            move_dir = target.position - transform.position;
            move_dir.Normalize();
        }


    }

    private void Awake()
    {
        
        Destroy(gameObject, lifetime);

    
    }

    private void Update()
    {

        Move();

    }
    public void Move()
    {
        transform.Translate(move_dir * move_speed * Time.deltaTime);
    }

    public void DestroySelf()
    {
        Destroy(gameObject, .1f);
    }

    public void HideObject()
    {
        gameObject.SetActive(false);
    }

    private Transform FindNearestTarget()
    {
        //查找玩家当前位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        return player != null ? player.transform : null;
    }

}
