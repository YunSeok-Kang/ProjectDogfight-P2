using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTriggerTime : AmmoTrigger
{
    public float triggerTime;

    private void Start()
    {
        StartCoroutine("EffectAfterTime");
    }
    IEnumerator EffectAfterTime()
    {
        yield return new WaitForSeconds(triggerTime);
        Trigger();
    }
}
