/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */

using Pathfinding;
using UnityEngine;

public class backForthMover : MonoBehaviour, IAITargetGiver
{
    #region Public Fields
    [Range(0.5f, 3f)]
    public float calculateFrecuency = 0.5f;
    public Vector3 DesiredPoint { get => desiredPoint; set => desiredPoint = value; }
    public float NextTargetDistance { get => nextTargetDistance; set => nextTargetDistance = value; }
    public Transform[] patrolPoints;
    #endregion

    #region Private Fields
    [SerializeField]
    private int patrolIndex = 0;
    [SerializeField, Range(0.2f,2)]
    private float nextTargetDistance = 0.5f;
    private Path currentPath;
    private Seeker seeker;
    private Vector3 desiredPoint;
    [SerializeField]
    private int currentWayPointIndex = 0;
    private bool reachedEndPath = false;

    #endregion

    #region Public Methods
    public bool areWeFinishedCalculating()
    {
        return seeker.IsDone();
    }

    public Path getCurrentPath()
    {
        return currentPath;
    }

    public Vector3 getNextWaypoint()
    {
        return currentPath.vectorPath[currentWayPointIndex++];
    }

    public Vector3 getCurrentWaypoint()
    {
        if (currentWayPointIndex < currentPath.vectorPath.Count)
            return currentPath.vectorPath[currentWayPointIndex];
        else
            return currentPath.vectorPath[currentPath.vectorPath.Count - 1];

    }

    public void onWayPoint()
    {
        Debug.Log("onwaypointCalled");
        currentWayPointIndex++;

        if (currentWayPointIndex >= currentPath.vectorPath.Count)
        {

            patrolIndex++;
            currentWayPointIndex = 0;
            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
                desiredPoint = patrolPoints[0].position;

                calculatePath();
            }
            else 
            {
                desiredPoint = patrolPoints[patrolIndex].position;
                calculatePath();
            }
        }      

    }



    #endregion

    #region Private Methods
    private void calculatePath()
    {
        if (seeker.IsDone())
        {

            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
                seeker.CancelCurrentPathRequest();
            }
            seeker.StartPath(transform.position, desiredPoint, onPathCalculated);
            
        }
    }

    private void onPathCalculated(Path path)
    {
        if (!path.error)
        {
            currentPath = path;
            currentWayPointIndex = 0;
        }
    }
    #endregion


#if true
    #region Unity API


    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        desiredPoint = patrolPoints[0].position;
        //seeker.StartPath(transform.position, desiredPoint, onPathCalculated);
        InvokeRepeating("calculatePath", 0, calculateFrecuency);

    }

    private void FixedUpdate()
    {
        if(currentPath == null)
        {
            return;
        }


    }

    #endregion
#endif

}