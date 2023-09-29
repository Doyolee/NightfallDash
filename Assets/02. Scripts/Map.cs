using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform[] mapObjects; //���� ���� �� �����Ǿ��ִ� �ʵ�

    public Queue<Transform> mapQ; // �ʵ��� ��Ƽ� ������ ť


    [HideInInspector]
    public int gameLevel = 0;
    [HideInInspector]
    public int mapLevel = 0;
    [HideInInspector]
    public int spawnLevel = 0;

    public float startSpeed = 50;
    public float moveSpeed;
    public int nextLevelDistance=1000;
    public int distance;
    Transform tr;
    int increaseSpeed = 4;
    void Start()
    {
        tr=GetComponent<Transform>();

        // ť �ʱ�ȭ
        mapQ = new Queue<Transform>(mapObjects);

        //���ۺ��� �����Ǿ��ִ� �ʵ��� Pool�� ���
        for (int i = 0; i < mapObjects.Length; i++)
            GameManager.instance.poolManager.Pools[0].Add(mapObjects[i].gameObject);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isStart)
            return;

        // ť�� ��� ù��° ���(���� �տ� �ִ� ��)�� �����´�.
        Transform gm = mapQ.Peek(); 

        //���� �տ��ִ� ���� ��ġ�� -128 ���ϰ� �Ǹ� ��Ȱ��ȭ �� �� �� ��ȯ
        if (gm.position.z <= -192)
        {
            mapQ.Dequeue();
            gm.gameObject.SetActive(false);

            GameManager.instance.mapSpawner.GetNextMap();
        }

        // moveSpeed�� ���� ���� -z�� �̵�(�÷��̾�� ������ ���� ���� ȿ��)
        tr.Translate(Vector3.back * moveSpeed*Time.deltaTime,Space.World);

        // map�� z��ǥ�� ������ �����ϰ� ����� �ٲ㼭 �Ÿ�������� ���
        distance = -Mathf.CeilToInt(tr.position.z); 
        if(!GameManager.instance.isGameOver)
            GameManager.instance.distanceScore=distance;

        gameLevel = distance / nextLevelDistance;

        //objectSpawner�� �÷��̾� +96���� �����Ǹ� �� -32���� ����Ǿ��ϹǷ� �������� distance+128
        spawnLevel = (distance + 128) / nextLevelDistance;

        //map�� �÷��̾�� 192��ŭ �տ��� �����ǹǷ� �������� distance+192
        mapLevel = (distance + 192) / nextLevelDistance;

        //���� �Ÿ����� �ӵ� ����
        if (gameLevel < 10)
        {
            increaseSpeed = 4;
        }
        else
        {
            increaseSpeed = 1;
        }

        if (!GameManager.instance.isGameOver)
            moveSpeed = startSpeed + increaseSpeed*gameLevel;
    }
}
