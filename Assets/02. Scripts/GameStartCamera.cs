using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameStartCamera : MonoBehaviour
{
    float time;
    Quaternion front;
    Quaternion back;

    bool isFront = false;
    bool isBack = false;
    bool isLast = false;

    private void Start()
    {
        front = Quaternion.Euler(0, 180f, 0f);
        back = Quaternion.Euler(0, 350, 0);
        time = 0;
        StartCoroutine(waitStart());
    }
    void Update()
    {
        if (isFront)
            firstCamera();
        else if (isBack)
            secondCamera();
        else if(isLast)
            lastCamera();
    }

    IEnumerator waitStart()
    {
        yield return new WaitForSeconds(1);
        isFront = true;
        time = 0;
        print("@@1");
        yield return new WaitForSeconds(1.5f);
        isBack = true;
        isFront = false;
        time = 0;
        print("@@2");
        yield return new WaitForSeconds(2);
        isLast = true;
        isFront = false;
        isBack = false;
        time = 0;
        print("@@3");
        yield return new WaitForSeconds(0.5f);
        isLast= false;
        GameManager.instance.isStart = true;
    }
    void firstCamera()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / 15);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, front, t);
    }
    void secondCamera()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / 1.5f);
        transform.localRotation = Quaternion.Lerp(front, back,t);
    }

    void lastCamera()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / 0.5f);
        transform.localRotation = Quaternion.Lerp(back, front, t);
    }
}
