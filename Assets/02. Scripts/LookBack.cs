using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookBack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CameraMovement cameraMovement;
    
    //��ư ������ ������
    public void OnPointerDown(PointerEventData eventData)
    {
        cameraMovement.isRotate = true;
    }

    //������ ��ư�� ������
    public void OnPointerUp(PointerEventData eventData)
    {
        cameraMovement.isRotate = false;
    }
}
