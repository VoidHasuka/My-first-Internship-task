using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Bullet : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform target;
    public float angle;
    [SerializeField] private Vector3 move_dir;
    [SerializeField] private float move_speed;
    [SerializeField] private float lifetime = 5f;
    private float chaseDistance = 5f; // 追踪距离
    private Vector3 mouse_position;


    private void Start()
    {
        cam = Camera.main;

        mouse_position = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        
        mouse_position.z = 0;
            
    }

    private void Awake()
    {
        // 查找最近的目标
        target = FindNearestTarget();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        WayToTarget();

        Move();


    }

    private void WayToTarget()
    {
        if (target != null)
        {
            // 计算角色与怪物之间的距离
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= chaseDistance)
            {
                // 如果距离小于设定的阈值，则开始追踪，并且速度减少
                // 计算方向并更新移动方向

                move_dir = target.position - transform.position;
                move_dir.Normalize(); // 确保方向向量是单位向量
                move_speed = 10;

            }

            else if (move_dir == Vector3.zero)
            {
                move_dir = mouse_position - transform.position;
                move_dir.Normalize();
            }

        }
        else if (move_dir == Vector3.zero)
        {
            move_dir = mouse_position - transform.position;
            move_dir.Normalize();
        }
    }

    public void Move()
    {
        transform.Translate(move_dir * move_speed * Time.deltaTime);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void HideObject()
    {
        gameObject.SetActive(false);
    }


    private Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        // 查找所有目标对象
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject go in targets)
        {
            float distance = Vector3.Distance(transform.position, go.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = go.transform;
            }
        }

        return nearestTarget;
    }

   
}
