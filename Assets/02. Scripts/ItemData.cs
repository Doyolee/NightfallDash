using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Scripts Object/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("# Main Info")]
    public int ItemType;    //총 타입
    public int BulletType;  //총알 타입
    public int ShellType;   //탄피 타입
    //0번 - 권총
    //1번 - 석궁
    //2번 - 샷건
    //3번 - AKM
    //4번 - 바주카

    [Header("# Item Data")]
    public int Ammo;    //총알수
    public float Damage; //총 데미지
    public float FireTime; //발사 속도

    [Header("# Sound")]
    public AudioClip shoot;
    public AudioClip slideback;
    public AudioClip slidefront;
}
