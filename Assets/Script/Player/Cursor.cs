using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 mouse_position;
    private float chaseDistance = 3f; // 追踪距离
    [SerializeField] private float moveSpeed = 10f; // 靶心平滑移动速度

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // 获取鼠标位置
        mouse_position = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_position.z = 0;

        // 查找最近的目标
        Transform nearestTarget = FindNearestTarget();

        Vector3 targetPosition; // 目标位置

        if (nearestTarget != null)
        {
            // 计算指针与最近目标之间的距离、鼠标与目标之间的距离
            float distanceToTarget = Vector2.Distance(transform.position, nearestTarget.position);
            float mouseToMonster = Vector2.Distance(mouse_position, nearestTarget.position);
            if (distanceToTarget < chaseDistance && mouseToMonster < chaseDistance)
            {
                // 如果目标在范围内，直接跟随目标
                targetPosition = nearestTarget.position;
            }
            else
            {
                // 否则跟随鼠标位置
                targetPosition = mouse_position;
            }
        }
        else
        {
            // 如果没有目标，指针直接跟随鼠标位置
            targetPosition = mouse_position;
        }

        // 使用 Lerp 平滑移动光标
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue; // 设置一个较大的初始距离

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
