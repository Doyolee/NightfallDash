using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public PoolManager poolManager;
    public Map entileMap;
    public Text bonusScoreText;
    public BossZombie bossZombie;
    public CameraEffect cameraEffect;

    public void Start()
    {
        //���� ���� ��
        if (UserManager.userInstance.perks[0])
        {
            //���� ���⸦ Ǯ���� ������ �÷��̾� �ٷ� �տ� ��ġ
            GameObject startingWeapon = poolManager.GetPools(13);
            startingWeapon.transform.position = new Vector3(0, 0.4f, -28);
            startingWeapon.transform.parent = entileMap.transform;

            //�Ž��������� ���� ȭ�鿡�� ������ �ʰ� ����
            MeshRenderer meshRenderer = startingWeapon.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }
        //���� +10%
        if (UserManager.userInstance.perks[1])
        {
            //+10% Text ������
            bonusScoreText.gameObject.SetActive(true);
            //int�θ� ����ϱ� ���� 10���� ���� ���� ������
            UserManager.userInstance.getScore += UserManager.userInstance.getScore/10;
        }
        //���� ���� �ӵ� -10%
        if (UserManager.userInstance.perks[2])
        {
            bossZombie.bossSpeed *=0.9f;
        }

        //perks[3](�Ѿ� ����)�� �� ���� ��ũ��Ʈ���� ó��

        //�� ȿ�� ���� �ӵ� +20%
        if (UserManager.userInstance.perks[4])
        {
            cameraEffect.bloodTimerCo *= 0.8f;
        }
    }


}
