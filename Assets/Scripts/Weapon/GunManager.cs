using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();

    private void Awake()
    {
        if(guns.Count ==0)
        {
            Debug.LogError("GunManager : guns 비어있음");
        }
    }

    public void Fire()
    {
        for (int i = 0; i < guns.Count; i ++)
        {
            guns[i].StartCoroutine("Fire");
        }
    }
}
