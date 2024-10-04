using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAudio : MonoBehaviour
{
   [SerializeField] AudioDefination audioAnmi;

    public void FXsound()
    {
        audioAnmi.PlayAudioClip();
    }
}
