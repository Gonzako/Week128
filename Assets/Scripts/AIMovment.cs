using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */


[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(IAITargetGiver))]
public class AIMovment : MonoBehaviour, IFixedBaseMovement
{
    #region Public Fields
    public LayerMask groundLayer;

    #region Events
    public event Action<onSoftEdgeArgs> onFallableEdge;
    public event Action<onHardEdgeArgs> onCliff;
    public event Action<onSoftEdgeArgs> onWallHit;
    #endregion

    #endregion

    #region Private Fields
    [SerializeField] private Animator animator;
    #region RequiredComponents
    Collider2D col;
    IAITargetGiver targetGiver;
    Rigidbody2D rb;
    SpriteRenderer characterRenderer; 
    #endregion
    [SerializeField] float speed = 100;
    [Range(0, 3)]
    [SerializeField] int maxFallDown = 1;
    [SerializeField] float maxDistance = 20f;
    [SerializeField, Range(1, 2)] float forwardRaycastMultiplier = 1.15f;
    [SerializeField, Range(0.1f, 0.4f)] float raycastDownLeeway = 0.2f;

    onSoftEdgeArgs fallableArgs = new onSoftEdgeArgs() { willDoSomething = false };
    onHardEdgeArgs cliffArgs = new onHardEdgeArgs() { willDoSomething = false };
    bool canJump = true;
    bool canMoveForward = true;
    float disabledTime;
    int currentSide = 1;
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods
    private void MoveToTarget()
    { 

        var direction = (Vector2)targetGiver.getCurrentWaypoint() - rb.position;
        var distance = Vector2.Distance(rb.position, targetGiver.getCurrentWaypoint());
        if (distance < targetGiver.NextTargetDistance)
        {
            targetGiver.onWayPoint();
        }
        direction.y = direction.y / 2;
        direction = direction.normalized;

        if(targetGiver.DesiredPoint.x - transform.position.x > 0)
        {
            currentSide = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);

        }
        else
        {
            currentSide = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }

        rb.position += (direction * speed * Time.fixedDeltaTime); 

    }
    private void CheckGround()
    {
        var hit = Physics2D.Raycast(new Vector2(transform.position.x + currentSide * (col.bounds.extents.x * forwardRaycastMultiplier + col.offset.x),
                                    transform.position.y + col.bounds.extents.y), -transform.up, maxDistance, groundLayer);


        if (hit)
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position +
                (col.bounds.extents.x * forwardRaycastMultiplier + col.offset.x) * currentSide * transform.right
                + transform.up * col.bounds.extents.y,
                -transform.up * hit.distance, Color.red);
#endif

            if (hit.distance > col.bounds.size.y + raycastDownLeeway)
            {
                //Fallable groundCheck
                if (hit.distance < maxFallDown + col.bounds.size.y && rb.velocity.y == 0)
                {
#if UNITY_EDITOR
                    Debug.Log("onFallableGround");
#endif
                    fallableArgs.hit = hit;
                    onFallableEdge?.Invoke(fallableArgs);

                    if (fallableArgs.willDoSomething)
                    {
                        disabledTime = Time.time + fallableArgs.timeToWait;
                        fallableArgs.resetValues();
                    }


                }
                else
                {
                    cliffArgs.hit = hit;
                    Debug.Log("Over a cliff");
                    canMoveForward = false;
                    onCliff?.Invoke(cliffArgs);
                    if (cliffArgs.willDoSomething)
                    {
                        disabledTime = Time.time + cliffArgs.timeToWait;
                        cliffArgs.resetValues();
                    }
                }
            }
            else if (hit.distance < col.bounds.extents.y)
            {
                fallableArgs.hit = hit;


                onWallHit?.Invoke(fallableArgs);

                if (fallableArgs.willDoSomething)
                {
                    disabledTime = fallableArgs.timeToWait;
                    fallableArgs.resetValues();
                }

            }
            else
            {
                canMoveForward = true;
            }

        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + transform.up * col.bounds.extents.y + (col.bounds.extents.x * forwardRaycastMultiplier + col.offset.x) * currentSide * transform.right, -transform.up * maxDistance, Color.red);
#endif
            canMoveForward = false;
            onCliff?.Invoke(cliffArgs);
            if (cliffArgs.willDoSomething)
            {
                disabledTime = Time.time + cliffArgs.timeToWait;
                cliffArgs.resetValues();
            }

        }
    }
    #endregion


#if true
    #region Unity API

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        characterRenderer = GetComponentInChildren<SpriteRenderer>();
        targetGiver = GetComponent<IAITargetGiver>();
    }

    void FixedUpdate()
    {
        if (disabledTime < Time.time)
        {
            CheckGround();
            MoveToTarget();
        }
    }




    #endregion
#endif


}
