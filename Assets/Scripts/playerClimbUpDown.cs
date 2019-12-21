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
    [SerializeField] float jumpUpAmount = 2f, jumpDownAmount = 0.1f;
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


        Vector2 endPos = (Vector2)Vector2Int.FloorToInt(obj.hit.point) + Vector2.one/2 + new Vector2(0, col.bounds.extents.y);

        jumpingTween = rb.DOJump(endPos, jumpDownAmount, 0, timeToJump);

    }

    private void handleJump(onSoftEdgeArgs obj)
    {
        Debug.Log("Tried to jump");
        if (obj.hit.distance > 0)
        {
            obj.willDoSomething = true;
            obj.timeToWait = timeToJump + 0.05f;

            Vector2 endPos = Vector2Int.FloorToInt(obj.hit.point) + new Vector2(0, col.bounds.extents.y);


            jumpingTween = rb.DOJump(endPos, jumpUpAmount, 0, timeToJump);

        }
    }

    #endregion
    
#if true

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