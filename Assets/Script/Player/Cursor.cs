using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 mouse_position;
    private float chaseDistance = 3f; // ׷�پ���
    [SerializeField] private float moveSpeed = 10f; // ����ƽ���ƶ��ٶ�

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // ��ȡ���λ��
        mouse_position = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_position.z = 0;

        // ���������Ŀ��
        Transform nearestTarget = FindNearestTarget();

        Vector3 targetPosition; // Ŀ��λ��

        if (nearestTarget != null)
        {
            // ����ָ�������Ŀ��֮��ľ��롢�����Ŀ��֮��ľ���
            float distanceToTarget = Vector2.Distance(transform.position, nearestTarget.position);
            float mouseToMonster = Vector2.Distance(mouse_position, nearestTarget.position);
            if (distanceToTarget < chaseDistance && mouseToMonster < chaseDistance)
            {
                // ���Ŀ���ڷ�Χ�ڣ�ֱ�Ӹ���Ŀ��
                targetPosition = nearestTarget.position;
            }
            else
            {
                // ����������λ��
                targetPosition = mouse_position;
            }
        }
        else
        {
            // ���û��Ŀ�ָ꣬��ֱ�Ӹ������λ��
            targetPosition = mouse_position;
        }

        // ʹ�� Lerp ƽ���ƶ����
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue; // ����һ���ϴ�ĳ�ʼ����

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

}
