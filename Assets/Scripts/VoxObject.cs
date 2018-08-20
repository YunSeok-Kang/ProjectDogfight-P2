using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 프로젝트의 오브젝트 그룹중 최상단. UE4로 비유하면 Actor의 역할. 
/// </summary>
public class VoxObject : MonoBehaviour {

    private float _hp = 0f;

    public float HP
    {
        set
        {
            if (value < _hp) // hp가 깎일 예정이면 조건문 시행
            {
                float finalDamage = CalculateActualDamage(value); // 실제로 받을 데미지 계산 
                if (finalDamage > 0) // 실제로 데미지를 받았으면
                {
                    _hp -= finalDamage;
                    OnDamaged(finalDamage);
                }
            }

            if (value > _hp) 
            {
                float finalHealing = CalculateActualHealing(value); // 실제로 회복할 HP 계산
                if (finalHealing > 0)
                {
                    _hp += finalHealing;
                    OnHealed(finalHealing);
                }
            }
        }

        get
        {
            return _hp;
        }
    }


    // ----------------------------------------------------- Unity Event Functions ----------------------------------------------------- //
    private void Awake()
    {
        if (!Init())
        {
            Debug.LogError("VoxObject : 초기화 실패");
            return;
        }
    }

    // Use this for initialization
    private void Start ()
    {
        
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {

	}


    // ----------------------------------------------------- VoxObject Functions ----------------------------------------------------- //

    protected virtual float CalculateActualDamage(float damage)
    {
        float finalDamage = damage;

        // 원하는거 하세요

        return finalDamage;
    }

    protected virtual float CalculateActualHealing(float healing)
    {
        float finalHealing = healing;

        return finalHealing;
    }

    protected virtual void OnDamaged(float damage)
    {

    }

    protected virtual void OnHealed(float healPoint)
    {

    }

    protected virtual void OnHPZero()
    {

    }


    /// <summary>
    /// 초기화 진행. 성공시 true, 실패시 false 반환.
    /// </summary>
    /// <returns></returns>
    protected virtual bool Init()
    {
        // hp 세팅 등
        

        return true;
    }

    protected virtual void VoxDestroy(float destroyDelay = 1f)
    {
        // 이펙트(있을 시 플레이)
        
        // 일정 시간 경과후 파괴
        Destroy(gameObject, destroyDelay);
    }
}
