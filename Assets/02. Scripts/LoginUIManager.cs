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

    //���̵�� �α��� ����
    public void Login()
    {
        PlayerPrefs.SetInt("InLogin", 1);   //prefs�� ���̵� ���� �α����ߴٴ°��� ����
        SceneManager.LoadScene("LobbyScene");   //�κ������
        UserManager.userInstance.SetLobbyData();    //�κ� ������ ��������
        UserManager.userInstance.isLobby = false;

        state = "";
        ID.text = "";
        PW.text = "";
    }
    //�α��� ����
    public void LoginFail()
    {
        loginFail.SetActive(true);
    }
    //�α��� ���� ȭ�� �ݱ�
    public void CloseLoginFail()
    {
        loginFail.SetActive(false);
    }
    //ȸ������ ����
    public void RegFail()
    {
        regFail.SetActive(true);
    }
    //ȸ������ ����
    public void RegSuccess()
    {
        regSuccess.SetActive(true);
    }
    //ȸ������ ���� ȭ�� �ݱ�
    public void CloseRegFail()
    {
        regFail.SetActive(false);
    }
    //ȸ������ ���� ȭ�� �ݱ�
    public void CloseRegSucces()
    {
        regSuccess.SetActive(false);
    }
    //�Խ�Ʈ�� �α���
    public void GuestLogin()
    {
        if(PlayerPrefs.GetInt("Distance") > 0)
        {
            UserManager.userInstance.isLobby = false;
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            PlayerPrefs.SetInt("InLogin", 0);   //prefs�� �Խ�Ʈ�� �α����ߴٰ� ����
            PlayerPrefs.SetInt("Distance", 0);  //�Ÿ�������
            PlayerPrefs.SetInt("Zombie", 0);    //��������
            PlayerPrefs.SetInt("Score", 0);     //����������
            PlayerPrefs.SetInt("Coin", 500);    //���ε�����
            PlayerPrefs.SetInt("MaxDistance", 0);//�ִ� �Ÿ� ������
            PlayerPrefs.SetInt("MaxScore", 0);  //�ִ� ���� ������

            UserManager.userInstance.isLobby = false;
            SceneManager.LoadScene("LobbyScene");
        }
    }
    private void Update()
    {
        //ȭ�� ��ġ�ϸ� ���۵ǵ��� üũ
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

        //log�� state�� ���� üũ
        //SetData���� �޾ƿ�
        //log�� state�� ���°� �ٸ���
        if (state != log.text.ToString())
        {
            //����ȭ
            state = log.text.ToString();

            switch (state)
            {
                //ȸ������ ����
                case "Complete":
                    RegSuccess();
                    break;
                //�α��� ����
                case "Login Fail":
                    LoginFail();
                    break;
                //�α��� ����
                case "Login Complete":
                    Login();
                    break;
                //ȸ������ ����
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

    //���� ���� �� ȭ�� ��ġ �ؽ�Ʈ ������ �ڷ�ƾ
    IEnumerator ShowContinueText()
    {
        while (!tapToContinue)
        {
            if (continueText.activeSelf) continueText.SetActive(false);
            else continueText.SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }
    }
    //�α��� ��ư ������
    IEnumerator LoginAct()
    {
        //�Էµ� ���̵�, ��й�ȣ �����͸� SetData�� ����
        UserManager.userInstance.setdata._ID_Value = ID.text.ToString();
        UserManager.userInstance.setdata._PW_Value = PW.text.ToString();
        yield return null;
        //���� ���� �����͸� ���� SetData�� GetAccount�� ȣ���Ͽ� �α��� �õ�
        UserManager.userInstance.setdata.GetAccount();
        yield return new WaitForSeconds(0.1f);
        //�õ� �� ���� �޼����� log�� ���
        log.text = UserManager.userInstance.setdata._CheckStatus;
    }
    //ȸ������ ��ư ������
    IEnumerator RegAct()
    {
        //�Էµ� ���̵�, ��й�ȣ �����͸� SetData�� ����
        UserManager.userInstance.setdata._ID_Value = ID.text.ToString();
        UserManager.userInstance.setdata._PW_Value = PW.text.ToString();
        yield return null;
        //���� ���� �����͸� ���� SetData�� SendtAccount�� ȣ���Ͽ� ȸ������ �õ�
        UserManager.userInstance.setdata.SendAccount();
        yield return new WaitForSeconds(0.1f);
        //�õ� �� ���� �޼����� log�� ���
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
