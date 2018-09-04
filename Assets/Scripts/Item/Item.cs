using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Item : VoxObject
{
    public AudioClip pickupClip = null;

    public virtual void Use()
    {

        PlaySound();
        VoxDestroy();
    }

    public virtual void Use(VoxObject target)
    {
        PlaySound();
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

    private void PlaySound()
    {
        if (pickupClip != null)
        {
            var tempGO = new GameObject("tempSource");
            tempGO.transform.position = this.transform.position;
            var src = tempGO.AddComponent<AudioSource>();
            src.PlayOneShot(pickupClip, 0.5f);
            Destroy(tempGO, pickupClip.length);
        }
    }
}
