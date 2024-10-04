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

    //ÐüÍ£
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightedTextColor;
    }

    // Àë¿ª
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalTextColor;
    }

}
