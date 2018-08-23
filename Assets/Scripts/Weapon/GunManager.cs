using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();
    public float reloadTime = 0f;
    private bool isCanFire = true;
    private void Awake()
    {
        if(guns.Count ==0)
        {
            Debug.LogError("GunManager : guns 비어있음");
        }
    }

    public void PullTrigger()
    {
        if (isCanFire)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                guns[i].StartCoroutine("Fire");
            }
        }
    }
    public void Reload()
    {
        StartCoroutine("StartReload");
    }
    private IEnumerator StartReload()
    {
        isCanFire = false;

        yield return new WaitForSeconds(reloadTime);
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].Reload();
        }
        isCanFire = true;

        yield return null;
    }
}
