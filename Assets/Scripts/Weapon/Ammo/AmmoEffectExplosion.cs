using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEffectExplosion : AmmoEffect
{
    public ParticleSystem explosionEffect;
    public float centerDamage;
    public float centerRadius; // 100% 데미지 받는 폭심지
    public float middleRadius; // 50% 데미지 받는 존
    public float totalRadius; // 그외 25데미지 받는 반경

    void OnDrawGizmosSelected()
    {
        DrawExplosiveRadius();
    }
    private void DrawExplosiveRadius()
    {
        Color explosiveRed = Color.red;
        explosiveRed.a = 0.25f;
        Color explosiveYellow = Color.yellow;
        explosiveYellow.a = 0.25f;
        Color explosiveGreen = Color.green;
        explosiveGreen.a = 0.25f;

        Gizmos.color = explosiveRed;
        Gizmos.DrawSphere(transform.position, centerRadius);

        Gizmos.color = explosiveYellow;
        Gizmos.DrawSphere(transform.position, middleRadius);

        Gizmos.color = explosiveGreen;
        Gizmos.DrawSphere(transform.position, totalRadius);
    }
    public override void Effect()
    {
        Explosion();
        base.Effect();
    }
    protected virtual void Explosion()
    {
        CreateExplosionEffect();

        Collider[] colliders = Physics.OverlapSphere(transform.position, totalRadius);
        foreach (Collider hit in colliders)
        {
            if (hit == this.gameObject)
            {
                continue;
            }
            GiveDamageByReductionRate(hit.gameObject,
            CalculateDamageReductionRateByDistance(hit.gameObject));
        }
    }
    private void CreateExplosionEffect()
    {
        var particleEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        float totalDuration = particleEffect.main.duration + particleEffect.subEmitters.GetSubEmitterSystem(0).main.duration;
        Destroy(particleEffect.gameObject, totalDuration);
    }

    private float CalculateDamageReductionRateByDistance(GameObject other)
    {
        float distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance <= centerRadius) //100%
        {
            Debug.Log(other.name + " 100");
            return 1f;
        }
        else if (distance <= middleRadius) //50%
        {
            Debug.Log(other.name + " 50");
            return 0.5f;
        }
        else //25%
        {
            Debug.Log(other.name + " 25");
            return 0.25f;
        }
    }
    private void GiveDamageByReductionRate(GameObject other, float reduction)
    {
        other.GetComponent<VoxObject>().HP -= centerDamage * reduction;
    }
}