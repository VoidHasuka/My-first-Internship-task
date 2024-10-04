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

    public GameObject heartPrefab1; // 实心Heart
    public GameObject heartPrefab2; // 空心Heart

    public Transform heartsContainer; // 放置Heart的UI容器


    public List<Image> healthImages; // 用于存储所有Heart图像
    public Vector2 offset = new Vector2(+110f, 0f); // 设置新创建的Heart的偏移量

    private void Start()
    {
        // 确保Heart在开始的时候按照顺序存储
        healthImages = new List<Image> { healthImage1, healthImage2, healthImage3, healthImage4, healthImage5 };
    }


    public void OnHealthChange(float leave)
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < leave)
            {
                //小于生命值设置为满
                healthImages[i].fillAmount = 1;
            }
            else
            {
                //大于生命值设置为空
                healthImages[i].fillAmount = 0;
            }
        }
    }

    // 每次玩家升级时调用，当满血时动态添加一个Heart
    public void AddHeart()
    {
        // 创建空心Heart
        GameObject newHeartFalse = Instantiate(heartPrefab1, heartsContainer);
        // 创建实心Heart
        GameObject newHeartTrue = Instantiate(heartPrefab2, heartsContainer);
        

        Image heartImage1 = newHeartFalse.GetComponent<Image>(); 
        Image heartImage2 = newHeartTrue.GetComponent<Image>();  

        // 设置空心Heart的位置
        RectTransform heartRect2 = newHeartTrue.GetComponent<RectTransform>();

        if (healthImages.Count > 0)
        {
            // 获取最后一个Heart的位置
            RectTransform lastHeartRect = healthImages[healthImages.Count - 1].GetComponent<RectTransform>();
            Vector2 lastHeartPosition = lastHeartRect.anchoredPosition;

            // 在最后一个Heart的基础上加上偏移量
            heartRect2.anchoredPosition = lastHeartPosition + offset;
        }
        else
        {
            // 如果是第一个Heart，放置在默认位置
            heartRect2.anchoredPosition = new Vector2(0f, 0f);
        }

        // 将空心Heart添加到列表中（只是用于确认位置）
        healthImages.Add(heartImage1);

        // 设置实心Heart与空心Heart相同位置（叠加）
        RectTransform heartRect1 = newHeartFalse.GetComponent<RectTransform>();
        heartRect1.anchoredPosition = heartRect2.anchoredPosition;

        //删除列表中空心Heart并添加为实心Heart
        healthImages.Remove(heartImage1);
        healthImages.Add(heartImage2);

        // 实心Heart设置为满
        heartImage2.fillAmount = 1;
    }
}
