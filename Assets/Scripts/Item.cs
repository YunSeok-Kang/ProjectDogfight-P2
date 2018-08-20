using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Item : VoxObject
{
    public virtual void Use()
    {

    }

    public virtual void Use(VoxObject target)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(""))
        {

        }
    }
}
