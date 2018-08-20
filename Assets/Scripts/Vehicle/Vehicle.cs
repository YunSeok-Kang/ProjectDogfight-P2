using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : VoxObject
{
    public Controller controller = null;

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
}
