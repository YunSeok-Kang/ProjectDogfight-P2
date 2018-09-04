using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FireButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool isFiring;
    public virtual void OnPointerDown(PointerEventData ped)
    {
        isFiring = true;
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        isFiring = false;
    }
    public bool GetFireInput()
    {
        return isFiring;
    }
}
