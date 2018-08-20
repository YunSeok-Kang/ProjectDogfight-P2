using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Weapon
{
    public float lifetime = 5f;

	// Use this for initialization
	void Start ()
    {
        VoxDestroy(lifetime);
	}
}
