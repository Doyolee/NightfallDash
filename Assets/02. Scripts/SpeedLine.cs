using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLine : MonoBehaviour
{
    public Map map;
    ParticleSystem ps;

    void Start()
    {
        ps= GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //오토바이가 달리기 시작하면 재생, 오토바이가 멈추면 중지
        if (GameManager.instance.isStart) ps.Play();
        else if(GameManager.instance.isGameOver) ps.Stop();
        var main = ps.main;
        var emission = ps.emission;
        var shape=ps.shape;

        //맵 스피드에 따라 집중선의 속도와 파티클 방출 속도 증가
        main.startSpeed = map.moveSpeed * 2;
        emission.rateOverTime = map.moveSpeed;
        //게임레벨에 반비례해서 방출하는 형태의 각도를 줄인다.(더 집중되어 보이게)
        if (shape.radius > 20)
            shape.radius = 100 - map.gameLevel * 10;
    }
}
