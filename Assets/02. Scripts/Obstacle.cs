using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Update()
    {
        //����ģ ������Ʈ ��Ȱ��ȭ
        if (gameObject.transform.position.z <= -64)
        {
            gameObject.SetActive(false);
        }
    }

}
