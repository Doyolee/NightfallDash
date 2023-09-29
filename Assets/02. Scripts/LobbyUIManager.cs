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
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && !Ranking.activeSelf && !Perk.activeSelf) //�κ�ȭ�鿡�� ����X ��ŷX ��X �϶�
        {
            EscapeExitSelect.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Setting.activeSelf && !Ranking.activeSelf && !Perk.activeSelf) //�κ�ȭ�鿡�� ����O ��ŷX ��X �϶�
        {
            Setting.SetActive(false);
            LogOut.SetActive(false);
            GameExit.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && Ranking.activeSelf && !Perk.activeSelf) //�κ�ȭ�鿡�� ����X ��ŷO ��X �϶�
        {
            Ranking.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf && !Ranking.activeSelf && Perk.activeSelf) //�κ�ȭ�鿡�� ����X ��ŷX ��O �϶�
        {
            Perk.SetActive(false);
        }

        //�κ� ������ ���� ǥ��
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
