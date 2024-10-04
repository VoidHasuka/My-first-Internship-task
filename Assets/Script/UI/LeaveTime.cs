using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaveTime : MonoBehaviour
{
    public float totalTime = 1200f; // 总时间
    private float timeRemaining;
    public TextMeshProUGUI timerText;

    public static bool GameIsPaused = false;
    public GameObject WinMenuUI;//结算UI
    public AudioDefination WinSound;//结算的小曲
    [SerializeField] public AudioSource BGMSource;//关掉循环播放

    void Start()
    {
        WinMenuUI.SetActive(false);
        timeRemaining = totalTime; // 初始化
    }

    void Update()
    {
        // 计算剩余时间
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            if (!GameIsPaused)
            {
                //时间结束哩
                WinMenuUI.SetActive(true);
                //关停BGM的循环功能
                BGMSource.loop = false;
                //播放小曲
                WinSound.PlayAudioClip();
                Time.timeScale = 0.0f;
                GameIsPaused = true;      
            }
        }
    }

    private void UpdateTimerText()
    {
        //更新文本
        float minutes = Mathf.Floor(timeRemaining / 60);
        float seconds = timeRemaining % 60;
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
