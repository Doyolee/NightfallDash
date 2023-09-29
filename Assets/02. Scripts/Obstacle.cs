using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Update()
    {
        //지나친 오브젝트 비활성화
        if (gameObject.transform.position.z <= -64)
        {
            gameObject.SetActive(false);
        }
    }

}
