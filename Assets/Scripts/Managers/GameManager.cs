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
    [SerializeField]
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
            VoxEventManager.Instance.PostNotifycation("ScoreHasChanged", _score);
        }
    }

    private void Awake()
    { 
        if (Instance.dontDestroyThisObect != null)
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
            return;
        }

        Instance.dontDestroyThisObect = this;
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Init()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Score = 0;

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
    }

    private void Start()
    {
       PlayerCameraManager.Instance.Target = _playerPlane.gameObject;
        //도발 말풍선 지우기
        GameUIManager.Instance.teaseImage.gameObject.SetActive(false);
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
        // 캠 매니저가 적을 비추도록.
        MakeRandomEnemyToTeasePlayer();

        GameEnded();

        Instantiate(loseResultPrefab);

    }

    private void MakeRandomEnemyToTeasePlayer()
    {
        var waveChildren = WaveManager.Instance.currentWave.transform.GetComponentsInChildren<Vehicle>();

        //도발 외칠 적 고르기
        var newCamTarget = waveChildren[Random.Range(0, waveChildren.Length)];
        PlayerCameraManager.Instance.Target = newCamTarget.gameObject;

        //UI를 World 포지션에 맞춰줘야 할 때 카메라를 기준으로하므로, 
        // 먼저 카메라를 타겟에다 옮겨줘야함.
        PlayerCameraManager.Instance.transform.position = newCamTarget.gameObject.transform.position;
        PlayerCameraManager.Instance.transform.position += PlayerCameraManager.Instance.minOffset;

        //이미지 옮기기
        var teasePos = Camera.main.WorldToScreenPoint(newCamTarget.gameObject.transform.position);
        GameUIManager.Instance.teaseImage.gameObject.SetActive(true);
        GameUIManager.Instance.teaseImage.transform.position = teasePos;
        GameUIManager.Instance.teaseImage.transform.position += new Vector3(300 ,100,0);
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
