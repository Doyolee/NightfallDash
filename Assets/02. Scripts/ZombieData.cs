using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Zombie", menuName = "Scripts Object/ZombieData")]
public class ZombieData : ScriptableObject
{
    [Header("# Main Info")]
    public int ZombieType;  //스폰되는 좀비 타입
    //0번 - 일반좀비
    //1번 - 중무장좀비
    //2번 - 탱크좀비
    //3번 - 보스좀비

    [Header("# Enemy Data")]
    public float Health;    //체력
    //일반좀비 - 40
    //중무장좀비 - 70
    //탱크좀비 - 150
    //보스좀비 - 1000000

    [Header("# Sound")]
    public AudioClip[] Attack;
}
