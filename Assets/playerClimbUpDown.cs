/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */

using System;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// This will make the playerJump down/up 
/// Jumping up will happen if distance > 0 might generate trouble with slopes
/// </summary>
public class playerClimbUpDown : MonoBehaviour
{
    #region Public Fields

    #endregion

    #region Private Fields
    [SerializeField] float jumpUpAmount = 2f, jumpDownAmount = 2f;
    [SerializeField] float timeToJump = 0.4f;
    private Tween jumpingTween;
    Collider2D col;
    Rigidbody2D rb;
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    private void handleFall(onSoftEdgeArgs obj)
    {

        obj.willDoSomething = true;
        obj.timeToWait = timeToJump + 0.05f;


        Vector2 endPos = obj.hit.collider.transform.position + new Vector3(0, obj.hit.collider.bounds.extents.y + col.bounds.extents.y);

        jumpingTween = rb.DOJump(endPos, jumpDownAmount, 1, timeToJump);

    }

    private void handleJump(onSoftEdgeArgs obj)
    {
        if (obj.hit.distance > 0)
        {
            obj.willDoSomething = true;
            obj.timeToWait = timeToJump + 0.05f;

            Vector2 endPos = obj.hit.collider.transform.position + new Vector3(0, obj.hit.collider.bounds.extents.y + col.bounds.extents.y);

            jumpingTween = rb.DOJump(endPos, jumpUpAmount, 1, timeToJump);

        }
    }

    #endregion
    
#if false
    #region Unity API

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        characterMovement.onFallableEdge += handleFall;
        characterMovement.onWallHit += handleJump;
    }


    private void OnDisable()
    {
        characterMovement.onFallableEdge -= handleFall;
        characterMovement.onWallHit -= handleJump;
    }

    #endregion
#endif
 
}