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

    [SerializeField] private GameObject floatPoint; //浮动伤害数字显示

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
                //执行受伤
                invincible_counter = invincible_time;
                current_health -= attacker.damage;
                on_take_damage?.Invoke(attacker.transform);
                //我们仍未知道为什么指定floatPoint之后仍然会报悬空物体的错
                if (floatPoint != null)
                {
                    GameObject gb =Instantiate(floatPoint, transform.position, Quaternion.identity) as GameObject;
                    gb.transform.GetChild(0).GetComponent<TextMesh>().text=attacker.damage.ToString();
                }
            }
            else
            {
                //执行死亡
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
