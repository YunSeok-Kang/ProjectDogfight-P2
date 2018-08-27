using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{

    public GameObject target;
    private Transform _targetTrans;


    [Header("Speed")]
    public float positionFollowSpeed;
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




    void Start()
    {
        //baseOffset = transform.position - targetToFollow.transform.position;
        _targetTrans = target.transform;
        maxAltMinusMinAlt = maxAltitude - minAltitude;
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            //확대축소 비율 구하기
            float magnitudeFromTop = maxAltitude - _targetTrans.position.y;
            float magnitudeFromBottom = Mathf.Abs(minAltitude - _targetTrans.position.y);

            Vector3 newCameraPosition;
            newCameraPosition = _targetTrans.position;
            newCameraPosition += GetCameraForwardByMagnitude(magnitudeFromBottom);
            newCameraPosition += GetCameraOffsetByMagnitude(magnitudeFromTop);

            MoveCameraSmootly(GetCameraAngleByMagnitude(magnitudeFromTop), newCameraPosition);
        }
    }



    private void MoveCameraSmootly(Quaternion newAngle, Vector3 newPosition)
    {
        //부드럽게 전환
        transform.rotation = Quaternion.Slerp(transform.rotation,
            newAngle,
            angleFollowSpeed * Time.deltaTime);
        transform.position = Vector3.Slerp(transform.position,
            newPosition,
            positionFollowSpeed * Time.deltaTime);
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
}
