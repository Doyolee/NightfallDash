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
        //로비에 들어왔을 때 랭킹 데이터 받아오기
        if(UserManager.userInstance.isLobby)
        {
            ShowDistanceRanking();
            ShowScoreRanking();
            ShowZombieRanking();
        }
    }
    //스코어 랭킹 데이터 가져오기
    public void ShowScoreRanking()
    {
        //아이디로 로그인한 상태면
        if (UserManager.userInstance.isLoggedIn)
        {
            //스코어 랭킹 데이터를 받아와 text에 출력
            for (int i = 0; i < UserManager.userInstance.RankingID.Count; i++)
            {
                ScoreID[i].text = UserManager.userInstance.RankingID[i];
                ScoreData[i].text = UserManager.userInstance.RankingData[i];
                ScoreRank[i].text = $"{i + 1}";
            }
        }
        //게스트 로그인한 상태면
        else
        {
            //랭킹 데이터를 안보여줌
            for (int i = 0; i < UserManager.userInstance.RankingID.Count; i++)
            {
                ScoreID[i].text = "";
                ScoreData[i].text = "";
                ScoreRank[i].text = "";
            }
        }
    }
    //좀비 랭킹 데이터 가져오기
    public void ShowZombieRanking()
    {
        //아이디로 로그인한 상태면
        if (UserManager.userInstance.isLoggedIn)
        {
            //스코어 랭킹 데이터를 받아와 text에 출력
            for (int i = 0; i < UserManager.userInstance.RankingZombieID.Count; i++)
            {
                ZombieID[i].text = UserManager.userInstance.RankingZombieID[i];
                ZombieData[i].text = UserManager.userInstance.RankingZombieData[i];
                ZombieRank[i].text = $"{i + 1}";
            }
        }
        //게스트 로그인한 상태면
        else
        {
            //랭킹 데이터를 안보여줌
            for (int i = 0; i < UserManager.userInstance.RankingZombieID.Count; i++)
            {
                ZombieID[i].text = "";
                ZombieData[i].text = "";
                ZombieRank[i].text = "";
            }
        }
    }
    //거리 랭킹 데이터 가져오기
    public void ShowDistanceRanking()
    {
        //아이디로 로그인한 상태면
        if (UserManager.userInstance.isLoggedIn)
        {
            //스코어 랭킹 데이터를 받아와 text에 출력
            for (int i = 0; i < UserManager.userInstance.RankingDistanceID.Count; i++)
            {
                DistanceID[i].text = UserManager.userInstance.RankingDistanceID[i];
                DistanceData[i].text = UserManager.userInstance.RankingDistanceData[i];
                DistanceRank[i].text = $"{i + 1}";
            }
        }
        //게스트 로그인한 상태면
        else
        {
            //랭킹 데이터를 안보여줌
            for (int i = 0; i < UserManager.userInstance.RankingDistanceID.Count; i++)
            {
                DistanceID[i].text = "";
                DistanceData[i].text = "";
                DistanceRank[i].text = "";
            }
        }
            
    }

    //랭킹 버튼 눌렀을때
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
    //랭킹 닫기 버튼 눌렀을때
    public void CloseRankingPanel()
    {
        RankingPanel.SetActive(false);
    }
    //다음 혹은 이전버튼 눌렀을때
    public void CloseRankingPage()
    {
        for(int i = 0; i < RankingPage.Length; i++)
            RankingPage[i].SetActive(false);
    }
    //다음 버튼 눌렀을때
    public void nextPage()
    {
        page++; 
        if (page > 2)
            page = 0;
        RankingPage[page].SetActive(true);
    }
    //이전 버튼 눌렀을때
    public void previousPage()
    {
        page--; 
        if (page < 0)
            page = 2;
        RankingPage[page].SetActive(true);
    }
}
