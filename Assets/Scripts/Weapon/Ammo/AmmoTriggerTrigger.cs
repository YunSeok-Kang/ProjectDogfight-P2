using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTriggerTrigger : AmmoTrigger {
    public bool isEffectOnWall = true;

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag(ammo.enemyTag))
        {
            Trigger(other.gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            if (isEffectOnWall)
            {
                Trigger();
            }
            else
            {
                ammo.VoxDestroy();
            }
        }
    }
}

