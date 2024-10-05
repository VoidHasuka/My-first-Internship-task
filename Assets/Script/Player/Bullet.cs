using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Bullet : MonoBehaviour, IPoolable
{

    private Camera cam;
    [SerializeField] private Transform target;
    public float angle;
    [SerializeField] private Vector3 move_dir;
    [SerializeField] private float move_speed;
    [SerializeField] private float lifetime = 3f; //生命周期
    private float lifetimecount;
    private float chaseDistance = 5f; // 追踪距离
    private Vector3 mouse_position;




    private void Start()
    {
        cam = Camera.main;

        mouse_position = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        
        mouse_position.z = 0;

        lifetimecount = lifetime;



    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        // 查找最近的目标
        target = FindNearestTarget();

        WayToTarget();

        Move();

        lifetimecount -= Time.deltaTime;

        if(lifetimecount < 0)
        {
            //回收
            ObjectPoolManager.ReturnObject(gameObject);
        }
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

    //感谢我自己还留了个接触摧毁的接口让我不用去翻这个代码
    public void DestroySelf()
    {
        ObjectPoolManager.ReturnObject(gameObject);
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

    // 当对象从池中获取时调用，可以理解为新的Start()函数
    public void OnObjectReuse()
    {

        // 重置属性
        lifetimecount = lifetime;
        mouse_position = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        mouse_position.z = 0;
        move_speed = 15;
        //重置伤害
        gameObject.GetComponent<Attack>().damage = Player.bulletDamage;


    }


    // 当对象被回收到池中时调用
    public void OnObjectReturn()
    {
        // 清理状态，准备下次使用
        //移动
        transform.Translate(Vector3.zero);
        move_dir = Vector3.zero;
        //追踪对象
        gameObject.GetComponent<Bullet>().target = null;


    }

    //禁用轨迹避免撕裂
    public IEnumerator TrailBan()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
        yield return new WaitForSeconds(0.1f); 
        trail.enabled = true;
    }


}
