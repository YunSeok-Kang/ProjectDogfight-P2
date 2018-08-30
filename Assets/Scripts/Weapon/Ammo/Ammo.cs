using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Weapon
{
    public float lifetime = 5f;
    public ParticleSystem ammoDestroyEffect = null;
    [HideInInspector]
    public string enemyTag;

	// Use this for initialization
	void Start ()
    {
        VoxDestroy(lifetime);
	}
    protected override void VoxDestroy(float destroyDelay = 0)
    {
        if (ammoDestroyEffect != null)
        {
            var particleEffect = Instantiate(ammoDestroyEffect, transform.position, transform.rotation);
            float totalDuration = particleEffect.main.duration + particleEffect.subEmitters.GetSubEmitterSystem(0).main.duration;
            Destroy(particleEffect.gameObject, totalDuration);
        }
        base.VoxDestroy(destroyDelay);
    }

}
