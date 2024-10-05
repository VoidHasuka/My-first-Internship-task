using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaveTime : MonoBehaviour
{
    public float totalTime = 1200f; // ��ʱ��
    private float timeRemaining;
    public TextMeshProUGUI timerText;

    public static bool GameIsPaused = false;
    public GameObject WinMenuUI;//����UI
    public AudioDefination WinSound;//�����С��
    [SerializeField] public AudioSource BGMSource;//�ص�ѭ������

    void Start()
    {
        WinMenuUI.SetActive(false);
        timeRemaining = totalTime; // ��ʼ��
        GameIsPaused = false;
    }

    void Update()
    {
        // ����ʣ��ʱ��
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            if (!GameIsPaused)
            {
                //ʱ�������
                WinMenuUI.SetActive(true);
                //��ͣBGM��ѭ������
                BGMSource.loop = false;
                //����С��
                WinSound.PlayAudioClip();
                GameIsPaused = true;
                Time.timeScale = 0.0f;
            }
        }
    }

    private void UpdateTimerText()
    {
        //�����ı�
        float minutes = Mathf.Floor(timeRemaining / 60);
        float seconds = timeRemaining % 60;
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
