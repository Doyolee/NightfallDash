using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{
    Vector3 originPos;
    Quaternion originRot;

    void Start()
    {
        //ī�޶��� ���� ��ġ ����
        originPos = transform.localPosition;
        originRot = Quaternion.Euler(0, -180, 0);

    }

    //ī�޶� ��鸲 ���� �ð�, ��鸮�� ����
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

        //ī�޶� ��ġ ���� ����
        transform.localPosition = originPos;
    }

    // �÷��̾� �нú� ����
    public IEnumerator ShakeCameraAlways(float magnitude)
    {
        //�ڷ� ���ƺ��� �߿��� ���� 10��, x��� y������ ��鸲
        if (isRotate)
        {
            magnitude *= 10;
            Vector3 randomShake = new Vector3(Random.insideUnitSphere.x * magnitude, Random.insideUnitSphere.y * magnitude, Random.insideUnitSphere.z * magnitude);
            transform.localPosition = originPos + randomShake;
        }
        //�� �ܿ��� y�����θ� ��鸲
        else
        {
            Vector3 randomShake = new Vector3(0, Random.insideUnitSphere.y * magnitude, 0);
            transform.localPosition = originPos + randomShake;
        }

        yield return null;
    }

    public bool isRotate = false;
    //�ڷ� �� �� ȸ���� ����
    Quaternion rot = Quaternion.Euler(0f, 359f, 0f);
    //��������� ����
    Quaternion bikeRot = Quaternion.Euler(0f, -180f, 0f);
    //������� �ݶ��̴��� ����
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
