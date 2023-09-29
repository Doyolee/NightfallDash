using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    Rigidbody rb;
    public Collider coll;

    public Transform firePos;
    public ParticleSystem fireFx;
    public Light fireLight;
    public GameObject slider;
    public GameObject cocked;
    public GameObject Shell;
    public GameObject ShellOutPos;
    public GameObject Fx;
    public GameObject gun;

    bool isDead = false;    //플레이어가 사망유무

    private float fireTimer;
    float fireLateTime;

    Vector3 originSlider;
    Vector3 originPos;
    public RotateGun rotateGun;

    public ItemData itemData;
    AudioSource sound;

    float damage;
    int ammo;
    int BulletType;
    int ShellType;

    bool canShoot;  //준비 모션중 발포안됨

    private void Start()
    {
        originSlider = new Vector3(-0.07f, 0.15f, -0.05f);
        rb = GetComponentInParent<Rigidbody>();
        originPos = transform.localPosition;
        sound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        ammo = itemData.Ammo;   //총알 수
        if (UserManager.userInstance.perks[3]) ammo += 4;

        damage = itemData.Damage;   // 총 데미지
        fireLateTime = itemData.FireTime;   //발사 시간
        BulletType = itemData.BulletType;   //총알
        ShellType = itemData.ShellType; //탄피
        canShoot = false; //준비 모션중 발포안됨
        isDead = false;
        coll.enabled = false;
        waitTime = 0;

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
        waitTime += Time.deltaTime;
        if (rotateGun.isFire && canShoot && fireTimer >= fireLateTime && ammo >= 1)
        {
            Shoot();
            //SpereCastAll을 통해 전방 일정 범위 안에있는 오브젝트는 다 데미지 받음
            AttackShpere();
            //총알 수 --
            ammo--;
            GameManager.instance.ammo = ammo;

            Fx.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 5));
            fireFx.Play();
            StartCoroutine(Flicker());
            StartCoroutine(Slider());
            sound.PlayOneShot(itemData.shoot, 1);

            fireTimer = 0f;
            rotateGun.isFire = false;
        }
        //총 쏘고 반동 일어날때
        if (isRecoling)
        {
            recoilStartTime += Time.deltaTime;
            if (recoilStartTime > 0.5f)
            {
                //0.5초 지나면 반동회복
                isrecovering = true;
            }
        }
        //반동 회복
        if (isrecovering)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, Time.deltaTime * 4f);
            isRecoling = false;
        }
        //플레이어가 죽었으면
        if (GameManager.instance.isGameOver)
        {
            StartCoroutine(DropGun());
        }
    }

    IEnumerator Flicker()
    {
        //총구 화염
        fireLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        fireLight.enabled = false;
    }

    IEnumerator Slider()
    {
        while (true)
        {
            //슬라이더 뒤로 넘어감
            slider.transform.Translate(Vector3.right * Time.deltaTime * -1.5f);
            //노리쇠 뒤로 넘어감
            cocked.transform.localRotation = Quaternion.Euler(-90 * Time.deltaTime * -2.5f, -90f, 90f);
            //슬라이더 최대치로 넘어갔으면
            if (slider.transform.localPosition.x <= -0.3f)
            {
                //슬라이더 최대위치로 고정
                slider.transform.localPosition = new Vector3(-0.3f, originSlider.y, originSlider.z);
                //노리쇠 최대 위치로 고정
                cocked.transform.localRotation = Quaternion.Euler(0, -90f, 90f);
                sound.PlayOneShot(itemData.slideback, 1);
                break;
            }
            yield return null;
        }

        //풀매니저에서 탄피 가져오기
        GameObject shell = GameManager.instance.ItemPoolManager.GetItemPools(ShellType);
        //탄피 리지드바디 가져오기
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shell.transform.position = Shell.transform.position;
        //랜덤한 반향으로 회전
        shell.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        //탄피 배출 위치을 위한 방향
        Vector3 dir = ShellOutPos.transform.position - shell.transform.position;
        //AddForce로 탄피 배출
        shellRb.AddForce(dir * 5f, ForceMode.Impulse);

        //총알수 없으면 코루틴 탈출
        //슬라이더 뒤로 넘어간 상태로 정지
        if (ammo <= 0)
            yield break;

        while (true)
        {
            //슬라이더 앞으로 이동
            slider.transform.Translate(Vector3.right * Time.deltaTime * 1.5f);
            //노리쇠 앞으로 이동
            cocked.transform.localRotation = Quaternion.Euler(0.1f * Time.deltaTime * 2.5f, -90f, 90f);
            //슬라이더 최대치로 앞으로 이동했으면
            if (slider.transform.localPosition.x >= originSlider.x)
            {
                //노리쇠 원상태로 복구
                cocked.transform.localRotation = Quaternion.Euler(-90f, -90f, 90f);
                //슬라이더 원상태로 복구
                slider.transform.localPosition = originSlider;
                sound.PlayOneShot(itemData.slidefront, 1);
                yield break;
            }
            yield return null;
        }
    }

    void Shoot()
    {
        //풀매니저에서 총알 가져옴
        GameObject bullet = GameManager.instance.ItemPoolManager.GetItemPools(BulletType);
        bullet.transform.position = firePos.transform.position;
        bullet.transform.rotation = firePos.transform.rotation;
        bullet.GetComponent<Bullet>().damage = damage;
        StartCoroutine(Recoil());
    }

    IEnumerator GetGun()
    {
        transform.localRotation = Quaternion.Euler(0f, -90f, 30f);
        yield return null;

        while (true)
        {
            //슬라이더 뒤로 넘어감
            slider.transform.Translate(Vector3.right * Time.deltaTime * -1.5f);
            //노리쇠 뒤로 넘어감
            cocked.transform.localRotation = Quaternion.Euler(-90 * Time.deltaTime * -2.5f, -90f, 90f);
            //슬라이더 최대치로 넘어갔으면
            if (slider.transform.localPosition.x <= -0.3f)
            {
                //슬라이더 최대위치로 고정
                slider.transform.localPosition = new Vector3(-0.3f, originSlider.y, originSlider.z);
                //노리쇠 최대 위치로 고정
                cocked.transform.localRotation = Quaternion.Euler(0, -90f, 90f);
                sound.PlayOneShot(itemData.slideback, 1);
                break;
            }
            yield return null;
        }
        while (true)
        {
            //슬라이더 앞으로 이동
            slider.transform.Translate(Vector3.right * Time.deltaTime * 1.5f);
            //노리쇠 앞으로 이동
            cocked.transform.localRotation = Quaternion.Euler(0.1f * Time.deltaTime * 2.5f, -90f, 90f);
            //슬라이더 최대치로 앞으로 이동했으면
            if (slider.transform.localPosition.x >= originSlider.x)
            {
                //노리쇠 원상태로 복구
                cocked.transform.localRotation = Quaternion.Euler(-90f, -90f, 90f);
                //슬라이더 원상태로 복구
                slider.transform.localPosition = originSlider;
                sound.PlayOneShot(itemData.slidefront, 1);
                break;
            }
            yield return null;
        }
        StartCoroutine(ReadyToShoot());
    }

    private float recoilStartTime;      // 총기 반동 시작 시간
    bool isRecoling = false;    //총기 반동 시작
    bool isrecovering = false;  //총기 반동 회복
    IEnumerator Recoil()
    {
        isRecoling = true;
        isrecovering = false;
        recoilStartTime = 0;
        float recoliLateZ = Random.Range(8f, 12f);
        float recoliLateY = Random.Range(-3f, 3f);
        float recoliLateX = Random.Range(-3f, 3f);
        Vector3 recoliV = new Vector3(transform.localPosition.x - recoliLateX, transform.localPosition.y - recoliLateY, transform.localPosition.z - recoliLateZ);
        transform.rotation = Quaternion.Euler(-3f, 0, 0) * transform.rotation;
        transform.localPosition = Vector3.Lerp(transform.localPosition, recoliV, Time.deltaTime * 20f);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(3f, 0, 0) * transform.rotation;
    }

    IEnumerator DropGun()
    {
        Vector3 dir = firePos.transform.position - transform.position;
        rb.AddForce(dir * 20, ForceMode.Impulse);   //총 날림
        rb.useGravity = true;   //중력 영향
        rb.constraints = RigidbodyConstraints.None; //포지션, 로테이션 고정해제
        coll.enabled = true;    //콜라이더 활성화
        isDead = true;  //죽었음
        yield return new WaitForSeconds(4);
        rb.isKinematic = true;
    }

    IEnumerator ReadyToShoot()
    {
        while (true)
        {
            if (rotateGun.isGetGun)
            {
                transform.localRotation = Quaternion.Euler(0, -90, 0);
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
                print("범위안에 데미지받음" + damage);
            }
        }
    }
}
