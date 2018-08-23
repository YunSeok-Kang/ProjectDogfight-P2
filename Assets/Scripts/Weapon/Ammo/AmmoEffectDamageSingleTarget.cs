using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEffectDamageSingleTarget : AmmoEffect
{
    public float damage = 0f;

    public override void Effect(GameObject target)
    {
        target.GetComponent<VoxObject>().HP -= damage;
        base.Effect(target);
    }
}
