using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class UserManager : MonoBehaviour
{
    public static UserManager userInstance;

    public SetData setdata;

    public string ID;
    public string userNum;

    public bool isLoggedIn;

    [Header("Game Data")]
    public int Distance;
    public int Zombie;
    public int Score;
    public int Coin;
    public int MaxDistance;
    public int MaxScore;
    public int getDistance;
    public int getZombie;
    public int getScore;
    public int getCoin;

    [Header("Ranking")]
    public List<string> RankingID;
    public List<string> RankingData;
    public List<string> RankingTime;

    public List<string> RankingZombieID;
    public List<string> RankingZombieData;

    public List<string> RankingDistanceID;
    public List<string> RankingDistanceData;

    [Header("State")]
    public bool isGameover = false;
    public bool isLobby = false;
    public bool isRankingNull = false;
    public bool isNewScoreRecord = false;
    public bool isNewDistanceRecord = false;

    [Header("Perk Data")]
    public bool[] perks = { false };
    // 1. start with a weapon
    // 2. score +10%
    // 3. boss speed -10%
    // 4. More Ammo +50%
    // 5. More Item +25%

    [HideInInspector]
    //퍽에 사용한 금액
    public int perkCost = 0;

    private void Awake()
    {
        setdata = GetComponent<SetData>();
    }

    private void Start()
    {
        if (userInstance == null)
        {
            userInstance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        AudioListener.volume = PlayerPrefs.GetInt("sound");

        if (SceneManager.GetActiveScene().name == "LobbyScene") //로비씬일때
        {
            //게스트 로그인일때
            if (PlayerPrefs.GetInt("InLogin") == 0)
            {
                isLoggedIn = false;
                GuestGetData();
            }
            //아이디 로그인일때
            else
                isLoggedIn = true;

            if (isLobby)
                return;

            if (isLoggedIn)
            {
                //로비씬에 사용될 데이터를 데이터베이스에서 가져옴
                SetLobbyData();
            }
            else
            {
                //로비씬에 사용될 데이터를 플레이어프리프에서 가져옴
                GuestSetData();
                GuestGetData();
            }

            //로비로 돌아올 시 퍽 해제
            for(int i = 0; i < perks.Length; i++)
            {
                perks[i] = false;
            }

            isLobby = true;
            isGameover = false;
        }
        if (SceneManager.GetActiveScene().name == "GameScene") //게임씬일때
        {
            if (GameManager.instance.isGameOver)
            {
                //신기록 해제
                isNewDistanceRecord = false;
                isNewScoreRecord = false;
                if (isGameover)
                    return;

                //게임오버된 순간의 데이터들
                getDistance = GameManager.instance.distanceScore;
                getZombie = GameManager.instance.zombie;
                getScore = (getDistance + (getZombie + 1) * 10);
                getCoin = (getDistance / 20) + (getZombie*5) + (GameManager.instance.getItem);

                //현재 스코어가 지금 로그인된 계정의 최대 스코어보다 높으면 갱신
                if (getScore > MaxScore)
                {
                    isNewScoreRecord = true;    //신기록
                    //아이디 로그인일 때
                    if (isLoggedIn)
                        SetMaxScore(getScore);      //데이터베이스 최고점수 갱신
                    else
                        GuestUpdateMaxScore();      //게스트 최고점수 갱신
                }
                //현재 거리가 지금 로그인된 계정의 최대 거리보다 높으면 갱신
                if (getDistance > MaxDistance)
                {
                    isNewDistanceRecord = true;    //신기록
                    //아이디 로그인일 때
                    if (isLoggedIn)
                        SetMaxDistance(getDistance);    //데이터베이스 최고거리 갱신
                    else
                        GuestUpdateMaxDistance();   //게스트 최고거리 갱신
                }

                //아이디 로그인일 때
                if (isLoggedIn)
                {
                    //랭킹 코루틴 실행
                    StartCoroutine(GetScoreRankingData());
                    //지금 로그인된 계정의 데이터를 새로 갱신
                    SetUserData();
                }

                isGameover = true;
            }
        }
    }
    //로비 데이터 가져오기
    public void SetLobbyData()
    {
        setdata._NUM_Value = userNum; //현재 로그인된 계정의 넘버 저장

        //데이터베이스에서 코인, 거리, 좀비, 최대거리, 최대점수 가져옴
        setdata.GetGold(0);
        setdata.GetDistance(0);
        setdata.GetZombie(0);
        setdata.GetMaxDistance();
        setdata.GetMaxScore();

        //점수 랭킹 리스트 초기화
        RankingData.RemoveAll(item => true);
        RankingID.RemoveAll(item => true);
        RankingTime.RemoveAll(item => true);
        //점수 랭킹 리스트 갱신
        setdata.GetRankScore();

        //거리 랭킹 리스트 초기화
        RankingDistanceData.RemoveAll(item => true);
        RankingDistanceID.RemoveAll(item => true);

        //거리 랭킹 리스트 갱신
        setdata.GetRankDistance();

        //좀비 랭킹 리스트 초기화
        RankingZombieData.RemoveAll(item => true);
        RankingZombieID.RemoveAll(item => true);

        //좀비 랭킹 리스트 갱신
        setdata.GetRankZombie();
    }

    public void SetUserData()
    {
        //게임오버되면 새로 데이터 갱신
        setdata.GetGold(getCoin - perkCost);
        setdata.GetDistance(getDistance);
        setdata.GetZombie(getZombie);
        setdata.GetMaxScore();
        setdata.GetMaxDistance();
    }

    public void SetMaxDistance(int distance)
    {
        setdata.SetMaxDistance(distance);   //최대 거리 갱신
    }

    public void SetMaxScore(int score)
    {
        setdata.SetMaxScore(score); //최대 점수 갱신
    }

    public void SetRanking()
    {
        setdata.SetRank(ID, getScore);  //최대점수 랭킹 갱신
    }

    public IEnumerator GetScoreRankingData()
    {
        //랭킹 리스트 초기화
        RankingData.RemoveAll(item => true);
        RankingID.RemoveAll(item => true);
        RankingTime.RemoveAll(item => true);

        //날짜 데이터
        DateTime date1;
        DateTime date2;

        bool RankID = false;

        yield return null;
        //랭킹 리스트 갱신
        setdata.GetRankScore();

        yield return new WaitForSeconds(1f);

        //랭킹리스트에 중복아이디 있는지 체크
        foreach (string id in RankingID)
        {
            print("@ " + id + " = 랭킹아이디" + ID + " = 현재 로그인아이디");
            if (id == ID)
            {
                print("@ 이미 랭킹 데이터가 있음");
                RankID = true;
            }
        }
        if (RankingData.Count >= 1)
        {
            for (int i = 0; i < RankingData.Count - 1; i++)
            {
                //동일점수가 있을시
                if (RankingData[i] == RankingData[i + 1])
                {
                    //문자열을 DateTime 타입으로 변환
                    if (DateTime.TryParse(RankingData[i], out date1) && DateTime.TryParse(RankingData[i + 1], out date2))
                    {
                        //i + 1번 리스트 데이터의 날짜가 더 빠를시
                        if (date1 > date2)
                        {
                            //랭크id 순서 변경
                            string id = RankingID[i]; // 첫 번째 원소를 임시 변수에 저장
                            RankingID[i] = RankingID[i + 1]; // 두 번째 원소의 값을 첫 번째 원소로 옮김
                            RankingID[i + 1] = id; // 임시 변수에 저장된 값을 두 번째 원소로 옮김

                            //랭크data 순서 변경
                            string data = RankingData[i];
                            RankingData[i] = RankingData[i + 1];
                            RankingData[i + 1] = data;

                            //랭크datetime 순서 변경
                            string time = RankingTime[i];
                            RankingTime[i] = RankingTime[i + 1];
                            RankingTime[i + 1] = time;
                        }
                    }
                }
            }
        }

        //랭킹데이터가 5개보다 없고 중복아이디가 없으면 랭킹데이터에 저장
        if (RankingData.Count < 5 && !RankID)
        {
            print("@ 데이터가 없어서 바로 랭킹");
            SetRanking();
        }
        else
        {
            //랭킹에 중복아이디가 있을시
            if (RankID)
            {
                foreach (string rankId in RankingID)
                {
                    print(rankId + "랭킹아이디" + ID + "현재아이디");
                    if (rankId == ID)
                    {
                        print("@ 중복됨");
                        print("@ 맥스스코어" + MaxScore);
                        print("@ 겟스코어" + getScore);
                        if (MaxScore <= getScore)
                        {
                            //점수만 새로 갱신
                            setdata.UpdateRank(getScore);
                            print("@ 업데이트함");
                        }
                        else
                            print("@ 점수가 낮아 업데이트X");

                        yield break;
                    }
                }
            }
            else
            {
                //얻은 점수가 랭킹점수보다 높을 시
                foreach (string rankScore in RankingData)
                {
                    print("@ " + rankScore + "랭킹스코어" + getScore + "지금 점수");
                    if (int.Parse(rankScore) < getScore)
                    {
                        print("@ 랭킹갱신");
                        SetRanking();
                        yield break;
                    }
                }
            }
        }
        yield return null;
    }

    //게스트 로비 데이터 가져오기
    public void GuestGetData()
    {
        Distance = PlayerPrefs.GetInt("Distance");
        Zombie = PlayerPrefs.GetInt("Zombie");
        Score = PlayerPrefs.GetInt("Score");
        Coin = PlayerPrefs.GetInt("Coin");
        MaxDistance = PlayerPrefs.GetInt("MaxDistance");
        MaxScore = PlayerPrefs.GetInt("MaxScore");
    }
    //게스트 데이터 업데이트
    public void GuestSetData()
    {
        PlayerPrefs.SetInt("Distance", Distance + getDistance);
        PlayerPrefs.SetInt("Zombie", Zombie + getZombie);
        PlayerPrefs.SetInt("Coin", Coin + getCoin - perkCost);
    }
    //게스트 최고 점수 업데이트
    public void GuestUpdateMaxScore()
    {
        PlayerPrefs.SetInt("MaxScore", getScore);
    }
    //게스트 최고 거리 업데이트
    public void GuestUpdateMaxDistance()
    {
        PlayerPrefs.SetInt("MaxDistance", getDistance);
    }
}
