using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffect : MonoBehaviour
{
    public Image[] bloodEff;
    // bloodEff[0]: ���� �浹 �� �߾� �� ȿ��
    // bloodEff[1]: ���� �浹 �� ���� �� ȿ��
    // bloodEff[2]: ���� �浹 �� ���� �� ȿ��
    // bloodEff[3]: ���� ���� �� �����ڸ� �� ȿ��
    // bloodEff[4]: ���� ���� �� ��� �� ȿ��

    //���� �浹 �� �� ȿ���� ����� Ÿ�̸�
    float[] bloodTimer= {0,0,0};

    //bloodTimer�� �������� ��
    [HideInInspector]
    public float bloodTimerCo=2f;
    


    private void Update()
    {
        //���� �浹 �� ȿ������ �ð��� ������ ������ ������� �޼���
        for (int i = 0; i < 3; i++)
        {
            bloodTimer[i] -= Time.deltaTime;

            if (bloodTimer[i] <= 0) continue;

            //���� Ÿ�̸� �ð��� ����Ͽ� ���İ� ����
            bloodEff[i].color = new Color(bloodEff[i].color.r, bloodEff[i].color.g, bloodEff[i].color.b, bloodTimer[i]/10f);

        }
    }

    //���� ���� �� ȭ���� �����ڸ��� �� Ƣ��� ȿ��
    public void SideBloodEffect(float alphaValue)
    {
        bloodEff[3].color = new Color(bloodEff[3].color.r, bloodEff[3].color.g, bloodEff[3].color.b, alphaValue);
    }

    //���� ���� �� ȭ���� �߾ӿ� �� Ƣ��� ȿ��
    public void CenterBloodEffect(float alphaValue)
    {
        bloodEff[4].color = new Color(bloodEff[4].color.r, bloodEff[4].color.g, bloodEff[4].color.b, alphaValue);
    }

    //����� �ε��� ���⿡ ���� �߾�, ����, ������ �ǰ� Ƣ�� ȿ��
    public void CameraBloodEff(float direction)
    {
        //�߾� �ʱ� Ÿ�̸Ӱ�
        if (bloodTimer[0] <= 0) bloodTimer[0] = bloodTimerCo*1.5f;

        if (direction==1)
        {
            //�ǰ� ���� ���� �ִ� ���¶�� Ÿ�̸Ӱ��� ��ø�ؼ� ����
            if (bloodTimer[1] > 0)
            {
                bloodTimer[0] += bloodTimerCo;
                bloodTimer[1] += bloodTimerCo;
            }
            //�ǰ� ���� ���¶�� �ʱ� Ÿ�̸Ӱ� ����
            else
            {
                bloodTimer[1] = bloodTimerCo*2;
            }

            //Ÿ�̸��� �ִ밪 ����
            if (bloodTimer[1] > 10) bloodTimer[1] = 10f;
        }
        else
        {
            //�ǰ� ���� ���� �ִ� ���¶�� Ÿ�̸Ӱ��� ��ø�ؼ� ����
            if (bloodTimer[2] > 0)
            {
                bloodTimer[0] += bloodTimerCo;
                bloodTimer[2] += bloodTimerCo;
            }
            //�ǰ� ���� ���¶�� �ʱ� Ÿ�̸Ӱ� ����
            else
            {
                bloodTimer[2] = bloodTimerCo*2;

            }

            //Ÿ�̸��� �ִ밪 ����
            if (bloodTimer[2] > 10) bloodTimer[2] = 10f;
        }

    }
}
