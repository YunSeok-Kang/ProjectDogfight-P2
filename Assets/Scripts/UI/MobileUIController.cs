using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_IOS || UNITY_ANDROID
        return;
#else
        Destroy(gameObject);
#endif
    }
}
