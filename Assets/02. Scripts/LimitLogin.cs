using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitLogin : MonoBehaviour
{
    public InputField ID;
    public InputField PW;
    private int maxLength = 19;

    private void Start()
    {
        // InputField에 OnValueChanged 이벤트를 추가합니다.
        ID.onValueChanged.AddListener(IDOnInputValueChanged);
        PW.onValueChanged.AddListener(PWOnInputValueChanged);
    }

    private void IDOnInputValueChanged(string newText)
    {
        // 입력된 텍스트의 길이를 체크하여 20자를 초과하면 잘라냅니다.
        if (newText.Length > maxLength)
            ID.text = newText.Substring(0, maxLength);
    }
    private void PWOnInputValueChanged(string newText)
    {
        // 입력된 텍스트의 길이를 체크하여 20자를 초과하면 잘라냅니다.
        if (newText.Length > maxLength)
            PW.text = newText.Substring(0, maxLength);
    }
}
