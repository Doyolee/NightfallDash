using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [Header("User Data")]
    public Text Lobby_Zombie;
    public Text Lobby_MaxDistance;
    public Text Lobby_MaxScore;
    public Text Perk_Coin;

    public GameObject EscapeExitSelect;
    public GameObject Setting;
    public GameObject Perk;
    public GameObject Ranking;

    public GameObject LogOut;
    public GameObject GameExit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && !Ranking.activeSelf && !Perk.activeSelf) //로비화면에서 설정X 랭킹X 퍽X 일때
        {
            EscapeExitSelect.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Setting.activeSelf && !Ranking.activeSelf && !Perk.activeSelf) //로비화면에서 설정O 랭킹X 퍽X 일때
        {
            Setting.SetActive(false);
            LogOut.SetActive(false);
            GameExit.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && Ranking.activeSelf && !Perk.activeSelf) //로비화면에서 설정X 랭킹O 퍽X 일때
        {
            Ranking.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && !Ranking.activeSelf && Perk.activeSelf) //로비화면에서 설정X 랭킹X 퍽O 일때
        {
            Perk.SetActive(false);
        }

        //로비 데이터 정보 표기
        Lobby_Zombie.text = $"{UserManager.userInstance.Zombie}";
        Lobby_MaxScore.text = $"{UserManager.userInstance.MaxScore}";
        Lobby_MaxDistance.text = $"{UserManager.userInstance.Distance} M";
        Perk_Coin.text = $"{UserManager.userInstance.Coin}";
    }

    public void back()
    {
        EscapeExitSelect.SetActive(false);
    }
}
