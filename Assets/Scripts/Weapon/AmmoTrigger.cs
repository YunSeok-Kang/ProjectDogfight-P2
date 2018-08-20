using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoEffect))]
public class AmmoTrigger : AmmoModel
{
    public AmmoEffect ammoEffect = null;

    private void Start()
    {
        if (!ammoEffect)
        {
            Debug.LogError("AmmoTrigger : AmmoEffect 없음.");
        }
    }

    protected virtual void Trigger()
    {
        ammoEffect.Effect();
    }

    protected virtual void Trigger(GameObject target)
    {
        ammoEffect.Effect(target);
    }
}
