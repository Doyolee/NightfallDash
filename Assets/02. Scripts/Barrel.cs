using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamage
{
    public GameObject explosionEff; // ���� ����Ʈ

    public float exposionRadius; //���� �ݰ�
    AudioSource audioSource;
    public AudioClip expotionSfx;

    //�巳���� ������ �� Ǯ�� �ְ� �ٽ� ������ �� �����̼� ������ ���� ��
    Quaternion originRot;
    void Start()
    {
        audioSource=GetComponent<AudioSource>();

        //���� �����̼� ���� ����
        originRot = transform.rotation;
    }

    private void OnEnable()
    {
        //������Ʈ�� �����̼��� originRot���� ����
        transform.rotation = originRot;
    }
    //�巳�� ����
    void ExpBarrel()
    {
        GameObject effect = Instantiate(explosionEff, transform.position, Quaternion.identity);
        Destroy(effect,2f);

        IndirectDamage(transform.position);

        audioSource.PlayOneShot(expotionSfx,1f);
    }

    void IndirectDamage(Vector3 pos)
    {
        // ���� ���� ��� �巳�� ���̾� ����
        Collider[] colls = Physics.OverlapSphere(pos, exposionRadius,1<<10);

        //�����ǿ��� ��¦ �տ��� ExplosionForce ����
        foreach(var coll in colls)
        {
            var _rb=coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            _rb.AddExplosionForce(600f, pos+Vector3.back*0.3f, exposionRadius, 10f);
        }
    }
    
    //�����ļ� �־����� ��Ȱ��ȭ
    private void Update()
    {
        if (transform.position.z < -64)
        {
            gameObject.SetActive(false);
        }
    }

    public float health = 30f;

    //�������� �޾� ü���� 0���ϰ� �Ǹ� ����
    public void GetDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            ExpBarrel();
        }
    }
}
