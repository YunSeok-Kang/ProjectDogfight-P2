using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Weapon
{
    public float lifetime = 5f;
    public string enemyTag;
    public ParticleSystem ammoDestructionParticle = null;

    // Use this for initialization
    void Start()
    {
        VoxDestroy(lifetime);
    }
    protected override void VoxDestroyLogic()
    {
        if (ammoDestructionParticle != null)
        {
            var particleEffect = Instantiate(ammoDestructionParticle, transform.position, transform.rotation);
            float totalDuration = particleEffect.main.duration;
            for (int i = 0; i < particleEffect.subEmitters.subEmittersCount; i++)
            {
                totalDuration += particleEffect.subEmitters.GetSubEmitterSystem(i).main.duration;
            }
            Destroy(particleEffect.gameObject, totalDuration);
        }
        base.VoxDestroyLogic();
    }
}
