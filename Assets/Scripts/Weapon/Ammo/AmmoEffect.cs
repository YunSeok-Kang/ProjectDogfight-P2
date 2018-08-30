using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoEffect : AmmoModel
{
    protected Ammo ammo;
    private void Awake()
    {
        ammo = GetComponent<Ammo>();
    }
    public virtual void Effect()
    {
        ammo.VoxDestroy();
    }

    public virtual void Effect(GameObject target)
    {
        ammo.VoxDestroy(); 
    }
}
