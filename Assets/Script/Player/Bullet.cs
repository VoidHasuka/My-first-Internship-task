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
    [SerializeField] private float lifetime = 3f; //��������
    private float lifetimecount;
    private float chaseDistance = 5f; // ׷�پ���
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
        // ���������Ŀ��
        target = FindNearestTarget();

        WayToTarget();

        Move();

        lifetimecount -= Time.deltaTime;

        if(lifetimecount < 0)
        {
            //����
            ObjectPoolManager.ReturnObject(gameObject);
        }
    }

    private void WayToTarget()
    {
        if (target != null)
        {
            // �����ɫ�����֮��ľ���
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= chaseDistance)
            {
                // �������С���趨����ֵ����ʼ׷�٣������ٶȼ���
                // ���㷽�򲢸����ƶ�����

                move_dir = target.position - transform.position;
                move_dir.Normalize(); // ȷ�����������ǵ�λ����
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

    //��л���Լ������˸��Ӵ��ݻٵĽӿ����Ҳ���ȥ���������
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

        // ��������Ŀ�����
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

    // ������ӳ��л�ȡʱ���ã��������Ϊ�µ�Start()����
    public void OnObjectReuse()
    {

        // ��������
        lifetimecount = lifetime;
        mouse_position = cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        mouse_position.z = 0;
        move_speed = 15;
        //�����˺�
        gameObject.GetComponent<Attack>().damage = Player.bulletDamage;


    }


    // �����󱻻��յ�����ʱ����
    public void OnObjectReturn()
    {
        // ����״̬��׼���´�ʹ��
        //�ƶ�
        transform.Translate(Vector3.zero);
        move_dir = Vector3.zero;
        //׷�ٶ���
        gameObject.GetComponent<Bullet>().target = null;


    }

    //���ù켣����˺��
    public IEnumerator TrailBan()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
        yield return new WaitForSeconds(0.1f); 
        trail.enabled = true;
    }


}
