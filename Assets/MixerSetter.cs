using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerSetter : MonoBehaviour
{
    public AudioMixer mixer;

    public void ToggleMute()
    {
        float masterVol = 0;
        mixer.GetFloat("MasterVol", out masterVol);
        if (masterVol < 0)
        {
            mixer.SetFloat("MasterVol", 0);
        }
        else
        {
            mixer.SetFloat("MasterVol", -80);
        }
    }
}
