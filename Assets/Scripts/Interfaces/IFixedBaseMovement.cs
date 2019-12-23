using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixedBaseMovement 
{
    event Action<onSoftEdgeArgs> onFallableEdge;
    event Action<onHardEdgeArgs> onCliff;
    event Action<onSoftEdgeArgs> onWallHit;
}
