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

    #endregion

    #region Private Fields
    private float nextTargetDistance = 0.5f;
    private Path currentPath;
    private Seeker seeker;
    [SerializeField]
    private Vector3 desiredPoint;
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
        return currentPath.vectorPath[currentWayPointIndex];
    }

    public void onWayPoint()
    {

    }
  


    #endregion

    #region Private Methods
    private void calculatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, desiredPoint, onPathCalculated);
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


#if false
    #region Unity API

    void Start()
    {
    }
 
    void FixedUpdate()
    {
    }

    #endregion
#endif

}