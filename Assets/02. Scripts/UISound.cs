using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    AudioSource sound;
    public AudioClip[] clip;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    //¹öÆ° ´­·¶À» ¶§
    public void UIButtonSound()
    {
        sound.PlayOneShot(clip[0], 1);
    }
    //´Ý±â ¹öÆ° ´­·¶À» ¶§
    public void UIBackButtonSound()
    {
        sound.PlayOneShot(clip[1], 1);
    }
}
