using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Airplane))]
public class PlayerAirplaneController : PlayerController
{
    private Airplane _playerAirplane = null;

	// Use this for initialization
	void Start ()
    {
		if (_playerAirplane == null)
        {
            _playerAirplane = gameObject.GetComponent<Airplane>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //CrossPlatformInputManager.GetAxis("Vertical");
        //bool airBrakes = CrossPlatformInputManager.GetButton("Fire1");
    }

    float GetUserPitch()
    {
        float pitchingValue = Input.GetAxis("Vertical");
        pitchingValue = Mathf.Clamp(pitchingValue, -1, 1);

        Debug.Log("Pitching Value: " + pitchingValue);
        return pitchingValue;

        //CrossPlatformInputManager.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        float pitchingValue = GetUserPitch();

        _playerAirplane.Move(pitchingValue, 1, false);   
    }
}
