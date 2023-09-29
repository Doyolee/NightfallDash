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
        // InputField�� OnValueChanged �̺�Ʈ�� �߰��մϴ�.
        ID.onValueChanged.AddListener(IDOnInputValueChanged);
        PW.onValueChanged.AddListener(PWOnInputValueChanged);
    }

    private void IDOnInputValueChanged(string newText)
    {
        // �Էµ� �ؽ�Ʈ�� ���̸� üũ�Ͽ� 20�ڸ� �ʰ��ϸ� �߶���ϴ�.
        if (newText.Length > maxLength)
            ID.text = newText.Substring(0, maxLength);
    }
    private void PWOnInputValueChanged(string newText)
    {
        // �Էµ� �ؽ�Ʈ�� ���̸� üũ�Ͽ� 20�ڸ� �ʰ��ϸ� �߶���ϴ�.
        if (newText.Length > maxLength)
            PW.text = newText.Substring(0, maxLength);
    }
}
