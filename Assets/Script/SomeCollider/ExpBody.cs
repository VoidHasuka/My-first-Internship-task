using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ExpBody : MonoBehaviour
{
    public int expAmount;

    //寻路和子弹类似，所以这里我就直接copy子弹的脚本了
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 move_dir;
    [SerializeField] private float move_speed;
    private float chaseDistance = 5f; // 追踪距离
    private float lifetime = 30f; //生命周期
    [SerializeField] private AudioDefination audioDefination;//经验音效


    private void Start()
    {
        //如果长时间未拾取就消失
        DestroyWithTime();
    }

    private void Awake()
    {
        // 查找最近的目标
        target = FindNearestTarget();
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
            // 计算角色与经验之间的距离
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= chaseDistance)
            {
                // 如果距离小于设定的阈值，则开始追踪
                // 计算方向并更新移动方向
                move_dir = target.position - transform.position;
                move_dir.Normalize(); // 确保方向向量是单位向量
            }
        }
    }

        //碰撞之后角色经验增加
    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Player>()?.TakeExp(this);
    }

    public void DestroyItself()
    {
        audioDefination.PlayAudioClip();
        Destroy(gameObject);
    }
    
    public void DestroyWithTime()
    {
        Destroy(gameObject, lifetime);
    }

    private Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        // 查找所有目标对象
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
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
    public void Move()
    {
        transform.Translate(move_dir * move_speed * Time.deltaTime);
    }
}
