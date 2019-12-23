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
public class actorClimpUpDown : MonoBehaviour
{
    #region Public Fields

    #endregion

    #region Private Fields
    [SerializeField] float jumpUpAmount = 2f, jumpDownAmount = 0.1f;
    [SerializeField] float timeToJump = 0.4f;
    bool canWeTween = false;
    private Tween jumpingTween;
    Collider2D col;
    Rigidbody2D rb;
    IFixedBaseMovement charMover;
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    private void handleFall(onSoftEdgeArgs obj)
    {
        if (jumpingTween == null || !jumpingTween.IsPlaying())
        {
            canWeTween = true;
        }

        if (canWeTween)
        {
            obj.willDoSomething = true;
            obj.timeToWait = timeToJump + 0.05f;


            Vector2 endPos = (Vector2)Vector2Int.FloorToInt(obj.hit.point) + Vector2.right / 2 + new Vector2(0, col.bounds.extents.y);

            jumpingTween = rb.DOJump(endPos, jumpDownAmount, 0, timeToJump); 
        }

    }

    private void handleJump(onSoftEdgeArgs obj)
    {
        if(jumpingTween == null || !jumpingTween.IsPlaying())
        {
            canWeTween = true;
        }

        if (obj.hit.distance > 0 && canWeTween)
        {
            Debug.Log(gameObject.name + " tried to jump");
            obj.willDoSomething = true;
            obj.timeToWait = timeToJump + 0.05f;

            Vector2 endPos = (Vector2)Vector2Int.FloorToInt(obj.hit.point) + Vector2.right/2 + new Vector2(0, col.bounds.extents.y);


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
        charMover = GetComponent<IFixedBaseMovement>();
        charMover.onFallableEdge += handleFall;
        charMover.onWallHit += handleJump;
    }


    private void OnDisable()
    {
        charMover.onFallableEdge -= handleFall;
        charMover.onWallHit -= handleJump;
    }

    #endregion
#endif
 
}