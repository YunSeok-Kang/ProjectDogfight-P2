using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public PlayerCameraManager camManager = null;
    

	// Use this for initialization
	void Start ()
    {
        if (camManager == null)
        {
            Debug.LogError("GameStart: PlayerCameraManager가 없습니다.");
        }

        VoxObject[] voxObjects = FindObjectsOfType<VoxObject>();
        Debug.Log(voxObjects.Length);
        foreach (VoxObject voxObject in voxObjects)
        {
            Debug.Log(voxObject.name);
            voxObject.ActivateObject(false);
        }

        // 재시작을 한 경우라면
        if (GameManager.Instance.restartCount > 0)
        {
            // Tap to start 눌릴 필요 없이 바로 진행
            StartGame();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_IOS || UNITY_ANDROID
        // 화면이 터치되었으면
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    StartGame();
                    break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            StartGame();
        }

#else
        if (Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
#endif


    }


    public void StartGame()
    {
        VoxObject[] voxObjects = FindObjectsOfType<VoxObject>();
        foreach (VoxObject voxObject in voxObjects)
        {
            voxObject.ActivateObject(true);
        }

        
        // 카메라 임시 이동(애니메이션 없이 바로 이동)
        camManager.enabled = true;

        WaveManager.Instance.StartPreWave();

        Destroy(gameObject);
    }
}
