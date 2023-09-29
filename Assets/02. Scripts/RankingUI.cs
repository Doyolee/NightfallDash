using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    public GameObject[] RankingPage;
    public GameObject RankingPanel;

    public Text[] ScoreID;
    public Text[] ScoreData;
    public Text[] ScoreRank;

    public Text[] ZombieID;
    public Text[] ZombieData;
    public Text[] ZombieRank;

    public Text[] DistanceID;
    public Text[] DistanceData;
    public Text[] DistanceRank;

    int page = 0;

    public GameObject GuestLogin;

    private void Update()
    {
        //�κ� ������ �� ��ŷ ������ �޾ƿ���
        if(UserManager.userInstance.isLobby)
        {
            ShowDistanceRanking();
            ShowScoreRanking();
            ShowZombieRanking();
        }
    }
    //���ھ� ��ŷ ������ ��������
    public void ShowScoreRanking()
    {
        //���̵�� �α����� ���¸�
        if (UserManager.userInstance.isLoggedIn)
        {
            //���ھ� ��ŷ �����͸� �޾ƿ� text�� ���
            for (int i = 0; i < UserManager.userInstance.RankingID.Count; i++)
            {
                ScoreID[i].text = UserManager.userInstance.RankingID[i];
                ScoreData[i].text = UserManager.userInstance.RankingData[i];
                ScoreRank[i].text = $"{i + 1}";
            }
        }
        //�Խ�Ʈ �α����� ���¸�
        else
        {
            //��ŷ �����͸� �Ⱥ�����
            for (int i = 0; i < UserManager.userInstance.RankingID.Count; i++)
            {
                ScoreID[i].text = "";
                ScoreData[i].text = "";
                ScoreRank[i].text = "";
            }
        }
    }
    //���� ��ŷ ������ ��������
    public void ShowZombieRanking()
    {
        //���̵�� �α����� ���¸�
        if (UserManager.userInstance.isLoggedIn)
        {
            //���ھ� ��ŷ �����͸� �޾ƿ� text�� ���
            for (int i = 0; i < UserManager.userInstance.RankingZombieID.Count; i++)
            {
                ZombieID[i].text = UserManager.userInstance.RankingZombieID[i];
                ZombieData[i].text = UserManager.userInstance.RankingZombieData[i];
                ZombieRank[i].text = $"{i + 1}";
            }
        }
        //�Խ�Ʈ �α����� ���¸�
        else
        {
            //��ŷ �����͸� �Ⱥ�����
            for (int i = 0; i < UserManager.userInstance.RankingZombieID.Count; i++)
            {
                ZombieID[i].text = "";
                ZombieData[i].text = "";
                ZombieRank[i].text = "";
            }
        }
    }
    //�Ÿ� ��ŷ ������ ��������
    public void ShowDistanceRanking()
    {
        //���̵�� �α����� ���¸�
        if (UserManager.userInstance.isLoggedIn)
        {
            //���ھ� ��ŷ �����͸� �޾ƿ� text�� ���
            for (int i = 0; i < UserManager.userInstance.RankingDistanceID.Count; i++)
            {
                DistanceID[i].text = UserManager.userInstance.RankingDistanceID[i];
                DistanceData[i].text = UserManager.userInstance.RankingDistanceData[i];
                DistanceRank[i].text = $"{i + 1}";
            }
        }
        //�Խ�Ʈ �α����� ���¸�
        else
        {
            //��ŷ �����͸� �Ⱥ�����
            for (int i = 0; i < UserManager.userInstance.RankingDistanceID.Count; i++)
            {
                DistanceID[i].text = "";
                DistanceData[i].text = "";
                DistanceRank[i].text = "";
            }
        }
            
    }

    //��ŷ ��ư ��������
    public void ShowRankingPanel()
    {
        page = 0;
        RankingPanel.SetActive(true);
        RankingPage[page].SetActive(true);
        if (UserManager.userInstance.isLoggedIn)
            GuestLogin.SetActive(false);
        else
            GuestLogin.SetActive(true);
    }
    //��ŷ �ݱ� ��ư ��������
    public void CloseRankingPanel()
    {
        RankingPanel.SetActive(false);
    }
    //���� Ȥ�� ������ư ��������
    public void CloseRankingPage()
    {
        for(int i = 0; i < RankingPage.Length; i++)
            RankingPage[i].SetActive(false);
    }
    //���� ��ư ��������
    public void nextPage()
    {
        page++; 
        if (page > 2)
            page = 0;
        RankingPage[page].SetActive(true);
    }
    //���� ��ư ��������
    public void previousPage()
    {
        page--; 
        if (page < 0)
            page = 2;
        RankingPage[page].SetActive(true);
    }
}
