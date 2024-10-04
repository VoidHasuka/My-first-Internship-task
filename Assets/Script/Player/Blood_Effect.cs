using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood_Effect : MonoBehaviour
{
    public float destroyTime;
    void Start()
    {
        Destroy(gameObject,destroyTime);
    }


}
