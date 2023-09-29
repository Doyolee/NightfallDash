using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookBack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CameraMovement cameraMovement;
    
    //버튼 누르고 있을때
    public void OnPointerDown(PointerEventData eventData)
    {
        cameraMovement.isRotate = true;
    }

    //누르던 버튼을 땠을때
    public void OnPointerUp(PointerEventData eventData)
    {
        cameraMovement.isRotate = false;
    }
}
