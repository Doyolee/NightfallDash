using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    public Text log;
    public string state;

    public GameObject loginFail;
    public GameObject regFail;
    public GameObject regSuccess;

    public GameObject continueText;
    public GameObject loginPanel;
    bool tapToContinue = false;

    public Text ID;
    public Text PW;

    private void Start()
    {
        StartCoroutine(ShowContinueText());
        state = "";
    }

    //아이디로 로그인 성공
    public void Login()
    {
        PlayerPrefs.SetInt("InLogin", 1);   //prefs에 아이디를 통해 로그인했다는것을 저장
        SceneManager.LoadScene("LobbyScene");   //로비씬으로
        UserManager.userInstance.SetLobbyData();    //로비 데이터 가져오기
        UserManager.userInstance.isLobby = false;

        state = "";
        ID.text = "";
        PW.text = "";
    }
    //로그인 실패
    public void LoginFail()
    {
        loginFail.SetActive(true);
    }
    //로그인 실패 화면 닫기
    public void CloseLoginFail()
    {
        loginFail.SetActive(false);
    }
    //회원가입 실패
    public void RegFail()
    {
        regFail.SetActive(true);
    }
    //회원가입 성공
    public void RegSuccess()
    {
        regSuccess.SetActive(true);
    }
    //회원가입 실패 화면 닫기
    public void CloseRegFail()
    {
        regFail.SetActive(false);
    }
    //회원가입 성공 화면 닫기
    public void CloseRegSucces()
    {
        regSuccess.SetActive(false);
    }
    //게스트로 로그인
    public void GuestLogin()
    {
        if(PlayerPrefs.GetInt("Distance") > 0)
        {
            UserManager.userInstance.isLobby = false;
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            PlayerPrefs.SetInt("InLogin", 0);   //prefs에 게스트로 로그인했다고 저장
            PlayerPrefs.SetInt("Distance", 0);  //거리데이터
            PlayerPrefs.SetInt("Zombie", 0);    //좀비데이터
            PlayerPrefs.SetInt("Score", 0);     //점수데이터
            PlayerPrefs.SetInt("Coin", 500);    //코인데이터
            PlayerPrefs.SetInt("MaxDistance", 0);//최대 거리 데이터
            PlayerPrefs.SetInt("MaxScore", 0);  //최대 점수 데이터

            UserManager.userInstance.isLobby = false;
            SceneManager.LoadScene("LobbyScene");
        }
    }
    private void Update()
    {
        //화면 터치하면 시작되도록 체크
        if (!tapToContinue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tapToContinue = true;
                continueText.SetActive(false);
                //loginPanel.SetActive(true);
                GuestLogin();
            }
            else return;
        }

        //log와 state로 상태 체크
        //SetData에서 받아옴
        //log와 state의 상태가 다르면
        if (state != log.text.ToString())
        {
            //동일화
            state = log.text.ToString();

            switch (state)
            {
                //회원가입 성공
                case "Complete":
                    RegSuccess();
                    break;
                //로그인 실패
                case "Login Fail":
                    LoginFail();
                    break;
                //로그인 성공
                case "Login Complete":
                    Login();
                    break;
                //회원가입 실패
                case "Fail":
                    RegFail();
                    break;
                default:
                    break;
            }
        }
        else
            return;
    }

    //게임 시작 시 화면 터치 텍스트 깜박임 코루틴
    IEnumerator ShowContinueText()
    {
        while (!tapToContinue)
        {
            if (continueText.activeSelf) continueText.SetActive(false);
            else continueText.SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }
    }
    //로그인 버튼 누를때
    IEnumerator LoginAct()
    {
        //입력된 아이디, 비밀번호 데이터를 SetData에 전송
        UserManager.userInstance.setdata._ID_Value = ID.text.ToString();
        UserManager.userInstance.setdata._PW_Value = PW.text.ToString();
        yield return null;
        //전송 받은 데이터를 통해 SetData의 GetAccount를 호출하여 로그인 시도
        UserManager.userInstance.setdata.GetAccount();
        yield return new WaitForSeconds(0.1f);
        //시도 후 받은 메세지를 log에 출력
        log.text = UserManager.userInstance.setdata._CheckStatus;
    }
    //회원가입 버튼 누를때
    IEnumerator RegAct()
    {
        //입력된 아이디, 비밀번호 데이터를 SetData에 전송
        UserManager.userInstance.setdata._ID_Value = ID.text.ToString();
        UserManager.userInstance.setdata._PW_Value = PW.text.ToString();
        yield return null;
        //전송 받은 데이터를 통해 SetData의 SendtAccount를 호출하여 회원가입 시도
        UserManager.userInstance.setdata.SendAccount();
        yield return new WaitForSeconds(0.1f);
        //시도 후 받은 메세지를 log에 출력
        log.text = UserManager.userInstance.setdata._CheckStatus;
    }
    public void LoginAction()
    {
        StartCoroutine(LoginAct());
    }
    public void RegAction()
    {
        StartCoroutine (RegAct());
    }
}
