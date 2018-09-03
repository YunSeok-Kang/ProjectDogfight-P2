using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Vehicle
{
    protected override void OnActivateObject()
    {
        GunManager[] gunManagers = gameObject.GetComponentsInChildren<GunManager>();
        foreach (GunManager gunManager in gunManagers)
        {
            gunManager.enabled = true;
        }
    }

    protected override void OnUnActivateObject()
    {
        GunManager[] gunManagers = gameObject.GetComponentsInChildren<GunManager>();
        foreach (GunManager gunManager in gunManagers)
        {
            gunManager.enabled = false;
        }
    }
}
