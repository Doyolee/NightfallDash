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
        //Tip �������� ǥ��
        int ran = Random.Range(0, Tips.Length);
        tipText.text = $"Tip : {Tips[ran]}";
    }
}
