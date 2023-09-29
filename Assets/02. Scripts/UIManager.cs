using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text distance;
    public Text zombie;
    public Text ammo;
    public GameObject ammoImg;
    public Image Fade;
    public GameObject button;
    public GameObject fireButton;
    public GameObject Pause;
    public GameObject Lobby;

    public GameObject X;
    private void Start()
    {
        StartCoroutine(FadeIn());

        if (PlayerPrefs.GetInt("sound") == 0)
        {
            X.SetActive(true);
        }
    }
    void Update()
    {
        //���� �������� �� �Ⱥ��̰�
        if(!GameManager.instance.isStart)
        {
            distance.text = "";
            zombie.text = "";
            ammo.text = "";
            button.SetActive(false);
            fireButton.SetActive(false);
            return;
        }
        //���� �� �����ð� ������ ��
        distance.text = GameManager.instance.distanceScore.ToString() + "M";
        zombie.text = $"{GameManager.instance.zombie} KILL";
        button.SetActive(true);

        //�� �Ծ��� ��
        if (GameManager.instance.player.getGun)
        {
            fireButton.SetActive(false);
            ammoImg.SetActive(true);
            fireButton.SetActive(true); //enable ������ ���ٰ� Ŵ
            ammo.text = GameManager.instance.ammo.ToString();
        }
        //������ ���� ���Ծ��� ��
        else
        {
            ammoImg.SetActive(false);
            ammo.text = "";
        }
        //���� ���� �Ǿ��� ��
        if(GameManager.instance.isGameOver)
        {
            ammoImg.SetActive(false);
            distance.text = "";
            zombie.text = "";
            ammo.text = "";
            button.SetActive(false);
            fireButton.SetActive(false);
        }
        //���� ���� �ڷΰ��� ��ư ������ ��
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.instance.isGameOver)
        {
            PauseButton();
        }
    }

    //���� ���� fadein ȿ�� �ڷ�ƾ
    IEnumerator FadeIn()
    {
        float fadeTime = 1f;
        while (fadeTime > 0)
        {
            fadeTime -= 0.01f;
            yield return new WaitForSeconds(0.05f);
            Fade.color = new Color(0, 0, 0, fadeTime);
        }
    }
    //�Ͻ����� ��ư ������ ��
    public void PauseButton()
    {
        Time.timeScale = 0f;    //���� ����
        Pause.SetActive(true);  //�Ͻ����� ȭ�� Ȱ��ȭ
        GameManager.instance.player.StopBikeSound();    //������� �Ҹ� ����
    }
    //�κ� ��ư ������ ��
    public void LobbyButton() 
    { 
        Lobby.SetActive(true);
    }
    //�κ�� �����⿡�� Ȯ�� ������ ��
    public void Yes()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    //�κ�� �����⿡�� ��� ������ ��
    public void No()
    {
        Lobby.SetActive(false);
    }
    //�ݱ� ��ư ������ ��
    public void PauseClose()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.player.RestartBikeSound();
    }

    public void Sound(int sound)
    {
        PlayerPrefs.SetInt("sound", sound);
    }
}
