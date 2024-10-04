using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Exp : MonoBehaviour
{
    public Image MaxExp;
    public Image CurrentExp;

    public void OnExpChange(float percentage)
    {
        CurrentExp.fillAmount = percentage;
    }

}
