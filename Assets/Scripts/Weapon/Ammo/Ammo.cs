using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoEffect))]
[RequireComponent(typeof(AmmoTrigger))]
[RequireComponent(typeof(AmmoPropellant))]
public class Ammo : Weapon
{
    //public string enemyTag;
    public float lifetime = 5f;
    public string enemyTag;

	// Use this for initialization
	void Start ()
    {
        VoxDestroy(lifetime);
	}
}
