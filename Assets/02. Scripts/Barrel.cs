using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamage
{
    public GameObject explosionEff; // 폭파 이펙트

    public float exposionRadius; //폭발 반경
    AudioSource audioSource;
    public AudioClip expotionSfx;

    //드럼통이 폭파한 뒤 풀에 넣고 다시 꺼내올 때 로테이션 조정을 위한 값
    Quaternion originRot;
    void Start()
    {
        audioSource=GetComponent<AudioSource>();

        //현재 로테이션 값을 저장
        originRot = transform.rotation;
    }

    private void OnEnable()
    {
        //오브젝트의 로테이션을 originRot으로 조정
        transform.rotation = originRot;
    }
    //드럼통 폭발
    void ExpBarrel()
    {
        GameObject effect = Instantiate(explosionEff, transform.position, Quaternion.identity);
        Destroy(effect,2f);

        IndirectDamage(transform.position);

        audioSource.PlayOneShot(expotionSfx,1f);
    }

    void IndirectDamage(Vector3 pos)
    {
        // 범위 내의 모든 드럼통 레이어 검출
        Collider[] colls = Physics.OverlapSphere(pos, exposionRadius,1<<10);

        //포지션에서 살짝 앞에서 ExplosionForce 적용
        foreach(var coll in colls)
        {
            var _rb=coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            _rb.AddExplosionForce(600f, pos+Vector3.back*0.3f, exposionRadius, 10f);
        }
    }
    
    //지나쳐서 멀어지면 비활성화
    private void Update()
    {
        if (transform.position.z < -64)
        {
            gameObject.SetActive(false);
        }
    }

    public float health = 30f;

    //데미지를 받아 체력이 0이하가 되면 폭발
    public void GetDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            ExpBarrel();
        }
    }
}
