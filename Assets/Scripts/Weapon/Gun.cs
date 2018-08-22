using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Chamber")]
    public bool isUnlimited = false;
    public bool isLoadedOnStart = true;
    public int maxCapacity;
    public int loadedCapacity;
    public GameObject ammoType;

    [Header("Chamber Performance")]
    public float delayOfFire;

    [Header("Muzzle")]
    public Transform muzzle;

    private bool isCanFire = true;
    private string enemyTag;



    protected override bool Init()
    {
        if(isLoadedOnStart)
        {
            loadedCapacity = maxCapacity;
        }

        string thisTag = transform.root.GetComponent<Vehicle>().tag;
        enemyTag = (thisTag == "Player") ? "Enemy" : "Player";

        return true;
    }

    public IEnumerator Fire()
    {
        if (isCanFire)
        {
            if (loadedCapacity> 0 || isUnlimited)
            {
                SpawnAmmo();
                if (!isUnlimited)
                {
                    loadedCapacity--;
                }
            }
            isCanFire = false;
            yield return new WaitForSeconds(delayOfFire);
            isCanFire = true;
        }
        else
        {
            yield return null;
        }
    }
    private void SpawnAmmo()
    {
        var newAmmo =
           Instantiate(ammoType,
           muzzle.position,
           muzzle.rotation);
        newAmmo.GetComponent<AmmoPropellant>().Propel(muzzle.forward);
        newAmmo.GetComponent<Ammo>().enemyTag = this.enemyTag;
    }
}
