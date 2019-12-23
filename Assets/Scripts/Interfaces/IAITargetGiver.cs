using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public interface IAITargetGiver
{

    Path getCurrentPath();

    Vector3 getNextWaypoint();
    Vector3 DesiredPoint { get; set; }
    float NextTargetDistance { get; set; }

    Vector3 getCurrentWaypoint();



    void onWayPoint();

    bool areWeFinishedCalculating();

}
