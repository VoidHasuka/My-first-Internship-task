using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ExpBody : MonoBehaviour
{
    public int expAmount;

    //Ѱ·���ӵ����ƣ����������Ҿ�ֱ��copy�ӵ��Ľű���
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 move_dir;
    [SerializeField] private float move_speed;
    private float chaseDistance = 5f; // ׷�پ���
    private float lifetime = 30f; //��������
    [SerializeField] private AudioDefination audioDefination;//������Ч


    private void Start()
    {
        //�����ʱ��δʰȡ����ʧ
        DestroyWithTime();
    }

    private void Awake()
    {
        // ���������Ŀ��
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
            // �����ɫ�뾭��֮��ľ���
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= chaseDistance)
            {
                // �������С���趨����ֵ����ʼ׷��
                // ���㷽�򲢸����ƶ�����
                move_dir = target.position - transform.position;
                move_dir.Normalize(); // ȷ�����������ǵ�λ����
            }
        }
    }

        //��ײ֮���ɫ��������
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

        // ��������Ŀ�����
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
