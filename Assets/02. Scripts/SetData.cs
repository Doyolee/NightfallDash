using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetData : MonoBehaviour
{
    /*public string _Account_Reg = "http://localhost/ndInsertUser.php";
    public string _Account_Tbl = "http://localhost/ndInsertTables.php";
    public string _Account_Get = "http://localhost/ndGetUser.php";
    public string _Ranking_Set = "http://localhost/ndGetRanking.php";
    public string _Data_Get = "http://localhost/ndGetData.php";
    public string _Get_Ranking = "http://localhost/ndSendRanking.php";
    public string _Update_Ranking = "http://localhost/ndUpdateRanking.php";*/

    [Header("# PHP")]
    public string _Account_Reg = "http://192.168.107.180/ndInsertUser.php";
    public string _Account_Tbl = "http://192.168.107.180/ndInsertTables.php";
    public string _Account_Get = "http://192.168.107.180/ndGetUser.php";
    public string _Ranking_Set = "http://192.168.107.180/ndGetRanking.php";
    public string _Data_Get = "http://192.168.107.180/ndGetData.php";
    public string _Get_Ranking = "http://192.168.107.180/ndSendRanking.php";
    public string _Update_Ranking = "http://192.168.107.180/ndUpdateRanking.php";

    public string _CheckStatus;
    public string _ID_Value;
    public string _PW_Value;

    public string _ID_Value_string;
    public string _NUM_Value;
    public int _Gold_Value;
    public int _DISTANCE_Value;
    public int _ZOMBIE_Value;
    public int _SCORE_Value;

    //회원가입
    public void SendAccount()
    {
        StartCoroutine(SetAccout(_ID_Value, _PW_Value));
    }

    IEnumerator SetAccout(string _id, string _pw)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("ID_Value", _id.Trim());
        _PostData.AddField("PW_Value", _pw.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Account_Reg, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("Success"))
                {
                    _CheckStatus = "Complete";
                    InsertTables();
                }
                else
                    _CheckStatus = "Fail";
            }

            _sendData.Dispose();
        }

        _PW_Value = "";
    }

    //로그인
    public void GetAccount()
    {
        StartCoroutine(GetAccout(_ID_Value, _PW_Value));
    }

    IEnumerator GetAccout(string _id, string _pw)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("ID_Value", _id.Trim());
        _PostData.AddField("PW_Value", _pw.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Account_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    _CheckStatus = "Login Fail";

                }
                else
                {
                    _CheckStatus = "Login Complete";
                    UserManager.userInstance.ID = _ID_Value.ToString();
                    UserManager.userInstance.userNum = _sendData.downloadHandler.text.ToString();
                }
            }

            _sendData.Dispose();
        }

        _PW_Value = "";
    }

    //회원가입 후 새로 테이블 생성
    public void InsertTables()
    {
        StartCoroutine(InsertTables(_ID_Value));
    }

    IEnumerator InsertTables(string _id)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("ID_Value", _id.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Account_Tbl, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    _CheckStatus = _sendData.downloadHandler.text;

                }
                else
                {
                    _CheckStatus = "Complete";
                }
            }

            _sendData.Dispose();
        }
    }

    //골드 가져오기
    public void GetGold(int coin)
    {
        StartCoroutine(GetGold(_NUM_Value, "1", coin));
    }

    IEnumerator GetGold(string _num, string _data, int _gold)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());
        _PostData.AddField("GOLD_Value", _gold);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    //_CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    //_CheckStatus.text = "Complete";
                    UserManager.userInstance.Coin = int.Parse(_sendData.downloadHandler.text.ToString());
                    print(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //좀비 가져오기
    public void GetZombie(int zombie)
    {
        StartCoroutine(GetZombie(_NUM_Value, "2", zombie));
    }

    IEnumerator GetZombie(string _num, string _data, int _zom)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());
        _PostData.AddField("ZOMBIE_Value", _zom);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    //_CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    print(_sendData.downloadHandler.text);
                    UserManager.userInstance.Zombie = int.Parse(_sendData.downloadHandler.text.ToString());
                    ;
                }
            }
            _sendData.Dispose();
        }
    }
    //거리 가져오기
    public void GetDistance(int distance)
    {
        StartCoroutine(GetDistance(_NUM_Value, "3", distance));
    }

    IEnumerator GetDistance(string _num, string _data, int _dist)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());
        _PostData.AddField("DIST_Value", _dist);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    UserManager.userInstance.Distance = int.Parse(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //최대 거리 업데이트
    public void SetMaxDistance(int distance)
    {
        StartCoroutine(SetMaxDistance(_NUM_Value, "4", distance));
    }

    IEnumerator SetMaxDistance(string _num, string _data, int _dist)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());
        _PostData.AddField("DIST_Value", _dist);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    UserManager.userInstance.MaxDistance = int.Parse(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //최대 점수 업데이트
    public void SetMaxScore(int score)
    {
        StartCoroutine(SetMaxScore(_NUM_Value, "5", score));
    }

    IEnumerator SetMaxScore(string _num, string _data, int _score)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());
        _PostData.AddField("SCORE_Value", _score);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    UserManager.userInstance.MaxScore = int.Parse(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //최대 점수 가져오기
    public void GetMaxScore()
    {
        StartCoroutine(GetMaxScore(_NUM_Value, "7"));
    }

    IEnumerator GetMaxScore(string _num, string _data)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    UserManager.userInstance.MaxScore = int.Parse(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //최대 거리 가져오기
    public void GetMaxDistance()
    {
        StartCoroutine(GetMaxDistance(_NUM_Value, "6"));
    }

    IEnumerator GetMaxDistance(string _num, string _data)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("DATA_Value", _data.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Data_Get, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                _CheckStatus = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                    UserManager.userInstance.MaxDistance = int.Parse(_sendData.downloadHandler.text.ToString());
                }
            }
            _sendData.Dispose();
        }
    }
    //랭킹 데이터 가져오기
    public void SetRank(string id, int score)
    {
        StartCoroutine(Rank(_NUM_Value, id, score));
    }

    IEnumerator Rank(string _num, string _id, int _score)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("ID_Value", _id.Trim());
        _PostData.AddField("SCO_Value", _score);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Ranking_Set, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                // _CheckStatus.text = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    //_CheckStatus.text = "Complete";
                }
            }
            _sendData.Dispose();
        }
    }
    //랭킹 업데이트
    public void UpdateRank(int score)
    {
        StartCoroutine(UpdateRank(_NUM_Value, score));
    }

    IEnumerator UpdateRank(string _num, int _score)
    {
        WWWForm _PostData = new WWWForm();
        _PostData.AddField("NUM_Value", _num.Trim());
        _PostData.AddField("SCO_Value", _score);

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Update_Ranking, _PostData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                //_CheckStatus.text = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    //_CheckStatus.text = _sendData.downloadHandler.text;

                }
                else
                {
                    // _CheckStatus.text = "Complete";
                }
            }
            _sendData.Dispose();
        }
    }
    //랭킹 좀비 json 가져오기
    public void GetRankZombie()
    {
        StartCoroutine(GetRankZombie("2"));
    }

    IEnumerator GetRankZombie(string _rank)
    {
        WWWForm _postData = new WWWForm();
        _postData.AddField("RANK_Value", _rank.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Get_Ranking, _postData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                //_CheckStatus.text = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    //_CheckStatus.text = _sendData.downloadHandler.text;
                }
                else
                {
                    // _CheckStatus.text = "Complete";

                    string rankData = _sendData.downloadHandler.text;
                    Debug.Log(rankData);

                    var rank = JsonUtility.FromJson<RankData>(rankData);

                    // 데이터 값 출력 및 리스트에 저장
                    List<string> rankZombieList = new List<string>();
                    List<string> rankUserIDList = new List<string>();

                    foreach (RankZombie obj in rank.resultsZombie)
                    {
                        rankZombieList.Add(obj.zombie);
                        rankUserIDList.Add(obj.userID);
                    }

                    // 리스트 출력
                    foreach (string zombie in rankZombieList)
                    {
                        UserManager.userInstance.RankingZombieData.Add(zombie);
                    }

                    foreach (string userID in rankUserIDList)
                    {
                        UserManager.userInstance.RankingZombieID.Add(userID);
                    }

                }
            }
        }
    }
    //랭킹 거리 json 가져오기
    public void GetRankDistance()
    {
        StartCoroutine(GetRankDistance("3"));
    }

    IEnumerator GetRankDistance(string _rank)
    {
        WWWForm _postData = new WWWForm();
        _postData.AddField("RANK_Value", _rank.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Get_Ranking, _postData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                //_CheckStatus.text = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    // _CheckStatus.text = _sendData.downloadHandler.text;
                }
                else
                {
                    //_CheckStatus.text = "Complete";

                    string rankData = _sendData.downloadHandler.text;
                    Debug.Log(rankData);

                    var rank = JsonUtility.FromJson<RankData>(rankData);

                    // 데이터 값 출력 및 리스트에 저장
                    List<string> rankDistanceList = new List<string>();
                    List<string> rankUserIDList = new List<string>();

                    foreach (RankDistance obj in rank.resultsDistance)
                    {
                        rankDistanceList.Add(obj.distance);
                        rankUserIDList.Add(obj.userID);
                    }

                    // 리스트 출력
                    foreach (string distance in rankDistanceList)
                    {
                        UserManager.userInstance.RankingDistanceData.Add(distance);
                    }

                    foreach (string userID in rankUserIDList)
                    {
                        UserManager.userInstance.RankingDistanceID.Add(userID);
                    }

                }
            }
        }
    }

    //랭킹 점수 json 가져오기
    public void GetRankScore()
    {
        StartCoroutine(GetRankScore("1"));
    }

    IEnumerator GetRankScore(string _rank)
    {
        WWWForm _postData = new WWWForm();
        _postData.AddField("RANK_Value", _rank.Trim());

        using (UnityWebRequest _sendData = UnityWebRequest.Post(_Get_Ranking, _postData))
        {
            yield return _sendData.SendWebRequest();

            if (_sendData.isNetworkError || _sendData.isHttpError)
            {
                //  _CheckStatus.text = _sendData.error;
            }
            else
            {
                if (_sendData.downloadHandler.text.Equals("99"))
                {
                    //_CheckStatus.text = _sendData.downloadHandler.text;
                }
                else
                {
                    // _CheckStatus.text = "Complete";

                    string rankData = _sendData.downloadHandler.text;
                    Debug.Log(rankData);

                    var rank = JsonUtility.FromJson<RankData>(rankData);

                    // 데이터 값 출력 및 리스트에 저장
                    List<string> rankScoreList = new List<string>();
                    List<string> rankUserIDList = new List<string>();
                    List<string> rankUserDateList = new List<string>();

                    foreach (RankScore obj in rank.resultsScore)
                    {
                        rankScoreList.Add(obj.score);
                        rankUserIDList.Add(obj.userID);
                        rankUserDateList.Add(obj.rankTime);
                    }

                    // 리스트 출력
                    foreach (string score in rankScoreList)
                    {
                        UserManager.userInstance.RankingData.Add(score);
                    }

                    foreach (string userID in rankUserIDList)
                    {
                        UserManager.userInstance.RankingID.Add(userID);
                    }

                    foreach (string rankTime in rankUserDateList)
                    {
                        UserManager.userInstance.RankingTime.Add(rankTime);
                    }
                }
            }
        }
    }
    [System.Serializable]
    public class RankData
    {
        public RankDistance[] resultsDistance;
        public RankZombie[] resultsZombie;
        public RankScore[] resultsScore;
    }
    [System.Serializable]
    public class RankScore
    {
        public string userID;
        public string score;
        public string rankTime;
    }
    [System.Serializable]
    public class RankDistance
    {
        public string userID;
        public string distance;
    }
    [System.Serializable]
    public class RankZombie
    {
        public string userID;
        public string zombie;
    }
}

