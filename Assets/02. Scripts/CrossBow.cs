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

    bool isDead = false;    //�÷��̾ �������

    Animator anim;

    bool canShoot;  //�غ� ����� �����ȵ�

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
        ammo = itemData.Ammo;   //�Ѿ� ��
        if (UserManager.userInstance.perks[3]) ammo += 3;
        damage = itemData.Damage;   // �� ������
        fireLateTime = itemData.FireTime;   //�߻� �ð�
        BulletType = itemData.BulletType;   //�Ѿ�
        anim.SetBool("Holding", true);
        canShoot = false; //�غ� ����� �����ȵ�
        isDead = false;
        coll[0].enabled = false;
        coll[1].enabled = false;

        StartCoroutine(GetGun());   //�� ȹ�� ���

        GameManager.instance.ammo = ammo;
        GameManager.instance.maxAmmo = ammo;
    }

    float waitTime;
    void Update()
    {
        playerPosition = GameManager.instance.player.transform.position + Vector3.forward * 10f; // ���� ������Ʈ�� ��ġ  
        if (isDead)
            return;

        fireTimer += Time.deltaTime;
        if (rotateGun.isFire && fireTimer >= fireLateTime && ammo >= 1 && canShoot)
        {
            //�߻� �ִϸ��̼� ����
            anim.SetBool("Fire", true);
            //SpereCastAll�� ���� ���� ���� ���� �ȿ��ִ� ������Ʈ�� �� ������ ����
            AttackShpere();
            //�߻� �ð� �ʱ�ȭ
            fireTimer = 0;
            rotateGun.isFire = false;
        }
        //�÷��̾ �׾�����
        if (GameManager.instance.isGameOver)
        {
            StartCoroutine(DropGun());
        }
    }

    //�߻� �޼���
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    void Shoot()
    {
        //Ǯ�Ŵ������� ȭ�� ��������
        GameObject arrow = GameManager.instance.ItemPoolManager.GetItemPools(BulletType);
        arrow.transform.position = firePos.transform.position;
        arrow.transform.rotation = firePos.transform.rotation;
        arrow.GetComponent<Arrow>().damage = damage;
        animArrow.SetActive(false);
        ammo--;
        GameManager.instance.ammo = ammo;
        sound.PlayOneShot(itemData.shoot, 1);
    }

    //ȭ�� ���� �� �߻� �غ�
    void readyFire()
    {
        anim.SetBool("Fire", false);
    }

    //ȭ�� ����
    void getArrow()
    {
        animArrow.SetActive(true);
        sound.PlayOneShot(itemData.slideback, 1);
    }

    //ȭ���� �� ��
    void emptyArrow()
    {
        //ȭ�� �� ���� empty�ִϸ��̼ǿ��� ����
        if (ammo <= 0)
            anim.SetBool("Holding", false);
    }

    //�� �������� �� �� ���
    IEnumerator GetGun()
    {
        //�� ���� 30 ����̱�
        transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ReadyToShoot());
    }

    //���ӿ��� �Ǿ����� �� ���󰡰���
    IEnumerator DropGun()
    {
        Vector3 dir = firePos.transform.position - transform.position;
        rb.AddForce(dir * 20, ForceMode.Impulse);   //�� ����
        rb.useGravity = true;   //�߷� �������
        rb.constraints = RigidbodyConstraints.None; //������, �����̼� ��������
        coll[0].enabled = true;    //�ݶ��̴� Ȱ��ȭ
        coll[1].enabled = true;    //�ݶ��̴� Ȱ��ȭ
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
                transform.localRotation = Quaternion.Euler(0, 0, 0);
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
