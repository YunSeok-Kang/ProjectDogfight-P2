using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Airplane))]
public class AIAirplaneController : AIController
{
    private Airplane _AIAirplane = null;

    [SerializeField]
    // How sensitively the AI applies the pitch controls
    private float _pitchSensitivity = .5f;

    [SerializeField]
    // The speed at which the plane will wander laterally
    private float _lateralWanderSpeed = 0.11f;

    [SerializeField]
    // The amount that the plane can wander by when heading for a target
    private float _lateralWanderDistance = 5;

    [SerializeField]
    // The maximum angle that the AI will attempt to make plane can climb at
    private float _maxClimbAngle = 360;            

    [SerializeField]
    // This increases the effect of the controls based on the plane's speed.
    private float _speedEffect = 0.01f;           

    // Used for generating random point on perlin noise so that the plane will wander off path slightly
    private float _randomPerlin;

    //[SerializeField]
    //private Transform _target = null;

    public AIAirplaneRouteManager routeManager = null;


    // 공격 기능

    public GunManager gunManager = null;

    /// <summary>
    /// 비행기 사정거리
    /// </summary>
    public float fireRange = 10f; 

    // Use this for initialization
    private void Start()
    {
        if (_AIAirplane == null)
        {
            _AIAirplane = gameObject.GetComponent<Airplane>();
        }

        if (sight == null)
        {
            sight = gameObject.GetComponentInChildren<Sight>();
        }

        if (gunManager == null)
        {
            gunManager = gameObject.GetComponentInChildren<GunManager>();
        }

        if (routeManager == null)
        {
            // 없으면 플레이어 무조건 따라가도록 함.
            routeManager = gameObject.AddComponent<AIAirplaneRouteManager>();

            //Debug.LogError("AIAirplaneController: AI 비행기를 비행을 위해 routeManager 정보가 꼭 필요합니다.");
        }

        routeManager.targetController = this;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(sight.transform.position, sight.transform.TransformDirection(transform.forward) * fireRange);
    //}

    private void FixedUpdate()
    {
        Fly();

        Debug.DrawRay(sight.transform.position,
             sight.transform.TransformDirection(Vector3.forward) * fireRange,
             Color.red);

        RaycastHit hit;
        bool isHit = Physics.Raycast(sight.transform.position, sight.transform.TransformDirection(Vector3.forward), out hit, fireRange);
        if (isHit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                gunManager.PullTrigger();
            }
        }
    }

    private void Fly()
    {
        if (routeManager.CurrentTarget == null)
        {
            Debug.Log("타겟 없음");

            _AIAirplane.Move(0, 0, false);
            return;
        }

        // make the plane wander from the path, useful for making the AI seem more human, less robotic.
        //Vector3 targetPos = _target.position + transform.right * (Mathf.PerlinNoise(Time.time * _lateralWanderSpeed, _randomPerlin) * 2 - 1) * _lateralWanderDistance;
        Vector3 targetPos = routeManager.CurrentTarget.position;

        // adjust the yaw and pitch towards the target
        Vector3 localTarget = transform.InverseTransformPoint(targetPos);
        float targetAnglePitch = -Mathf.Atan2(localTarget.y, localTarget.z);

        // Set the target for the planes pitch, we check later that this has not passed the maximum threshold
        targetAnglePitch = Mathf.Clamp(targetAnglePitch, -_maxClimbAngle * Mathf.Deg2Rad,
                                       _maxClimbAngle * Mathf.Deg2Rad);

        // calculate the difference between current pitch and desired pitch
        float changePitch = targetAnglePitch - _AIAirplane.PitchAngle;

        // AI applies elevator control (pitch, rotation around x) to reach the target angle
        float pitchInput = changePitch * _pitchSensitivity;

        // adjust how fast the AI is changing the controls based on the speed. Faster speed = faster on the controls.
        float currentSpeedEffect = 1 + (_AIAirplane.ForwardSpeed * _speedEffect);
        pitchInput *= currentSpeedEffect;

        _AIAirplane.Move(pitchInput, 1, false);
    }
}
