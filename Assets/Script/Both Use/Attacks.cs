using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attack_range;

    public float attack_rate;

    //��ײ֮��ִ�е�����
    //ԭ�����ʼ�õ���stay�����ѹ���֡��
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
