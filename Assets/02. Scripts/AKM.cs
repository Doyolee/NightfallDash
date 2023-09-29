using System.Collections;
using UnityEngine;

public class AKM : MonoBehaviour
{
    Rigidbody rb;
    public Collider coll;

    public ItemData itemData;

    public GameObject firePos;
    public ParticleSystem fireFx;
    public Light fireLight;
    public Rigidbody magazine;
    public GameObject Shell;
    public GameObject ShellOutPos;
    public GameObject Fx;
    public GameObject slider;
    AudioSource sound;

    private float fireTimer;
    float fireLateTime;

    Vector3 originSlider;
    public RotateGun rotateGun;

    bool isDead = false;    //�÷��̾ �������

    float damage;
    int ammo;
    int BulletType;
    int ShellType;

    bool canShoot;  //�غ� ����� �����ȵ�
    Vector3 originPos;
    private void OnEnable()
    {
        ammo = itemData.Ammo;   //�Ѿ� ��
        if (UserManager.userInstance.perks[3]) ammo += 12;
        damage = itemData.Damage;   // �� ������
        fireLateTime = itemData.FireTime;   //�߻� �ð�
        BulletType = itemData.BulletType;   //�Ѿ�
        ShellType = itemData.ShellType; //ź��
        magazine.isKinematic = true;    //źâ ����
        originSlider = Vector3.zero;
        canShoot = false; //�غ� ����� �����ȵ�
        isDead = false;
        coll.enabled = false;
        sound = GetComponent<AudioSource>();

        StartCoroutine(GetGun());   //�� ȹ�� ���

        GameManager.instance.ammo = ammo;
        GameManager.instance.maxAmmo = ammo;
    }
    void Update()
    {
        playerPosition = GameManager.instance.player.transform.position + Vector3.forward * 10f; // ���� ������Ʈ�� ��ġ  
        if (isDead)
            return;
       
        fireTimer += Time.deltaTime;

        if (rotateGun.isFire && fireTimer >= fireLateTime && ammo >= 1 && canShoot)
        {
            //Ǯ�Ŵ������� �Ѿ� ������
            Shoot();
            StartCoroutine(Slider());

            //SpereCastAll�� ���� ���� ���� ���� �ȿ��ִ� ������Ʈ�� �� ������ ����
            AttackShpere();

            //�Ѿ� �� --
            ammo--;
            GameManager.instance.ammo = ammo;

            Fx.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 20));
            fireFx.Play();
            StartCoroutine(Flicker());
            sound.PlayOneShot(itemData.shoot, 1);
            fireTimer = 0f;
        }
        //�� ��� �ݵ� �Ͼ��
        if (isRecoling)
        {
            recoilStartTime += Time.deltaTime;
            if (recoilStartTime > 0.5f)
            {
                //0.5�� ������ �ݵ�ȸ��
                isrecovering = true;
            }
        }
        //�ݵ� ȸ��
        if (isrecovering)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, Time.deltaTime * 4f);
            isRecoling = false;
        }
        //�÷��̾ �׾�����
        if (GameManager.instance.isGameOver)
        {
            StartCoroutine(DropGun());
        }
    }

    IEnumerator Flicker()
    {
        //�ѱ� ȭ��
        fireLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        fireLight.enabled = false;
    }

    //�� �߻� �޼���
    void Shoot()
    {
        //Ǯ�Ŵ������� �Ѿ� ��������
        GameObject bullet = GameManager.instance.ItemPoolManager.GetItemPools(BulletType);
        bullet.transform.position = firePos.transform.position;
        bullet.transform.rotation = firePos.transform.rotation;
        bullet.GetComponent<AKM_Bullet>().damage = damage;
        StartCoroutine(Recoil());
    }

    //�� �߻��Ҷ����� �����̴� �̵�
    IEnumerator Slider()
    {
        while (true)
        {
            //�����̴� �ڷ� �Ѿ
            slider.transform.Translate(Vector3.up * Time.deltaTime * -1.5f);
            //�����̴� �ִ�ġ�� �Ѿ����
            if (slider.transform.localPosition.z >= 1.5f)
            {
                //�����̴� �ִ���ġ�� ����
                slider.transform.localPosition = new Vector3(-1.5f, originSlider.y, originSlider.z);
                break;
            }
            yield return null;
        }

        //Ǯ�Ŵ������� ź�� ��������
        GameObject shell = GameManager.instance.ItemPoolManager.GetItemPools(ShellType);
        //ź�� ������ٵ� ��������
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shell.transform.position = Shell.transform.position;
        //������ �������� ȸ��
        shell.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        //ź�� ���� ��ġ�� ���� ����
        Vector3 dir = ShellOutPos.transform.position - shell.transform.position;
        //AddForce�� ź�� ����
        shellRb.AddForce(dir * 5f, ForceMode.Impulse);

        //�Ѿ˼� ������ �ڷ�ƾ Ż��
        //�����̴� �ڷ� �Ѿ ���·� ����
        if (ammo <= 0)
        {
            slider.transform.localPosition = new Vector3(0, 0, 1.5f);
            yield break;
        }

        while (true)
        {
            //�����̴� ������ �̵�
            slider.transform.Translate(Vector3.up * Time.deltaTime * 1.5f);
            //�����̴� �ִ�ġ�� ������ �̵�������
            if (slider.transform.localPosition.z <= 0)
            {
                //�����̴� �����·� ����
                slider.transform.localPosition = originSlider;
                break;
            }
            yield return null;
        }
    }

    //�� �������� �� �� ���
    IEnumerator GetGun()
    {
        //�� ���� 30 ����̱�
        transform.localRotation = Quaternion.Euler(0f, 180f, -45f);
        yield return null;

        while (true)
        {
            //�����̴� �ڷ� �Ѿ
            slider.transform.Translate(Vector3.up * Time.deltaTime * -0.3f);
            //�����̴� �ִ�ġ�� �Ѿ����
            if (slider.transform.localPosition.z >= 1.5f)
            {
                //�����̴� �ִ���ġ�� ����
                slider.transform.localPosition = new Vector3(-1.5f, originSlider.y, originSlider.z);
                sound.PlayOneShot(itemData.slideback, 1);
                break;
            }
            yield return null;
        }
        while (true)
        {
            //�����̴� ������ �̵�
            slider.transform.Translate(Vector3.up * Time.deltaTime * 0.3f);
            //�����̴� �ִ�ġ�� ������ �̵�������
            if (slider.transform.localPosition.z <= 0)
            {
                //�����̴� �����·� ����
                slider.transform.localPosition = originSlider;
                sound.PlayOneShot(itemData.slidefront, 1);
                break;
            }
            yield return null;
        }

        StartCoroutine(ReadyToShoot());
    }

    private float recoilStartTime;      // �ѱ� �ݵ� ���� �ð�
    bool isRecoling = false;    //�ѱ� �ݵ� ����
    bool isrecovering = false;

    //�� �򶧸��� �����ϰ� �ݵ���
    IEnumerator Recoil()
    {
        isRecoling = true;
        isrecovering = false;
        recoilStartTime = 0;
        float recoliLateZ = Random.Range(0f, 2f);
        float recoliLateY = Random.Range(-3f, 3f);
        float recoliLateX = Random.Range(-3f, 3f);
        Vector3 recoliV = new Vector3(transform.localPosition.x - recoliLateX, transform.localPosition.y - recoliLateY, transform.localPosition.z - recoliLateZ);
        transform.rotation = Quaternion.Euler(-2f, 0, 0) * transform.rotation;
        transform.localPosition = Vector3.Lerp(transform.localPosition, recoliV, Time.deltaTime * 20f);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(2f, 0, 0) * transform.rotation;
    }
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        originPos = transform.localPosition;
    }
    
    //���ӿ��� �Ǿ����� �� ���󰡰���
    IEnumerator DropGun()
    {
        Vector3 dir = firePos.transform.position - transform.position;
        rb.AddForce(dir * 20, ForceMode.Impulse);   //�� ����
        rb.useGravity = true;   //�߷� ����
        rb.constraints = RigidbodyConstraints.None; //������, �����̼� ��������
        coll.enabled = true;    //�ݶ��̴� Ȱ��ȭ
        isDead = true;  //�׾���
        yield return new WaitForSeconds(4);
        rb.isKinematic = true;
    }

    //�� ���� ����߿��� �� �������
    IEnumerator ReadyToShoot()
    {
        while (true)
        {
            if (rotateGun.isGetGun)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                canShoot = true;
                yield break;
            }
            else
                yield return null;
        }
    }

    float detectionRange = 5f; // ���� ����
    Vector3 playerPosition; // ���� ������Ʈ�� ��ġ   

    private void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = Color.yellow;

        // ��ü ����� �׸��� (���� ���� ǥ��)
        Gizmos.DrawWireSphere(playerPosition, detectionRange);
    }

    //���� �����ȿ� �ִ� ��� ��������� �������ִ� �޼���
    void AttackShpere()
    {
        // ���� ���� ������ ����ĳ��Ʈ ����
        RaycastHit[] hits = Physics.SphereCastAll(playerPosition, detectionRange, Vector3.forward, 0);

        // ����� ������Ʈ ó��
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
