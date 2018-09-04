using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoEffect : AmmoModel
{
    protected Ammo ammo;
    //ammo Effect when ammo hit anything
    public ParticleSystem effectParticle = null;
    public AudioSource effectAudio = null;
    public AudioClip effectClip = null;
    private void Awake()
    {
        ammo = GetComponent<Ammo>();
        effectAudio = GetComponent<AudioSource>();
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
            effectAudio.pitch = Random.Range(0.75f, 1.5f);
            effectAudio.PlayOneShot(effectClip);
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
