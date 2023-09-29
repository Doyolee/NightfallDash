using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform[] mapObjects; //게임 시작 시 생성되어있는 맵들

    public Queue<Transform> mapQ; // 맵들을 담아서 관리할 큐


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

        // 큐 초기화
        mapQ = new Queue<Transform>(mapObjects);

        //시작부터 생성되어있는 맵들을 Pool에 등록
        for (int i = 0; i < mapObjects.Length; i++)
            GameManager.instance.poolManager.Pools[0].Add(mapObjects[i].gameObject);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isStart)
            return;

        // 큐에 담긴 첫번째 요소(가장 앞에 있는 맵)를 가져온다.
        Transform gm = mapQ.Peek(); 

        //가장 앞에있는 맵의 위치가 -128 이하가 되면 비활성화 후 새 맵 소환
        if (gm.position.z <= -192)
        {
            mapQ.Dequeue();
            gm.gameObject.SetActive(false);

            GameManager.instance.mapSpawner.GetNextMap();
        }

        // moveSpeed에 맞춰 맵이 -z로 이동(플레이어는 앞으로 가는 듯한 효과)
        tr.Translate(Vector3.back * moveSpeed*Time.deltaTime,Space.World);

        // map의 z좌표를 정수로 내림하고 양수로 바꿔서 거리기록으로 사용
        distance = -Mathf.CeilToInt(tr.position.z); 
        if(!GameManager.instance.isGameOver)
            GameManager.instance.distanceScore=distance;

        gameLevel = distance / nextLevelDistance;

        //objectSpawner은 플레이어 +96에서 생성되며 맵 -32에서 변경되야하므로 레벨계산시 distance+128
        spawnLevel = (distance + 128) / nextLevelDistance;

        //map은 플레이어보다 192만큼 앞에서 생성되므로 레벨계산시 distance+192
        mapLevel = (distance + 192) / nextLevelDistance;

        //일정 거리마다 속도 증가
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
