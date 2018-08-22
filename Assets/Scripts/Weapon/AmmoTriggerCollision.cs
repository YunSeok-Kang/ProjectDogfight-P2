using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTriggerCollision : AmmoTrigger
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ammo.enemyTag))
        {
            ammoEffect.Effect(collision.gameObject);
        }
    }
}
