using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffect : MonoBehaviour
{
    public Image[] bloodEff;
    // bloodEff[0]: 좀비 충돌 시 중앙 피 효과
    // bloodEff[1]: 좀비 충돌 시 좌측 피 효과
    // bloodEff[2]: 좀비 충돌 시 우측 피 효과
    // bloodEff[3]: 게임 오버 시 가장자리 피 효과
    // bloodEff[4]: 게임 오버 시 충앙 피 효과

    //좀비 충돌 시 피 효과를 사용할 타이머
    float[] bloodTimer= {0,0,0};

    //bloodTimer에 더해지는 값
    [HideInInspector]
    public float bloodTimerCo=2f;
    


    private void Update()
    {
        //좀비 충돌 피 효과들이 시간이 지나면 서서히 사라지는 메서드
        for (int i = 0; i < 3; i++)
        {
            bloodTimer[i] -= Time.deltaTime;

            if (bloodTimer[i] <= 0) continue;

            //남은 타이머 시간에 비례하여 알파값 감소
            bloodEff[i].color = new Color(bloodEff[i].color.r, bloodEff[i].color.g, bloodEff[i].color.b, bloodTimer[i]/10f);

        }
    }

    //게임 오버 시 화면의 가장자리에 피 튀기는 효과
    public void SideBloodEffect(float alphaValue)
    {
        bloodEff[3].color = new Color(bloodEff[3].color.r, bloodEff[3].color.g, bloodEff[3].color.b, alphaValue);
    }

    //게임 오버 시 화면의 중앙에 피 튀기는 효과
    public void CenterBloodEffect(float alphaValue)
    {
        bloodEff[4].color = new Color(bloodEff[4].color.r, bloodEff[4].color.g, bloodEff[4].color.b, alphaValue);
    }

    //좀비와 부딪힌 방향에 따라 중앙, 좌측, 우측에 피가 튀는 효과
    public void CameraBloodEff(float direction)
    {
        //중앙 초기 타이머값
        if (bloodTimer[0] <= 0) bloodTimer[0] = bloodTimerCo*1.5f;

        if (direction==1)
        {
            //피가 아직 남아 있는 상태라면 타이머값을 중첩해서 적용
            if (bloodTimer[1] > 0)
            {
                bloodTimer[0] += bloodTimerCo;
                bloodTimer[1] += bloodTimerCo;
            }
            //피가 없는 상태라면 초기 타이머값 적용
            else
            {
                bloodTimer[1] = bloodTimerCo*2;
            }

            //타이머의 최대값 설정
            if (bloodTimer[1] > 10) bloodTimer[1] = 10f;
        }
        else
        {
            //피가 아직 남아 있는 상태라면 타이머값을 중첩해서 적용
            if (bloodTimer[2] > 0)
            {
                bloodTimer[0] += bloodTimerCo;
                bloodTimer[2] += bloodTimerCo;
            }
            //피가 없는 상태라면 초기 타이머값 적용
            else
            {
                bloodTimer[2] = bloodTimerCo*2;

            }

            //타이머의 최대값 설정
            if (bloodTimer[2] > 10) bloodTimer[2] = 10f;
        }

    }
}
