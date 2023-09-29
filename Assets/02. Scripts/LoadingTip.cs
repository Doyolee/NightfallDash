using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTip : MonoBehaviour
{
    public Text tipText;
    public string[] Tips;
    void Start()
    {
        //Tip 랜덤으로 표기
        int ran = Random.Range(0, Tips.Length);
        tipText.text = $"Tip : {Tips[ran]}";
    }
}
