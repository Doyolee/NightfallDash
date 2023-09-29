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

    //fadeOut�� ���� Ÿ�̸�
    float fadeOutTime=0;
    //fadeOut�� �����ϴ� �ð�
    float fadeTime = 0;
    void Update()
    {
        //���� �����Ǹ� ȭ�� fadeoutȿ��
        if (GameManager.instance.isGameOver)
        {
            fadeOutTime += Time.deltaTime;
        }

        //�������� ��������
        if (GameManager.instance.player.isBoss)
        {
            gameOverTime = 5.5f;
            fadeTime = 3.5f;
        }
        //�������� ��������
        else if (GameManager.instance.player.cameraShake.isZombieCrash)
        {
            gameOverTime = 4f;
            fadeTime = 1.5f;
        }
        //�� �ܷ� ���� ���ӿ��� �Ǿ�����
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

        //���ӿ��� �� �� ���� �ð� ������ ���ӿ��� ����â Ȱ��ȭ
        if (GameManager.instance.gameOverTimer >= gameOverTime) 
        {
            gameOverUI.SetActive(true); //���ӿ��� ����â
            distanceScore.text = GameManager.instance.distanceScore.ToString() + " M";  //�Ÿ� ��
            Score.text = $"{UserManager.userInstance.getScore}";    //����
            Zombie.text = $"{GameManager.instance.zombie} KILL";    //���� ���� ��
            GetCoin.text = $"+ {UserManager.userInstance.getCoin}"; //���� ����
            Coin.text = $"{UserManager.userInstance.Coin - UserManager.userInstance.perkCost}"; //���� ����
    
            //�Ÿ� �ű���̸�
            if(UserManager.userInstance.isNewDistanceRecord)
                BestDistance.SetActive(true);
            //���� �ű���̸�
            if(UserManager.userInstance.isNewScoreRecord)
                BestScore.SetActive(true);
            //���� ���� �߾�����
            if(UserManager.userInstance.perkCost != 0)
            {
                //�� ���� ���� ǥ��
                PerkCoin.text = $"- {UserManager.userInstance.perkCost}";
                Perks.text = $"Perk";
            }

            Ammo.SetActive(false);
            //���� �ð� 0 �Ͻ�����
            Time.timeScale = 0f;

            //ȭ�� ��ġ�ϸ� �κ�� ����
            if (Input.GetMouseButtonDown(0))
                Lobby();
        }
    }

    //�κ������ ��
    public void Lobby()
    {
        SceneManager.LoadScene("LobbyScene");
        UserManager.userInstance.isLobby = false;
    }

    //fadeOutȿ�� �ڷ�ƾ
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
