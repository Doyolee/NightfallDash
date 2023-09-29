using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RotateGun rotateGun;

    private void OnEnable()
    {
        rotateGun = GameManager.instance.player.Items.GetComponent<RotateGun>();
    }
    
    //��ư ������ ������
    public void OnPointerDown(PointerEventData eventData)
    {
        rotateGun.isFire = true;
    }

    //������ ��ư���� �� ������
    public void OnPointerUp(PointerEventData eventData)
    {
        rotateGun.isFire = false;
    }
}
