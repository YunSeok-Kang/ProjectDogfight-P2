using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountText : MonoBehaviour {

    public Text enemyCountText = null;


    // Use this for initialization
    private void Start()
    {
        if (enemyCountText == null)
        {
            enemyCountText = gameObject.GetComponent<Text>();
            if (enemyCountText == null)
            {
                Debug.LogError("EnemyCountText: Text UI를 찾을 수 없습니다.");
            }
        }

        VoxEventManager.Instance.AddObserver("UpdateEnemyCount", UpdateEnemyCount);
    }

    private void UpdateEnemyCount(object p_count)
    {
        int numb = System.Convert.ToInt32(p_count);

        enemyCountText.text =  numb.ToString("00");
        
    }
}
