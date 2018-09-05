using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    static private WaveManager _instance = null;
    static public WaveManager Instance
    {
        private set
        {
            _instance = value;
        }

        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<WaveManager>();

                if (_instance == null)
                {
                    Debug.LogError("WaveManager를 Hierarchy 탭에서 찾지 못했습니다.");
                }
            }

            return _instance;
        }
    }

    private float _currentWaveHP = 0f;
    private float _totalWaveHP = 0f;

    private bool _isPreWave = true;

    [SerializeField]
    private Vehicle[] _enemyVehicles;

    //private Queue<GameObject> _waveQueue = null;

    public List<GameObject> _waveList = null;

    public GameObject preWave;
    public GameObject[] waves;

    public Transform waveCreatingTransform = null;

    public GameObject currentWave = null;
    public float currentWaveHP = 0f;

    public AudioClip scoreClip = null;

    // 0 = PreWave
    public int currentWaveCount = 0;

    public int leftEnemies = 0;

    private void Start()
    {
        if (waveCreatingTransform == null)
        {
            waveCreatingTransform = GameObject.Find("Objects").transform;
        }

        _waveList = new List<GameObject>();
        foreach (GameObject wave in waves)
        {
            _waveList.Add(wave);
        }

        //_waveQueue = new Queue<GameObject>();
        //foreach (GameObject wave in waves)
        //{
        //    _waveQueue.Enqueue(wave);
        //}
    }

    private void CreateWave(GameObject waveObject)
    {
        GameObject tempWave = Instantiate(waveObject);
        tempWave.transform.parent = waveCreatingTransform;

        leftEnemies = 0;

        _enemyVehicles = tempWave.GetComponentsInChildren<Vehicle>();
        foreach (Vehicle vehicle in _enemyVehicles)
        {
            _totalWaveHP += vehicle.HP;
            vehicle.onHPZeroEvent += VehicleDestroyEvent;

            // 비행기면 추가적으로 취해줄 액션
            if (vehicle.GetType() == typeof(Airplane))
            {
                Airplane enemyAirplane = vehicle as Airplane;
                enemyAirplane.onCrashedEvent += VehicleCrashingEvent;
            }

            leftEnemies++;

            // Screen Indicator 리스트에 넣는 코드;
        }

        _currentWaveHP = _totalWaveHP;

        currentWave = tempWave;
    }

    private float GetCurrentWaveHP()
    {
        _currentWaveHP = 0;

        foreach (Vehicle vehicle in _enemyVehicles)
        {
            _currentWaveHP += vehicle.HP;
        }

        currentWaveHP = _currentWaveHP;
        return _currentWaveHP;
    }

    public void StartPreWave()
    {
        _isPreWave = true;
        CreateWave(preWave);
    }

    private void StartNextWave()
    {
        GameObject nextWave = null;
        int numberOfWaves = _waveList.Count;

        if (numberOfWaves > 0)
        {
            int randomValue = Random.Range(0, numberOfWaves);
            nextWave = _waveList[randomValue];
            _waveList.RemoveAt(randomValue);

            currentWaveCount++;
            VoxEventManager.Instance.PostNotifycation("WaveChanged", currentWaveCount);
        }
        else
        {
            //다음 웨이브가 없다는 뜻
            // = 승리
            VoxEventManager.Instance.PostNotifycation("AllWavesWereCleared", null);
            return;
        }

        //try
        //{
        //    // 다음 웨이브
        //    nextWave = _waveQueue.Dequeue();
        //}
        //catch (System.InvalidOperationException e) // 큐가 비었을 시
        //{
        //    // 다음 웨이브가 없다는 뜻
        //    // = 승리
        //    VoxEventManager.Instance.PostNotifycation("AllWavesWereCleared", null);
        //    return;
        //}

        _isPreWave = false;
        CreateWave(nextWave);
    }


    private void SetCurrentWavesScore(Vehicle info)
    {
        //// 이렇게 원래 빼려고 했는데,
        //// MaxHP를 몰라서 이러한 연산이 불가능 함.
        //_currentWaveHP -= info.hp

        // 점수 Component가 있으면 점수 추가.
        MyScore scoreComponent = info.gameObject.GetComponent<MyScore>();
        if (scoreComponent)
        {
            GameManager.Instance.Score += scoreComponent.score;

            var tempGO = new GameObject("tempSource");
            tempGO.transform.position = this.transform.position;
            var src = tempGO.AddComponent<AudioSource>();
            src.PlayOneShot(scoreClip, 0.75f);
            Destroy(tempGO, scoreClip.length);
        }

        // hpRatio로 계산시, 적이 아직 남았는데도 승리 화면이 떠 버려서 0으로 바꿈.
        if (GetCurrentWaveHP() == 0f)
        {
            StartNextWave();
        }

        leftEnemies--;
        VoxEventManager.Instance.PostNotifycation("UpdateEnemyCount", leftEnemies);

        //이전 코드
        /* if (_isPreWave)
         {
             if (GetCurrentWaveHP() == 0f)
             {
                 StartNextWave();
             }
         }
         else
         {
             float hpRatio = GetCurrentWaveHP() / _totalWaveHP;
             Debug.Log(hpRatio);
             if (hpRatio < 0.2f)
             {
                 // 다음 웨이브
                 StartNextWave();
             }
         }*/
    }

    // Vehicle state checking methods
    public void VehicleDestroyEvent(Vehicle info)
    {
        SetCurrentWavesScore(info);

        // Screen Indicator에서 빼는 코드;
    }

    //이 코드를 없애고,
    //기존 VehicleDestoryEvent에 hp =0하는 코드를 넣어서 통합하는게 낫지 않을까.
    public void VehicleCrashingEvent(Airplane info)
    {
        // 그냥 파괴되어버리면 HP가 남는 문제가 있음.
        info.HP = 0;
        currentWaveCount = 0;

        SetCurrentWavesScore(info);

        // Screen Indicator에서 빼는 코드;
    }

}
