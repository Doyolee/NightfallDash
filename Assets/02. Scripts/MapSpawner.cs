using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    //전체 맵
    public Map entileMap;
    //배경
    public BackGround backGround;
    public SpawnManager spawnManager;

    //맵데이터들과 현재 맵데이터
    public MapData[] mapDatas;
    MapData currMapData;

    int currMapIndex;
    // 다음 차례로 불러올 맵프리팝 인덱스
    int nextMapIndex;
    
    //좀비 스폰 확률값을 담을 변수
    float zombie1=0f;
    float zombie2=0f;
    float zombie3=0f;

    //우측 스폰 시 1 / 좌측 스폰 시 -1
    int spawnDirection=1;


    void Start()
    {
        currMapIndex = 0; // 0부터 시작
        currMapData = mapDatas[0];

        GetZombieSpawnP();
    }

    //다음 맵을 풀에서 가져오는 메서드
    public void GetNextMap()
    {
        GameObject nextMap;

        //풀링에서 현재 인덱스의 맵을 스폰한다.
        nextMap = GameManager.instance.poolManager.GetPools(currMapIndex);


        //맵을 배치하고 entileMap의 자식으로 넣는다
        nextMap.transform.position = new Vector3(0, 0, entileMap.mapQ.Peek().position.z + 256);
        nextMap.transform.parent = entileMap.transform;

        //큐에 추가
        entileMap.mapQ.Enqueue(nextMap.transform);

        //MapLevel이 증가함에 따라 맵인덱스 변경
        if (entileMap.mapLevel % 3 == 1)
            nextMapIndex = 2;
        else if (entileMap.mapLevel % 3 == 2)
            nextMapIndex = 4;
        else // entileMap.mapLevel%3==0
            nextMapIndex = 0;


        //맵 전환 시(현재 맵과 다음 맵의 인덱스가 다를 시)
        if (currMapIndex!=nextMapIndex)
        {
            //맵데이터 변경
            //터널 맵에서 기본 맵으로 바뀔 때 배경 위치도 원래 위치로
            if (nextMapIndex == 0)
            {
                currMapData = mapDatas[0];
                backGround.transform.position = backGround.OriginPos;
            }
            else if (nextMapIndex == 2) currMapData = mapDatas[1];
            else currMapData = mapDatas[2];

            spawnManager.standardSpawnTime = currMapData.spawnTimer;

            GetZombieSpawnP();


            //0번 맵에서 2번맵으로 전환 시, 전환용 오브젝트 스폰
            if (currMapIndex == 0)
            {
                GameObject mapObject = GameManager.instance.poolManager.GetPools(currMapIndex + 1);
                mapObject.transform.position = new Vector3(0, 0.4f, nextMap.transform.position.z + 32);

                //랜덤하게 우측/ 좌측 반환
                int randomValue = Random.Range(0, 2);
                if (randomValue == 0)
                {
                    spawnDirection = -1;
                }
                else
                {
                    spawnDirection = 1;
                }

                //맵 전환용 오브젝트를 랜덤으로 좌측이나 우측으로 향하도록한다.
                mapObject.transform.localScale = new Vector3(spawnDirection, 1, 1);
                mapObject.transform.parent = entileMap.transform;

            }
            //2번맵에서 4번맵으로 바꾸는 단계에서는 맵 180도 회전해서 스폰
            if (currMapIndex == 4)
            {
                nextMap.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            currMapIndex = nextMapIndex;

        }

    }

    //좀비들의 스폰확률을 변경하는 메서드
    void GetZombieSpawnP()
    {
        //spawnLevel이 3단계를 지날 때마다(세 가지 맵 컨셉을 모두 지나온 뒤) 상위 좀비 스폰확률이 증가
        if (entileMap.spawnLevel / 3 == 0)
        {
            print("Case1");
            zombie1 = 0.7f;
            zombie2 = 0.3f;
            zombie3 = 0;
        }
        else if (entileMap.spawnLevel / 3 == 1)
        {
            print("Case2");
            zombie1 = 0.6f;
            zombie2 = 0.3f;
            zombie3 = 0.1f;
        }
        else if (entileMap.spawnLevel / 3 == 2)
        {
            print("Case3");
            zombie1 = 0.5f;
            zombie2 = 0.3f;
            zombie3 = 0.2f;
        }
        else
        {
            print("Case4");
            zombie1 = 0.5f;
            zombie2 = 0.3f;
            zombie3 = 0.3f;
        }

        //맵데이터에서 받아온 값으로 좀비, 장애물, 아이템 스폰확률을 정하고
        //좀비 확률 중에서 좀비 종류 별 확률을 다시 정한다.
        spawnManager.spawnPZombie1 = currMapData.spawnPZombie * zombie1;
        spawnManager.spawnPZombie2 = currMapData.spawnPZombie * zombie2;
        spawnManager.spawnPZombie3 = currMapData.spawnPZombie * zombie3;
        spawnManager.spawnPObstacle = currMapData.spawnPObstacle;
        spawnManager.spawnPItemBox = currMapData.spawnPItemBox;
    }




}

