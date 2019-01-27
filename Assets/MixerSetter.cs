using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerSetter : MonoBehaviour
{
    public AudioMixer mixer;
    public Sprite On;
    public Sprite Off;

    private GameObject image;

    private void Awake()
    {
        image = transform.GetChild(0).gameObject;
    }

    public void ToggleMute()
    {
        float masterVol = 0;
        mixer.GetFloat("MasterVol", out masterVol);
        if (masterVol < 0)
        {
            mixer.SetFloat("MasterVol", 0);
            image.GetComponent<Image>().sprite = On;
        }
        else
        {
            mixer.SetFloat("MasterVol", -80);
            image.GetComponent<Image>().sprite = Off;
        }
    }
}
