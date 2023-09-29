using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUI;
    public Text gameOverText;
    public Text distanceScore;
    public Text Zombie;
    public Text Score;
    public Text Coin;
    public Text GetCoin;
    public Text PerkCoin;
    public Text Perks;
    public GameObject BestDistance;
    public GameObject BestScore;
    public GameObject Ammo;
    public Image fadeOut;

    bool isFade = false;
    float gameOverTime;

    //fadeOut을 위한 타이머
    float fadeOutTime=0;
    //fadeOut을 시작하는 시간
    float fadeTime = 0;
    void Update()
    {
        //게임 오버되면 화면 fadeout효과
        if (GameManager.instance.isGameOver)
        {
            fadeOutTime += Time.deltaTime;
        }

        //보스한테 잡혔을때
        if (GameManager.instance.player.isBoss)
        {
            gameOverTime = 5.5f;
            fadeTime = 3.5f;
        }
        //좀비한테 잡혔을때
        else if (GameManager.instance.player.cameraShake.isZombieCrash)
        {
            gameOverTime = 4f;
            fadeTime = 1.5f;
        }
        //그 외로 인해 게임오버 되었을때
        else
        {
            gameOverTime = 4f;
            fadeTime = 2f;
        }

        if(fadeOutTime>fadeTime)
        {
            if(!isFade)
                StartCoroutine(FadeOut());
        }

        //게임오버 된 후 일정 시간 지나면 게임오버 정보창 활성화
        if (GameManager.instance.gameOverTimer >= gameOverTime) 
        {
            gameOverUI.SetActive(true); //게임오버 정보창
            distanceScore.text = GameManager.instance.distanceScore.ToString() + " M";  //거리 수
            Score.text = $"{UserManager.userInstance.getScore}";    //점수
            Zombie.text = $"{GameManager.instance.zombie} KILL";    //잡은 좀비 수
            GetCoin.text = $"+ {UserManager.userInstance.getCoin}"; //받은 코인
            Coin.text = $"{UserManager.userInstance.Coin - UserManager.userInstance.perkCost}"; //현재 코인
    
            //거리 신기록이면
            if(UserManager.userInstance.isNewDistanceRecord)
                BestDistance.SetActive(true);
            //점수 신기록이면
            if(UserManager.userInstance.isNewScoreRecord)
                BestScore.SetActive(true);
            //퍽을 구매 했었으면
            if(UserManager.userInstance.perkCost != 0)
            {
                //퍽 구매 가격 표기
                PerkCoin.text = $"- {UserManager.userInstance.perkCost}";
                Perks.text = $"Perk";
            }

            Ammo.SetActive(false);
            //게임 시간 0 일시정지
            Time.timeScale = 0f;

            //화면 터치하면 로비로 나감
            if (Input.GetMouseButtonDown(0))
                Lobby();
        }
    }

    //로비씬으로 감
    public void Lobby()
    {
        SceneManager.LoadScene("LobbyScene");
        UserManager.userInstance.isLobby = false;
    }

    //fadeOut효과 코루틴
    IEnumerator FadeOut()
    {
        isFade = true;
        float fadeTime = 0f;
        while (fadeTime < 1f)
        {
            fadeTime += 0.02f;
            yield return new WaitForSeconds(0.01f);
            fadeOut.color = new Color(0, 0, 0, fadeTime);
        }
    }
}
