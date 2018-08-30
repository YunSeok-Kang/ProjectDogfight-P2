using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossController : AIController
{
    public GameObject target;
    public GunManager mainGun;
    public GunManager missilePlatform;
    public float mainGunRange;
    public float mainGunRotateSpeed;


    private void Update()
    {
        PatternTwo();
        //PatternOne();   
    }
    private void PatternOne()
    {
        if (target != null)
        {
            RotateGunTowardsTarget();
            ShootOnRay();
            Reload();
        }
    }
    private void PatternTwo()
    {
        if(target !=null)
        {
            ShootMissle(target);
        }
    }
    private void ShootMissle(GameObject target)
    {
        missilePlatform.PullTrigger(target);
    }
    private void RotateGunTowardsTarget()
    {
        Vector3 targetDir = target.transform.position - mainGun.transform.position;

        float step = mainGunRotateSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(mainGun.transform.forward, targetDir, step, 0.0f);
        mainGun.transform.rotation = Quaternion.LookRotation(newDir);
        mainGun.transform.eulerAngles = new Vector3(Mathf.Clamp(mainGun.transform.eulerAngles.x, 280, 360),
            Mathf.Clamp(mainGun.transform.eulerAngles.y, 0, 100), 0);
    }
    private void ShootOnRay()
    {
        Debug.DrawRay(sight.transform.position,
             sight.transform.TransformDirection(Vector3.forward) * mainGunRange,
             Color.red);

        RaycastHit hit;
        if (Physics.Raycast(sight.transform.position,
            sight.transform.TransformDirection(Vector3.forward),
            out hit, mainGunRange))
        {
            if (hit.transform.gameObject == target)
            {
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        mainGun.PullTrigger();
    }

    private void Reload()
    {
        if (mainGun.guns[0].loadedCapacity <= 0)
        {
            mainGun.Reload();
        }
    }
}
