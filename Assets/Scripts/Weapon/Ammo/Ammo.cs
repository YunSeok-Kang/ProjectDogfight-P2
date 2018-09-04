using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Weapon
{
    public float lifetime = 5f;
    public string enemyTag;
    public ParticleSystem particleOnDestroy = null;
    public AudioClip clipOnDestroy = null;

    // Use this for initialization
    void Start()
    {
        VoxDestroy(lifetime);
    }
    protected override void VoxDestroyLogic()
    {
        if (particleOnDestroy != null)
        {
            var particleEffect = Instantiate(particleOnDestroy, transform.position, transform.rotation);
            float totalDuration = particleEffect.main.duration;
            for (int i = 0; i < particleEffect.subEmitters.subEmittersCount; i++)
            {
                totalDuration += particleEffect.subEmitters.GetSubEmitterSystem(i).main.duration;
            }
            Destroy(particleEffect.gameObject, totalDuration);
        }

        if (clipOnDestroy != null)
        {
            var tempGO = new GameObject("tempSource");
            tempGO.transform.position = this.transform.position;
            var src = tempGO.AddComponent<AudioSource>();
            src.PlayOneShot(clipOnDestroy, 0.5f);
            Destroy(tempGO, clipOnDestroy.length);
        }

        base.VoxDestroyLogic();
    }
}
