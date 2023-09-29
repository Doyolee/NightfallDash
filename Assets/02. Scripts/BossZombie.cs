using System.Collections;
using UnityEngine;

public class BossZombie : MonoBehaviour, IDamage
{
    [SerializeField]
    public ZombieData zombieData;   //좀비 스크립터블 

    public GameObject[] bloods;

    float attackDistance;   //공격 사거리

    Collider coll;
    Rigidbody rb;
    Vector3 originPos;
    AudioSource sound;
    public AudioClip[] clip;

    Animator anim;

    float stopTimer; // 보스좀비가 멈추는 시간

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
        //스폰 위치
        originPos = transform.position;
        //공격 거리 (맵 이동속도에 비례하여 증가)
        attackDistance = 5f;
        sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //소환되면 콜라이더와 리지드바디 활성화, 공격콜라이더 비활성화, Walk애니메이션 실행
        coll.enabled = true;
        rb.isKinematic = false;
        //anim.SetBool("Walk", true);
        playSound();
        StartCoroutine(Scream());
    }
    void Update()
    {
        //stopTimer가 0이하면 원래 스피드
        //stopTimer가 있으면 속드 0
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

        //보스좀비가 플레이어랑 일정 거리가 되었을때
        if (GameManager.instance.player.isBoss)
        {
            stoprunSound(); //발 사운드 종료
            StartCoroutine(Attack());   //공격 모션 코루틴
            if (!isOver)
            {
                StartCoroutine(GetPlayerSound());   //플레이어 죽일때 사운드 코루틴
            }
        }
        //게임오버가 되면
        else if (GameManager.instance.isGameOver)
        {
            stoprunSound(); //발 사운드 종료
            anim.SetTrigger("Attack");  //공격 애니메이션 재생
        }
        //게임 진행중일때
        else if(!GameManager.instance.player.isBoss)
        {
            //speed에 맞게 앞으로 이동함
            transform.position = new Vector3(originPos.x, originPos.y, transform.position.z + Time.deltaTime * speed);
            //플레이어 카메라 기준으로 x축을 따라감
            transform.position = new Vector3(GameManager.instance.player.playerCamera.transform.position.x, originPos.y, transform.position.z);

        }

    }

    private void FixedUpdate()
    {
        //플레이어와의 거리
        float dist = Vector3.Distance(GameManager.instance.player.playerCamera.transform.position, transform.position);

        if(!isScream)
        {
            // 거리가 멀면 소리가 들리지 않고 가까워질수록 볼륨이 커지도록 수치를 적용합니다.
            float volume = 1f - Mathf.Clamp01(dist/50);
            sound.volume = volume;
        }
        
        //거리가 공격 사거리보다 짧으면
        if (dist <= attackDistance)
        {
            GameManager.instance.map.moveSpeed = 0f;
            GameManager.instance.player.isBoss = true;
            GameManager.instance.isGameOver = true;
        }
    }
    
    //IDamage 인터페이스
    public void GetDamage(float damage)
    {
        //rb.AddForce(transform.position - GameManager.instance.player.transform.position * 0.1f, ForceMode.Impulse);

        //데미지를 입을 때마다 stopTime이 증가해서 보스좀비가 멈춘다.
        //데미지 별로 더할 스탑타임을 정한다.
        //임시로 0.5f
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

    //게임 시작 포효
    IEnumerator Scream()
    {
        yield return new WaitForSeconds(3.2f); //시간 대기
        isScream = true;
        stoprunSound(); //발 사운드 종료
        anim.SetTrigger("Scream"); //포효 애니메이션 실행
        sound.PlayOneShot(clip[0], 1);  //포효 사운드 실행
        anim.SetBool("Walk", true); //Walk애니메이션 미리 실행 포효 애니메이션 끝나자마자 바로 Walk애니메이션 실행되도록
        yield return new WaitForSeconds(3f);
        isScream = false;
        playSound();    //발 사운드 시작
    }

    //플레이어 붙잡았을때 공격
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(GameManager.instance.player.playerCamera.transform.position.x, 0.4f, -28); //적절한 위치로 이동
        transform.rotation = Quaternion.Euler(10, 0, 0);    //카메라에 잘 보이게 살짝 기울임
        anim.SetTrigger("Attack");  //공격 애니메이션 실행

        yield return new WaitForSeconds(1.5f);
        GameManager.instance.player.cameraEffect.SideBloodEffect(1f);   //사이드화면에 피
        GameManager.instance.player.cameraEffect.CenterBloodEffect(1f); //가운데화면에 피
    }

    //발 사운드
    public void startrunSound()
    {
        sound.PlayOneShot(clip[1], 1);
    }
    //InvokeRepeating 으로 0.5초마다 계속 반복실행
    public void playSound()
    {
        InvokeRepeating("startrunSound", 0f ,0.5f);
    }
    //CancleInvoke로 반복실행 해제
    public void stoprunSound()
    {
        CancelInvoke("startrunSound");
    }

    //플레이어 붙잡았을때 사운드 실행
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