using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveNumberText : MonoBehaviour
{
    public Text waveNumberText = null;


    // Use this for initialization
    private void Start()
    {
        if (waveNumberText == null)
        {
            waveNumberText = gameObject.GetComponent<Text>();
            if (waveNumberText == null)
            {
                Debug.LogError("WaveNumberText: Text UI를 찾을 수 없습니다.");
            }
        }

        VoxEventManager.Instance.AddObserver("WaveChanged", UpdateWaveNumber);
    }

    private void UpdateWaveNumber(object p_number)
    {
        int numb = System.Convert.ToInt32(p_number);

        if (numb == 0)
        {
            waveNumberText.text = "손풀기";
        }
        else
        {
            waveNumberText.text = "Wave " + numb.ToString();
        }
    }

}
