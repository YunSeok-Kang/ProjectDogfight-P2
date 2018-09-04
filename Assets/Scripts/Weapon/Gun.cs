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

    protected bool _isCanFire = true;
    protected string _enemyTag;


    public AudioSource source = null;
    public AudioClip firingSound= null;

    protected override bool Init()
    {
        if (isLoadedOnStart)
        {
            loadedCapacity = maxCapacity;
        }
        string thisTag = transform.parent.GetComponentInParent<VoxObject>().tag;
        _enemyTag = (thisTag == "Player") ? "Enemy" : "Player";

        /*if (source == null)
        {
            Debug.LogError("Gun : Audio Source 없음");
        }*/

        return true;
    }

    public void Fire()
    {
        StartCoroutine("StartFire");
    }
    /// <summary>
    /// 미사일 런처 등, target을 가져야하는 총들을 위해 작성한 함수
    /// </summary>
    /// <param name="target"></param>
    public virtual void Fire(GameObject target)
    {
    }

    protected IEnumerator StartFire()
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
    protected IEnumerator FireSingleShot()
    {
        SpawnAmmoByCapacity();
        yield return null;
    }
    protected IEnumerator FireBurstShot()
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

    protected void SpawnAmmoByCapacity()
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

    protected void SpawnAmmo()
    {
        var newAmmo =
           Instantiate(ammoType,
           muzzle.position,
           muzzle.rotation);
        newAmmo.GetComponent<AmmoPropellant>().Propel(muzzle.forward);
        newAmmo.GetComponent<Ammo>().enemyTag = this._enemyTag;

        PlaySound();
    }

    private void PlaySound()
    {
        if (source != null && firingSound !=null)
        {
            source.pitch = Random.Range(0.75f, 1.5f);
            source.PlayOneShot(firingSound);
        }
    }
}
