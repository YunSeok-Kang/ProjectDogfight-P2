using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Airplane))]
public class PlayerAirplaneController : PlayerController
{
    private Airplane _playerAirplane = null;

    [Header("Gun")]
    [SerializeField]
    private GunManager _gunManger = null;
    public GunManager GunManager
    {
        get
        {
            return _gunManger;
        }
    }

    public float gizmoRadius = 25f;
    public bool isDrawGizmo = false;
    //비행기 존재 확인하려고 기즈모 그림
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(isDrawGizmo)
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    // Use this for initialization
    private void Start ()
    {
		if (_playerAirplane == null)
        {
            _playerAirplane = gameObject.GetComponent<Airplane>();  
        }
	}

    private void Update()
    {
        GetUserGunControl();
    }

    private void FixedUpdate()
    {
        float pitchingValue = GetUserPitch();

        _playerAirplane.Move(pitchingValue, 1, false);

    }

    private float GetUserPitch()
    {
#if UNITY_IOS || UNITY_ANDROID
        return PoolPitchByJoystick();
#else
        return PoolPitchByKeyboard();
#endif
    }

    private void GetUserGunControl()
    {
#if UNITY_IOS || UNITY_ANDROID
        if(fireButton.GetFireInput())
        {
            Fire();
        }
#else
        if (Input.GetButton("Fire1"))
        {
        Fire();
        }
#endif
    }

    public float PoolPitchByKeyboard()
    {
        float pitchingValue = Input.GetAxis("Vertical");
        pitchingValue = Mathf.Clamp(pitchingValue, -1, 1);

        // Debug.Log("Pitching Value: " + pitchingValue);
        return pitchingValue;

        //CrossPlatformInputManager.GetAxis("Vertical");
    }
    public float PoolPitchByJoystick()
    {

        float h = joystick.GetHorizontalValue();
        h = Mathf.Clamp(h, -1, 1);

        float v = joystick.GetVerticalValue();
        v = Mathf.Clamp(v, -1, 1);

        Vector2 direction = new Vector2(h, v);

        return v;
    }
    public void Fire()
    {
        PlayerCameraManager.Instance.StartCoroutine("Shake");
        _gunManger.PullTrigger();
    }
}
