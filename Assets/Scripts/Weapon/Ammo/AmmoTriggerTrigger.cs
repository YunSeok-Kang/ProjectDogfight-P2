using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTriggerTrigger : AmmoTrigger {
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag(ammo.enemyTag))
        {
            Trigger(other.gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            Trigger();
        }
    }
}

