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
    
    //버튼 누르고 있을때
    public void OnPointerDown(PointerEventData eventData)
    {
        rotateGun.isFire = true;
    }

    //누르던 버튼에서 손 땠을때
    public void OnPointerUp(PointerEventData eventData)
    {
        rotateGun.isFire = false;
    }
}
