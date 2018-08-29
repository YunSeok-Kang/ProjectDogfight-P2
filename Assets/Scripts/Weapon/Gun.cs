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

    [Header("Firing Mode")]
    public bool isFireBurst = false;

    [Header("Burst Options")]
    public int burstRounds = 0;
    public float delayAfterBurst = 0f;

    [Header("Muzzle")]
    public Transform muzzle;

    private bool _isCanFire = true;
    private string _enemyTag;



    protected override bool Init()
    {
        if (isLoadedOnStart)
        {
            loadedCapacity = maxCapacity;
        }

        string thisTag = transform.root.GetComponent<VoxObject>().tag;
        _enemyTag = (thisTag == "Player") ? "Enemy" : "Player";
        return true;
    }

    public void Fire()
    {
        StartCoroutine("StartFire");
    }

    private IEnumerator StartFire()
    {
        if (_isCanFire)
        {
            _isCanFire = false;
            if (isFireBurst)
            {
                StartCoroutine("FireBurstShot");
                yield return new WaitForSeconds(delayAfterBurst);
            }
            else
            {
                StartCoroutine("FireSingleShot");
                yield return new WaitForSeconds(delayOfFire);
            }
            _isCanFire = true;
        }
    }
    private IEnumerator FireSingleShot()
    {
        SpawnAmmoByCapacity();
        yield return null;
    }
    private IEnumerator FireBurstShot()
    {
        for (int i = 0; i < burstRounds; i++)
        {
            SpawnAmmoByCapacity();
            yield return new WaitForSeconds(delayOfFire);
        }
        yield return null;
    }

    public void Reload()
    {
        loadedCapacity = maxCapacity;
    }

    private void SpawnAmmoByCapacity()
    {
        if (loadedCapacity > 0 || isUnlimited)
        {
            SpawnAmmo();
            if (!isUnlimited)
            {
                loadedCapacity--;
            }
        }
    }

    private void SpawnAmmo()
    {
        var newAmmo =
           Instantiate(ammoType,
           muzzle.position,
           muzzle.rotation);
        newAmmo.GetComponent<AmmoPropellant>().Propel(muzzle.forward);
        newAmmo.GetComponent<Ammo>().enemyTag = this._enemyTag;
    }
}
