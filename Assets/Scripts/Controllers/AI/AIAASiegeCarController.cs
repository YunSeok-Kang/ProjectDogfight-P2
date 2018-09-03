using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAASiegeCarController : AIAACarController {


    public float siegeDistance = 100f;
    public bool isCanSiege = false;

	// Update is called once per frame
	protected override void Update () {
        base.Update();

        if(target != null && isCanThrust == true)
        {
            if (isCanSiege)
            {
                if (MesureDistanceWithTarget())
                {
                    isCanThrust = false;
                }
            }
        }
	}

    private bool MesureDistanceWithTarget()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= siegeDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
