using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Item : VoxObject
{
    public virtual void Use()
    {
        VoxDestroy();
    }

    public virtual void Use(VoxObject target)
    {
        VoxDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var vox = other.gameObject.GetComponent<VoxObject>();
            Use(vox);
        }
    }
}
