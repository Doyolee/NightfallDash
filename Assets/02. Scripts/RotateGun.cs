using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    float waitTime = 0.5f;
    float time;
    public bool isGetGun;
    public bool isFire = false;

    Quaternion getGun;
    Quaternion zeroPos;
    Quaternion zero;
    Vector3 gunPoint;

    private void OnEnable()
    {
        time = 0;
        getGun = Quaternion.Euler(-70f, transform.rotation.y, transform.rotation.z);
        zeroPos = Quaternion.Euler(0, transform.rotation.y, 0);
        isGetGun = false;
    }
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        /*// 터치 입력 감지
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    print("터치" + Input.touchCount + "총");
                    isUIPressed = true;
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Ended)
                        touch.phase = TouchPhase.Canceled;
                }
                else
                {
                    print("터치" + Input.touchCount);
                    // 카메라의 월드 좌표 기준으로 포인터의 위치를 변환합니다.
                    Vector3 touchPosition = Input.GetTouch(i).position;
                    touchPosition.z = Camera.main.farClipPlane;
                    gunPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                    // 총구의 방향을 계산합니다.
                    Vector3 direction = gunPoint - transform.position;
                    direction.Normalize();
                    // 방향 벡터를 총구의 회전 값으로 변환합니다.
                    transform.rotation = Quaternion.LookRotation(direction);
                    isFire = true;
                    isUIPressed = false;
                    StartCoroutine(resetPos());
                }
            }
        }*/
        // 터치 입력 감지
        /*if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!EventSystem.current.IsPointerOverGameObject(i))
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began) //해당 터치가 시작되었다면
                    {
                        // 카메라의 월드 좌표 기준으로 포인터의 위치를 변환합니다.
                        Vector3 touchPosition = Input.GetTouch(i).position;
                        touchPosition.z = Camera.main.farClipPlane;
                        gunPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                        // 총구의 방향을 계산합니다.
                        Vector3 direction = gunPoint - transform.position;
                        direction.Normalize();
                        // 방향 벡터를 총구의 회전 값으로 변환합니다.
                        transform.rotation = Quaternion.LookRotation(direction);
                        isFire = true;
                        isUIPressed = false;
                        StartCoroutine(resetPos());
                    }
                }

                print("터치" + Input.touchCount + EventSystem.current.IsPointerOverGameObject(i));
            }
        }*/

        time += Time.deltaTime;
        //총 습득했을 때
        if (!isGetGun)
        {
            //총을 위에서 아래로 돌림
            float t = Mathf.Clamp01(time / waitTime);
            transform.rotation = Quaternion.Lerp(getGun, zeroPos, t);
            if (transform.rotation == zeroPos)
            {
                isGetGun = true;
            }
        }
        //총 습득 후 isGetGun이 true일때
        else
        {
            //플레이더 뒤돌지 않을 상태일때
            if (!GameManager.instance.player.playerCamera.isRotate)
            {
                zero = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, zero, Time.deltaTime * 30);
            }
            //플레이더 뒤돈 상태일때
            else if (!GameManager.instance.player.playerCamera.isRotate)
            {
                zero = Quaternion.Euler(-10, -180, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, zero, Time.deltaTime * 30);
            }
        }
    }
}
