using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] objectSpawner; // 스포너의 위치


    float randomX; //랜덤 x좌표를 설정하기 위한 난수
    Vector3 randomPosition; // 랜덤 x좌표

    float spawnTime;

    [HideInInspector]
    public float standardSpawnTime;
    // map에 따라 standardSpawnTime 변화

    [HideInInspector]
    public float spawnPZombie1=0.36f;
    [HideInInspector]
    public float spawnPZombie2 = 0.18f;
    [HideInInspector]
    public float spawnPZombie3 = 0.06f;
    [HideInInspector]
    public float spawnPObstacle = 0.3f;
    [HideInInspector]
    public float spawnPItemBox = 0.1f;

    int poolIndex; // 소환할 오브젝트풀 인덱스


    void Start()
    {
        standardSpawnTime = 0.4f;
        //objectSpawner들 각각 소환 코루틴 실행
        for(int i=0;i<objectSpawner.Length;i++)
        {
            StartCoroutine(SpawnObject(objectSpawner[i]));
        }

    }

    IEnumerator SpawnObject(Transform _objectSpawner)
    {
        while (!GameManager.instance.isGameOver)
        {
            //맵이 움직이는 속도가 빨라지면 spawn되는 속도도 빨라지게 한다.
            //10단계 까지는 0.03f 씩 빨라지고, 이후로는 0.01씩 빨라진다.
            if (spawnTime > standardSpawnTime-3f)
                spawnTime = standardSpawnTime - 0.03f * GameManager.instance.map.gameLevel;
            else if(spawnTime>0.5f)
                spawnTime = standardSpawnTime - 0.01f * GameManager.instance.map.gameLevel;

            // 오브젝트 소환할 x좌표 범위
            randomX = Random.Range(0,16f);
            // 오브젝트 소환할 위치
            randomPosition = _objectSpawner.position + Vector3.right * randomX; 

            //소환확률 구현을 위한 난수
            float randomValue = Random.value; // 0,0~1.0 사이의 랜덤한 값
            //각 오브젝트들이 맵에 소환되는 확률을 적용하는 메서드
            SpawnProbability(randomValue);

            Collider[] colliders=new Collider[10];
            //OverlapSphere와 비교했을 때, 미리 할당된 배열에 담기때문에 검출속도 빠름
            Physics.OverlapSphereNonAlloc(randomPosition, 1f,colliders, 1 << 11);

            if (colliders[0]==null)
            {
                //소환 실행
                GameObject currObject = GameManager.instance.poolManager.GetPools(poolIndex);

                //randomPosition에 currObject소환
                currObject.transform.position = randomPosition;
               
                //만약 소환한 장애물이 영역을 벗어나는 경우를 방지하기 위해 위치 조정
                float collSize = currObject.GetComponent<Collider>().bounds.size.x/2;

                if (currObject.transform.position.x-collSize < _objectSpawner.transform.position.x)
                {
                    currObject.transform.Translate(Vector3.right * collSize,Space.World);
                }
                else if (currObject.transform.position.x + collSize > _objectSpawner.transform.position.x + 16)
                {
                    currObject.transform.Translate(-Vector3.right * collSize, Space.World);
                }

                //Map의 자식으로 넣어서 맵에서 움직이게
                currObject.transform.parent = GameManager.instance.map.transform; // map의 자식으로

            }

            //스폰타임에 랜덤한 값을 더해서 소폰시간을 유연하게
            yield return new WaitForSeconds(spawnTime+Random.Range(-0.05f, 0.05f));
        }
    }

    //6 - 좀비 , 7 - 좀비2, 8 - 좀비3, 9~13 - 장애물, 14 - itembox
    void SpawnProbability(float randomValue)
    {
        //normal zombie
        if (randomValue < spawnPZombie1)
            poolIndex = 6;
        //hard zombie
        else if (randomValue < spawnPZombie1 + spawnPZombie2)
            poolIndex = 7;
        //heavy zombie
        else if (randomValue < spawnPZombie1 + spawnPZombie2 + spawnPZombie3)
            poolIndex = 8;
        //Obstacle
        else if (randomValue < spawnPZombie1 + spawnPZombie2 + spawnPZombie3 + spawnPObstacle)
            poolIndex = Random.Range(9, 13);
        //ItemBox
        else
            poolIndex = 13;
    }


}



