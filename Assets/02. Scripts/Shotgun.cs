using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shotgun : MonoBehaviour
{
    Rigidbody rb;
    public Collider coll;

    public ItemData itemData;

    public GameObject firePos;
    public ParticleSystem fireFx;
    public Light fireLight;
    public GameObject slider;
    public GameObject Shell;
    public GameObject ShellOutPos;
    public GameObject Fx;
    AudioSource sound;

    bool isDead = false;    //�÷��̾ �������

    private float fireTimer;
    float fireLateTime;

    Vector3 originSlider;
    Vector3 originPos;
    public RotateGun rotateGun;

    float damage;
    int ammo;
    int BulletType;
    int ShellType;

    bool canShoot;  //�غ� ����� �����ȵ�

    private void OnEnable()
    {
        ammo = itemData.Ammo;   //�Ѿ� ��

        if (UserManager.userInstance.perks[3]) ammo += 3;

        damage = itemData.Damage;   // �� ������
        fireLateTime = itemData.FireTime;   //�߻� �ð�
        BulletType = itemData.BulletType;   //�Ѿ�
        ShellType = itemData.ShellType; //ź��
        originSlider = Vector3.zero;
        canShoot = false; //�غ� ����� �����ȵ�
        fireTimer = 1f;
        isDead = false;
        coll.enabled = false;

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
            for (int i = 0; i < 8; i++)
            {
                Vector3 randomRotation = new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f));
                //Ǯ�Ŵ������� �Ѿ� ������
                GameObject bullet = GameManager.instance.ItemPoolManager.GetItemPools(BulletType);
                bullet.transform.position = firePos.transform.position;
                bullet.transform.rotation = Quaternion.Euler(randomRotation) * firePos.transform.rotation;

                bullet.GetComponent<Bullet>().damage = damage;
                rotateGun.isFire = false;
            }
            //SpereCastAll�� ���� ���� ���� ���� �ȿ��ִ� ������Ʈ�� �� ������ ����
            AttackShpere();
            //�Ѿ� �� --
            ammo--;
            GameManager.instance.ammo = ammo;
            StartCoroutine(Recoil());

            Fx.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 20));
            fireFx.Play();
            StartCoroutine(Flicker());
            StartCoroutine(Slider());
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

    IEnumerator Slider()
    {
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            //�����̴� �ڷ� �Ѿ
            slider.transform.Translate(Vector3.right * Time.deltaTime * 0.8f);

            //�����̴� �ִ�ġ�� �Ѿ����
            if (slider.transform.localPosition.x >= 0.09f)
            {
                //�����̴� �ִ���ġ�� ����
                slider.transform.localPosition = new Vector3(0.09f, originSlider.y, originSlider.z);
                sound.PlayOneShot(itemData.slideback, 1);

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
            yield break;

        while (true)
        {
            //�����̴� ������ �̵�
            slider.transform.Translate(Vector3.right * Time.deltaTime * -0.8f);
            //�����̴� �ִ�ġ�� ������ �̵�������
            if (slider.transform.localPosition.x <= originSlider.x)
            {
                //�����̴� �����·� ����
                slider.transform.localPosition = originSlider;
                sound.PlayOneShot(itemData.slidefront, 1);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator GetGun()
    {
        transform.localRotation = Quaternion.Euler(0f, 90f, -30f);
        yield return null;

        while (true)
        {
            //�����̴� �ڷ� �Ѿ
            slider.transform.Translate(Vector3.right * Time.deltaTime * 0.75f);
            //�����̴� �ִ�ġ�� �Ѿ����
            if (slider.transform.localPosition.x >= 0.1f)
            {
                //�����̴� �ִ���ġ�� ����
                slider.transform.localPosition = new Vector3(0.1f, originSlider.y, originSlider.z);
                sound.PlayOneShot(itemData.slideback, 1);
                break;
            }
            yield return null;
        }
        while (true)
        {
            //�����̴� ������ �̵�
            slider.transform.Translate(Vector3.right * Time.deltaTime * -0.75f);
            //�����̴� �ִ�ġ�� ������ �̵�������
            if (slider.transform.localPosition.x <= 0)
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
    bool isrecovering = false;  //�ѱ� �ݵ� ȸ��
    IEnumerator Recoil()
    {
        isRecoling = true;
        isrecovering = false;
        recoilStartTime = 0;
        float recoliLateZ = Random.Range(150f, 170f);
        float recoliLateY = Random.Range(-10f, 10f);
        float recoliLateX = Random.Range(-10f, 10f);
        Vector3 recoliV = new Vector3(transform.localPosition.x - recoliLateX, transform.localPosition.y - recoliLateY, transform.localPosition.z - recoliLateZ);
        transform.rotation = Quaternion.Euler(-4f, 0, 0) * transform.rotation;
        transform.localPosition = Vector3.Lerp(transform.localPosition, recoliV, Time.deltaTime * 20f);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(4f, 0, 0) * transform.rotation;
    }
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        originPos = transform.localPosition;
        sound = GetComponent<AudioSource>();
    }
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
    IEnumerator ReadyToShoot()
    {
        while(true)
        {
            if (rotateGun.isGetGun)
            {
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                canShoot = true;
                yield break;
            }
            else
                yield return null;
        }
    }
    float detectionRange = 5f; // ���� ����
    Vector3 playerPosition; 

    private void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = Color.yellow;

        // ��ü ����� �׸��� (���� ���� ǥ��)
        Gizmos.DrawWireSphere(playerPosition, detectionRange);
    }

    void AttackShpere()
    {
        // ���� ���� ������ ����ĳ��Ʈ ����
        RaycastHit[] hits = Physics.SphereCastAll(playerPosition, detectionRange, Vector3.forward, 0);

        // ����� ������Ʈ ó��
        foreach (RaycastHit hit in hits)
        {
            GameObject detectedObject = hit.collider.gameObject;
            Zombie zombie = detectedObject.GetComponent<Zombie>();
            if(zombie != null)
            {
                zombie.GetDamage(damage * 8);
                print("�����ȿ� ����������" + damage * 8);
            }
        }
    }
}
