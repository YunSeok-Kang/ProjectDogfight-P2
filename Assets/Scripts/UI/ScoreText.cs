using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text scoreText = null;

	// Use this for initialization
	private void Start ()
    {
		if (scoreText == null)
        {
            scoreText = gameObject.GetComponent<Text>();
            if (scoreText == null)
            {
                Debug.LogError("ScoreText: Text UI를 찾을 수 없습니다.");
            }
        }

        VoxEventManager.Instance.AddObserver("ScoreHasChanged", UpdateScore);
	}

    private void UpdateScore(object p_score)
    {
        int score = System.Convert.ToInt32(p_score);

        scoreText.text = score.ToString("000000000");
    }
}
