using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPower : Item {

    public GameObject newGunToAttach;
    public float gunLastTime;
    public override void Use(VoxObject target)
    {
        var playerController = target.gameObject.GetComponent<PlayerAirplaneController>();
        var gunMng = playerController.GunManager;


     if(gunMng.guns.Count <= 2)
        {
            var newGun = Instantiate(newGunToAttach,gunMng.transform).GetComponent<Gun>();
            gunMng.AddGun(newGun);

            if (gunMng.guns.Count <= 2)
            {
                newGun.transform.position = playerController.gunSpawnPositions[0].position;
                newGun.transform.rotation = playerController.gunSpawnPositions[0].rotation;
            }
            else
            {
                newGun.transform.position = playerController.gunSpawnPositions[1].position;
                newGun.transform.rotation = playerController.gunSpawnPositions[1].rotation;
            }
        }
        base.Use(target);
    }
}
