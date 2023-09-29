using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Off());
    }
    private void OnEnable()
    {
        StartCoroutine(Off());
    }

    IEnumerator Off()
    {
        //5초후 탄피 비활성화
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }
}
