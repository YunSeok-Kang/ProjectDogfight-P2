using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleLauncher : Gun
{
    public override void Fire(GameObject target)
    {
        StartCoroutine("StartFire",target);
    }

    protected IEnumerator StartFire(GameObject target)
    {
        if (_isCanFire)
        {
            _isCanFire = false;
            if (isFireBurst)
            {
                StartCoroutine("FireBurstShot", target);
                yield return new WaitForSeconds(delayAfterBurst);
            }
            else
            {
                StartCoroutine("FireSingleShot", target);
                yield return new WaitForSeconds(delayOfFire);
            }
            _isCanFire = true;
        }
    }
    protected IEnumerator FireSingleShot(GameObject target)
    {
        SpawnAmmoByCapacity(target);
        yield return null;
    }
    protected IEnumerator FireBurstShot(GameObject target)
    {
        for (int i = 0; i < burstRounds; i++)
        {
            SpawnAmmoByCapacity(target);
            yield return new WaitForSeconds(delayOfFire);
        }
        yield return null;
    }

    protected void SpawnAmmoByCapacity(GameObject target)
    {
        if (loadedCapacity > 0 || isUnlimited)
        {
            SpawnAmmo(target);
            if (!isUnlimited)
            {
                loadedCapacity--;
            }
        }
    }

    protected void SpawnAmmo(GameObject target)
    {
        var newAmmo =
           Instantiate(ammoType,
           muzzle.position,
           muzzle.rotation);
        newAmmo.GetComponent<AmmoPropellantHoming>().target = target;
        newAmmo.GetComponent<AmmoPropellantHoming>().Propel(muzzle.forward);
        newAmmo.GetComponent<Ammo>().enemyTag = this._enemyTag;
    }
}
