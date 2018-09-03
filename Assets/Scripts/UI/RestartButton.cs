using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour 
{
    public void ButtonPressed()
    {
        GameManager.Instance.RestartGame();
    }
}
