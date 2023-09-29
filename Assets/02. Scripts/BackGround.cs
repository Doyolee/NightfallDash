using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Map entileMap;
    public Vector3 OriginPos;

    private void Start()
    {
        OriginPos = transform.position;
    }
    private void Update()
    {
        transform.Translate(Vector3.back*0.5f*Time.deltaTime);
    }
}
