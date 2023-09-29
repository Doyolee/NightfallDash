using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public PoolManager poolManager;
    public Map entileMap;
    public Text bonusScoreText;
    public BossZombie bossZombie;
    public CameraEffect cameraEffect;

    public void Start()
    {
        //시작 무기 퍽
        if (UserManager.userInstance.perks[0])
        {
            //랜덤 무기를 풀에서 가져와 플레이어 바로 앞에 배치
            GameObject startingWeapon = poolManager.GetPools(13);
            startingWeapon.transform.position = new Vector3(0, 0.4f, -28);
            startingWeapon.transform.parent = entileMap.transform;

            //매쉬렌더러를 꺼서 화면에는 보이지 않게 설정
            MeshRenderer meshRenderer = startingWeapon.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }
        //점수 +10%
        if (UserManager.userInstance.perks[1])
        {
            //+10% Text 보여줌
            bonusScoreText.gameObject.SetActive(true);
            //int로만 계산하기 위해 10으로 나눈 값을 더해줌
            UserManager.userInstance.getScore += UserManager.userInstance.getScore/10;
        }
        //보스 좀비 속도 -10%
        if (UserManager.userInstance.perks[2])
        {
            bossZombie.bossSpeed *=0.9f;
        }

        //perks[3](총알 증가)는 각 무기 스크립트에서 처리

        //피 효과 제거 속도 +20%
        if (UserManager.userInstance.perks[4])
        {
            cameraEffect.bloodTimerCo *= 0.8f;
        }
    }


}
