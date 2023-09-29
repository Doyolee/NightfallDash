using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{
    Vector3 originPos;
    Quaternion originRot;

    void Start()
    {
        //카메라의 원래 위치 저장
        originPos = transform.localPosition;
        originRot = Quaternion.Euler(0, -180, 0);

    }

    //카메라를 흔들림 지속 시간, 흔들리는 강도
    public IEnumerator ShakeCamera(float duration = 1f, float magnitude = 1)
    {
        float passTime = 0f;

        while (passTime < duration)
        {

            Vector3 randomShake = Random.insideUnitSphere * magnitude;
            transform.localPosition = originPos + randomShake;

            passTime += Time.deltaTime;
            yield return 0.2f;

        }

        //카메라 위치 원상 복구
        transform.localPosition = originPos;
    }

    // 플레이어 패시브 진동
    public IEnumerator ShakeCameraAlways(float magnitude)
    {
        //뒤로 돌아보는 중에는 진동 10배, x축과 y축으로 흔들림
        if (isRotate)
        {
            magnitude *= 10;
            Vector3 randomShake = new Vector3(Random.insideUnitSphere.x * magnitude, Random.insideUnitSphere.y * magnitude, Random.insideUnitSphere.z * magnitude);
            transform.localPosition = originPos + randomShake;
        }
        //그 외에는 y축으로만 흔들림
        else
        {
            Vector3 randomShake = new Vector3(0, Random.insideUnitSphere.y * magnitude, 0);
            transform.localPosition = originPos + randomShake;
        }

        yield return null;
    }

    public bool isRotate = false;
    //뒤로 돌 때 회전할 각도
    Quaternion rot = Quaternion.Euler(0f, 359f, 0f);
    //오토바이의 각도
    Quaternion bikeRot = Quaternion.Euler(0f, -180f, 0f);
    //오토바이 콜라이더의 각도
    Quaternion bikeCollRot = Quaternion.Euler(90f, -180f, 0f);

    public Transform[] bike;
    Quaternion bike1 = Quaternion.Euler(0, -180, 0);
    Quaternion bike2 = Quaternion.Euler(90, -180, 0);

    IEnumerator LookBack()
    {
        bike[0].transform.rotation = bikeRot;
        bike[1].transform.rotation = bikeCollRot;
        transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, rot, 400 * Time.deltaTime);
        yield return null;
    }
    IEnumerator LookFront()
    {
        if (transform.parent.rotation.eulerAngles.y == originRot.eulerAngles.y)
        {
            yield break;
        }

        bike[0].transform.rotation = bike1;
        bike[1].transform.rotation = bike2;

        transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, originRot, 400 * Time.deltaTime);
        yield return null;
    }

    private void Update()
    {
        if (GameManager.instance.isGameOver)
            return;

        if (isRotate)
        {
            StartCoroutine(LookBack());
        }
        else
        {
            StartCoroutine(LookFront());
        }
    }
}
