using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraMovement playerCamera;
    public Transform bike;
    public CameraEffect cameraEffect;
    public CameraShake cameraShake;
    public AudioClip[] clip;
    AudioSource sound;
    public AudioSource bikeSound;

    //플레이어 움직이는 속도
    public float playerSpeed = 10f;

    //플레이어 넘어지는 속도
    float fallDuration = 0.5f; // 1초에 걸쳐서 넘어짐
    float passTime = 0f;

    bool isColliding = false;
    bool isContact = false;
    bool isCar = false;
    bool isStart = false;

    Quaternion startRotation;
    Quaternion targetRotation; //넘어질 때 각도

    // 좌우로 움직일 때 기울어지는 속도와 각도
    float rotateSpeed = 90f;
    public float zMoveRotation = 30;
    float xInput;
    public float Ac = 2f;

    //충돌 방향 - 오른쪽 = 1, 왼쪽 = -1
    float collideDirection;

    Rigidbody rb;
    Transform tr;

    Quaternion originRot;

    Collider coll;  //플레이어 콜라이더
    public Collider[] bikeColl; //바이크 콜라이더

    public bool isBoss = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        startRotation = tr.rotation;
        coll = GetComponent<Collider>();
        sound = GetComponent<AudioSource>();
        bikeSound.PlayOneShot(clip[0], 1);
    }

    void Update()
    {
        if (!GameManager.instance.isStart)
            return;


        //오토바이 전복 후 3초동안 이명과 플레이어 넘어지는 메서드
        if (isColliding && passTime <= 3f)
        {
            EarNoiseSound();
            PlayerFallDown();
        }

        if (GameManager.instance.isGameOver || isBoss)
        {
            bikeSound.volume -= Time.deltaTime * 0.1f;
            return;
        }

        //진행하는 동안 오토바이에 계속 진동
        StartCoroutine(playerCamera.ShakeCameraAlways(0.3f));

        //좌우로 움직이기 위해 입력값을 저장
        xInput = Input.acceleration.x * 3; // 가속도 센서의 x값을 xInput에 대입

        //입력값의 최대치 설정
        if (xInput > 1)
            xInput = 1;
        else if (xInput < -1)
            xInput = -1f;

        //조종하지 않고 있는 상태에서도 오토바이가 좌우로 움직이게 하기 위해 기본 수치 적용
        if (xInput >= 0)
        {
            xInput+=0.2f;
        }
        else
        {
            xInput +=-0.2f;
        }

        //가드레일에 부딪혔을때
        if (isContact)
        {
            crashTime += Time.deltaTime;

            //강제로 반대방향으로 이동
            xInput = collideDirection * -1;

            if (crashTime >= 0.2f)
            {
                crashTime = 0;
                isContact = false;
            }
        }

        //차량류 오브젝트 옆에 부딪혔을때
        if (isCar)
        {
            carTime += Time.deltaTime;

            //강제로 반대방향으로 이동
            xInput = collideDirection * -1;

            if (carTime >= 0.1f)
            {
                carTime = 0;
                isCar = false;
            }
        }

        if (!playerCamera.isRotate)
        {
            Vector3 newVelocity = new Vector3(xInput* 15f, 0f, 0f);
            rb.velocity = newVelocity;

            //움직이는 방향에 따라 각도를 기울이는 코루틴
            StartCoroutine(MoveRotate(xInput));
        }
    }


    public IEnumerator MoveRotate(float _input)
    {
        //xInput 값이 들어오면 방향에 맞게 회전
        //xInput 값이 0이면 원래 각도로 돌아옴
        //입력이 있을 때 xInput값은 1,-1이므로 zMoveRotation에 곱해 방향을 구현 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, _input * zMoveRotation), rotateSpeed * Time.deltaTime);

        yield return null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isGameOver) return;

        //부딪힌 장애물의 파티클 시스템 오브젝트 저장
        ParticleSystem ps = collision.gameObject.GetComponentInChildren<ParticleSystem>();

        //플레이어 콜라이더의 수치들을 가져오기 위해 collider.bounds를 변수로 저정
        var playerCollider = GetComponent<Collider>().bounds;
        float collidePoint = collision.contacts[0].point.x;
        //부딪힌 콜라이더의 수치들을 가져오기 위해 collider.bounds를 변수로 저정
        var Collisioncollider = collision.gameObject.GetComponent<Collider>().bounds;

        //center.x의 위치에 따라 충돌 방향을 변수에 저장
        //장애물이 플레이어 오른쪽이면 1, 왼쪽이면 -1
        if (playerCollider.center.x <= Collisioncollider.center.x)
            collideDirection = 1;
        else
            collideDirection = -1;

        //좀비에 부딪혔을 때
        if (collision.gameObject.CompareTag("ZOMBIE"))
        {
            IDamage damage = collision.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                //좀비 체력 감소
                damage.GetDamage(50);

                sound.PlayOneShot(clip[5], 1);
            }

            Zombie zombie = collision.gameObject.GetComponent<Zombie>();

            //카메라에 피를 튀기는 메서드
            cameraEffect.CameraBloodEff(collideDirection);

            //카메라 흔들림
            StartCoroutine(playerCamera.ShakeCamera(0.5f, 1f));

            //부딪혔을 때 좀비가 체력이 남아있다면
            //좀비 사망 카메라 작동 및 게임 오버
            if (zombie.health > 50)
            {
                cameraShake.isZombieCrash = true;
                GameManager.instance.isGameOver = true;
                rb.isKinematic = true;
            }

        }
        //드럼통에 부딪혔을 때
        else if (collision.gameObject.CompareTag("BARREL"))
        {
            IDamage damage = collision.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                //드럼통 체력 감소
                damage.GetDamage(100);
            }
            //카메라 흔들림
            StartCoroutine(playerCamera.ShakeCamera(0.5f, 1f));

            //폭발 사운드
            sound.PlayOneShot(clip[3], 1);

            if (GameManager.instance.isGameOver)
                return;

            rb.AddForce(collision.transform.position - transform.position * 15, ForceMode.Impulse);
            BarrelKnockback();
        }
        //가드레일에 부딪혔을 때
        else if (collision.gameObject.CompareTag("SIDEWALL"))
        {
            if (GameManager.instance.isGameOver)
                return;
            //장애물이 플레이어 오른쪽이면 1, 왼쪽이면 -1
            if (tr.position.x < collision.transform.position.x)
                collideDirection = 1;
            else
                collideDirection = -1;

            StartCoroutine(playerCamera.ShakeCamera(0.3f, 4f));
            sound.PlayOneShot(clip[2], 1);
            PlayerKnockBack(10);
        }
        //장애물에 부딪혔을 때
        else if (collision.gameObject.CompareTag("OBSTACLE"))
        {

            //파티클시스템이 있다면 실행
            if (ps != null)
            {
                ps.Play();
            }

            //collider.bounds.max -> 우측
            //clollider.bounds.min -> 좌측
            if (Collisioncollider.size.x < 2 || collidePoint > Collisioncollider.max.x - 0.6f || playerCollider.center.x < Collisioncollider.min.x + 0.6f)
            {

                StartCoroutine(playerCamera.ShakeCamera(0.3f, 5f));
                PlayerKnockBack(10);
                sound.PlayOneShot(clip[2], 1);
            }
            else
            {

                StartCoroutine(playerCamera.ShakeCamera(0.7f, 10f));
                sound.PlayOneShot(clip[3], 1);

                targetRotation = Quaternion.Euler(0, 180, -collideDirection * 90f);

                //충돌 시 실행할 메서드를 위한  bool값
                isColliding = true;
                GameManager.instance.GameOver();

            }

        }
        //부딪히면 무조건 게임 오버되는 장애물
        else if (collision.gameObject.CompareTag("HARDOBSTACLE"))
        {
            StartCoroutine(playerCamera.ShakeCamera(0.7f, 10f));

            //부딪힌 반대 방향으로 넘어지는 각도를 타겟로테이션으로 저장
            targetRotation = Quaternion.Euler(0, 180, -collideDirection * 90f);
            sound.PlayOneShot(clip[3], 1);

            //충돌 시 실행할 메서드를 위한  bool값
            isColliding = true;
            GameManager.instance.GameOver();
        }
    }

    //장애물에 부딪혔을 떄 튕겨져 나가는 메서드
    void PlayerKnockBack(float power)
    {
        rb.AddForce(Vector3.left * collideDirection * power, ForceMode.Impulse);

        isContact = true;
    }
    //장애물에 부딪혔을 떄 튕겨져 나가는 메서드
    void CarKnockBack(float power)
    {
        rb.AddForce(Vector3.left * collideDirection * power, ForceMode.Impulse);

    }

    void PlayerFallDown()
    {
        //피 효과
        cameraEffect.SideBloodEffect(0.5f);

        //시간에 따라 타겟로테이션으로 회전
        passTime += Time.deltaTime;
        float t = Mathf.Clamp01(passTime / fallDuration);
        tr.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        coll.enabled = false;   //플레이어 콜라이더 비활성화
        rb.constraints = RigidbodyConstraints.None; //포지션, 로테이션 고정해제
        bikeColl[0].isTrigger = false;  //바이크 콜라이더 활성화
        bikeColl[1].isTrigger = false;  //바이크 콜라이더 활성화

        //완전히 넘어지면 한번 더 카메라 흔들림
        if (tr.rotation == targetRotation)
        {
            StartCoroutine(playerCamera.ShakeCamera(1, 0.1f));
            isColliding = false;

            cameraShake.CrashCarCamera();
        }
    }

    public Vector3 gunPoint;

    public bool getGun;
    public GameObject Items;

    public void deleteItem()
    {
        Items.SetActive(false);
    }

    float crashTime;
    float carTime;

    //드럼통 폭발 시 게임 오버 메서드
    void BarrelKnockback()
    {
        //피 효과
        cameraEffect.SideBloodEffect(1f);

        //시간에 따라 타겟로테이션으로 회전
        passTime += Time.deltaTime;
        float t = Mathf.Clamp01(passTime / 0.2f);
        tr.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        coll.enabled = false;   //플레이어 콜라이더 비활성화
        rb.constraints = RigidbodyConstraints.None; //포지션, 로테이션 고정해제
        bikeColl[0].isTrigger = false;  //바이크 콜라이더 활성화
        bikeColl[1].isTrigger = false;  //바이크 콜라이더 활성화
        GameManager.instance.GameOver();
        cameraShake.CrashBarrelCamera();
    }

    //게임 시작과 오토바이 사운드
    public void StartBikeSound()
    {
        if (!isStart)
        {
            bikeSound.loop = true;
            bikeSound.clip = clip[1];
            bikeSound.Play();
        }
        isStart = true;
    }

    //오토바이 소리 조정하는 메서드
    public void StopBikeSound()
    {
        bikeSound.Stop();
    }
    public void RestartBikeSound()
    {
        bikeSound.Play ();
    }

    //게임 오버 시 이명사운드
    public void EarNoiseSound()
    {
        sound.PlayOneShot(clip[4], 0.7f);
    }
}