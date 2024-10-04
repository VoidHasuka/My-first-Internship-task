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

    static public GameObject WhatInformationNow; //����ѡ�����������Ϣ

    static public bool is_choosed = false; //ȫ�ֹ���̬����
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

    //��ͣ
    public void OnPointerEnter(PointerEventData eventData)
    {
        //������Ϣ
        if (!is_choosed&&WhatInformationNow!=WeaponInformation)
        {
            WhatInformationNow?.SetActive(false);
            WeaponInformation?.SetActive(true);
            WhatInformationNow = WeaponInformation;
        }
    }

    // �뿪
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
