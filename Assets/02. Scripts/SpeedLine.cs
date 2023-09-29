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
        //������̰� �޸��� �����ϸ� ���, ������̰� ���߸� ����
        if (GameManager.instance.isStart) ps.Play();
        else if(GameManager.instance.isGameOver) ps.Stop();
        var main = ps.main;
        var emission = ps.emission;
        var shape=ps.shape;

        //�� ���ǵ忡 ���� ���߼��� �ӵ��� ��ƼŬ ���� �ӵ� ����
        main.startSpeed = map.moveSpeed * 2;
        emission.rateOverTime = map.moveSpeed;
        //���ӷ����� �ݺ���ؼ� �����ϴ� ������ ������ ���δ�.(�� ���ߵǾ� ���̰�)
        if (shape.radius > 20)
            shape.radius = 100 - map.gameLevel * 10;
    }
}
