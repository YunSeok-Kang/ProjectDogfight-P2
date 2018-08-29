using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTriggerTime : AmmoTrigger
{
    public float triggerTime;
    [Header("Use Random")]
    public bool isUsingRandomTriggerTime = false;
    public float minTriggerTime;
    public float maxTriggerTime;

    private void Start()
    {
        StartCoroutine("EffectAfterTime");
    }
    IEnumerator EffectAfterTime()
    {
        if(isUsingRandomTriggerTime)
        {
            triggerTime = Random.Range(minTriggerTime, maxTriggerTime);
        }
        yield return new WaitForSeconds(triggerTime);
        Trigger();
    }
}
