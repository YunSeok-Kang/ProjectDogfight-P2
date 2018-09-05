using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAirplaneRouteManager : MonoBehaviour {
    
    [SerializeField]
    private Transform _currentTarget = null;

    private int _currentWaypointIndex = -1;
    private bool isLastWaypoint = false; // waypoint의 마지막이면 해당 변수 True.

    public Transform CurrentTarget
    {
        private set
        {
            _currentTarget = value;
        }

        get
        {
            return _currentTarget;
        }
    }

    // AIAirplaneController 측에서 자동으로 해당 정보를 채워줌.
    public AIAirplaneController targetController = null;

    public GameObject[] waypoints = null;

    public float waypointTriggingCheckSize = 30f;

	// Use this for initialization
	void Start ()
    {
        waypoints = new GameObject[0];

        CurrentTarget = GetNext();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLastWaypoint)
            CheckTarget();
    }

    private Transform GetNext()
    {
        //// Waypoint 마지막 끝나면 Player의 비행기를 쫓아감
        //if (_currentWaypointIndex == (waypoints.Length - 1))
        //{
        //    isLastWaypoint = true;
        //    return FindPlayerPlane();
        //}
        //else
        //{
        //    _currentWaypointIndex++;
        //}

        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;


        Debug.Log("waypoints[" + _currentWaypointIndex + "].transform");
        return waypoints[_currentWaypointIndex].transform;
    }

    private Transform FindPlayerPlane()
    {
        PlayerAirplaneController playerPlaneController = FindObjectOfType<PlayerAirplaneController>();
        return playerPlaneController.transform;
    }
    

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            //Gizmos.DrawSphere(CurrentTarget.position, waypointTriggingCheckSize);
        }
    }

    private void CheckTarget()
    {
        //RaycastHit rayHit;
        //bool isHit = Physics.SphereCast(CurrentTarget.position, waypointTriggingCheckSize, CurrentTarget.forward, out rayHit);
        //if (isHit)
        //{
        //    Debug.Log("맞음");
        //}

        RaycastHit[] rayHits;
        rayHits = Physics.SphereCastAll(CurrentTarget.position, waypointTriggingCheckSize, Vector3.up, 0f);

        if (rayHits.Length > 0)
        {
            foreach (RaycastHit rayHit in rayHits)
            {
                // 범위 안으로 들어온 녀석이 targetController의 gameObject와 같다면
                if (targetController.gameObject == rayHit.collider.gameObject)
                {
                    CurrentTarget = GetNext();
                }
            }
        }
    }
}
