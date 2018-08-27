using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {
    public Vector3 direction;
    public List<Transform> rotateTargets = new List<Transform>();
    public float rotateSpeed;

	void Update ()
    {
        for(int i= 0; i < rotateTargets.Count; i ++)
        { 
            rotateTargets[i].Rotate(direction * rotateSpeed * Time.deltaTime);
	    }
    }
}
