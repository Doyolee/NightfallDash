using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    //��ü ��
    public Map entileMap;
    //���
    public BackGround backGround;
    public SpawnManager spawnManager;

    //�ʵ����͵�� ���� �ʵ�����
    public MapData[] mapDatas;
    MapData currMapData;

    int currMapIndex;
    // ���� ���ʷ� �ҷ��� �������� �ε���
    int nextMapIndex;
    
    //���� ���� Ȯ������ ���� ����
    float zombie1=0f;
    float zombie2=0f;
    float zombie3=0f;

    //���� ���� �� 1 / ���� ���� �� -1
    int spawnDirection=1;


    void Start()
    {
        currMapIndex = 0; // 0���� ����
        currMapData = mapDatas[0];

        GetZombieSpawnP();
    }

    //���� ���� Ǯ���� �������� �޼���
    public void GetNextMap()
    {
        GameObject nextMap;

        //Ǯ������ ���� �ε����� ���� �����Ѵ�.
        nextMap = GameManager.instance.poolManager.GetPools(currMapIndex);


        //���� ��ġ�ϰ� entileMap�� �ڽ����� �ִ´�
        nextMap.transform.position = new Vector3(0, 0, entileMap.mapQ.Peek().position.z + 256);
        nextMap.transform.parent = entileMap.transform;

        //ť�� �߰�
        entileMap.mapQ.Enqueue(nextMap.transform);

        //MapLevel�� �����Կ� ���� ���ε��� ����
        if (entileMap.mapLevel % 3 == 1)
            nextMapIndex = 2;
        else if (entileMap.mapLevel % 3 == 2)
            nextMapIndex = 4;
        else // entileMap.mapLevel%3==0
            nextMapIndex = 0;


        //�� ��ȯ ��(���� �ʰ� ���� ���� �ε����� �ٸ� ��)
        if (currMapIndex!=nextMapIndex)
        {
            //�ʵ����� ����
            //�ͳ� �ʿ��� �⺻ ������ �ٲ� �� ��� ��ġ�� ���� ��ġ��
            if (nextMapIndex == 0)
            {
                currMapData = mapDatas[0];
                backGround.transform.position = backGround.OriginPos;
            }
            else if (nextMapIndex == 2) currMapData = mapDatas[1];
            else currMapData = mapDatas[2];

            spawnManager.standardSpawnTime = currMapData.spawnTimer;

            GetZombieSpawnP();


            //0�� �ʿ��� 2�������� ��ȯ ��, ��ȯ�� ������Ʈ ����
            if (currMapIndex == 0)
            {
                GameObject mapObject = GameManager.instance.poolManager.GetPools(currMapIndex + 1);
                mapObject.transform.position = new Vector3(0, 0.4f, nextMap.transform.position.z + 32);

                //�����ϰ� ����/ ���� ��ȯ
                int randomValue = Random.Range(0, 2);
                if (randomValue == 0)
                {
                    spawnDirection = -1;
                }
                else
                {
                    spawnDirection = 1;
                }

                //�� ��ȯ�� ������Ʈ�� �������� �����̳� �������� ���ϵ����Ѵ�.
                mapObject.transform.localScale = new Vector3(spawnDirection, 1, 1);
                mapObject.transform.parent = entileMap.transform;

            }
            //2���ʿ��� 4�������� �ٲٴ� �ܰ迡���� �� 180�� ȸ���ؼ� ����
            if (currMapIndex == 4)
            {
                nextMap.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            currMapIndex = nextMapIndex;

        }

    }

    //������� ����Ȯ���� �����ϴ� �޼���
    void GetZombieSpawnP()
    {
        //spawnLevel�� 3�ܰ踦 ���� ������(�� ���� �� ������ ��� ������ ��) ���� ���� ����Ȯ���� ����
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

        //�ʵ����Ϳ��� �޾ƿ� ������ ����, ��ֹ�, ������ ����Ȯ���� ���ϰ�
        //���� Ȯ�� �߿��� ���� ���� �� Ȯ���� �ٽ� ���Ѵ�.
        spawnManager.spawnPZombie1 = currMapData.spawnPZombie * zombie1;
        spawnManager.spawnPZombie2 = currMapData.spawnPZombie * zombie2;
        spawnManager.spawnPZombie3 = currMapData.spawnPZombie * zombie3;
        spawnManager.spawnPObstacle = currMapData.spawnPObstacle;
        spawnManager.spawnPItemBox = currMapData.spawnPItemBox;
    }




}

