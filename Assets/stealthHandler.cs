/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;


/// <summary>
/// Manages animation and stealth state
/// </summary>
public class stealthHandler : MonoBehaviour
{
    #region Public Fields
    public bool Visible = true;
    public float stealthTime = 0.3f;
    #endregion

    #region Private Fields
    characterMovement charMov;
    Animator animator;
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void camoStopHandle()
    {
        animator.SetBool("Stealthing", false);
        StartCoroutine(turnVisibleAfterXTime());
    }

    private void camoStartHandle()
    {
        Visible = false;
        animator.SetBool("Stealthing", true);
    }

    private IEnumerator turnVisibleAfterXTime()
    {
        yield return new WaitForSeconds(stealthTime);
        Visible = true;
    }
    #endregion


#if true
    #region Unity API

    void Awake()
    {
        animator = GetComponent<Animator>();
        charMov = GetComponent<characterMovement>();
        charMov.onCamoStart += camoStartHandle;
        charMov.onCamoFinish += camoStopHandle;
    }



    private void OnDisable()
    {
        charMov.onCamoStart -= camoStartHandle;
        charMov.onCamoFinish -= camoStopHandle;
    }
    void FixedUpdate()
    {
    }



    #endregion
#endif

}