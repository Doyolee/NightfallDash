using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject bike;
    public Collider[] bikes;
    public CameraEffect cameraEffect;
    public AudioClip hit;
    AudioSource sound;

    Rigidbody rb;
    Quaternion originRot;
    Quaternion zomobieCrashRot;

    Vector3 origin;
    Vector3 smashPos;
    Quaternion oriRot;
    Quaternion dropRot;
    Quaternion lookBoss;

    float rotTime;
    float rotateDuration = 1f;
    float posTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1.0f;
        originRot = Quaternion.Euler(0, 180, 0);
        zomobieCrashRot = Quaternion.Euler(-90f, 180, 0);
        sound = GetComponent<AudioSource>();
    }

    public void CrashCarCamera()
    {
        rb.isKinematic = false;
        StartCoroutine(ThrowCamera(transform.position));
    }
    public void CrashBarrelCamera()
    {
        rb.isKinematic = false;
        StartCoroutine(ThrowCamera(transform.position));
    }

    IEnumerator ThrowCamera(Vector3 dir)
    {
        rb.mass = 50;
        rb.AddForce(dir * 2, ForceMode.Impulse);   //카메라 날림
        rb.useGravity = true;   //중력 영향
        rb.constraints = RigidbodyConstraints.None; //포지션, 로테이션 고정해제
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY; //리지드바디 포지션X, Y 로테이션 Y 고정
        yield return new WaitForSeconds(2f);
        transform.SetParent(null);
        yield return new WaitForSeconds(1f);
        //rb.isKinematic = true;
    }

    bool isRotate = false;
    public bool isZombieCrash = false;
    bool isDead = false;
    bool isLookBoss = false;
    bool isEnd = false;
    IEnumerator CrashZombie()
    {
        if (!isRotate)
        {
            rotTime = 0f;

        }
        float t = Mathf.Clamp01(rotTime / rotateDuration);
        transform.localRotation = Quaternion.Lerp(originRot, zomobieCrashRot, t);
        Vector3 deadPos = new Vector3(0, 70, 120);
        transform.localPosition = deadPos;
        isRotate = true;
        if (t == 1)
        {
            isDead = true;
            rb.isKinematic = true;
            yield return new WaitForSeconds(1);

            GameObject zombie = GameManager.instance.poolManager.GetPools(14);
            Quaternion zombieRot = Quaternion.Euler(10, 0, 0);
            zombie.transform.localPosition = new Vector3(transform.position.x, 1.3f, -34);
            zombie.transform.localRotation = zombieRot;
            zombie.GetComponent<Rigidbody>().isKinematic = true;
            zombie.GetComponent<Collider>().enabled = false;
            zombie.GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(0.4f);
            cameraEffect.SideBloodEffect(1f);
            sound.PlayOneShot(hit, 1);
            yield return new WaitForSeconds(0.8f);
            cameraEffect.CenterBloodEffect(1f);
            sound.PlayOneShot(hit, 1);
        }
        yield return null;
    }

    private void Update()
    {
        if(isEnd) return;

        rotTime += Time.deltaTime;
        posTime += Time.deltaTime;

        if (isZombieCrash)
        {
            if (!isDead)
                StartCoroutine(CrashZombie());
        }
        else if (GameManager.instance.player.isBoss)
        {
            if (!isDead)
            {
                posTime = 0;
                transform.SetParent(null);
                SmashBike();
                rb.useGravity = true;   //중력 영향
                rb.constraints = RigidbodyConstraints.FreezeAll; //포지션, 로테이션 고정
                origin = transform.localPosition;
                smashPos = new Vector3(transform.localPosition.x, 1, transform.localPosition.z + 10);
                oriRot = Quaternion.Euler(0, 0, 0);
                dropRot = Quaternion.Euler(0, -70, 0);
                lookBoss = Quaternion.Euler(-90, -70, 0);
                isDead = true;
            }
            else
            {
                if (isLookBoss)
                {
                    lootAtBoss();
                    return;
                }

                if (!isRotate)
                    SmashCamera();
                else if (isRotate)
                    SmashRotateCamera();
            }
        }
    }

    void SmashCamera()
    {
        float t = Mathf.Clamp01(posTime / 0.5f);
        float easeInOutT = Mathf.SmoothStep(0f, 1f, t);
        transform.position = Vector3.Lerp(origin, smashPos, easeInOutT);
        if (transform.position.z >= -24)
        {
            isRotate = true;
            posTime = 0;
        }
    }
    void SmashRotateCamera()
    {
        float t = Mathf.Clamp01(posTime / 1f);
        float easeInOutT = Mathf.SmoothStep(0f, 1f, t);
        transform.localRotation = Quaternion.Lerp(oriRot, dropRot, easeInOutT);
        if (transform.localRotation.eulerAngles.y == 290)
        {
            isLookBoss = true;
            posTime = 0;
        }
    }
    void lootAtBoss()
    {
        float t = Mathf.Clamp01(posTime);
        float easeInOutT = Mathf.SmoothStep(0f, 1f, t);
        transform.localRotation = Quaternion.Lerp(dropRot, lookBoss, easeInOutT);
        if (transform.localPosition.x <= -89)
        {
            isEnd = true;
            posTime = 0; 
        }
    }

    void SmashBike()
    {
        bike.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bike.GetComponent<Rigidbody>().AddForce(Vector3.left * 500, ForceMode.Impulse);
        bike.GetComponent<Collider>().enabled = false;
        bikes[0].isTrigger = false;
        bikes[1].isTrigger = false;
    }
}
