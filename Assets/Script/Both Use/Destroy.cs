using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{ 
    public void DestroySelf()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject); // ���ٸ�����
        }
        Destroy(gameObject);
    }

    public void HideObject()
    {
        gameObject.SetActive(false);
    }
}
