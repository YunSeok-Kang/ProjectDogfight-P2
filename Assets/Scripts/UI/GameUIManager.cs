using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    static private GameUIManager _instance = null;
    static public GameUIManager Instance
    {
        private set
        {
            _instance = value;
        }

        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameUIManager>();


                if (_instance == null)
                {
                    Debug.LogError("GameManager를 Hierarchy 탭에서 찾지 못했습니다.");
                }
            }

            return _instance;
        }
    }

    public Text scoreText;
    public Text waveNumberText;
    public Text enemyNumberText;
    public Image teaseImage;
}
