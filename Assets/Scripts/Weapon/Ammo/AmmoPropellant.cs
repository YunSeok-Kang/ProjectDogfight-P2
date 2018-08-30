using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoPropellant : AmmoModel
{
    public float force = 0f;
    public abstract void Propel(Vector3 direction);
}
