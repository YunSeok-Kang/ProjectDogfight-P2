using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoEffect : AmmoModel
{
    protected Ammo ammo;
    //ammo Effect when ammo hit anything
    public ParticleSystem effectParticle = null;
    public AudioClip effectClip = null;
    private void Awake()
    {
        ammo = GetComponent<Ammo>();
    }
    public virtual void Effect()
    {
        PlayEffect();

        ammo.VoxDestroy();
    }

    public virtual void Effect(GameObject target)
    {
        PlayEffect();

        ammo.VoxDestroy(); 
    }

    protected virtual void PlayEffect()
    {
        if (effectClip != null)
        {
            var tempGO = new GameObject("tempSource");
            tempGO.transform.position = this.transform.position;
            var src = tempGO.AddComponent<AudioSource>();
            src.PlayOneShot(effectClip, 0.5f);
            Destroy(tempGO, effectClip.length);
        }

        if(effectParticle != null)
        {
            var particleEffect = Instantiate(effectParticle, transform.position, transform.rotation);
            float totalDuration = particleEffect.main.duration;
            for (int i = 0; i < particleEffect.subEmitters.subEmittersCount; i++)
            {
                totalDuration += particleEffect.subEmitters.GetSubEmitterSystem(i).main.duration;
            }
            Destroy(particleEffect.gameObject, totalDuration);
        }
    }
}
