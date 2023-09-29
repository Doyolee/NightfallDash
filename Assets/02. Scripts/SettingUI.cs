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
    //게임종료 버튼 누르면
    public void GameExit()
    {
        ExitSelect.SetActive(true);
    }
    public void SoundSetting(int sound)
    {
        PlayerPrefs.SetInt("sound", sound);
    }
    //로그아웃 확인 눌렀을 때
    public void LogOutYes()
    {
        SceneManager.LoadScene("LoginScene");
    }
    //로그아웃 취소 눌렀을 때
    public void LogOutNo()
    {
        LogoutSelect.SetActive(false);
    }
    //게임종료 확인 눌렀을 때
    public void GameExitYes()
    {
        Application.Quit();
    }
    //게임종료 취소 눌렀을 때
    public void GameExitNo()
    {
        ExitSelect.SetActive(false);
    }
}
