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
        //게임 시작했을 때 안보이게
        if(!GameManager.instance.isStart)
        {
            distance.text = "";
            zombie.text = "";
            ammo.text = "";
            button.SetActive(false);
            fireButton.SetActive(false);
            return;
        }
        //시작 후 일정시간 지났을 때
        distance.text = GameManager.instance.distanceScore.ToString() + "M";
        zombie.text = $"{GameManager.instance.zombie} KILL";
        button.SetActive(true);

        //총 먹었을 때
        if (GameManager.instance.player.getGun)
        {
            fireButton.SetActive(false);
            ammoImg.SetActive(true);
            fireButton.SetActive(true); //enable 때문에 껏다가 킴
            ammo.text = GameManager.instance.ammo.ToString();
        }
        //아직도 총을 못먹었을 때
        else
        {
            ammoImg.SetActive(false);
            ammo.text = "";
        }
        //게임 오버 되었을 때
        if(GameManager.instance.isGameOver)
        {
            ammoImg.SetActive(false);
            distance.text = "";
            zombie.text = "";
            ammo.text = "";
            button.SetActive(false);
            fireButton.SetActive(false);
        }
        //게임 도중 뒤로가기 버튼 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.instance.isGameOver)
        {
            PauseButton();
        }
    }

    //게임 시작 fadein 효과 코루틴
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
    //일시정지 버튼 눌렀을 때
    public void PauseButton()
    {
        Time.timeScale = 0f;    //게임 정지
        Pause.SetActive(true);  //일시정지 화면 활성화
        GameManager.instance.player.StopBikeSound();    //오토바이 소리 정지
    }
    //로비 버튼 눌렀을 때
    public void LobbyButton() 
    { 
        Lobby.SetActive(true);
    }
    //로비로 나가기에서 확인 눌렀을 때
    public void Yes()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    //로비로 나가기에서 취소 눌렀을 때
    public void No()
    {
        Lobby.SetActive(false);
    }
    //닫기 버튼 눌렀을 때
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
