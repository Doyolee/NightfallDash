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

    //�÷��̾� �����̴� �ӵ�
    public float playerSpeed = 10f;

    //�÷��̾� �Ѿ����� �ӵ�
    float fallDuration = 0.5f; // 1�ʿ� ���ļ� �Ѿ���
    float passTime = 0f;

    bool isColliding = false;
    bool isContact = false;
    bool isCar = false;
    bool isStart = false;

    Quaternion startRotation;
    Quaternion targetRotation; //�Ѿ��� �� ����

    // �¿�� ������ �� �������� �ӵ��� ����
    float rotateSpeed = 90f;
    public float zMoveRotation = 30;
    float xInput;
    public float Ac = 2f;

    //�浹 ���� - ������ = 1, ���� = -1
    float collideDirection;

    Rigidbody rb;
    Transform tr;

    Quaternion originRot;

    Collider coll;  //�÷��̾� �ݶ��̴�
    public Collider[] bikeColl; //����ũ �ݶ��̴�

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


        //������� ���� �� 3�ʵ��� �̸�� �÷��̾� �Ѿ����� �޼���
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

        //�����ϴ� ���� ������̿� ��� ����
        StartCoroutine(playerCamera.ShakeCameraAlways(0.3f));

        //�¿�� �����̱� ���� �Է°��� ����
        xInput = Input.acceleration.x * 3; // ���ӵ� ������ x���� xInput�� ����

        //�Է°��� �ִ�ġ ����
        if (xInput > 1)
            xInput = 1;
        else if (xInput < -1)
            xInput = -1f;

        //�������� �ʰ� �ִ� ���¿����� ������̰� �¿�� �����̰� �ϱ� ���� �⺻ ��ġ ����
        if (xInput >= 0)
        {
            xInput+=0.2f;
        }
        else
        {
            xInput +=-0.2f;
        }

        //���巹�Ͽ� �ε�������
        if (isContact)
        {
            crashTime += Time.deltaTime;

            //������ �ݴ�������� �̵�
            xInput = collideDirection * -1;

            if (crashTime >= 0.2f)
            {
                crashTime = 0;
                isContact = false;
            }
        }

        //������ ������Ʈ ���� �ε�������
        if (isCar)
        {
            carTime += Time.deltaTime;

            //������ �ݴ�������� �̵�
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

            //�����̴� ���⿡ ���� ������ ����̴� �ڷ�ƾ
            StartCoroutine(MoveRotate(xInput));
        }
    }


    public IEnumerator MoveRotate(float _input)
    {
        //xInput ���� ������ ���⿡ �°� ȸ��
        //xInput ���� 0�̸� ���� ������ ���ƿ�
        //�Է��� ���� �� xInput���� 1,-1�̹Ƿ� zMoveRotation�� ���� ������ ���� 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, _input * zMoveRotation), rotateSpeed * Time.deltaTime);

        yield return null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isGameOver) return;

        //�ε��� ��ֹ��� ��ƼŬ �ý��� ������Ʈ ����
        ParticleSystem ps = collision.gameObject.GetComponentInChildren<ParticleSystem>();

        //�÷��̾� �ݶ��̴��� ��ġ���� �������� ���� collider.bounds�� ������ ����
        var playerCollider = GetComponent<Collider>().bounds;
        float collidePoint = collision.contacts[0].point.x;
        //�ε��� �ݶ��̴��� ��ġ���� �������� ���� collider.bounds�� ������ ����
        var Collisioncollider = collision.gameObject.GetComponent<Collider>().bounds;

        //center.x�� ��ġ�� ���� �浹 ������ ������ ����
        //��ֹ��� �÷��̾� �������̸� 1, �����̸� -1
        if (playerCollider.center.x <= Collisioncollider.center.x)
            collideDirection = 1;
        else
            collideDirection = -1;

        //���� �ε����� ��
        if (collision.gameObject.CompareTag("ZOMBIE"))
        {
            IDamage damage = collision.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                //���� ü�� ����
                damage.GetDamage(50);

                sound.PlayOneShot(clip[5], 1);
            }

            Zombie zombie = collision.gameObject.GetComponent<Zombie>();

            //ī�޶� �Ǹ� Ƣ��� �޼���
            cameraEffect.CameraBloodEff(collideDirection);

            //ī�޶� ��鸲
            StartCoroutine(playerCamera.ShakeCamera(0.5f, 1f));

            //�ε����� �� ���� ü���� �����ִٸ�
            //���� ��� ī�޶� �۵� �� ���� ����
            if (zombie.health > 50)
            {
                cameraShake.isZombieCrash = true;
                GameManager.instance.isGameOver = true;
                rb.isKinematic = true;
            }

        }
        //�巳�뿡 �ε����� ��
        else if (collision.gameObject.CompareTag("BARREL"))
        {
            IDamage damage = collision.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                //�巳�� ü�� ����
                damage.GetDamage(100);
            }
            //ī�޶� ��鸲
            StartCoroutine(playerCamera.ShakeCamera(0.5f, 1f));

            //���� ����
            sound.PlayOneShot(clip[3], 1);

            if (GameManager.instance.isGameOver)
                return;

            rb.AddForce(collision.transform.position - transform.position * 15, ForceMode.Impulse);
            BarrelKnockback();
        }
        //���巹�Ͽ� �ε����� ��
        else if (collision.gameObject.CompareTag("SIDEWALL"))
        {
            if (GameManager.instance.isGameOver)
                return;
            //��ֹ��� �÷��̾� �������̸� 1, �����̸� -1
            if (tr.position.x < collision.transform.position.x)
                collideDirection = 1;
            else
                collideDirection = -1;

            StartCoroutine(playerCamera.ShakeCamera(0.3f, 4f));
            sound.PlayOneShot(clip[2], 1);
            PlayerKnockBack(10);
        }
        //��ֹ��� �ε����� ��
        else if (collision.gameObject.CompareTag("OBSTACLE"))
        {

            //��ƼŬ�ý����� �ִٸ� ����
            if (ps != null)
            {
                ps.Play();
            }

            //collider.bounds.max -> ����
            //clollider.bounds.min -> ����
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

                //�浹 �� ������ �޼��带 ����  bool��
                isColliding = true;
                GameManager.instance.GameOver();

            }

        }
        //�ε����� ������ ���� �����Ǵ� ��ֹ�
        else if (collision.gameObject.CompareTag("HARDOBSTACLE"))
        {
            StartCoroutine(playerCamera.ShakeCamera(0.7f, 10f));

            //�ε��� �ݴ� �������� �Ѿ����� ������ Ÿ�ٷ����̼����� ����
            targetRotation = Quaternion.Euler(0, 180, -collideDirection * 90f);
            sound.PlayOneShot(clip[3], 1);

            //�浹 �� ������ �޼��带 ����  bool��
            isColliding = true;
            GameManager.instance.GameOver();
        }
    }

    //��ֹ��� �ε����� �� ƨ���� ������ �޼���
    void PlayerKnockBack(float power)
    {
        rb.AddForce(Vector3.left * collideDirection * power, ForceMode.Impulse);

        isContact = true;
    }
    //��ֹ��� �ε����� �� ƨ���� ������ �޼���
    void CarKnockBack(float power)
    {
        rb.AddForce(Vector3.left * collideDirection * power, ForceMode.Impulse);

    }

    void PlayerFallDown()
    {
        //�� ȿ��
        cameraEffect.SideBloodEffect(0.5f);

        //�ð��� ���� Ÿ�ٷ����̼����� ȸ��
        passTime += Time.deltaTime;
        float t = Mathf.Clamp01(passTime / fallDuration);
        tr.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        coll.enabled = false;   //�÷��̾� �ݶ��̴� ��Ȱ��ȭ
        rb.constraints = RigidbodyConstraints.None; //������, �����̼� ��������
        bikeColl[0].isTrigger = false;  //����ũ �ݶ��̴� Ȱ��ȭ
        bikeColl[1].isTrigger = false;  //����ũ �ݶ��̴� Ȱ��ȭ

        //������ �Ѿ����� �ѹ� �� ī�޶� ��鸲
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

    //�巳�� ���� �� ���� ���� �޼���
    void BarrelKnockback()
    {
        //�� ȿ��
        cameraEffect.SideBloodEffect(1f);

        //�ð��� ���� Ÿ�ٷ����̼����� ȸ��
        passTime += Time.deltaTime;
        float t = Mathf.Clamp01(passTime / 0.2f);
        tr.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        coll.enabled = false;   //�÷��̾� �ݶ��̴� ��Ȱ��ȭ
        rb.constraints = RigidbodyConstraints.None; //������, �����̼� ��������
        bikeColl[0].isTrigger = false;  //����ũ �ݶ��̴� Ȱ��ȭ
        bikeColl[1].isTrigger = false;  //����ũ �ݶ��̴� Ȱ��ȭ
        GameManager.instance.GameOver();
        cameraShake.CrashBarrelCamera();
    }

    //���� ���۰� ������� ����
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

    //������� �Ҹ� �����ϴ� �޼���
    public void StopBikeSound()
    {
        bikeSound.Stop();
    }
    public void RestartBikeSound()
    {
        bikeSound.Play ();
    }

    //���� ���� �� �̸����
    public void EarNoiseSound()
    {
        sound.PlayOneShot(clip[4], 0.7f);
    }
}