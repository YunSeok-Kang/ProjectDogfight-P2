using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPower : Item
{
    public override void Use(VoxObject target)
    {
        var playerController = target.GetComponentInParent<PlayerAirplaneController>();
        var gunMng = playerController.GunManager;

        if (gunMng.guns[1].gameObject.activeSelf == false)
        {
            gunMng.guns[1].gameObject.SetActive(true);
        }
        else if (gunMng.guns[2].gameObject.activeSelf == false)
        {
            gunMng.guns[2].gameObject.SetActive(true);
        }

        base.Use(target);
    }
}
