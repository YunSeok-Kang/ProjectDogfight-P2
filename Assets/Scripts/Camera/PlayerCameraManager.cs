using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    static private PlayerCameraManager _instance = null;
    static public PlayerCameraManager Instance
    {
        private set
        {
            _instance = value;
        }

        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerCameraManager>();


                if (_instance == null)
                {
                    Debug.LogError("PlayerCameraManager를 Hierarchy 탭에서 찾지 못했습니다.");
                }
            }

            return _instance;
        }
    }

    private GameObject _target;
    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            _targetTrans = _target.transform;
        }
    }
    private Transform _targetTrans;


    [Header("Speed")]
    //[Range(0,1)]
    public float positionFollowSpeed;
   // [Range(0,1)]
    public float angleFollowSpeed;
    [Header("Altitude")]
    public float maxAltitude = 300f;
    public float minAltitude = 100f;
    private float maxAltMinusMinAlt;
    [Header("ForwardOffset")]
    public float maxForward;
    public float minForward;
    [Header("AngleOffset")]
    public Quaternion maxAngle = Quaternion.Euler(30, 0, 0);
    public Quaternion minAngle = Quaternion.Euler(0, 0, 0);
    [Header("PositionOffset")]
    public Vector3 maxOffset;
    public Vector3 minOffset;

    [Header("For Checkout")]
    [SerializeField]
    private Vector3 _currentOffset;
    [SerializeField]
    private float _currentForward;
    [SerializeField]
    private Quaternion _currentAngle;

    [SerializeField]
    private float cameraStartingTime = 15;

    [Header("Shake")]
    public float duration = 0.25f;
    public float magnitude = 0.1f;

    void Start()
    {
        maxAltMinusMinAlt = maxAltitude - minAltitude;

        StartCoroutine("StartSpeed");
    }
    private IEnumerator StartSpeed()
    {
        float tempPosSpd = positionFollowSpeed;
        float tempAngSpd = angleFollowSpeed;
        float currentTime;

        positionFollowSpeed = 0;
        angleFollowSpeed = 0;

        currentTime = Time.time;
        cameraStartingTime = cameraStartingTime + Time.time;
        while (true)
        {
            if(positionFollowSpeed == tempPosSpd && angleFollowSpeed == tempAngSpd)
            {
                break;
            }
            positionFollowSpeed = Mathf.Lerp(0, tempPosSpd, currentTime/ cameraStartingTime);
            angleFollowSpeed = Mathf.Lerp(0, tempAngSpd, currentTime / cameraStartingTime);

            currentTime = Time.time;

            yield return new WaitForFixedUpdate();
        }
    }
    void FixedUpdate()
    {
        if (_target != null && _target.CompareTag("Player"))
        {
            //확대축소 비율 구하기
            float magnitudeFromTop = maxAltitude - _targetTrans.position.y;
            float magnitudeFromBottom = Mathf.Abs(minAltitude - _targetTrans.position.y);

            Vector3 newCameraPosition;
            newCameraPosition = _targetTrans.position;
            newCameraPosition += GetCameraForwardByMagnitude(magnitudeFromBottom);
            newCameraPosition += GetCameraOffsetByMagnitude(magnitudeFromTop);

            MoveCameraSmootly(GetCameraAngleByMagnitude(magnitudeFromTop), newCameraPosition, angleFollowSpeed, positionFollowSpeed);
        }
        else
        {
            //적을 쫒아갈 경우
            Vector3 newCameraPosition;
            newCameraPosition = _targetTrans.position;
            newCameraPosition += new Vector3(0,5,40);

             MoveCameraSmootly(new Quaternion(0,180,0,1), newCameraPosition, 10, 10);
        }
    }



    private void MoveCameraSmootly(Quaternion newAngle, Vector3 newPosition, float angleSpeed, float positionSpeed)
    {
        //부드럽게 전환
        transform.rotation = Quaternion.Slerp(transform.rotation,
            newAngle,
            angleSpeed * Time.deltaTime);
        transform.position = Vector3.Slerp(transform.position,
            newPosition,
            positionSpeed * Time.deltaTime);
    }

    private Vector3 GetCameraForwardByMagnitude(float magnitude)
    {
        float bigger, smaller;
        if(minForward > maxForward)
        {
            bigger = minForward;
            smaller = maxForward;
        }
        else
        {
            bigger = maxForward;
            smaller = minForward;
        }

        //가수 구하기
        float minusForward = (bigger - smaller) / maxAltMinusMinAlt;
        float calculatedForward = (bigger - (minusForward * magnitude));

        //클램프
       float clampedForward;
       clampedForward = Mathf.Clamp(calculatedForward, smaller, bigger);
       _currentForward = clampedForward;
       return _targetTrans.forward * clampedForward;
    }

    private Vector3 GetCameraOffsetByMagnitude(float magnitude)
    {
        //가수 구하기
        float minusY = (maxOffset.y - minOffset.y) / maxAltMinusMinAlt;
        float minusZ = (maxOffset.z - minOffset.z) / maxAltMinusMinAlt;
        Vector3 minusOffset = new Vector3(0, minusY, minusZ);

        Vector3 calculatedOffset = maxOffset - (minusOffset * magnitude);

        //클램프
        float clampedY = Mathf.Clamp(calculatedOffset.y, minOffset.y, maxOffset.y);
        float clampedZ = Mathf.Clamp(calculatedOffset.z, minOffset.z, maxOffset.z); //Z는 커질때마다 -로 뒤로 물러나므로, max와 min이 바뀌어야함.
        calculatedOffset = new Vector3(0, clampedY, clampedZ);

        return _currentOffset =calculatedOffset;
    }

    private Quaternion GetCameraAngleByMagnitude(float magnitude)
    {
        float minusXDegree = (maxAngle.x - minAngle.x) / maxAltMinusMinAlt;
        float minusYDegree = (maxAngle.y - minAngle.y) / maxAltMinusMinAlt;
        float minusZDegree = (maxAngle.z - minAngle.z) / maxAltMinusMinAlt;
        float calculatedXDgree = maxAngle.x - (minusXDegree * magnitude);
        float calculatedYDgree = maxAngle.y - (minusYDegree * magnitude);
        float calculatedZDgree = maxAngle.z - (minusZDegree * magnitude);
        calculatedXDgree = Mathf.Clamp(calculatedXDgree, minAngle.x, maxAngle.x);
        calculatedYDgree = Mathf.Clamp(calculatedXDgree, minAngle.y, maxAngle.y);
        calculatedZDgree = Mathf.Clamp(calculatedXDgree, minAngle.z, maxAngle.z);

        Quaternion newQuaternion = Quaternion.Euler(calculatedXDgree, calculatedYDgree, calculatedZDgree);
        return _currentAngle = newQuaternion;
    }


    public IEnumerator Shake()
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;

            // We want to reduce the shake from full power to 0 starting half way through
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float z = Random.value * 2.0f - 1.0f;

            x *= magnitude * damper;
            z *= magnitude * damper;

            transform.position += new Vector3(x, z, 0);

            yield return null;
        }
    }
}
