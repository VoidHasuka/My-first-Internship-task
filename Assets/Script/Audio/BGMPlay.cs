using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    [SerializeField] private AudioDefination audioBGM;
    void Start()
    {
        audioBGM.PlayAudioClip();
    }


}
