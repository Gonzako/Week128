/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[RequireComponent(typeof(Collider2D))]
public class characterMovement : MonoBehaviour
{
    #region Public Fields
    public LayerMask groundLayer;

    #region Events
    public static event Action<onSoftEdgeArgs> onFallableEdge;
    public static event Action<onHardEdgeArgs> onCliff;
    public static event Action<onSoftEdgeArgs> onWallHit;
    #endregion

    #endregion

    #region Private Fields
    [SerializeField] private float jumpForce = 10;
    Collider2D col;
    Rigidbody2D rb;
    SpriteRenderer characterRenderer;
    [SerializeField] float speed = 1;
    [Range(0, 3)]
    [SerializeField] int maxFallDown = 1;
    [SerializeField] float maxDistance = 20f;
    [SerializeField, Range(1, 2)] float forwardRaycastMultiplier = 1.15f;
    [SerializeField, Range(0.1f, 0.4f)] float raycastDownLeeway  = 0.2f;

    onSoftEdgeArgs fallableArgs = new onSoftEdgeArgs() { willDoSomething = false};
    onHardEdgeArgs cliffArgs = new onHardEdgeArgs() { willDoSomething = false};
    bool canJump = true;
    bool canMoveForward = true;
    float disabledTime;
    int currentSide = 1;
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods
    private void MovePlayer()
    {
        /*
         
        1 is left
        -1 right
         
         */
        if (Input.GetAxisRaw("Horizontal") * currentSide >= 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rb.velocity = new Vector2 (speed * currentSide, rb.velocity.y);
            }
        }
        else
        {
            characterRenderer.flipX = !characterRenderer.flipX;
            currentSide = currentSide * -1;
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce / Time.fixedDeltaTime));
            canJump = false; 
        }
    }
    private void CheckGround()
    {
        int side = characterRenderer.flipX ? -1 : 1;
        var hit = Physics2D.Raycast(new Vector2(transform.position.x + currentSide * (col.bounds.extents.x * forwardRaycastMultiplier + col.offset.x),
                                    transform.position.y + col.bounds.extents.y), -transform.up, maxDistance, groundLayer);


        if (hit)
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + (col.bounds.extents.x *forwardRaycastMultiplier + col.offset.x ) * currentSide * transform.right + transform.up*col.bounds.extents.y, -transform.up * hit.distance, Color.red);
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.distance);
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
            else if(hit.distance < col.bounds.extents.y)
            {
                fallableArgs.hit = hit;


                onWallHit?.Invoke(fallableArgs);

                if (fallableArgs.willDoSomething)
                {
                    disabledTime = fallableArgs.timeToWait;
                    fallableArgs.resetValues();
                }
                
            } else
            {
                canMoveForward = true;
            }

        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + transform.up * col.bounds.extents.y+ (col.bounds.extents.x * forwardRaycastMultiplier + col.offset.x) * currentSide * transform.right, -transform.up * maxDistance, Color.red);
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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        characterRenderer = GetComponentInChildren<SpriteRenderer>();
    }
 
    void FixedUpdate()
    {
        if (disabledTime < Time.time)
        {
            CheckGround();
            MovePlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++) { 
        
            Debug.Log("contacted with normal " + collision.GetContact(i).normal + " Dot is " + Math.Abs(Vector2.Dot(collision.GetContact(i).normal, (Vector2)transform.up)));
            if (Math.Abs(Vector2.Dot(collision.GetContact(i).normal, (Vector2)transform.up)) > 0.9)
            {
            canJump = true;
            }
         }
    }
    



    #endregion
#endif


}

public class onSoftEdgeArgs
{
    public RaycastHit2D hit;
    public float timeToWait;
    public bool willDoSomething = false;

    public void resetValues()
    {
        hit = new RaycastHit2D();
        timeToWait = 0;
        willDoSomething = false;
    }
}

public class onHardEdgeArgs
{
    public float timeToWait;
    public bool willDoSomething = false;
    public RaycastHit2D hit;

    public void resetValues()
    {
        hit = new RaycastHit2D();
        timeToWait = 0;
        willDoSomething = false;
    }
}