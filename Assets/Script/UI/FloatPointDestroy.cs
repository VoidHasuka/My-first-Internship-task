using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPointDestroy : MonoBehaviour
{
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject,destroyTime);
    }
}
