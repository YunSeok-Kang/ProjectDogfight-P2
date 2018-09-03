using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Vehicle 클래스와 관련된 이벤트를 위해 존재
/// </summary>
/// <param name="myInfo"> 해당 Vehicle(this)의 정보를 argument로 전달함. </param>
public delegate void VehicleEvent(Vehicle myInfo);


public class Vehicle : VoxObject
{
    public Controller controller = null;

    public event VehicleEvent onHPZeroEvent = delegate { };

    protected override bool Init()
    {
        if (!base.Init())
        {
            return false;
        }

        if (!controller)
        {
            Debug.LogError("Vehicle : Controller 없음.");
        }

        return true;
    }

    protected override void OnActivateObject()
    {
        controller.enabled = true;

        base.OnActivateObject();
    }

    protected override void OnUnActivateObject()
    {
        controller.enabled = false;

        base.OnUnActivateObject();
    }

    protected override void OnHPZero()
    {
        onHPZeroEvent(this);

        base.OnHPZero();
    }
}
