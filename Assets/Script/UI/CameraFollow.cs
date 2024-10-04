using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothing;

    void FixedUpdate()
    {
       if(player != null)
        {
            if(transform.position!=player.position)
            {
                Vector3 targetPos = player.position;
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
}
