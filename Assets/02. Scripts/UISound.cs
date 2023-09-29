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

    //��ư ������ ��
    public void UIButtonSound()
    {
        sound.PlayOneShot(clip[0], 1);
    }
    //�ݱ� ��ư ������ ��
    public void UIBackButtonSound()
    {
        sound.PlayOneShot(clip[1], 1);
    }
}
