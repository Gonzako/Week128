using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class baseAITargetGiver : MonoBehaviour, IAITargetGiver
{
    [Range(0.5f, 3f)]
    public float calculateFrecuency = 0.5f;
    public Vector3 DesiredPoint { get => desiredPoint; set => desiredPoint = value; }
    public float NextTargetDistance { get => nextTargetDistance; set => nextTargetDistance = value; }

    private float nextTargetDistance = 0.5f;
    private Path currentPath;
    private Seeker seeker;
    [SerializeField]
    private Vector3 desiredPoint;
    private int currentWayPointIndex = 0;
    private bool reachedEndPath = false;
    
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

    private void onPathCalculated(Path path)
    {
        if (!path.error)
        {
            currentPath = path;
            currentWayPointIndex = 0;
        }
    }

    private void calculatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(transform.position, desiredPoint, onPathCalculated);
    }


    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        //seeker.StartPath(transform.position, desiredPoint, onPathCalculated);
        InvokeRepeating("calculatePath", 0, calculateFrecuency);
    }

    private void FixedUpdate()
    {
        if(currentPath == null)
        {
            return;
        }

        if(currentWayPointIndex >= currentPath.vectorPath.Count)
        {
            reachedEndPath = true;
        }
        else
        {
            reachedEndPath = false;
        }


    }

    public void onWayPoint()
    {
        currentWayPointIndex++;
    }
}
