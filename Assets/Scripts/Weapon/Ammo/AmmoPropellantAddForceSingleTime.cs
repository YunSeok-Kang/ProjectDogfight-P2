using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPropellantAddForceSingleTime : AmmoPropellant
{
    public override void Propel(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * force);
    }
}
