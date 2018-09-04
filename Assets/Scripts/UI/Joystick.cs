using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image backGroundImage;
    private Image joystickImage;
    private Vector3 inputVector;
    public bool isInverse;

    private void Start()
    {
        backGroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(backGroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
       //     pos.x = (pos.x / backGroundImage.rectTransform.sizeDelta.x);
       //     pos.y = (pos.y / backGroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, pos.y * 2 - 1, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

           // joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (backGroundImage.rectTransform.sizeDelta.x /3),
            joystickImage.rectTransform.anchoredPosition = new Vector3(0,
                                                                         inputVector.y * (backGroundImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }
    public float GetVerticalValue()
    {
        if (isInverse)
        {
            return inputVector.y * -1;
        }
        else
        {
            return inputVector.y;
        }
    }
}
