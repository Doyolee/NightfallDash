using System.Collections;
using UnityEngine;

public class BossZombie : MonoBehaviour, IDamage
{
    [SerializeField]
    public ZombieData zombieData;   //���� ��ũ���ͺ� 

    public GameObject[] bloods;

    float attackDistance;   //���� ��Ÿ�

    Collider coll;
    Rigidbody rb;
    Vector3 originPos;
    AudioSource sound;
    public AudioClip[] clip;

    Animator anim;

    float stopTimer; // �������� ���ߴ� �ð�

    [HideInInspector]
    public float bossSpeed=0.5f;
    float speed;
    bool isOver = false;
    bool isScream = false;
    private void Awake()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        //���� ��ġ
        originPos = transform.position;
        //���� �Ÿ� (�� �̵��ӵ��� ����Ͽ� ����)
        attackDistance = 5f;
        sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //��ȯ�Ǹ� �ݶ��̴��� ������ٵ� Ȱ��ȭ, �����ݶ��̴� ��Ȱ��ȭ, Walk�ִϸ��̼� ����
        coll.enabled = true;
        rb.isKinematic = false;
        //anim.SetBool("Walk", true);
        playSound();
        StartCoroutine(Scream());
    }
    void Update()
    {
        //stopTimer�� 0���ϸ� ���� ���ǵ�
        //stopTimer�� ������ �ӵ� 0
        if (stopTimer <= 0)
        {
            speed = bossSpeed;
            stopTimer = 0;
        }
        else
        {
            stopTimer -= Time.deltaTime;
            speed = 0;
        }

        //�������� �÷��̾�� ���� �Ÿ��� �Ǿ�����
        if (GameManager.instance.player.isBoss)
        {
            stoprunSound(); //�� ���� ����
            StartCoroutine(Attack());   //���� ��� �ڷ�ƾ
            if (!isOver)
            {
                StartCoroutine(GetPlayerSound());   //�÷��̾� ���϶� ���� �ڷ�ƾ
            }
        }
        //���ӿ����� �Ǹ�
        else if (GameManager.instance.isGameOver)
        {
            stoprunSound(); //�� ���� ����
            anim.SetTrigger("Attack");  //���� �ִϸ��̼� ���
        }
        //���� �������϶�
        else if(!GameManager.instance.player.isBoss)
        {
            //speed�� �°� ������ �̵���
            transform.position = new Vector3(originPos.x, originPos.y, transform.position.z + Time.deltaTime * speed);
            //�÷��̾� ī�޶� �������� x���� ����
            transform.position = new Vector3(GameManager.instance.player.playerCamera.transform.position.x, originPos.y, transform.position.z);

        }

    }

    private void FixedUpdate()
    {
        //�÷��̾���� �Ÿ�
        float dist = Vector3.Distance(GameManager.instance.player.playerCamera.transform.position, transform.position);

        if(!isScream)
        {
            // �Ÿ��� �ָ� �Ҹ��� �鸮�� �ʰ� ����������� ������ Ŀ������ ��ġ�� �����մϴ�.
            float volume = 1f - Mathf.Clamp01(dist/50);
            sound.volume = volume;
        }
        
        //�Ÿ��� ���� ��Ÿ����� ª����
        if (dist <= attackDistance)
        {
            GameManager.instance.map.moveSpeed = 0f;
            GameManager.instance.player.isBoss = true;
            GameManager.instance.isGameOver = true;
        }
    }
    
    //IDamage �������̽�
    public void GetDamage(float damage)
    {
        //rb.AddForce(transform.position - GameManager.instance.player.transform.position * 0.1f, ForceMode.Impulse);

        //�������� ���� ������ stopTime�� �����ؼ� �������� �����.
        //������ ���� ���� ��žŸ���� ���Ѵ�.
        //�ӽ÷� 0.5f
        stopTimer += 0.2f;
        StartCoroutine(hit());
    }
    IEnumerator hit()
    {
        yield return new WaitForSeconds(0.3f);
        sound.PlayOneShot(clip[4]);
        print("hitSound");

        int random = Random.Range(0, 9);

        if (random % 3 == 0)
            bloods[0].SetActive(true);
        else if (random % 3 == 1)
            bloods[1].SetActive(true);
        else
            bloods[2].SetActive(true);

        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < 3; i++)
        {
            bloods[i].SetActive(false);
            print("Blood " + i);
        }

    }

    //���� ���� ��ȿ
    IEnumerator Scream()
    {
        yield return new WaitForSeconds(3.2f); //�ð� ���
        isScream = true;
        stoprunSound(); //�� ���� ����
        anim.SetTrigger("Scream"); //��ȿ �ִϸ��̼� ����
        sound.PlayOneShot(clip[0], 1);  //��ȿ ���� ����
        anim.SetBool("Walk", true); //Walk�ִϸ��̼� �̸� ���� ��ȿ �ִϸ��̼� �����ڸ��� �ٷ� Walk�ִϸ��̼� ����ǵ���
        yield return new WaitForSeconds(3f);
        isScream = false;
        playSound();    //�� ���� ����
    }

    //�÷��̾� ��������� ����
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(GameManager.instance.player.playerCamera.transform.position.x, 0.4f, -28); //������ ��ġ�� �̵�
        transform.rotation = Quaternion.Euler(10, 0, 0);    //ī�޶� �� ���̰� ��¦ �����
        anim.SetTrigger("Attack");  //���� �ִϸ��̼� ����

        yield return new WaitForSeconds(1.5f);
        GameManager.instance.player.cameraEffect.SideBloodEffect(1f);   //���̵�ȭ�鿡 ��
        GameManager.instance.player.cameraEffect.CenterBloodEffect(1f); //���ȭ�鿡 ��
    }

    //�� ����
    public void startrunSound()
    {
        sound.PlayOneShot(clip[1], 1);
    }
    //InvokeRepeating ���� 0.5�ʸ��� ��� �ݺ�����
    public void playSound()
    {
        InvokeRepeating("startrunSound", 0f ,0.5f);
    }
    //CancleInvoke�� �ݺ����� ����
    public void stoprunSound()
    {
        CancelInvoke("startrunSound");
    }

    //�÷��̾� ��������� ���� ����
    IEnumerator GetPlayerSound()
    {
        isOver = true;
        yield return new WaitForSeconds(1f);
        sound.PlayOneShot(clip[2], 1);
        yield return new WaitForSeconds(2f);
        sound.PlayOneShot(clip[3], 1);
        GameManager.instance.player.EarNoiseSound();
    }
}