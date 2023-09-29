using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    private float fireTimer;
    float fireLateTime;

    public Transform firePos;
    public GameObject animArrow;
    Rigidbody rb;
    public Collider[] coll;
    AudioSource sound;

    public RotateGun rotateGun;

    bool isDead = false;    //플레이어가 사망유무

    Animator anim;

    bool canShoot;  //준비 모션중 발포안됨

    public ItemData itemData;

    float damage;
    int ammo;
    int BulletType;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        ammo = itemData.Ammo;   //총알 수
        if (UserManager.userInstance.perks[3]) ammo += 3;
        damage = itemData.Damage;   // 총 데미지
        fireLateTime = itemData.FireTime;   //발사 시간
        BulletType = itemData.BulletType;   //총알
        anim.SetBool("Holding", true);
        canShoot = false; //준비 모션중 발포안됨
        isDead = false;
        coll[0].enabled = false;
        coll[1].enabled = false;

        StartCoroutine(GetGun());   //총 획득 모션

        GameManager.instance.ammo = ammo;
        GameManager.instance.maxAmmo = ammo;
    }

    float waitTime;
    void Update()
    {
        playerPosition = GameManager.instance.player.transform.position + Vector3.forward * 10f; // 현재 오브젝트의 위치  
        if (isDead)
            return;

        fireTimer += Time.deltaTime;
        if (rotateGun.isFire && fireTimer >= fireLateTime && ammo >= 1 && canShoot)
        {
            //발사 애니메이션 실행
            anim.SetBool("Fire", true);
            //SpereCastAll을 통해 전방 일정 범위 안에있는 오브젝트는 다 데미지 받음
            AttackShpere();
            //발사 시간 초기화
            fireTimer = 0;
            rotateGun.isFire = false;
        }
        //플레이어가 죽었으면
        if (GameManager.instance.isGameOver)
        {
            StartCoroutine(DropGun());
        }
    }

    //발사 메서드
    //애니메이션 이벤트에서 호출
    void Shoot()
    {
        //풀매니저에서 화살 가져오기
        GameObject arrow = GameManager.instance.ItemPoolManager.GetItemPools(BulletType);
        arrow.transform.position = firePos.transform.position;
        arrow.transform.rotation = firePos.transform.rotation;
        arrow.GetComponent<Arrow>().damage = damage;
        animArrow.SetActive(false);
        ammo--;
        GameManager.instance.ammo = ammo;
        sound.PlayOneShot(itemData.shoot, 1);
    }

    //화살 장전 후 발사 준비
    void readyFire()
    {
        anim.SetBool("Fire", false);
    }

    //화살 장전
    void getArrow()
    {
        animArrow.SetActive(true);
        sound.PlayOneShot(itemData.slideback, 1);
    }

    //화살을 쏜 뒤
    void emptyArrow()
    {
        //화살 다 쓰면 empty애니메이션에서 멈춤
        if (ammo <= 0)
            anim.SetBool("Holding", false);
    }

    //총 습득했을 때 총 모션
    IEnumerator GetGun()
    {
        //총 위로 30 기울이기
        transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ReadyToShoot());
    }

    //게임오버 되었을때 총 날라가게함
    IEnumerator DropGun()
    {
        Vector3 dir = firePos.transform.position - transform.position;
        rb.AddForce(dir * 20, ForceMode.Impulse);   //총 날림
        rb.useGravity = true;   //중력 영향받음
        rb.constraints = RigidbodyConstraints.None; //포지션, 로테이션 고정해제
        coll[0].enabled = true;    //콜라이더 활성화
        coll[1].enabled = true;    //콜라이더 활성화
        isDead = true;  //죽었음
        yield return new WaitForSeconds(4);
        rb.isKinematic = true;
    }

    //총 습득 모션중에는 총 못쏘게함
    IEnumerator ReadyToShoot()
    {
        while (true)
        {
            if (rotateGun.isGetGun)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                canShoot = true;
                yield break;
            }
            else
                yield return null;
        }
    }
    float detectionRange = 5f; // 감지 범위
    Vector3 playerPosition; // 현재 오브젝트의 위치   

    private void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = Color.yellow;

        // 구체 기즈모 그리기 (감지 범위 표시)
        Gizmos.DrawWireSphere(playerPosition, detectionRange);
    }

    //일정 범위안에 있는 모든 좀비들한테 데미지주는 메서드
    void AttackShpere()
    {
        // 일정 범위 내에서 레이캐스트 검출
        RaycastHit[] hits = Physics.SphereCastAll(playerPosition, detectionRange, Vector3.forward, 0);

        // 검출된 오브젝트 처리
        foreach (RaycastHit hit in hits)
        {
            GameObject detectedObject = hit.collider.gameObject;
            Zombie zombie = detectedObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.GetDamage(damage);
            }
        }
    }
}
