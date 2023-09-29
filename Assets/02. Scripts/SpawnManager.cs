using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] objectSpawner; // �������� ��ġ


    float randomX; //���� x��ǥ�� �����ϱ� ���� ����
    Vector3 randomPosition; // ���� x��ǥ

    float spawnTime;

    [HideInInspector]
    public float standardSpawnTime;
    // map�� ���� standardSpawnTime ��ȭ

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

    int poolIndex; // ��ȯ�� ������ƮǮ �ε���


    void Start()
    {
        standardSpawnTime = 0.4f;
        //objectSpawner�� ���� ��ȯ �ڷ�ƾ ����
        for(int i=0;i<objectSpawner.Length;i++)
        {
            StartCoroutine(SpawnObject(objectSpawner[i]));
        }

    }

    IEnumerator SpawnObject(Transform _objectSpawner)
    {
        while (!GameManager.instance.isGameOver)
        {
            //���� �����̴� �ӵ��� �������� spawn�Ǵ� �ӵ��� �������� �Ѵ�.
            //10�ܰ� ������ 0.03f �� ��������, ���ķδ� 0.01�� ��������.
            if (spawnTime > standardSpawnTime-3f)
                spawnTime = standardSpawnTime - 0.03f * GameManager.instance.map.gameLevel;
            else if(spawnTime>0.5f)
                spawnTime = standardSpawnTime - 0.01f * GameManager.instance.map.gameLevel;

            // ������Ʈ ��ȯ�� x��ǥ ����
            randomX = Random.Range(0,16f);
            // ������Ʈ ��ȯ�� ��ġ
            randomPosition = _objectSpawner.position + Vector3.right * randomX; 

            //��ȯȮ�� ������ ���� ����
            float randomValue = Random.value; // 0,0~1.0 ������ ������ ��
            //�� ������Ʈ���� �ʿ� ��ȯ�Ǵ� Ȯ���� �����ϴ� �޼���
            SpawnProbability(randomValue);

            Collider[] colliders=new Collider[10];
            //OverlapSphere�� ������ ��, �̸� �Ҵ�� �迭�� ��⶧���� ����ӵ� ����
            Physics.OverlapSphereNonAlloc(randomPosition, 1f,colliders, 1 << 11);

            if (colliders[0]==null)
            {
                //��ȯ ����
                GameObject currObject = GameManager.instance.poolManager.GetPools(poolIndex);

                //randomPosition�� currObject��ȯ
                currObject.transform.position = randomPosition;
               
                //���� ��ȯ�� ��ֹ��� ������ ����� ��츦 �����ϱ� ���� ��ġ ����
                float collSize = currObject.GetComponent<Collider>().bounds.size.x/2;

                if (currObject.transform.position.x-collSize < _objectSpawner.transform.position.x)
                {
                    currObject.transform.Translate(Vector3.right * collSize,Space.World);
                }
                else if (currObject.transform.position.x + collSize > _objectSpawner.transform.position.x + 16)
                {
                    currObject.transform.Translate(-Vector3.right * collSize, Space.World);
                }

                //Map�� �ڽ����� �־ �ʿ��� �����̰�
                currObject.transform.parent = GameManager.instance.map.transform; // map�� �ڽ�����

            }

            //����Ÿ�ӿ� ������ ���� ���ؼ� �����ð��� �����ϰ�
            yield return new WaitForSeconds(spawnTime+Random.Range(-0.05f, 0.05f));
        }
    }

    //6 - ���� , 7 - ����2, 8 - ����3, 9~13 - ��ֹ�, 14 - itembox
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



