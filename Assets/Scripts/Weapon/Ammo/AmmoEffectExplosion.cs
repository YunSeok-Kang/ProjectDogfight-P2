using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEffectExplosion : AmmoEffect
{
    public ParticleSystem explosionEffect;

    public override void Effect()
    {
        Explosion();
        base.Effect();
    }
    protected virtual void Explosion()
    {
        var particleEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        float totalDuration = particleEffect.main.duration + particleEffect.subEmitters.GetSubEmitterSystem(0).main.duration;
        Destroy(particleEffect.gameObject, totalDuration);
    }
}
