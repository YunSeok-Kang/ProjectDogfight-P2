using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoEffect : AmmoModel
{
    public virtual void Effect()
    {
        Destroy(gameObject); //Vox Destroy??
    }

    public virtual void Effect(GameObject target)
    {
        Destroy(gameObject); //Vox Destroy??
    }

}
