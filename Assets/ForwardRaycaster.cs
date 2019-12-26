/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */
 
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ForwardRaycaster : MonoBehaviour
{
    #region Public Fields
    public LayerMask playerLayers;
    public float maxDistance = 2;
    #endregion
 
    #region Private Fields
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion


    #if true
    #region Unity API

    void Start()
    {

    }
 
    void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, transform.localScale, maxDistance, playerLayers);
        if (hit)
        {
            if (hit.collider.GetComponent<stealthHandler>().Visible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    #endregion
    #endif
 
}