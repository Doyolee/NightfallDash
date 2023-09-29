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
        /*// ��ġ �Է� ����
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    print("��ġ" + Input.touchCount + "��");
                    isUIPressed = true;
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Ended)
                        touch.phase = TouchPhase.Canceled;
                }
                else
                {
                    print("��ġ" + Input.touchCount);
                    // ī�޶��� ���� ��ǥ �������� �������� ��ġ�� ��ȯ�մϴ�.
                    Vector3 touchPosition = Input.GetTouch(i).position;
                    touchPosition.z = Camera.main.farClipPlane;
                    gunPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                    // �ѱ��� ������ ����մϴ�.
                    Vector3 direction = gunPoint - transform.position;
                    direction.Normalize();
                    // ���� ���͸� �ѱ��� ȸ�� ������ ��ȯ�մϴ�.
                    transform.rotation = Quaternion.LookRotation(direction);
                    isFire = true;
                    isUIPressed = false;
                    StartCoroutine(resetPos());
                }
            }
        }*/
        // ��ġ �Է� ����
        /*if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!EventSystem.current.IsPointerOverGameObject(i))
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began) //�ش� ��ġ�� ���۵Ǿ��ٸ�
                    {
                        // ī�޶��� ���� ��ǥ �������� �������� ��ġ�� ��ȯ�մϴ�.
                        Vector3 touchPosition = Input.GetTouch(i).position;
                        touchPosition.z = Camera.main.farClipPlane;
                        gunPoint = Camera.main.ScreenToWorldPoint(touchPosition);
                        // �ѱ��� ������ ����մϴ�.
                        Vector3 direction = gunPoint - transform.position;
                        direction.Normalize();
                        // ���� ���͸� �ѱ��� ȸ�� ������ ��ȯ�մϴ�.
                        transform.rotation = Quaternion.LookRotation(direction);
                        isFire = true;
                        isUIPressed = false;
                        StartCoroutine(resetPos());
                    }
                }

                print("��ġ" + Input.touchCount + EventSystem.current.IsPointerOverGameObject(i));
            }
        }*/

        time += Time.deltaTime;
        //�� �������� ��
        if (!isGetGun)
        {
            //���� ������ �Ʒ��� ����
            float t = Mathf.Clamp01(time / waitTime);
            transform.rotation = Quaternion.Lerp(getGun, zeroPos, t);
            if (transform.rotation == zeroPos)
            {
                isGetGun = true;
            }
        }
        //�� ���� �� isGetGun�� true�϶�
        else
        {
            //�÷��̴� �ڵ��� ���� �����϶�
            if (!GameManager.instance.player.playerCamera.isRotate)
            {
                zero = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, zero, Time.deltaTime * 30);
            }
            //�÷��̴� �ڵ� �����϶�
            else if (!GameManager.instance.player.playerCamera.isRotate)
            {
                zero = Quaternion.Euler(-10, -180, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, zero, Time.deltaTime * 30);
            }
        }
    }
}
