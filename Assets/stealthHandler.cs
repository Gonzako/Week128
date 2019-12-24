/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */

using System;
using UnityEngine;


/// <summary>
/// Manages animation and stealth state
/// </summary>
public class stealthHandler : MonoBehaviour
{
    #region Public Fields
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
    }

    private void camoStartHandle()
    {
        animator.SetBool("Stealthing", true);
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