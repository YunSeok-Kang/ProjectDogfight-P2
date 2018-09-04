using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    GameStart,
    GamePlaying,
    GameLose,
    GameWin,
};

public class GameManager : MonoBehaviour
{
    static private GameManager _instance = null;
    static public GameManager Instance
    {
        private set
        {
            _instance = value;
        }

        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                _instance.Init();


                if (_instance == null)
                {
                    Debug.LogError("GameManager를 Hierarchy 탭에서 찾지 못했습니다.");
                }
            }

            return _instance;
        }
    }

    [SerializeField]
    private Airplane _playerPlane = null;
    private GameManager dontDestroyThisObect = null;

    public int restartCount = 0;

    public GameObject winResultPrefab = null;
    public GameObject loseResultPrefab = null;

    public GameStart gameStartObject = null;

    /// <summary>
    /// 스코어.
    /// 적 오브젝트를 파괴했을 때 얻어지는 Score는 WaveManager에서 처리한다.
    /// </summary>
    private int _score = 0;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            //scoreUI.text = _score.ToString();
        }
    }

    private void Awake()
    {
        Debug.Log("시작");


        GameObject playerPlaneObj = FindObjectOfType<PlayerAirplaneController>().gameObject;
        Airplane plane = playerPlaneObj.GetComponent<Airplane>();

        if (plane == null)
        {
            Debug.LogError("GameManager: PlayerPlane을 찾을 수 없습니다.");
        }

        Debug.Log(plane);

        plane.onHPZeroEvent += PlayerOnHPZero;
        plane.onCrashedEvent += PlayerOnCrashed;

        _playerPlane = plane;

        VoxEventManager.Instance.AddObserver("AllWavesWereCleared", AllWavesWereCleared);

        if (dontDestroyThisObect != null)
        {
            Destroy(this.gameObject);
        }

        dontDestroyThisObect = this;
        DontDestroyOnLoad(this);
    }

    private void Init()
    {
        //if (_playerPlane == null)
        //{
        //    Debug.LogError("GameManager: PlayerPlane을 찾을 수 없습니다.");
        //}

        //if (gameStartObject == null)
        //{
        //    Debug.LogError("GameManager: gameStartObject를 찾을 수 없습니다.");
        //}

        

        //gameStartObject.enabled = true;
    }

    /// <summary>
    /// 여기 작동안함.
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int level)
    {
        //Debug.Log("시작");

        //GameObject playerPlaneObj = FindObjectOfType<PlayerAirplaneController>().gameObject;
        //Airplane plane = playerPlaneObj.GetComponent<Airplane>();

        //if (plane == null)
        //{
        //    Debug.LogError("GameManager: PlayerPlane을 찾을 수 없습니다.");
        //}

        //Debug.Log(plane);

        //plane.onHPZeroEvent += PlayerOnHPZero;
        //plane.onCrashedEvent += PlayerOnCrashed;

        //_playerPlane = plane;

        //VoxEventManager.Instance.AddObserver("AllWavesWereCleared", AllWavesWereCleared);
    }

    private void Start()
    {
        //// 테스트용 스크립트
        //StartCoroutine(Test());
    }

    //IEnumerator Test()
    //{
    //    yield return new WaitForSeconds(5f);
    //    PlayerLose();
    //}

    private void GameEnded()
    {
        VoxObject[] voxObjects = FindObjectsOfType<VoxObject>();
        foreach (VoxObject voxObject in voxObjects)
        {
            voxObject.ActivateObject(false);
        }
    }

    private void PlayerLose()
    {
        GameEnded();

        Instantiate(loseResultPrefab);
    }

    private void PlayerWin()
    {
        GameEnded();

        Instantiate(winResultPrefab);
    }

    /// <summary>
    /// 플레이어 비행기의 HP가 0이 되었을 때 실행
    /// </summary>
    /// <param name="playerPlane"></param>
    private void PlayerOnHPZero(Vehicle playerPlane)
    {
        PlayerLose();
    }

    private void PlayerOnCrashed(Airplane playerPlane)
    {
        PlayerLose();
    }

    private void AllWavesWereCleared(object param)
    {
        PlayerWin();
    }


    // ----------------------------------------------------------------- 외부에서 컨트롤하는 함수 -----------------------------------------------------------------
    public void RestartGame()
    {
        restartCount++;
        SceneManager.LoadScene("BasicScene"); // 씬 이름 제대로 적어야 함.
    }
}
