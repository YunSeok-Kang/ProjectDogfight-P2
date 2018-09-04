using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();
    public float reloadTime = 0f;
    private bool _isCanFire= true;
    private bool _isReloading = false;



    private void Awake()
    {
        if (guns.Count == 0)
        {
            Debug.LogError("GunManager : guns 비어있음");
        }
    }
    
    public void AddGun(Gun newGun)
    {
        guns.Add(newGun);
    }

    public void PullTrigger()
    {
        if (_isCanFire)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (guns[i].gameObject.activeSelf)
                {
                    guns[i].Fire();
                }
            }
        }
    }
    public void PullTrigger(GameObject target)
    {
        if (_isCanFire)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (guns[i].gameObject.activeSelf)
                {
                guns[i].Fire(target);
                }
            }
        }
    }

    public void Reload()
    {
        if (_isReloading == false)
        {
            StartCoroutine("StartReload");
        }
    }
    private IEnumerator StartReload()
    {
        _isReloading = true;
        _isCanFire= false;

        yield return new WaitForSeconds(reloadTime);

        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i].gameObject.activeSelf)
            {
                guns[i].Reload();
            }
        }
        _isCanFire= true;
        _isReloading = false;

        yield return null;
    }

 
}
