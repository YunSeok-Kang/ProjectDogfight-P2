using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossController : AIController
{
    public GameObject target;
    public GunManager mainGun;
    [Header("Missle")]
    public GunManager hydraManager;
    public GunManager hunterKillerManager;
    public Transform misslePlatform;
    public Transform missleTurret;
    public float misslePlatformRotateTime = 3f;
    [Header("Gun")]
    public float mainGunRange;
    public float mainGunRotateSpeed;

    private float curRotateTime;
    private float totalRotateTime;
    private bool isShootingMissles= false;
    private void Start()
    {
        target = FindObjectOfType<PlayerAirplaneController>().gameObject;

        if (target != null)
        {
            StartCoroutine("PatternManager");
        }
    }
    private void Update()
    {
        if (!isShootingMissles)
        {
            ShootAAGun();
        }
    }


    private IEnumerator PatternManager()
    {
        yield return new WaitForSeconds(5.0f);

        while (true)
        {
        isShootingMissles = true;
            yield return StartCoroutine("MisslePattern1");
            yield return new WaitForSeconds(3.0f);
            yield return StartCoroutine("MisslePattern2");
            yield return StartCoroutine("MisslePattern3");
        isShootingMissles = false;
            yield return new WaitForSeconds(15.0f);
        }
    }

    private IEnumerator MisslePattern1()
    {
        curRotateTime = Time.time;
        totalRotateTime = misslePlatformRotateTime + Time.time;
        while (true)
        {
            misslePlatform.localEulerAngles = Vector3.Slerp(Vector3.zero, new Vector3(0, -90, 0), curRotateTime / totalRotateTime);
            curRotateTime = Time.time;

            hydraManager.PullTrigger(target);
            if (curRotateTime >= totalRotateTime)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        hydraManager.Reload();
        yield return null;
    }
    private IEnumerator MisslePattern2()
    {
        missleTurret.localEulerAngles= new Vector3(-90, 0, 0);
        while (true)
        {
            hunterKillerManager.PullTrigger(target);
            if (hunterKillerManager.guns[0].loadedCapacity <= 0)
            {
                hunterKillerManager.Reload();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    private IEnumerator MisslePattern3()
    {
        misslePlatform.localEulerAngles = Vector3.zero;
        missleTurret.localEulerAngles = new Vector3(-25, 0, 0);
        yield return null;
    }
    private void ShootAAGun()
    {
        if (target != null)
        {
            if (mainGun.guns[0].loadedCapacity > 0)
            {
                RotateGunTowardsTarget();
            }
            ShootOnRay();
            Reload();
        }
    }
    private void RotateGunTowardsTarget()
    {
        Vector3 targetDir = target.transform.position - mainGun.transform.position;

        float step = mainGunRotateSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(mainGun.transform.forward, targetDir, step, 0.0f);
        mainGun.transform.rotation = Quaternion.LookRotation(newDir);
        /*mainGun.transform.eulerAngles = new Vector3(Mathf.Clamp(mainGun.transform.eulerAngles.x, 280, 360),
            Mathf.Clamp(mainGun.transform.eulerAngles.y, 0, 100), 0);*/
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
