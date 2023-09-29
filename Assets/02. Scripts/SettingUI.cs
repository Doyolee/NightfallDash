using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public  AudioMixer audioMixer;
    public GameObject LogoutSelect;
    public GameObject ExitSelect;
    public GameObject X;
    public void Start()
    {
        if(PlayerPrefs.GetInt("sound")==0)
            X.SetActive(true);
    }
    public void LogOut()
    {
        LogoutSelect.SetActive(true);
    }
    //�������� ��ư ������
    public void GameExit()
    {
        ExitSelect.SetActive(true);
    }
    public void SoundSetting(int sound)
    {
        PlayerPrefs.SetInt("sound", sound);
    }
    //�α׾ƿ� Ȯ�� ������ ��
    public void LogOutYes()
    {
        SceneManager.LoadScene("LoginScene");
    }
    //�α׾ƿ� ��� ������ ��
    public void LogOutNo()
    {
        LogoutSelect.SetActive(false);
    }
    //�������� Ȯ�� ������ ��
    public void GameExitYes()
    {
        Application.Quit();
    }
    //�������� ��� ������ ��
    public void GameExitNo()
    {
        ExitSelect.SetActive(false);
    }
}
