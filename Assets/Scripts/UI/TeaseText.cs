using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeaseText : MonoBehaviour {
    public Text teaseText;
    public string[] teastList;

    private void OnEnable()
    {
        teaseText.text = teastList[Random.Range(0, teastList.Length)];
    }
}
