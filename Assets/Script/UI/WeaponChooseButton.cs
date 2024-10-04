using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class WeaponChooseButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject WeaponInformation;

    static public GameObject WhatInformationNow; //现在选择的武器的信息

    static public bool is_choosed = false; //全局共享静态变量
    void Start()
    {
        if (WeaponInformation.activeSelf)
        {
            WhatInformationNow = WeaponInformation;
        }
    }

    void Update()
    {
        
    }

    //悬停
    public void OnPointerEnter(PointerEventData eventData)
    {
        //更新信息
        if (!is_choosed&&WhatInformationNow!=WeaponInformation)
        {
            WhatInformationNow?.SetActive(false);
            WeaponInformation?.SetActive(true);
            WhatInformationNow = WeaponInformation;
        }
    }

    // 离开
    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

    public void ChooseWeapon()
    {
        WhatInformationNow?.SetActive(false);
        is_choosed = true;
        WhatInformationNow = WeaponInformation;
        WeaponInformation?.SetActive(true);
    }
}
