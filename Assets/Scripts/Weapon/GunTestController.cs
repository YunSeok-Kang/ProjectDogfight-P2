using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTestController : MonoBehaviour {
    public GunManager gunManager;

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1"))
        {
            gunManager.PullTrigger();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            gunManager.Reload();
        }
	}
}
