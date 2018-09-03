using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 프로젝트의 오브젝트 그룹중 최상단. UE4로 비유하면 Actor의 역할. 
/// </summary>
public class VoxObject : MonoBehaviour {

    [SerializeField]
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
            
            if(value <= 0 )
            {
                OnHPZero();
            }
        }

        get
        {
            return _hp;
        }
    }


    // ----------------------------------------------------- Unity Event Functions ----------------------------------------------------- //
    protected virtual void Awake()
    {
        if (!Init())
        {
            Debug.LogError("VoxObject : 초기화 실패");
            return;
        }
    }

	// Update is called once per frame
	protected virtual void Update ()
    {

	}


    // ----------------------------------------------------- VoxObject Functions ----------------------------------------------------- //

    protected virtual float CalculateActualDamage(float value)
    {
        float finalDamage = _hp - value;

        // 원하는거 하세요

        return finalDamage;
    }

    protected virtual float CalculateActualHealing(float value)
    {
        float finalHealing = value - _hp;

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
        VoxDestroy();
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

    public void VoxDestroy(float destroyDelay = 0f)
    {
        StartCoroutine("VoxDestroySchedule", destroyDelay);
    }
    private IEnumerator VoxDestroySchedule(float destroyDelay = 0f)
    {
        yield return new WaitForSeconds(destroyDelay);
        VoxDestroyLogic();
        yield return null;
    }
    /// <summary>
    /// 파괴시 행동을 바꾸려면 이 함수를 변경
    /// </summary>
    /// <param name="destroyDelay"></param>
    protected virtual void VoxDestroyLogic()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Object를 Activate/Unactivate 시킬 때 취할 액션을 지정할 수 있음.
    /// </summary>
    /// <param name="activate"></param>
    public virtual void ActivateObject(bool activate)
    {
        if (activate)
        {
            OnActivateObject();
        }
        else
        {
            OnUnActivateObject();
        }
    }

    protected virtual void OnActivateObject()
    {
        this.enabled = true;
    }

    protected virtual void OnUnActivateObject()
    {
        this.enabled = false;
    }
}
