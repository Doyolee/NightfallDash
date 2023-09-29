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
    //�ܿ� ����� �ݾ�
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

        if (SceneManager.GetActiveScene().name == "LobbyScene") //�κ���϶�
        {
            //�Խ�Ʈ �α����϶�
            if (PlayerPrefs.GetInt("InLogin") == 0)
            {
                isLoggedIn = false;
                GuestGetData();
            }
            //���̵� �α����϶�
            else
                isLoggedIn = true;

            if (isLobby)
                return;

            if (isLoggedIn)
            {
                //�κ���� ���� �����͸� �����ͺ��̽����� ������
                SetLobbyData();
            }
            else
            {
                //�κ���� ���� �����͸� �÷��̾����������� ������
                GuestSetData();
                GuestGetData();
            }

            //�κ�� ���ƿ� �� �� ����
            for(int i = 0; i < perks.Length; i++)
            {
                perks[i] = false;
            }

            isLobby = true;
            isGameover = false;
        }
        if (SceneManager.GetActiveScene().name == "GameScene") //���Ӿ��϶�
        {
            if (GameManager.instance.isGameOver)
            {
                //�ű�� ����
                isNewDistanceRecord = false;
                isNewScoreRecord = false;
                if (isGameover)
                    return;

                //���ӿ����� ������ �����͵�
                getDistance = GameManager.instance.distanceScore;
                getZombie = GameManager.instance.zombie;
                getScore = (getDistance + (getZombie + 1) * 10);
                getCoin = (getDistance / 20) + (getZombie*5) + (GameManager.instance.getItem);

                //���� ���ھ ���� �α��ε� ������ �ִ� ���ھ�� ������ ����
                if (getScore > MaxScore)
                {
                    isNewScoreRecord = true;    //�ű��
                    //���̵� �α����� ��
                    if (isLoggedIn)
                        SetMaxScore(getScore);      //�����ͺ��̽� �ְ����� ����
                    else
                        GuestUpdateMaxScore();      //�Խ�Ʈ �ְ����� ����
                }
                //���� �Ÿ��� ���� �α��ε� ������ �ִ� �Ÿ����� ������ ����
                if (getDistance > MaxDistance)
                {
                    isNewDistanceRecord = true;    //�ű��
                    //���̵� �α����� ��
                    if (isLoggedIn)
                        SetMaxDistance(getDistance);    //�����ͺ��̽� �ְ�Ÿ� ����
                    else
                        GuestUpdateMaxDistance();   //�Խ�Ʈ �ְ�Ÿ� ����
                }

                //���̵� �α����� ��
                if (isLoggedIn)
                {
                    //��ŷ �ڷ�ƾ ����
                    StartCoroutine(GetScoreRankingData());
                    //���� �α��ε� ������ �����͸� ���� ����
                    SetUserData();
                }

                isGameover = true;
            }
        }
    }
    //�κ� ������ ��������
    public void SetLobbyData()
    {
        setdata._NUM_Value = userNum; //���� �α��ε� ������ �ѹ� ����

        //�����ͺ��̽����� ����, �Ÿ�, ����, �ִ�Ÿ�, �ִ����� ������
        setdata.GetGold(0);
        setdata.GetDistance(0);
        setdata.GetZombie(0);
        setdata.GetMaxDistance();
        setdata.GetMaxScore();

        //���� ��ŷ ����Ʈ �ʱ�ȭ
        RankingData.RemoveAll(item => true);
        RankingID.RemoveAll(item => true);
        RankingTime.RemoveAll(item => true);
        //���� ��ŷ ����Ʈ ����
        setdata.GetRankScore();

        //�Ÿ� ��ŷ ����Ʈ �ʱ�ȭ
        RankingDistanceData.RemoveAll(item => true);
        RankingDistanceID.RemoveAll(item => true);

        //�Ÿ� ��ŷ ����Ʈ ����
        setdata.GetRankDistance();

        //���� ��ŷ ����Ʈ �ʱ�ȭ
        RankingZombieData.RemoveAll(item => true);
        RankingZombieID.RemoveAll(item => true);

        //���� ��ŷ ����Ʈ ����
        setdata.GetRankZombie();
    }

    public void SetUserData()
    {
        //���ӿ����Ǹ� ���� ������ ����
        setdata.GetGold(getCoin - perkCost);
        setdata.GetDistance(getDistance);
        setdata.GetZombie(getZombie);
        setdata.GetMaxScore();
        setdata.GetMaxDistance();
    }

    public void SetMaxDistance(int distance)
    {
        setdata.SetMaxDistance(distance);   //�ִ� �Ÿ� ����
    }

    public void SetMaxScore(int score)
    {
        setdata.SetMaxScore(score); //�ִ� ���� ����
    }

    public void SetRanking()
    {
        setdata.SetRank(ID, getScore);  //�ִ����� ��ŷ ����
    }

    public IEnumerator GetScoreRankingData()
    {
        //��ŷ ����Ʈ �ʱ�ȭ
        RankingData.RemoveAll(item => true);
        RankingID.RemoveAll(item => true);
        RankingTime.RemoveAll(item => true);

        //��¥ ������
        DateTime date1;
        DateTime date2;

        bool RankID = false;

        yield return null;
        //��ŷ ����Ʈ ����
        setdata.GetRankScore();

        yield return new WaitForSeconds(1f);

        //��ŷ����Ʈ�� �ߺ����̵� �ִ��� üũ
        foreach (string id in RankingID)
        {
            print("@ " + id + " = ��ŷ���̵�" + ID + " = ���� �α��ξ��̵�");
            if (id == ID)
            {
                print("@ �̹� ��ŷ �����Ͱ� ����");
                RankID = true;
            }
        }
        if (RankingData.Count >= 1)
        {
            for (int i = 0; i < RankingData.Count - 1; i++)
            {
                //���������� ������
                if (RankingData[i] == RankingData[i + 1])
                {
                    //���ڿ��� DateTime Ÿ������ ��ȯ
                    if (DateTime.TryParse(RankingData[i], out date1) && DateTime.TryParse(RankingData[i + 1], out date2))
                    {
                        //i + 1�� ����Ʈ �������� ��¥�� �� ������
                        if (date1 > date2)
                        {
                            //��ũid ���� ����
                            string id = RankingID[i]; // ù ��° ���Ҹ� �ӽ� ������ ����
                            RankingID[i] = RankingID[i + 1]; // �� ��° ������ ���� ù ��° ���ҷ� �ű�
                            RankingID[i + 1] = id; // �ӽ� ������ ����� ���� �� ��° ���ҷ� �ű�

                            //��ũdata ���� ����
                            string data = RankingData[i];
                            RankingData[i] = RankingData[i + 1];
                            RankingData[i + 1] = data;

                            //��ũdatetime ���� ����
                            string time = RankingTime[i];
                            RankingTime[i] = RankingTime[i + 1];
                            RankingTime[i + 1] = time;
                        }
                    }
                }
            }
        }

        //��ŷ�����Ͱ� 5������ ���� �ߺ����̵� ������ ��ŷ�����Ϳ� ����
        if (RankingData.Count < 5 && !RankID)
        {
            print("@ �����Ͱ� ��� �ٷ� ��ŷ");
            SetRanking();
        }
        else
        {
            //��ŷ�� �ߺ����̵� ������
            if (RankID)
            {
                foreach (string rankId in RankingID)
                {
                    print(rankId + "��ŷ���̵�" + ID + "������̵�");
                    if (rankId == ID)
                    {
                        print("@ �ߺ���");
                        print("@ �ƽ����ھ�" + MaxScore);
                        print("@ �ٽ��ھ�" + getScore);
                        if (MaxScore <= getScore)
                        {
                            //������ ���� ����
                            setdata.UpdateRank(getScore);
                            print("@ ������Ʈ��");
                        }
                        else
                            print("@ ������ ���� ������ƮX");

                        yield break;
                    }
                }
            }
            else
            {
                //���� ������ ��ŷ�������� ���� ��
                foreach (string rankScore in RankingData)
                {
                    print("@ " + rankScore + "��ŷ���ھ�" + getScore + "���� ����");
                    if (int.Parse(rankScore) < getScore)
                    {
                        print("@ ��ŷ����");
                        SetRanking();
                        yield break;
                    }
                }
            }
        }
        yield return null;
    }

    //�Խ�Ʈ �κ� ������ ��������
    public void GuestGetData()
    {
        Distance = PlayerPrefs.GetInt("Distance");
        Zombie = PlayerPrefs.GetInt("Zombie");
        Score = PlayerPrefs.GetInt("Score");
        Coin = PlayerPrefs.GetInt("Coin");
        MaxDistance = PlayerPrefs.GetInt("MaxDistance");
        MaxScore = PlayerPrefs.GetInt("MaxScore");
    }
    //�Խ�Ʈ ������ ������Ʈ
    public void GuestSetData()
    {
        PlayerPrefs.SetInt("Distance", Distance + getDistance);
        PlayerPrefs.SetInt("Zombie", Zombie + getZombie);
        PlayerPrefs.SetInt("Coin", Coin + getCoin - perkCost);
    }
    //�Խ�Ʈ �ְ� ���� ������Ʈ
    public void GuestUpdateMaxScore()
    {
        PlayerPrefs.SetInt("MaxScore", getScore);
    }
    //�Խ�Ʈ �ְ� �Ÿ� ������Ʈ
    public void GuestUpdateMaxDistance()
    {
        PlayerPrefs.SetInt("MaxDistance", getDistance);
    }
}
