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

    public void ReturnObject()
    {
        if (transform.parent != null)
        {
            ObjectPoolManager.ReturnObject(transform.parent.gameObject);
        }
    }

    //��boomer��
    public void MonsterDead()
    {
        transform.parent.gameObject.GetComponent<Monster_Boomer>().SetLayerDead();
        transform.parent.gameObject.GetComponent<Monster_Boomer>().MonsterDead();
    }

    public void HideObject()
    {
        gameObject.SetActive(false);
    }
}
