using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAACarController : AICarController {

    public GameObject target = null;
    public GunManager gun = null;
    public float rotateSpeed = 10f;
    public float gunRange = 100f;
    public bool isCanThrust = true;

    protected virtual void Update()
    {
        if(isCanThrust)
        {
            car.Thrust();
        }
        if(target != null)
        {
            RotateGunTowardsTarget();
            ShootOnRay();
            Reload();
        }
    }
    /// <summary>
    /// 대공포를 타겟을 향해 추적
    /// </summary>
    private void RotateGunTowardsTarget()
    {
        Vector3 targetDir = target.transform.position - gun.transform.position;

        float step = rotateSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(gun.transform.forward, targetDir, step, 0.0f);
        gun.transform.rotation = Quaternion.LookRotation(newDir);
    }

    /// <summary>
    /// 레이케스팅 성공시 쏘기
    /// </summary>
    private void ShootOnRay()
    {
        Debug.DrawRay(sight.transform.position,
             sight.transform.TransformDirection(Vector3.forward) * gunRange,
             Color.red);

        RaycastHit hit;
        if (Physics.Raycast(sight.transform.position,
            sight.transform.TransformDirection(Vector3.forward),
            out hit, gunRange))
        {
            if(hit.transform.gameObject == target)
            {
                Shoot();    
            }
        }
    }
    private void Shoot()
    {
        gun.PullTrigger();
    }
    private void Reload()
    {
        if (gun.guns[0].loadedCapacity <= 0)
        {
            gun.Reload();
        }
    }
}
