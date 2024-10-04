using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;

    protected float sqrt = 1.414f;
    protected float min_flip_speed = 1.5f;

    [SerializeField] protected bool facing_right = true;
    [SerializeField] private int facing_dir = 1;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void Flip()
    {
        facing_dir = facing_dir * (-1);
        facing_right = !facing_right;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void FlipController()
    {
        if (rb.velocity.x > min_flip_speed && !facing_right)
        {
            Flip();
        }
        else if (rb.velocity.x < -min_flip_speed && facing_right)
        {
            Flip();
        }
    }


}
