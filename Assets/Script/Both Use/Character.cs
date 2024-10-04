using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] public float max_health;
    [SerializeField] public float current_health;

    [Header("Invincible time")]
    [SerializeField] protected float invincible_time;
    [SerializeField] float invincible_counter;

    [SerializeField] private GameObject floatPoint; //�����˺�������ʾ

    public UnityEvent<Transform> on_take_damage;

    public UnityEvent on_death;

    public UnityEvent<Character> OnHealthChange;


    private void Start()
    {
        current_health = max_health;
        OnHealthChange?.Invoke(this);
    }

    private void Update()
    {
        invincible_counter -= Time.deltaTime;
    }


    public void TakeDamage(Attack attacker)
    {
        if (invincible_counter < 0)
        {
            if (current_health > attacker.damage)
            {
                //ִ������
                invincible_counter = invincible_time;
                current_health -= attacker.damage;
                on_take_damage?.Invoke(attacker.transform);
                //������δ֪��Ϊʲôָ��floatPoint֮����Ȼ�ᱨ��������Ĵ�
                if (floatPoint != null)
                {
                    GameObject gb =Instantiate(floatPoint, transform.position, Quaternion.identity) as GameObject;
                    gb.transform.GetChild(0).GetComponent<TextMesh>().text=attacker.damage.ToString();
                }
            }
            else
            {
                //ִ������
                current_health = 0;
                on_death?.Invoke();
                if (floatPoint != null)
                {
                    GameObject gb = Instantiate(floatPoint, transform.position, Quaternion.identity) as GameObject;
                    gb.transform.GetChild(0).GetComponent<TextMesh>().text = attacker.damage.ToString();
                }
            }
        }
        OnHealthChange?.Invoke(this);
    }
}
