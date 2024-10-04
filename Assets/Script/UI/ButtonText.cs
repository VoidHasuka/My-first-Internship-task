using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public Color normalTextColor = Color.red;
    public Color highlightedTextColor = Color.white;

    public AudioDefination audioButton; //��ť��ͣ��ʱ�򲥷�һ��
    private bool is_played = false;

    //��ͣ
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightedTextColor;
        if (!is_played)
        {
            is_played = true;
            audioButton.PlayAudioClip();
        }
    }

    // �뿪
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalTextColor;
        is_played = false;
    }

}
