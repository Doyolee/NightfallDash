using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKM_Bullet : MonoBehaviour
{
    Rigidbody rb;
    TrailRenderer trailRenderer;

    [HideInInspector]
    public float damage;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Off());
    }

    private void OnEnable()
    {
        StartCoroutine(Off());
    }
    void Update()
    {
        //총알 발사
        transform.Translate(Vector3.forward * 10f);
        trailRenderer.enabled = true;

        // 총알을 발사한 위치와 방향
        Vector3 bulletOrigin = transform.position;
        Vector3 bulletDirection = transform.forward;

        // Raycast를 이용하여 충돌 감지
        RaycastHit hit;
        float maxDistance = 10; // 총알 속도에 따른 최대 이동 거리 설정
        if (Physics.Raycast(bulletOrigin, bulletDirection, out hit, maxDistance))
        {
            // 충돌한 객체를 가져옴
            GameObject hitObject = hit.collider.gameObject;

            // 예를 들어, 충돌한 객체가 IDamageable을 구현한 경우에 데미지를 주는 등의 동작을 수행할 수 있습니다.
            IDamage damageObj = hitObject.GetComponent<IDamage>();

            // IDamage 인터페이스를 상속하고 있는 오브젝트일때
            if (damageObj != null)
            {
                //GetDamage 메서드 호출, 트레일랜더러, 게임오브젝트 비활성화
                damageObj.GetDamage(damage);
                trailRenderer.enabled = false;
                gameObject.SetActive(false);
            }
            else
                StartCoroutine(Off());
        }
    }

    //총알 비활성화 코루틴
    IEnumerator Off()
    {
        //2초후 총알 비활성화
        yield return new WaitForSeconds(2f);
        trailRenderer.enabled=false;
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }
}
