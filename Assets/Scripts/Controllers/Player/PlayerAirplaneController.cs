using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Airplane))]
public class PlayerAirplaneController : PlayerController
{
    private Airplane _playerAirplane = null;
    [SerializeField]
    private GunManager _gunManger = null;
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
        float pitchingValue = Input.GetAxis("Vertical");
        pitchingValue = Mathf.Clamp(pitchingValue, -1, 1);

        Debug.Log("Pitching Value: " + pitchingValue);
        return pitchingValue;

        //CrossPlatformInputManager.GetAxis("Vertical");
    }

    private void GetUserGunControl()
    {
        if(Input.GetButton("Fire1"))
        {
            _gunManger.PullTrigger();
        }
    }
}
