using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Heart : MonoBehaviour
{
    public Image healthImage1;
    public Image healthImage2;
    public Image healthImage3;
    public Image healthImage4;
    public Image healthImage5;

    public GameObject heartPrefab1; // ʵ��Heart
    public GameObject heartPrefab2; // ����Heart

    public Transform heartsContainer; // ����Heart��UI����


    public List<Image> healthImages; // ���ڴ洢����Heartͼ��
    public Vector2 offset = new Vector2(+110f, 0f); // �����´�����Heart��ƫ����

    private void Start()
    {
        // ȷ��Heart�ڿ�ʼ��ʱ����˳��洢
        healthImages = new List<Image> { healthImage1, healthImage2, healthImage3, healthImage4, healthImage5 };
    }


    public void OnHealthChange(float leave)
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < leave)
            {
                //С������ֵ����Ϊ��
                healthImages[i].fillAmount = 1;
            }
            else
            {
                //��������ֵ����Ϊ��
                healthImages[i].fillAmount = 0;
            }
        }
    }

    // ÿ���������ʱ���ã�����Ѫʱ��̬���һ��Heart
    public void AddHeart()
    {
        // ��������Heart
        GameObject newHeartFalse = Instantiate(heartPrefab1, heartsContainer);
        // ����ʵ��Heart
        GameObject newHeartTrue = Instantiate(heartPrefab2, heartsContainer);
        

        Image heartImage1 = newHeartFalse.GetComponent<Image>(); 
        Image heartImage2 = newHeartTrue.GetComponent<Image>();  

        // ���ÿ���Heart��λ��
        RectTransform heartRect2 = newHeartTrue.GetComponent<RectTransform>();

        if (healthImages.Count > 0)
        {
            // ��ȡ���һ��Heart��λ��
            RectTransform lastHeartRect = healthImages[healthImages.Count - 1].GetComponent<RectTransform>();
            Vector2 lastHeartPosition = lastHeartRect.anchoredPosition;

            // �����һ��Heart�Ļ����ϼ���ƫ����
            heartRect2.anchoredPosition = lastHeartPosition + offset;
        }
        else
        {
            // ����ǵ�һ��Heart��������Ĭ��λ��
            heartRect2.anchoredPosition = new Vector2(0f, 0f);
        }

        // ������Heart��ӵ��б��У�ֻ������ȷ��λ�ã�
        healthImages.Add(heartImage1);

        // ����ʵ��Heart�����Heart��ͬλ�ã����ӣ�
        RectTransform heartRect1 = newHeartFalse.GetComponent<RectTransform>();
        heartRect1.anchoredPosition = heartRect2.anchoredPosition;

        //ɾ���б��п���Heart�����Ϊʵ��Heart
        healthImages.Remove(heartImage1);
        healthImages.Add(heartImage2);

        // ʵ��Heart����Ϊ��
        heartImage2.fillAmount = 1;
    }
}
